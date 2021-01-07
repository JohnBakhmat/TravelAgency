using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TravelAgency.Models {
    public class Operator {
        [Key]
        public int OperatorId { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string FatherName { get; set; }
        public DateTime BirthDate { get; set; }
        public string Phone { get; set; } 
        public ICollection<Vocation> Vocations { get; set; } = new List<Vocation>();
    }
}