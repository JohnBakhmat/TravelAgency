using System.ComponentModel.DataAnnotations;

namespace TravelAgency.Models {
    public class Hotel {
        [Key]
        public int HotelId { get; set; }
        public string Name { get; set; }
        public int Rating { get; set; }
        public HotelType Type { get; set; }
        public FoodType Food { get; set; }
    }
}