using ChineseSaleApi.Dto;
using ChineseSaleApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;

namespace ChineseSaleApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class AuthController : ControllerBase
    {

        private readonly IUserService _service;
        private readonly ILogger<AuthController> _logger;
        public AuthController(IUserService service, ILogger<AuthController> logger)
        {
            _service = service;
            _logger = logger;
        }


        //POST /api/Auth/Register
        [HttpPost]
        [Route("Register")]
        public async Task<ActionResult<BuyerDetailDto>> Register([FromBody] UserDto CreateUser)
        {
            try
            {
                _logger.LogInformation("Registering user with email: {Email}", CreateUser.UserName);
                var user = await _service.CreateUserAsync(CreateUser);
                return Ok(user);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(500, "cannot register user");
            }
        }

        //POST /api/Auth/Login
        [HttpPost]
        [Route("Login")]
        public async Task<ActionResult<LoginResponseDto>> Login([FromBody] LoginRequestDto loginDto)
        {
            try
            {
                var loginResponse = await _service.AuthenticateAsync(loginDto);
                return Ok(loginResponse);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(500, "cannot login user");
            }
        }



    }
}


