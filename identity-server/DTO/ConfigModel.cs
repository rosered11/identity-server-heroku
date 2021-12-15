using System.Collections.Generic;

namespace IdentityServer
{
    public class ClientSecretRequest
    {
        public string ClientId { get; set; }
        public string Secret { get; set; }
        public string RedirectUrl { get; set; }
        public IEnumerable<string> Scopes { get; set; }
    }
}