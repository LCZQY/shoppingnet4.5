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

        //public static IEnumerable<IdentityResource> IdentityResources => new List<IdentityResource>
        //    {
        //        new IdentityResources.OpenId(),
        //        new IdentityResources.Profile(),
        //        new IdentityResources.Email(),
        //    };


        public static IEnumerable<ApiResource> Apis =>
      new List<ApiResource>

           {
               new ApiResource("api1", "my api",new List<string>(){JwtClaimTypes.Role}),
               new ApiResource("inventoryapi", "this is inventory api"),
               new ApiResource("orderapi", "this is order api"),
               new ApiResource("productapi", "this is product api")
           };


        public static IEnumerable<Client> Clients => new List<Client>
        {
            new Client
            {
                ClientId = "client",
                //AllowedGrantTypes = GrantTypes.ClientCredentials,
                AllowedGrantTypes = GrantTypes.ResourceOwnerPasswordAndClientCredentials,
                ClientSecrets =
                {
                    new Secret("7fede4cc-a653-4827-acf1-9a1a91ced535".Sha256()) //凭证字符串
                },
                AllowedScopes = { "api1" }
            },
           
        };
        public static List<TestUser> Users =>

            new List<TestUser>
            {
                            new TestUser
                            {
                                SubjectId = "1",
                                Username = "alice",
                                Password = "password",
                                Claims = new List<Claim>(){new Claim(JwtClaimTypes.Role,"superadmin") }
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
