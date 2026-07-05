using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MvcProject.Models
{
    public class Department
    {
        public int Id { get; set; }
        
        [Display(Name = "Departman Adı")]
        public string Name { get; set; } = string.Empty;

        public ICollection<Role> Roles { get; set; } = new List<Role>();
    }
}
