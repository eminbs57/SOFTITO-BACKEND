using ObiletApp.Core.Common;
using System;

namespace ObiletApp.Core.Entities
{
    public class Campaign : BaseEntity
    {
        public string? Name { get; set; }
        public string? DiscountCode { get; set; }
        public decimal DiscountPercentage { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public int? CompanyId { get; set; }

        [System.ComponentModel.DataAnnotations.Schema.NotMapped]
        public string? CompanyName { get; set; }

        public Company? Company { get; set; }
    }
}