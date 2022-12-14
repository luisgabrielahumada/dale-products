using System;

namespace Services.Models
{
    public class SaleProducts
    {
        public int SaleProductsId { get; set; }
        public int SaleId { get; set; }
        public int ProductId { get; set; }
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }
        public bool IsEnabled { get; set; }
        public DateTime CreateDate { get; set; }
        public int Inventory { get; set; }
        public decimal Total { get; set; }
        public string Name { get; set; }
    }
}
