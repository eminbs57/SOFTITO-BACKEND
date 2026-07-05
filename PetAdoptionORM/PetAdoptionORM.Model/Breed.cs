using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace PetAdoptionORM.Model
{
    public class Breed
    {
        [Key]
        public int Id { get; set; }
        public string BreedName { get; set; }

        [ForeignKey("SpeciesId")]
        public int SpeciesId { get; set; }
        public Species Species { get; set; }
    }
}
