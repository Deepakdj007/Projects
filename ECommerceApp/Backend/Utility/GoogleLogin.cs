using Google.Apis.Auth;
using Microsoft.Extensions.Configuration;
using Google.Apis.Auth;

namespace Backend.Utility
{
    public class GoogleLogin
    {
        private readonly IConfiguration _configuration;
        public GoogleLogin(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        // Google token verification
        public async Task<GoogleJsonWebSignature.Payload> VerifyGoogleToken(string tokenId)
        {
            var googleSettings = _configuration.GetSection("GoogleSettings");
            var clientId = googleSettings["ClientId"]; // Get your client ID from appsettings.json

            var validPayload = await GoogleJsonWebSignature.ValidateAsync(tokenId, new GoogleJsonWebSignature.ValidationSettings
            {
                Audience = new List<string> { clientId }
            });

            return validPayload;
        }
    }
}
