using IdentityServer4.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Authentication
{
    public class Config
    {
        public static IEnumerable<ApiResource> Apis =>
      new List<ApiResource>

           {
               new ApiResource("api1", "my api"),
               new ApiResource("inventoryapi", "this is inventory api"),
               new ApiResource("orderapi", "this is order api"),
               new ApiResource("productapi", "this is product api")
           };


        public static IEnumerable<Client> Clients =>
        new List<Client>
        {
            new Client
            {
                ClientId = "client",
                AllowedGrantTypes = GrantTypes.ClientCredentials,
                ClientSecrets =
                {
                    new Secret("7fede4cc-a653-4827-acf1-9a1a91ced535".Sha256()) //凭证字符串
                },
                AllowedScopes = { "api1" }
            },
            new Client
               {
                   ClientId = "inventory",
                   AllowedGrantTypes = GrantTypes.ClientCredentials,

                   ClientSecrets =
                   {
                       new Secret("inventorysecret".Sha256())
                   },

                   AllowedScopes = { "inventoryapi" }
               },
                new Client
               {
                   ClientId = "order",
                   AllowedGrantTypes = GrantTypes.ClientCredentials,

                   ClientSecrets =
                   {
                       new Secret("ordersecret".Sha256())
                   },

                   AllowedScopes = { "orderapi" }
               },
                new Client
               {
                   ClientId = "product",
                   AllowedGrantTypes = GrantTypes.ClientCredentials,

                   ClientSecrets =
                   {
                       new Secret("productsecret".Sha256())
                   },

                   AllowedScopes = { "productapi" }
               }
        };
    }
}
