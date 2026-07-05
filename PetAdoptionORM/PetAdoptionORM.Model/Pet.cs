using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace PetAdoptionORM.Model
{
    public class Pet
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public string Photo { get; set; }
        public string HealthStatus { get; set; }

        [ForeignKey("BreedId")]
        public int BreedId { get; set; }
        public Breed Breed { get; set; }
    }
}
