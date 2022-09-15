using System;
using System.Collections.Generic;

namespace Services.Models
{
    public class SaleModel
    {
        public int SaleId { get; set; }
        public int CustomerId { get; set; }
        public bool IsEnabled { get; set; }
        public DateTime CreateDate { get; set; }
        List<SaleProductsModel> SaleProducts { get; set; }
    }
}
