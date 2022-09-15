using System;

namespace Services.Models
{
    public class Sale { 
        public int SaleId { get; set; }
        public int CustomerId { get; set; }
        public bool IsEnabled { get; set; }
        public DateTime CreateDate { get; set; }
    }
}
