using Microsoft.AspNetCore.Identity;

namespace projectcodefirst.Models
{
    public class Users : IdentityUser
    {
        public string FullName { get; set; }
    }
}
