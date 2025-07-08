using GenX.TaskEasyTool.Model.DTO_S;
using GenX.TaskEasyTool.Service.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GenX.TaskEasyTool.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
        public class AuthController : ControllerBase
        {
            private readonly IAuthService _service;
        public AuthController(IAuthService service) { _service = service; }

            [HttpPost("admin-register")]
            [AllowAnonymous]
            public IActionResult RegisterAdmin(UserRegisterDto dto)
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState); 

                var result = _service.RegisterAdmin(dto);
                return Ok(result);
            }


            [HttpPost("register-user")]
            [Authorize(Roles = "Admin")]
            public IActionResult RegisterUser(UserRegisterDto dto)
            {
                try
                {
                    var registerMessage = _service.RegisterUser(dto, User.Identity.Name);

                    var loginDto = new UserLoginDto
                    {
                        Username = dto.Username,
                        Password = dto.Password
                    };

                    var result = _service.Login(loginDto); // returns LoginResponseDto
                    return Ok(result);
                }
                catch (Exception ex)
                {
                    return BadRequest(new { message = ex.Message });
                }
            }

            [HttpPost("login")]
            [AllowAnonymous]
            public IActionResult Login(UserLoginDto dto)
            {
                {
                    try
                    {
                        var result = _service.Login(dto); 
                        return Ok(result); //  { id, username, token }
                    }
                    catch (Exception ex)
                    {
                        return BadRequest(new { message = ex.Message });
                    }
                }
            }

            [HttpPost("change-password")]
            [Authorize]
            public IActionResult ChangePassword(ChangePasswordDto dto)
            {
                return Ok(_service.ChangePassword(dto));
            }
                

            [HttpPost("forgot-password")]
            public IActionResult ForgotPassword(ForgotPasswordRequestDto dto)
            {
                try
                {
                    _service.ForgotPassword(dto.Email);
                    return Ok("OTP sent to your registered email.");
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }

        [HttpPost("verify-otp")]
        [AllowAnonymous]
        public IActionResult VerifyOtp([FromBody] VerifyOtpDto dto)
        {
            var result = _service.VerifyOtp(dto.Otp);
            if (result.IsSuccess)
                return Ok(new { message = result.Message });

            return BadRequest(new { message = result.Message });
        }

        [HttpPost("resend-otp")]
        [AllowAnonymous]
        public IActionResult ResendOtp([FromQuery] string email)
        {
            var result = _service.ResendOtp(email);
            return Ok(new { message = result });
        }

        [HttpPost("reset-password")]
        [AllowAnonymous]
        public IActionResult ResetPassword([FromQuery] string email, [FromBody] ResetPasswordDto dto)
        {
            try
            {
                _service.ResetPassword(email, dto);
                return Ok(new { message = "Password reset successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
        [HttpGet]
        public IActionResult GetAll()
        {
            var users = _service.GetAllUsers();
            return Ok(users);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var user = _service.GetUserById(id);
            if (user == null) return NotFound("User not found");
            return Ok(user);
        }


    }
}

