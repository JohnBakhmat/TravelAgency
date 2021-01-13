using System;
using System.ComponentModel.DataAnnotations;
using TravelAgency.Models;

namespace TravelAgency.Models {
    public class Tour {
        [Key]
        public int TourId { get; set; }
        public Hotel Hotel { get; set; }
        public int HotelId { get; set; }
        public string Country { get; set; }
        public double OneDayCost { get; set; }
        public double TransportCost { get; set; }
        public double VisaCost { get; set; }
        public VisaGoals VisaGoal { get; set; }
        public DateTime EndDate { get; set; }
    }
}