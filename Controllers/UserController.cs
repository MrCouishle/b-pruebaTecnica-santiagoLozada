using Microsoft.AspNetCore.Mvc;
using Services;

namespace Roulette.Controllers
{
    [Route("User")]
    [ApiController]
    public class UserController(IUserServices UserServices) : BaseService
    {
        private readonly IUserServices _userServices = UserServices;

        [HttpGet("GetUsers")]
        public async Task<ActionResult<List<User>>> GetUsers()
        {
            var response = await _userServices.GetUsers();
            return SendResponse(response);
        }

        [HttpGet("GetUser")]
        public async Task<ActionResult<User>> Get([FromQuery] string Name)
        {
            var response = await _userServices.Get(Name);
            return SendResponse(response);
        }

        [HttpPost]
        public async Task<ActionResult<UserCreateDto>> Create(UserCreateDto user)
        {
            var response = await _userServices.Create(user);
            return SendResponse(response);
        }
    }
}
