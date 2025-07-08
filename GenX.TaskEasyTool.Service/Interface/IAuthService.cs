using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GenX.TaskEasyTool.Model.DTO_S;

namespace GenX.TaskEasyTool.Service.Interface
{
    public interface IAuthService
    {
    string RegisterAdmin(UserRegisterDto dto);
    string RegisterUser(UserRegisterDto dto, string adminUsername);
    LoginResponseDto Login(UserLoginDto dto);
    string ChangePassword(ChangePasswordDto dto);
    void ForgotPassword(string email);
    VerifyOtpResponseDto VerifyOtp(string otp);
    string ResendOtp(string email);
    void ResetPassword(string email, ResetPasswordDto dto);
    List<UserResponseDto> GetAllUsers();
    UserResponseDto GetUserById(int id);

    }
}
