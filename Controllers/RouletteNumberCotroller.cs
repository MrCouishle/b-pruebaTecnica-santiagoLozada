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
        public async Task<ActionResult<Dictionary<int, string>>> Get()
        {
            var response = await _rouletteNumberServices.Get();
            return SendResponse(response);
        }

        [HttpGet("GetRandomNumber")]
        public async Task<ActionResult<RouletteNumberDto>> GetRandom()
        {
            var response = await _rouletteNumberServices.GetRandom();
            return SendResponse(response);
        }
    }
}
