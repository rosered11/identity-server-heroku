using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using IdentityModel;
using IdentityServer.Database;
using Microsoft.AspNetCore.Identity;

namespace IdentityServer
{
    public class AccountService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AccountService(UserManager<ApplicationUser> userManager
            , RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<IdentityResult> Create(UserRequest user)
        {
            var oldUser = await _userManager.FindByNameAsync(user.Username);
            await Delete(user.Username);
            
            var applicationUser = new ApplicationUser{ UserName = user.Username, Email = $"{user.FirstName}@demo.test", Name = $"{user.FirstName} {user.LastName}"  };
            var result = await _userManager.CreateAsync(applicationUser, user.Password);

            if(result.Succeeded){
                // Setup claims
                var claims = new List<Claim>{
                        new Claim(JwtClaimTypes.Name, applicationUser.Name),
                        new Claim(JwtClaimTypes.GivenName, user.FirstName),
                        new Claim(JwtClaimTypes.FamilyName, user.LastName),
                        new Claim(JwtClaimTypes.Email, applicationUser.Email),
                        new Claim(JwtClaimTypes.EmailVerified, "true", ClaimValueTypes.Boolean),
                        new Claim(JwtClaimTypes.WebSite, "http://demo.demo"),
                        new Claim(JwtClaimTypes.Address, "my address")
                    };
                if(user.Roles != null){
                    foreach(var role in user.Roles){
                        // Check and create role
                        if(!await _roleManager.RoleExistsAsync(role))
                        {
                            await _roleManager.CreateAsync(new IdentityRole(role));
                        }
                        claims.Add(new Claim(JwtClaimTypes.Role, role));
                    }
                    await _userManager.AddToRolesAsync(applicationUser, user.Roles);
                }
                await _userManager.AddClaimsAsync(applicationUser, claims);
            }

            return result;
        }

        public async Task Delete(string username, string provider = null)
        {
            ApplicationUser currentUser;
            if (provider == null)
            {
                currentUser = await _userManager.FindByNameAsync(username);
            }
            else
            {
                currentUser = await _userManager.FindByNameAsync($"{provider}_{username}");
            }

            if(currentUser != null)
            {
                await _userManager.DeleteAsync(currentUser);
            }
        }
    }
}