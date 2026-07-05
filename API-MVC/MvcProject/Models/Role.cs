using System.ComponentModel.DataAnnotations;

namespace MvcProject.Models
{
    public class Role
    {
        public int Id { get; set; }
        
        [Required]
        public string Name { get; set; }

        public int DepartmentId { get; set; }
        public Department Department { get; set; }
    }
}
