using System;
using System.Data;

namespace RMS_Business
{
    /// <summary>
    /// Represents a single line-item in a Sale (mirrors PurchaseProductBatches → clsBatch pattern).
    /// Maps to the ProductSales table.
    /// </summary>
    public class clsProductSale
    {
        public enum enMode { AddNew = 0, Update = 1 };
        public enMode Mode = enMode.AddNew;

        public int ProductSaleID { get; set; }
        public int SaleID { get; set; }
        public int ProductUnitID { get; set; }
        public decimal Quantity { get; set; }
        public decimal UnitPrice { get; set; }

        public clsProductSale()
        {
            ProductSaleID = -1;
            SaleID = -1;
            ProductUnitID = -1;
            Quantity = 0;
            UnitPrice = 0;
            Mode = enMode.AddNew;
        }
    }
}
