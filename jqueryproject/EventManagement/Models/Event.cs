using System.ComponentModel.DataAnnotations;

namespace EventManagement.Models
{
    public class Event
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Başlık zorunludur!")]

        public string Title { get; set; } = string.Empty;

        public string CategoryName { get; set; } = string.Empty;

        [Required]
        public DateTime Date { get; set; }

        public string Description { get; set; } = string.Empty;
        [Required(ErrorMessage = "Lütfen etkinlik için bir kontenjan belirleyin.")]
        [Range(1, int.MaxValue, ErrorMessage = "Kontenjan en az 1 kişi olmalıdır.")]
        public int Kota { get; set; }
        
        public string? ImageUrl { get; set; }

        // Navigation property for users who joined this event
        public virtual ICollection<AppUser> Attendees { get; set; } = new List<AppUser>();
    }
}