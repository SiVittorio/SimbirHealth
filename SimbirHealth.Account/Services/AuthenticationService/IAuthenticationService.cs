using SimbirHealth.Account.Models.Requests;

namespace SimbirHealth.Account.Services.AuthenticationService
{
    public interface IAuthenticationService
    {
        public Task<IResult> SignUp(SignUpRequest request);
        public Task<IResult> SignIn(string username, string password);
    }
}
