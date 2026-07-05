using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace PetAdoptionORM.Model
{
    public class Species
    {
        [Key]
        public int Id { get; set; }
        public string SpeciesName { get; set; }
    }
}
