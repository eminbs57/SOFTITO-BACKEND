using Microsoft.AspNetCore.Identity;

namespace PetAdoptionORM.Model
{
    public class AppUser : IdentityUser
    {
        public string FullName { get; set; }
        public string Address { get; set; }
    }
}
