using System;
using System.ComponentModel.DataAnnotations;

namespace TravelAgency.Models {
    public class Vocation {
        [Key]
        public int VocationId { get; set; }
        public DateTime BeginDateTime { get; set; }
        public int DaysCount { get; set; }
        
        public int TourId { get; set; }
        public Tour Tour { get; set; }
        
        public int OperatorId { get; set; }
        public Operator Operator { get; set; }

        public int CustomerId { get; set; }
        public Customer Customer { get; set; }
    }
}