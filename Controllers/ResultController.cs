using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services;

namespace Roulette.Controllers
{
    [Route("Result")]
    [ApiController]
    public class ResultController(IResultServices ResultServices) : BaseService
    {
        private readonly IResultServices _resultServices = ResultServices;

        [HttpGet("GetResults")]
        public async Task<ActionResult<List<Result>>> GetResults()
        {
            var response = await _resultServices.GetResults();
            return SendResponse(response);
        }

        [HttpGet("GetValidateResult")]
        public async Task<ActionResult<Result>> GetValidateResult([FromQuery] BetValidationDto bet)
        {
            var response = await _resultServices.GetValidateResult(bet);
            return SendResponse(response);
        }

        [HttpPost]
        public async Task<ActionResult<Result>> Create(ResultCreateDto result)
        {
            var response = await _resultServices.Create(result);
            return SendResponse(response);
        }
    }
}
