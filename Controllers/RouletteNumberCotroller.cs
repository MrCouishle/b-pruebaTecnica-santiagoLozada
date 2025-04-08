using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services;

namespace Roulette.Controllers
{
    [Route("RouletteNumber")]
    [ApiController]
    public class RouletteNumberController(IRouletteNumberServices RouletteNumberServices)
        : BaseService
    {
        private readonly IRouletteNumberServices _rouletteNumberServices = RouletteNumberServices;

        [HttpGet("GetRouletteNumbers")]
        public async Task<ActionResult<List<RouletteNumber>>> Get()
        {
            var response = await _rouletteNumberServices.Get();
            return SendResponse(response);
        }
    }
}
