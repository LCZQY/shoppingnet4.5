using IdentityServer4;
using IdentityServer4.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OAuth2IdentityServer.OAuth2
{
    public class Clients
    {

        /// <summary>
        /// 授权模式：客户端模式 GrantTypes.ClientCredentials
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<Client> GetClients()
        {
            return new List<Client>
            {
                //client credentials client（OK）
                new Client
                {
                    ClientId = "ClientIdOK",
                    AllowedGrantTypes = GrantTypes.ClientCredentials,//授权模式：客户端模式
                    AllowedScopes = { "ImageResource","Api" }, //允许访问的资源 GetResources()中配置的
                    ClientSecrets = { new Secret { Value= "ClientSecret".Sha256(), Expiration = DateTime.Now.AddMinutes(5)} }
                },
                //client credentials client（OK）
                new Client
                {
                    ClientId = "client1",
                    ClientName = "Example Client Credentials Client Application",
                    AllowedGrantTypes = GrantTypes.ClientCredentials,//授权模式：客户端模式
                    AllowedScopes = //对应的是 “OAuth2.Config.GetApiResources()” 或 “OAuth2.Config.GetResources()”中的资源
                    {
                        "ImageResource", "Api",
                        //IdentityServerConstants.StandardScopes.OpenId, //加上此参数报错
                        //IdentityServerConstants.StandardScopes.Profile,//加上此参数报错
                        //IdentityServerConstants.StandardScopes.OfflineAccess,//加上此参数报错
                    },
                    ClientSecrets = { new Secret("181a7853053b4be9b0ac9d9c709f3ecd".Sha256()) },
                    AllowedCorsOrigins = new List<string> { "http://localhost:44389" },
                    RedirectUris = { "http://localhost:6321/Home/AuthCode" },//登录成功重定向地址
                    PostLogoutRedirectUris = { "http://localhost:44389/" },//退出重定向地址
                    AccessTokenLifetime = 3600, //AccessToken过期时间， in seconds (defaults to 3600 seconds / 1 hour)
                    AuthorizationCodeLifetime = 300,  //设置AuthorizationCode的有效时间，in seconds (defaults to 300 seconds / 5 minutes)
                    AbsoluteRefreshTokenLifetime = 2592000,  //RefreshToken的最大过期时间，in seconds. Defaults to 2592000 seconds / 30 day
                },
                // resource owner password grant client
                new Client
                {
                    ClientId = "client2",
                    ClientName = "Example Client Credentials Client Application",
                    AccessTokenType = AccessTokenType.Jwt,
                    //AccessTokenType = AccessTokenType.Reference,
                    AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                    ClientSecrets = { new Secret("ClientSecret".Sha256()) },
                    AllowedCorsOrigins = new List<string> { "http://localhost:44389" },
                    RefreshTokenExpiration = TokenExpiration.Sliding,
                    SlidingRefreshTokenLifetime = 5*60,
                    UpdateAccessTokenClaimsOnRefresh = true,
                    AllowOfflineAccess = true, //刷新令牌来进行长时间的API访问
                    AllowedScopes = new List<string>
                    {
                        "ImageResource",
                        "Api",
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.OfflineAccess,
                    },
                    RedirectUris = { "http://localhost:6321/Home/AuthCode" },
                    PostLogoutRedirectUris = { "http://localhost:6321/" },
                    AccessTokenLifetime = 3600, //AccessToken过期时间， in seconds (defaults to 3600 seconds / 1 hour)
                    AuthorizationCodeLifetime = 300,  //设置AuthorizationCode的有效时间，in seconds (defaults to 300 seconds / 5 minutes)
                    AbsoluteRefreshTokenLifetime = 2592000,  //RefreshToken的最大过期时间，in seconds. Defaults to 2592000 seconds / 30 day
                },
                //Implicit
                new Client
                {
                    ClientId = "openIdConnectClient",
                    ClientName = "Example Implicit Client Application",
                    AllowedGrantTypes = GrantTypes.Implicit,
                    AllowedScopes = new List<string>
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.Email,
                        "role",
                        "customAPI.write"
                    },
                    RedirectUris = new List<string> {"https://localhost:44330/signin-oidc"},
                    PostLogoutRedirectUris = new List<string> {"https://localhost:44330"},
                    AccessTokenLifetime = 3600, //AccessToken过期时间， in seconds (defaults to 3600 seconds / 1 hour)
                    AuthorizationCodeLifetime = 300,  //设置AuthorizationCode的有效时间，in seconds (defaults to 300 seconds / 5 minutes)
                    AbsoluteRefreshTokenLifetime = 2592000,  //RefreshToken的最大过期时间，in seconds. Defaults to 2592000 seconds / 30 day
                }
            };
        }

        public static IEnumerable<Client> GetClientsTest()
        {
            return new List<Client>
            {
                new Client
                {
                    ClientId = "ClientId",
                    AllowedGrantTypes = GrantTypes.ClientCredentials,//授权模式：客户端模式
                    AllowedScopes ={ "ImageResource","Api" }, //允许访问的资源 GetResources()中配置的
                    ClientSecrets ={ new Secret { Value= "ClientSecret".Sha256(), Expiration=DateTime.Now.AddMinutes(5)} }
                }
            };
        }
    }
}
