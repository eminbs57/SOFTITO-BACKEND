namespace ApiProject.Models
{
    public class Room
    {
        public int Id { get; set; }
        public string RoomNumber { get; set; }
        public int RoomTypeId { get; set; }
        public bool IsAvailable { get; set; }
        public string RoomTypeName { get; set; } // for ViewAll
    }
}
