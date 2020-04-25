using IdentityModel;
using IdentityServer4;
using IdentityServer4.Models;
using IdentityServer4.Test;
using System.Collections.Generic;
using System.Security.Claims;

namespace Authentication
{
    public class Config
    {
        public static IEnumerable<IdentityResource> GetIdentityResourceResources()
        {
            return new List<IdentityResource>
            {
                new IdentityResources.OpenId(), //必须要添加，否则报无效的scope错误
                new IdentityResources.Profile(),
                new IdentityResources.Email(),
                new IdentityResource
                {
                    Name = "role",
                    UserClaims = new List<string> {"role"}
                }
            };
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<ApiResource> GetResources()
        {
            return new List<ApiResource>
            {
                //new ApiResource { Name = "ImageResource", Scopes ={ new Scope ("ImageResource") }},//Scopes必须配置，否则获取token时返回 invalid_scope                
                //new ApiResource { Name = "FileResourse" },
                new ApiResource ("api1","my api")
            };
        }

        public static IEnumerable<Client> Clients => new List<Client>
        {
            //客户端模式
            new Client
            {
                ClientId = "client",
                AllowedGrantTypes = GrantTypes.ClientCredentials,
                ClientSecrets ={
                      new Secret("secret".Sha256()) //凭证字符串
                },
                AllowedScopes = { "api1" }
            },
            //密码模式（安全级别高）
            new Client
            {
                ClientId = "password_client",
                AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                ClientSecrets ={
                        new Secret("secret".Sha256()) //凭证字符串
                },
                RequireClientSecret = false,
                AllowedScopes = { "api1" }
            },
             new Client
            {
                ClientId = "implicit_client",
                AllowedGrantTypes = GrantTypes.Implicit,
                ClientSecrets ={
                      new Secret("secret".Sha256()) //凭证字符串
                },
                AllowedScopes = { "api1" }
            },
        };


        public static List<TestUser> TestUsers =>

            new List<TestUser>
            {
                            new TestUser
                            {
                                SubjectId = "1",
                                Username = "alice",
                                Password = "password",
                                Claims = new List<Claim>(){

                                    new Claim(JwtClaimTypes.Role,"superadmin"),
                                    new Claim(JwtClaimTypes.Email,"scott@scottbrady91.com"),

                                }
                            },
                            new TestUser
                            {
                                SubjectId = "2",
                                Username = "bob",
                                Password = "password",
                                Claims = new List<Claim>(){new Claim(JwtClaimTypes.Role,"admin") }

                            }

            };

    }
}
