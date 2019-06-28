using Microsoft.IdentityModel.Clients.ActiveDirectory;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace ApiConsumer
{
    public class ApiClient
    {
        // Active Directory
        private readonly string authority = "AzureAd:Instance" + "AzureAd:TenantId";
        private readonly string appId = "AzureAd:ClientId";
        private readonly string appSecret = "AzureAd:ClientSecret";

        // API
        private readonly string resourceId = "Api:ResourceId";
        private readonly string baseUrl = "Api:BaseUrl";

        private readonly HttpClient client;
        private bool tokenSet = false;

        public ApiClient(HttpClient client)
        {
            this.client = client;
            this.client.BaseAddress = new Uri(baseUrl);
        }

        public async Task SetToken()
        {
            if (!tokenSet)
            {
                var authContext = new AuthenticationContext(authority);
                var credential = new ClientCredential(appId, appSecret);
                var authResult = await authContext.AcquireTokenAsync(resourceId, credential);
                var token = authResult.AccessToken;

                client.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", token);

                tokenSet = true;
            }
        }

        public async Task<string> GetValues()
        {
            await SetToken();

            var response = await client.GetAsync("/api/values");
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"{response.StatusCode}");
            }

            var content = await response.Content.ReadAsStringAsync();
            return content;
        }
    }
}
