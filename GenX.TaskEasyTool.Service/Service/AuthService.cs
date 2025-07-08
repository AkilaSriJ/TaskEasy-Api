using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using GenX.TaskEasyTool.Data.Interface;
using GenX.TaskEasyTool.Data.Models;
using GenX.TaskEasyTool.Model.DTO_S;
using GenX.TaskEasyTool.Service.Interface;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using GenX.TaskEasyTool.Service.JwtToken;

namespace GenX.TaskEasyTool.Service.Service
{
    public class AuthService:IAuthService
    {
        private readonly IUserRepository _repo;
        private readonly IConfiguration _config;
        private readonly IEmailService _emailService;
        private readonly JwtService _jwtService;

        public AuthService(IUserRepository repo, IConfiguration config, IEmailService emailService,JwtService jwtService)
        {
            _repo = repo;
            _config = config;
            _emailService = emailService;
            _jwtService = jwtService;
        }

        private string Hash(string password)
        {
           return BCrypt.Net.BCrypt.HashPassword(password);
        }
        private bool Verify(string password, string hash)
        {
           return BCrypt.Net.BCrypt.Verify(password, hash);
        }
            
        private string GenerateOtp()
        {
            return new Random().Next(100000, 999999).ToString(); 
        }

        public string RegisterAdmin(UserRegisterDto dto)
        {
            var existingAdmin = _repo.GetAll().Any(u => u.Role == "Admin");
            if (existingAdmin)
                return "Admin already exists. Use login instead.";

            if (dto.Role != "Admin")
                return "Only role allowed for this endpoint is 'Admin'.";

            var user = new User
            {
                Username = dto.Username,
                PasswordHash = Hash(dto.Password),
                Email = dto.Email,
                Role = dto.Role
            };

            _repo.Add(user);
            return "Admin registered successfully";
        }

        public string RegisterUser(UserRegisterDto dto, string adminUsername)
        {
            var admin = _repo.GetByUsername(adminUsername);
            if (admin == null || admin.Role != "Admin")
                return "Only admin can register users.";

            if (dto.Role == "Admin")
                return "Only the first Admin can be registered manually.";

            var user = new User
            {
                Username = dto.Username,
                PasswordHash = Hash(dto.Password),
                Email = dto.Email,
                Role = "User"
            };

            _repo.Add(user);
            return $"{dto.Role} registered successfully";
        }

        public LoginResponseDto Login(UserLoginDto dto)
        {
            var user = _repo.GetByUsername(dto.Username);
            if (user == null || !Verify(dto.Password, user.PasswordHash))
                throw new Exception("Invalid credentials Either username or password entered is wrong");

            var token = _jwtService.GenerateToken(user);

            return new LoginResponseDto
            {
                Token = token,
                Id = user.Id,
                Username = user.Username

            };
        }

        public string ChangePassword(ChangePasswordDto dto)
        {
            var user = _repo.GetByUsername(dto.Username);
            if (user == null || !Verify(dto.OldPassword, user.PasswordHash))
                return "Invalid credentials";

            user.PasswordHash = Hash(dto.NewPassword);
            _repo.Update(user);
            return "Password changed successfully";
        }
        public void ForgotPassword(string email)
        {
            var user = _repo.GetByEmail(email);
            if (user == null) throw new Exception("User not found");

            var otp = new Random().Next(100000, 999999).ToString();
            user.OtpCode = otp;
            user.OtpExpiryTime = DateTime.UtcNow.AddMinutes(5);
            _repo.Update(user);

            _emailService.Send(user.Email, "Password Reset OTP", $"Your OTP is: {otp}");
        }

        public VerifyOtpResponseDto VerifyOtp(string otp)
        {
            var user = _repo.GetAll().FirstOrDefault(u => u.OtpCode == otp);

            if (user == null)
                return new VerifyOtpResponseDto { IsSuccess = false, Message = "Invalid OTP" };

            if (user.OtpExpiryTime < DateTime.UtcNow)
                return new VerifyOtpResponseDto { IsSuccess = false, Message = "OTP expired" };

            return new VerifyOtpResponseDto { IsSuccess = true, Message = "OTP is valid" };
        }

        public string ResendOtp(string email)
        {
            var user = _repo.GetByEmail(email);
            if (user == null)
                return "User not found";

            var newOtp = GenerateOtp();
            var expiry = DateTime.UtcNow.AddMinutes(5);

            user.OtpCode = newOtp;
            user.OtpExpiryTime = expiry;
            _repo.Update(user);

            _emailService.Send(email, "Your OTP", $"Your new OTP is: {newOtp}");

            return "OTP resent to your email";
        }
        public void ResetPassword(string email, ResetPasswordDto dto)
        {
            var user = _repo.GetByEmail(email);
            if (user == null)
                throw new Exception("User not found");

            if (user.OtpCode == null || user.OtpExpiryTime < DateTime.UtcNow)
                throw new Exception("OTP not verified or expired");

            if (dto.NewPassword != dto.ConfirmPassword)
                throw new Exception("Passwords do not match");

            user.PasswordHash = Hash(dto.NewPassword);
            user.OtpCode = null;
            user.OtpExpiryTime = null;

            _repo.Update(user);
        }
        public List<UserResponseDto> GetAllUsers()
        {
            var users = _repo.GetAll();

            return users.Select(u => new UserResponseDto
            {
                Id = u.Id,
                Username = u.Username,
                Role = u.Role,
                Email = u.Email
            }).ToList();
        }

        public UserResponseDto GetUserById(int id)
        {
            var user = _repo.GetById(id);
            if (user == null) return null;

            return new UserResponseDto
            {
                Id = user.Id,
                Username = user.Username,
                Role = user.Role,
                Email = user.Email
            };
        }

    }
}
