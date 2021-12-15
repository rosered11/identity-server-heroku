using Microsoft.AspNetCore.Identity;

namespace IdentityServer.Database
{
    public class ApplicationUser : IdentityUser
    {
        public string Name { get; set; }
    }
}