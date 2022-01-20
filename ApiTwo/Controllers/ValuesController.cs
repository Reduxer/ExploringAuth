using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Net.Http;
using IdentityModel.Client;
using System.Threading.Tasks;

namespace ApiTwo.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public ValuesController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IActionResult> Index()
        {
            var identityClient = _httpClientFactory.CreateClient();
            var discoveyDocument = await identityClient.GetDiscoveryDocumentAsync("https://localhost:44365");

            var tokenRequest = new ClientCredentialsTokenRequest()
            {
                Address = discoveyDocument.TokenEndpoint,
                ClientId = "#001",
                ClientSecret = "#001",
                Scope = "ApiOne",
            };

            var tokenResponse = await identityClient.RequestClientCredentialsTokenAsync(tokenRequest);

            var apiOneClient = _httpClientFactory.CreateClient();
            apiOneClient.SetBearerToken(tokenResponse.AccessToken);

            var response = await apiOneClient.GetStringAsync("https://localhost:44391/weatherforecast");

            return Ok(response);
        }
    }
}
