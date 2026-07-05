using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace EventManagement.Models
{
    public class AppUser : IdentityUser<int>
    {
        // Navigation property for events the user has joined
        public virtual ICollection<Event> Events { get; set; } = new List<Event>();
    }
}