using System.ComponentModel.DataAnnotations;

namespace ApiProject.Models
{
    public class Vehicle
    {
        public int Id { get; set; }
        
        [Display(Name = "Plaka")]
        public string LicensePlate { get; set; } = string.Empty;
        
        [Display(Name = "Marka")]
        public string Brand { get; set; } = string.Empty;
        
        [Display(Name = "Kapasite")]
        public int Capacity { get; set; }
        
        [Display(Name = "Araç Tipi")]
        public string VehicleType { get; set; } = string.Empty;
    }
}
