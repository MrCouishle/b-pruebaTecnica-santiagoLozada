namespace Services
{
    public interface IResultServices
    {
        Task<Response<List<Result>>> GetResults();
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
            Result newResult = new Result
            {
                BetValue = result.BetValue,
                Profit = result.Profit,
                RemainingBalance = result.RemainingBalance,
                UserId = result.UserId,
                RouletteNumber = result.RouletteNumber,
                CreatedAt = result.CreatedAt,
            };

            _context.Result.Add(newResult);
            await _context.SaveChangesAsync();

            var createdResult = await _context.Result.FirstOrDefaultAsync(
                u => u.Id == newResult.Id
            );

            return SuccessResponse(createdResult!, MessageConst.ResultsSuccesCreate);
        }
    }
}
