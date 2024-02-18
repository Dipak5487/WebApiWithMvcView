using Core.Models.AccountModel;
using Microsoft.AspNetCore.Identity;

namespace Api.Model
{
    public class AuthenticateResponse
    {
        public SignInResult SignInResult { get; set; }
        public string Id { get; set; }
        public string Username { get; set; }
        public string Token { get; set; }


        public AuthenticateResponse(UserLogInResponse userLogInResponse, string token, SignInResult signInResult)
        {
            Id = userLogInResponse.Id;
            Username = userLogInResponse.Email;
            Token = token;
            SignInResult = signInResult;
        }
    }
}
