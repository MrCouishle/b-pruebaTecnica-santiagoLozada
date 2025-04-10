namespace Services
{
    public interface IResultServices
    {
        Task<Response<List<Result>>> GetResults();
        Task<Response<Result>> GetValidateResult(BetValidationDto bet);
        Task<Response<Result>> Create(ResultCreateDto result);
    }

    public class ResultServices(DataContext context) : ResponseService, IResultServices
    {
        private readonly DataContext _context = context;

        public async Task<Response<List<Result>>> GetResults()
        {
            var dbResult = await _context.Result.ToListAsync();

            if (dbResult.Count == 0)
            {
                return ErrorResponse<List<Result>>(MessageConst.ResultNoResults);
            }
            else
            {
                return SuccessResponse(dbResult, MessageConst.ResultsSuccesConsult);
            }
        }

        public async Task<Response<Result>> Create(ResultCreateDto result)
        {
            var user = await _context.User.FirstOrDefaultAsync(u => u.Id == result.UserId);
            if (user == null)
            {
                return ErrorResponse<Result>("Usuario no encontrado.");
            }

            user.Balance = user.Balance - result.BetValue + result.Profit;

            Result newResult = new Result
            {
                BetValue = result.BetValue,
                Profit = result.Profit,
                RemainingBalance = result.RemainingBalance,
                CurrentBalance = result.RemainingBalance,
                UserId = result.UserId,
                RouletteNumber = result.RouletteNumber,
                CreatedAt = result.CreatedAt,
            };

            _context.Result.Add(newResult);
            await _context.SaveChangesAsync();

            var createdResult = await _context.Result
                .Include(r => r.User)
                .FirstOrDefaultAsync(r => r.Id == newResult.Id);

            return SuccessResponse(createdResult!, MessageConst.ResultsSuccesCreate);
        }

        public async Task<Response<Result>> GetValidateResult(BetValidationDto bet)
        {
            var user = await _context.User.FirstOrDefaultAsync(u => u.Id == bet.UserId);
            if (user == null)
            {
                return ErrorResponse<Result>("Usuario no encontrado.");
            }
            if (!bet.ResultNumber.HasValue)
            {
                return ErrorResponse<Result>("Número de resultado no proporcionado.");
            }

            var resultColor = RouletteHelper.GetColor(bet.ResultNumber.Value);
            var resultColorStr = resultColor.ToString().ToLower();

            decimal profit = 0;

            // Validaciones de coincidencias
            bool colorMatch =
                !string.IsNullOrEmpty(bet.BetColor) && bet.BetColor.ToLower() == resultColorStr;
            bool numberMatch = bet.BetNumber.HasValue && bet.BetNumber.Value == bet.ResultNumber;

            bool evenOddMatch = false;
            if (!string.IsNullOrEmpty(bet.EvenOdd))
            {
                bool isEven = bet.ResultNumber % 2 == 0;
                if (bet.EvenOdd.ToLower() == "even" && isEven)
                    evenOddMatch = true;
                if (bet.EvenOdd.ToLower() == "odd" && !isEven)
                    evenOddMatch = true;
            }

            // Calcular ganancia
            if (numberMatch && string.IsNullOrEmpty(bet.BetColor)) // Solo número
            {
                profit = bet.BetValue * 3;
            }
            else if (numberMatch && colorMatch) // Número y color
            {
                profit = bet.BetValue * 3;
            }
            else if (evenOddMatch && colorMatch)
            {
                profit = bet.BetValue;
            }
            else if (evenOddMatch && string.IsNullOrEmpty(bet.BetColor)) // Solo par/impar
            {
                profit = bet.BetValue;
            }
            else if (colorMatch)
            {
                profit = bet.BetValue / 2;
            }

            var newResult = new Result
            {
                BetValue = bet.BetValue,
                Profit = profit,
                CurrentBalance = user.Balance - bet.BetValue + profit,
                RemainingBalance = user.Balance - bet.BetValue,
                User = user,
                UserId = user.Id,
                Winner = profit > 0,
                RouletteNumber = bet.ResultNumber.Value,
                CreatedAt = DateTime.UtcNow
            };

            return SuccessResponse(
                newResult,
                $"Resultado: número {bet.ResultNumber} ({resultColorStr}). Ganancia: {profit:C2}"
            );
        }
    }
}
