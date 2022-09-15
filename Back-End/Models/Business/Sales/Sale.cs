using System;
using System.Collections.Generic;

namespace Services.Models
{
    public class Sale { 
        public int SaleId { get; set; }
        public int CustomerId { get; set; }
        public bool IsEnabled { get; set; }
        public DateTime CreateDate { get; set; }
        public string Identification { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public List<SaleProducts> SaleProducts { get; set; }
    }
}
