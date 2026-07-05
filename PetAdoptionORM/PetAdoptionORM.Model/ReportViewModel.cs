using System;

namespace PetAdoptionORM.Model
{
    public class ReportViewModel
    {
        public string PetName { get; set; }
        public int Age { get; set; }
        public string HealthStatus { get; set; }
        public string BreedName { get; set; }
        public string SpeciesName { get; set; }
        
        // Başvuru raporları için yeni alanlar
        public string ApplicantName { get; set; }
        public DateTime? ApplicationDate { get; set; }
        public string AppStatus { get; set; }
    }
}
