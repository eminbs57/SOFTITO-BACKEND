using System.ComponentModel.DataAnnotations.Schema;
using ObiletApp.Core.Common;

namespace ObiletApp.Core.Entities
{
    public class Review : BaseEntity
    {
        public int TripId { get; set; }
        public string? UserId { get; set; }

        [NotMapped]
        public string? UserName { get; set; }

        public int Rating { get; set; } 
        public string? Comment { get; set; }

        public Trip? Trip { get; set; }
    }
}