using ObiletApp.Core.Common;
using ObiletApp.Core.Enums;

namespace ObiletApp.Core.Entities
{
    public class Passenger : BaseEntity
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? IdentityNumber { get; set; } 
        public Gender Gender { get; set; }
    }
}