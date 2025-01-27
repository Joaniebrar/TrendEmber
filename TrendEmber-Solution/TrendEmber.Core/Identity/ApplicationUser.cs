using Microsoft.AspNetCore.Identity;

namespace TrendEmber.Core.Identity
{
    public class ApplicationUser : IdentityUser
    {
        public ApplicationUser(string fullName)
        {
            FullName = fullName;
        }

        public string FullName { get; private set; }
    }
}
