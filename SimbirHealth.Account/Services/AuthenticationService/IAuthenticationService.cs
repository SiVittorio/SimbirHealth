using SimbirHealth.Account.Models.Requests;

namespace SimbirHealth.Account.Services.AuthenticationService
{
    public interface IAuthenticationService
    {
        Task<IResult> SignUp(SignUpRequest request);
        Task<IResult> SignIn(SignInRequest request);
        Task<IResult> ValidateToken(string token);
        Task<IResult> RefreshToken(string refreshToken);
        Task<IResult> SignOut(string token);
    }
}
