namespace Services
{
    public interface IRouletteNumberServices
    {
        Task<Response<List<RouletteNumber>>> Get();
    }

    public class RouletteNumberServices(DataContext context)
        : ResponseService,
            IRouletteNumberServices
    {
        private readonly DataContext _context = context;

        public async Task<Response<List<RouletteNumber>>> Get()
        {
            var dbRouletteNumber = await _context.RouletteNumber.ToListAsync();

            if (dbRouletteNumber.Count == 0)
            {
                return ErrorResponse<List<RouletteNumber>>(MessageConst.RouletteNumbersNoResults);
            }
            else
            {
                return SuccessResponse(dbRouletteNumber, MessageConst.RouletteNumbersSuccesConsult);
            }
        }
    }
}
