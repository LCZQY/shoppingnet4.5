using IdentityModel;
using IdentityModel.Client;
using IdentityServer4.Test;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using ZapiCore;
namespace Authentication.Controllers
{

    /// <summary>
    /// 获取Token
    /// </summary>
    [Route("api/token")]
    [ApiController]
    public class TokenController : Controller
    {


        /// <summary>
        /// 客户端模式获取token
        /// </summary>
        /// <returns></returns>
        [HttpGet("client")]
        public async Task<ResponseMessage<TokenReponse>> ClientTokenResponseAsync()
        {
            var response = new ResponseMessage<TokenReponse>() { Extension = new TokenReponse (){ } };

            var client = new HttpClient();
            var disco = await client.GetDiscoveryDocumentAsync("https://localhost:5000");
            if (disco.IsError)
            {
                response.Code = ResponseCodeDefines.ServiceError;
                response.Message = "访问认证中心失败";
                return response;
            }
            var clientrequest = new ClientCredentialsTokenRequest
            {
                Address = disco.TokenEndpoint,
                ClientId = "client",
                ClientSecret = "secret", //客户端凭据
                Scope = "api1" //可访问资源
            };
            var tokenResponse = await client.RequestClientCredentialsTokenAsync(clientrequest);
            if (tokenResponse.IsError)
            {
                response.Code = ResponseCodeDefines.ServiceError;
                response.Message = "获取Token失败";
                return response;
            }
            response.Extension = tokenResponse.Json.ToObject<TokenReponse>();
            return response;
        }


        /// <summary>
        /// 密码模式获取token
        /// </summary>
        /// <returns></returns>
        [HttpGet("password")]
        public async Task<ResponseMessage<TokenReponse>> PasswordTokenResponseAsync(string name, string pwd)
        {
            var response = new ResponseMessage<TokenReponse>() { Extension = null };
            var client = new HttpClient();
            var disco = await client.GetDiscoveryDocumentAsync("https://localhost:5000");
            if (disco.IsError)
            {
                response.Code = ResponseCodeDefines.ServiceError;
                response.Message = "访问认证中心失败";
                return response;
            }
            var psswordRequst = new PasswordTokenRequest
            {
                UserName = name,
                Password = pwd,
                Address = disco.TokenEndpoint,
                ClientId = "password_client",
                ClientSecret = "secret", //客户端凭据
                Scope = "api1" //可访问资源
            };
            var tokenResponse = await client.RequestPasswordTokenAsync(psswordRequst);
            if (tokenResponse.IsError)
            {
                response.Code = ResponseCodeDefines.ServiceError;
                response.Message = tokenResponse.ErrorDescription == "user_password_error" ? "密码或者用户名错误" : "获取Token失败";                
                return response;
            }
            response.Extension = tokenResponse.Json.ToObject<TokenReponse>();
            return response;
        }
    }

    public class User
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        public string Password { get; set; }

        public DateTime Birthday { get; set; }
    }
}