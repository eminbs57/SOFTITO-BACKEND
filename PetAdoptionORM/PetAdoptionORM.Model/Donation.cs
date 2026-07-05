using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace PetAdoptionORM.Model
{
    public class Donation
    {
        [Key]
        public int Id { get; set; }
        public decimal Amount { get; set; }
        public string Message { get; set; }
        public DateTime DonationDate { get; set; }

        public string AppUserId { get; set; } // Identity User Id
    }
}
