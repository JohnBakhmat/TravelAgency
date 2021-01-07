using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TravelAgency.Models {
    public class Customer {
        [Key]
        public int UserId { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string FatherName { get; set; }
        public DateTime BirthDate { get; set; }
        public string PassportCode { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public ICollection<Vocation> Vocations { get; set; } = new List<Vocation>();
    }
}