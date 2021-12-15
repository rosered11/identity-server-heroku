using System.Collections.Generic;
using System.Linq;
using IdentityServer4;
using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Mappers;
using IdentityServer4.Models;
using Microsoft.AspNetCore.Mvc;

namespace IdentityServer
{
    [Route("api/[controller]")]
    [ApiController]
    public class IdentityController : ControllerBase
    {
        private readonly ConfigurationDbContext _cofigContext;
        public IdentityController(ConfigurationDbContext cofigContext)
        {
            _cofigContext = cofigContext;
        }

        [HttpPost("ChangeClientSecret")]
        public IActionResult ChangeSecret([FromBody] ClientSecretRequest request)
        {
            var client = _cofigContext.Clients.SingleOrDefault(x => x.ClientId == request.ClientId);
            if(client != null) 
            {
                _cofigContext.Clients.Remove(client);
                _cofigContext.SaveChanges();
            }
            
            var clientModel = new Client{
                    ClientId = request.ClientId,
                    
                    AllowedGrantTypes = new List<string> { GrantType.AuthorizationCode },

                    // If not set will be error 'Code challenge required'
                    RequirePkce = false,

                    ClientSecrets = {
                        new Secret(request.Secret.Sha256())
                    },
                    RedirectUris = new List<string>(){
                        request.RedirectUrl
                    },
                    AllowedScopes = request.Scopes.ToList()
                };
            
            
            client = clientModel.ToEntity();
            _cofigContext.Clients.Add(client);
            _cofigContext.SaveChanges();

            return NoContent();
        }
    }
}