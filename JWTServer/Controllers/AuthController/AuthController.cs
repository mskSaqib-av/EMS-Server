using JWTServer.Model;
using JWTServer.ServiceRepository;
using JWTServer.Utilities;
using JWTServer.ViewModels.Users;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Security.Cryptography;
using Microsoft.EntityFrameworkCore;
using JWTServer.Processor;

namespace JWTServer.Controllers.AuthController
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthServiceRepository _authService;
        private readonly AppDbContext _context;

        private readonly IProcessor<UserLoginBaseModel> _IProcessor;

        public AuthController(AppDbContext context,IAuthServiceRepository authService, IProcessor<UserLoginBaseModel> IProcessor)
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

        [HttpPost("RegisterUser")]
        public async Task<IActionResult> RegisterUser([FromBody] UserLoginBaseModel request)
        {
            try
            {
                // Check if user already exists
                if (_context.Users.Any(u => u.Email == request.Email))
                    return BadRequest("User with this email already exists.");

                var user = new UserLogin
                {
                    Id = Guid.NewGuid(),
                    Code = request.UserName.Substring(0,2)+ "-01",
                    UserName = request.UserName,
                    Email = request.Email,
                    Phone = request.Phone,
                    DOB = request.DOB,
                    HashPassword = HashPassword(request.HashPassword)
                };

                _context.Users.Add(user);
                await _context.SaveChangesAsync();

                return Ok("User registered successfully.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            
        }

        private string HashPassword(string password)
        {
            using var sha = SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes(password);
            var hash = sha.ComputeHash(bytes);
            return Convert.ToBase64String(hash);
        }

        [HttpPost("login")]
        public async Task<IActionResult> LoginUser([FromBody] LoginRequest request)
        {
            try
            {
                // 1. Find user by email
                var user = await _context.Users
                    .FirstOrDefaultAsync(u => u.Email == request.Email);

                if (user == null)
                    return Unauthorized("User not found");

                // 2. Hash incoming password
                var hashedInputPassword = HashPassword(request.Password);

                // 3. Compare with stored hash
                if (user.HashPassword != hashedInputPassword)
                    return Unauthorized("Invalid password");

                // 4. Generate JWT token
                var token = _authService.GenerateJwtToken(user.Id.ToString(), user.Email);

                return Ok(new { token });
            }
            catch (Exception ex)
            {

                return BadRequest("An error occurred during login. Exception: "+ex);
            }
            
        }


    }
}
