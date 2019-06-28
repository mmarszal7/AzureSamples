using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using System.Threading.Tasks;

namespace ActiveDirectoryAPIdemo.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly string authority;
        private readonly string appId;
        private readonly string resourceId;

        private readonly string appSecret;

        public AuthController(IConfiguration configuration)
        {
            authority = configuration["AzureAd:Instance"] + configuration["AzureAd:TenantId"];
            appId = configuration["AzureAd:ClientId"];
            resourceId = configuration["AzureAd:ResourceID"];

            appSecret = configuration["AzureAd:ClientSecret"];
        }

        // GET api/auth
        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult<string>> GetToken()
        {
            var authContext = new AuthenticationContext(authority);
            var credential = new ClientCredential(appId, appSecret);
            var authResult = await authContext.AcquireTokenAsync(resourceId, credential);
            var token = authResult.AccessToken;

            return token;
        }
    }
}
