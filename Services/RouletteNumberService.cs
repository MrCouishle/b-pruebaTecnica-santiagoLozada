namespace Services
{
    public interface IRouletteNumberServices
    {
        Task<Response<Dictionary<int, string>>> Get();
        Task<Response<RouletteNumberDto>> GetRandom();
    }

    public class RouletteNumberServices(DataContext context)
        : ResponseService,
            IRouletteNumberServices
    {
        private readonly DataContext _context = context;

        public async Task<Response<Dictionary<int, string>>> Get()
        {
            var dbRouletteNumber = await _context.RouletteNumber.ToListAsync();

            if (dbRouletteNumber.Count == 0)
            {
                return ErrorResponse<Dictionary<int, string>>(
                    MessageConst.RouletteNumbersNoResults
                );
            }

            var resultDict = dbRouletteNumber
                .OrderBy(r => r.Id == 37 ? 0 : r.Id)
                .ToDictionary(r => r.Id == 37 ? 0 : r.Id, r => r.Color.ToString().ToLower());

            return SuccessResponse(resultDict, MessageConst.RouletteNumbersSuccesConsult);
        }

        public async Task<Response<RouletteNumberDto>> GetRandom()
        {
            var dbRouletteNumber = await _context.RouletteNumber.ToListAsync();

            if (dbRouletteNumber.Count == 0)
            {
                return ErrorResponse<RouletteNumberDto>(MessageConst.RouletteNumbersNoResults);
            }

            var random = new Random();
            var selected = dbRouletteNumber[random.Next(dbRouletteNumber.Count)];

            var resultDto = new RouletteNumberDto
            {
                Id = selected.Id,
                Color = selected.Color.ToString().ToLower()
            };

            return SuccessResponse(resultDto, "Número aleatorio seleccionado correctamente.");
        }
    }
}
