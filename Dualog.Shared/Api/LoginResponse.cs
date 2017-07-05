using Dualog.Shared.Extensions;

namespace Dualog.Shared.Api
{
    public class LoginResponse
    {
        public string Error { get; set; }
        public string Error_Description { get; set; }
        public string Access_Token { get; set; }
        public bool IsLoggedIn => !Access_Token.IsNullOrEmpty();
    }
}
