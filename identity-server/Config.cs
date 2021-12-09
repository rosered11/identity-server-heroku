using System;
using System.Collections.Generic;
using System.Security.Claims;
using IdentityModel;
using IdentityServer4;
using IdentityServer4.Models;
using IdentityServer4.Test;
using Microsoft.Extensions.Configuration;

namespace identity_server
{
    public class Config
    {
        public static IEnumerable<Client> Clients(IConfiguration config) =>
            new Client[]
            {
                // Machine to Machine
                new Client{
                    ClientId = "defaultClientOpenId",
                    //AllowedGrantTypes = GrantTypes.ClientCredentials,
                    AllowedGrantTypes = new List<string> { GrantType.AuthorizationCode },

                    // If not set will be error 'Code challenge required'
                    RequirePkce = false,

                    ClientSecrets = {
                        new Secret(config.GetValue<string>("Api:Secret").Sha256())
                    },
                    RedirectUris = new List<string>(){
                        config.GetValue<string>("Api:Redirect")
                    },
                    AllowedScopes = { 
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        "defaultApi" 
                    }
                },
                new Client{
                    ClientId = "defaultClient",
                    
                    // These grant types the client credentials cann't get user profile,
                    // If you need to userinfo from use login
                    // You should choose openid flow
                    AllowedGrantTypes = GrantTypes.ClientCredentials,

                    ClientSecrets = {
                        new Secret(config.GetValue<string>("Api:Secret").Sha256())
                    },
                    AllowedScopes = {
                        "defaultApi" 
                    }
                },

                // OpenId for authen user, and Oauth2 authen protect resource
                new Client{
                    ClientId = "flutterClient",
                    ClientName = "Flutter Client",
                    AllowedGrantTypes = new List<string> { GrantType.AuthorizationCode },
                    AllowRememberConsent = false,
                    RequirePkce = false,
                    RequireClientSecret = false,
                    RedirectUris = new List<string>(){
                        config.GetValue<string>("Mobile:Redirect")
                    },
                    AllowedScopes = new List<string>{
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.Address,
                        IdentityServerConstants.StandardScopes.Email,
                        "defaultApi",
                        "roles"
                    }
                },
                new Client{
                    ClientId = "default3Client",
                    ClientName = "MVC Web Client",
                    AllowedGrantTypes = GrantTypes.Hybrid,
                    RequirePkce = false,
                    AllowRememberConsent = false,
                    RedirectUris = new List<string>(){
                        "https://localhost:5003/signin-oidc"
                    },
                    PostLogoutRedirectUris = new List<string>(){
                        "https://localhost:5003/signout-callback-oidc"
                    },
                    ClientSecrets = new List<Secret>{
                        new Secret(config.GetValue<string>("Mvc:Secret").Sha256())
                    },
                    AllowedScopes = new List<string>{
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.Address,
                        IdentityServerConstants.StandardScopes.Email,
                        "defaultApi",
                        "roles"
                    }
                }
            };

        public static IEnumerable<ApiScope> ApiScopes =>
            new ApiScope[]
            {
                // protech client api
                new ApiScope("defaultApi", "Default API")
            };
        
        public static IEnumerable<ApiResource> ApiResources =>
            new ApiResource[]
            {

            };

        public static IEnumerable<IdentityResource> IdentityResources =>
            new IdentityResource[]
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                new IdentityResources.Address(),
                new IdentityResources.Email(),
                new IdentityResource(
                    "roles",
                    "Your role(s)",
                    new List<string>() { "role" }
                )
            };
        
        public static List<TestUser> TestUsers(IConfiguration config) =>
            new List<TestUser>
            {
                new TestUser{
                    SubjectId = "4DEE5D0B-2A9C-479A-9D77-8672A5A193A4",
                    Username = config.GetValue<string>("Rosered:User"),
                    Password = config.GetValue<string>("Rosered:Pass"),
                    Claims = new List<Claim>{
                        new Claim(JwtClaimTypes.Name, "rosered strauss"),
                        new Claim(JwtClaimTypes.GivenName, "rosered"),
                        new Claim(JwtClaimTypes.FamilyName, "strauss"),
                        new Claim(JwtClaimTypes.Email, "demo@demo.test"),
                        new Claim(JwtClaimTypes.EmailVerified, "true", ClaimValueTypes.Boolean),
                        new Claim(JwtClaimTypes.WebSite, "http://demo.demo"),
                        new Claim(JwtClaimTypes.Address, "my address"),
                        new Claim(JwtClaimTypes.Role, "user")
                    }
                },
                new TestUser{
                    SubjectId = "5F82AF0D-7ACB-4656-9A04-04B7838A59EF",
                    Username = config.GetValue<string>("Maruko:User"),
                    Password = config.GetValue<string>("Maruko:Pass"),
                    Claims = new List<Claim>{
                        new Claim(JwtClaimTypes.Name, "maruko arlale"),
                        new Claim(JwtClaimTypes.GivenName, "maruko"),
                        new Claim(JwtClaimTypes.FamilyName, "arlale"),
                        new Claim(JwtClaimTypes.Email, "maruko@maruko.maruko"),
                        new Claim(JwtClaimTypes.EmailVerified, "true", ClaimValueTypes.Boolean),
                        new Claim(JwtClaimTypes.WebSite, "http://maruko.maruko"),
                        new Claim(JwtClaimTypes.Address, "my address"),
                        new Claim(JwtClaimTypes.Role, "user")
                    }
                }
            };
    }
}