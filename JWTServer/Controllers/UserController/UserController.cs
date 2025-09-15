using JWTServer.Processor;
using JWTServer.ServiceRepository;
using JWTServer.Utilities;
using JWTServer.ViewModels.Users;
using Microsoft.AspNetCore.Mvc;

namespace JWTServer.Controllers.UserController
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IAuthServiceRepository _authService;
        private readonly AppDbContext _context;

        private readonly IProcessor<UserLoginBaseModel> _IProcessor;
        public UserController(AppDbContext context, IAuthServiceRepository authService, IProcessor<UserLoginBaseModel> IProcessor)
        {
            _context = context;
            _authService = authService;
            _IProcessor = IProcessor;
        }

        [HttpGet]
        [Route("GetUser")]
        public async Task<IActionResult> GetUser()
        {
            try
            {
                var result = await _IProcessor.ProcessGet(Guid.NewGuid(), User);
                return Ok(result);
            }
            catch (Exception e)
            {
                string innerexp = "";
                if (e.InnerException != null)
                {
                    innerexp = " Inner Error : " + e.InnerException.ToString();
                }
                return BadRequest(e.Message.ToString() + innerexp);
            }
        }
    }
}
