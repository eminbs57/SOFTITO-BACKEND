using System.ComponentModel.DataAnnotations;

namespace MvcProject.Models
{
    public class User
    {
        public int Id { get; set; }

        [Required]
        public string FirstName { get; set; }
        
        [Required]
        public string LastName { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
        
        public int RoleId { get; set; }
        public Role Role { get; set; }
    }
}
