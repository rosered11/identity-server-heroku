using System.Collections.Generic;

namespace IdentityServer
{
    public class UserRequest
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Password { get; set; }
        public string Username { get; set; }
        public IEnumerable<string> Roles { get; set; }
    }
}