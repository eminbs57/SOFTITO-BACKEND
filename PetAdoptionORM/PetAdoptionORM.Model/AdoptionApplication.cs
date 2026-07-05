using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace PetAdoptionORM.Model
{
    public class AdoptionApplication
    {
        [Key]
        public int Id { get; set; }
        public string ApplicantName { get; set; }
        public string Phone { get; set; }
        public DateTime ApplicationDate { get; set; }
        public string Status { get; set; }

        [ForeignKey("PetId")]
        public int PetId { get; set; }
        public Pet Pet { get; set; }

        public string AppUserId { get; set; } // Identity User Id
    }
}
