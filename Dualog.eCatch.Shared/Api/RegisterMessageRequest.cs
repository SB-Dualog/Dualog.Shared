namespace Dualog.eCatch.Shared.Api
{
    public class RegisterMessageRequest
    {
        public RegisterMessageRequest(string plainTextNaf, string gzippedSignatureData, string authCode)
        {
            PlainTextNaf = plainTextNaf;
            GzippedSignatureData = gzippedSignatureData;
            AuthCode = authCode;
        }
        public string PlainTextNaf { get; set; }
        public string GzippedSignatureData { get; set; }
        public string AuthCode { get; set; }
    }
}
