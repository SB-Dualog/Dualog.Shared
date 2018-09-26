namespace Dualog.eCatch.Shared.Api
{
    public class LoginRequest
    {
        public LoginRequest() { }

        public LoginRequest(string userName, string password, string grantType = "password")
        {
            UserName = userName;
            Password = password;
            Grant_Type = grantType;
        }
        public string UserName { get; set; }
        public string Password { get; set; }

        public string Grant_Type { get; set; }
    }
}
