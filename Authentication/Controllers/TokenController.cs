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
    //https://www.cnblogs.com/RainingNight/p/authorization-in-asp-net-core.html
    // 明日计划： 完成认证和授权的建立

    /// <summary>
    /// 获取Token
    /// </summary>
    [Route("api/token")]
    [ApiController]
    public class TokenController : Controller
    {

        //private readonly TestUserStore _userStore;
        //TokenController()//TestUserStore userStore)
        //{
        //    //_userStore = userStore;
        
        
        //}






        /// <summary>
        /// 客户端模式获取token
        /// </summary>
        /// <returns></returns>
        [HttpGet("client")]
        public async Task<ContentResult> ClientTokenResponseAsync()
        {
            var client = new HttpClient();
            var disco = await client.GetDiscoveryDocumentAsync("http://localhost:5000");
            if (disco.IsError)
            {
                Console.WriteLine(disco.Error);
                return null;
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
                Console.WriteLine(disco.Error);
                return null;
            }
            return Content(JsonHelper.ToJson(tokenResponse.Json));
        }


        /// <summary>
        /// 密码模式获取token
        /// </summary>
        /// <returns></returns>
        [HttpGet("password")]
        public async Task<ContentResult> PasswordTokenResponseAsync(string name,string pwd)
        {
            var client = new HttpClient();
            var disco = await client.GetDiscoveryDocumentAsync("http://localhost:5000");
            if (disco.IsError)
            {
                Console.WriteLine(disco.Error);
                return null;
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
                Console.WriteLine(disco.Error);
                return null;
            }
            return Content(JsonHelper.ToJson(tokenResponse.Json));
        }


        ///// <summary>
        ///// 获取Token
        ///// </summary>
        ///// <param name="userDto"></param>
        ///// <returns></returns>
        //[HttpGet("authenticate")]
        //public async Task<ContentResult> Authenticate()
        //{

        //    var user = FindUser("alice", "alice");
        //    if (user == null) return Content("用户名或者密码错误!");
        //    var tokenHandler = new JwtSecurityTokenHandler();
        //    var key = Encoding.ASCII.GetBytes(Secret);
        //    var authTime = DateTime.UtcNow;
        //    var expiresAt = authTime.AddDays(7);
        //    var tokenDescriptor = new SecurityTokenDescriptor
        //    {
        //        Subject = new ClaimsIdentity(new Claim[]
        //        {
        //            new Claim(JwtClaimTypes.Audience,"api1"),
        //            new Claim(JwtClaimTypes.Issuer,"https://localhost:5001"),
        //            new Claim(JwtClaimTypes.Id, user.Id.ToString()),
        //            new Claim(JwtClaimTypes.Name, user.Name),
        //            new Claim(JwtClaimTypes.Email, user.Email),
        //            new Claim(JwtClaimTypes.PhoneNumber, user.PhoneNumber)
        //        }),
        //        Expires = expiresAt,
        //        SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        //    };
        //    var token = tokenHandler.CreateToken(tokenDescriptor);
        //    var tokenString = tokenHandler.WriteToken(token);
        //    var reslut = JsonHelper.ToJson(new
        //    {
        //        access_token = tokenString,
        //        token_type = "Bearer",
        //        profile = new
        //        {
        //            sid = user.Id,
        //            name = user.Name,
        //            auth_time = new DateTimeOffset(authTime).ToUnixTimeSeconds(),
        //            expires_at = new DateTimeOffset(expiresAt).ToUnixTimeSeconds()
        //        }
        //    });
        //    return Content(reslut);

        //}


        //public User FindUser(string userName, string password)
        //{
        //    return UserStore._users.FirstOrDefault(_ => _.Name == userName && _.Password == password);
        //}
    }
    public class UserDto
    {
        public string UserName { get; set; }

        public string Password { get; set; }
    }
    public class UserStore
    {
            public static List<User> _users = new List<User>() {
                    new User {  Id=1, Name="alice", Password="alice", Email="alice@gmail.com", PhoneNumber="18800000001", Birthday=DateTime.Now },
                    new User {  Id=1, Name="bob", Password="bob", Email="bob@gmail.com", PhoneNumber="18800000002", Birthday=DateTime.Now.AddDays(1) }
            };

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