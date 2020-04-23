using IdentityModel.Client;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using ZapiCore;
namespace Authentication.Controllers
{
    //https://www.cnblogs.com/RainingNight/p/authorization-in-asp-net-core.html
    // 明日计划： 完成认证和授权的建立

    /// <summary>
    /// 获取Token
    /// </summary>
    [Route("api/token")]
    [ApiController]
    public class TokenController : Controller
    {
        /// <summary>
        /// 获取token
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ContentResult> GetTokenResponseAsync()
        {
            var client = new HttpClient();  
            var disco = await client.GetDiscoveryDocumentAsync("https://localhost:5001");
            if (disco.IsError)
            {
                Console.WriteLine(disco.Error);
                return  null;
            }
            var tokenResponse = await client.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest
            {
                Address = disco.TokenEndpoint,
                ClientId = "client",
                ClientSecret = "7fede4cc-a653-4827-acf1-9a1a91ced535", //客户端凭据
                Scope = "api1" //可访问资源
            });
            if (tokenResponse.IsError)
            {
                Console.WriteLine(disco.Error);
                return null;
            }        
            return  Content(JsonHelper.ToJson(tokenResponse));
        }
    }
}
