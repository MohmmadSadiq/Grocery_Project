using System;
using System.Data;
using RMS_DataAccess;

namespace RMS_Business
{
    public class clsSales : clsTransaction
    {
        public int SaleID { get; set; }
        public int? CustomerID { get; set; }
        public List<clsProductSale>? SaleItems { get; set; } // the list of product-sale lines associated with this sale

        public clsSales()
        {
            SaleID = -1;
            CustomerID = null;
            SaleItems = null;
            // Mode, TransactionID now inherited from clsTransaction
        }

        override public bool Save()
        {
            // Save or update Transaction first, but keep original Mode for Sale logic
            var originalMode = this.Mode;
            bool transactionSaved = base.Save();
            DataTable? detailsTable = SaleItemsListToDataTable(SaleItems ?? new List<clsProductSale>());

            if (!transactionSaved) return false;
            TransactionID = this.TransactionID; // ensure TransactionID is set
            this.Mode = originalMode; // restore original mode for Sale

            switch (Mode)
            {
                case enMode.AddNew:
                    var newID = clsSalesData.AddNewSale(TransactionID, CustomerID, detailsTable);
                    if (newID != -1) { SaleID = newID; Mode = enMode.Update; return true; }
                    else return false;
                case enMode.Update:
                    return clsSalesData.UpdateSale(SaleID, TransactionID, CustomerID);
            }
            return false;
        }

        new public static clsSales? Find(int SaleID)
        {
            int TransactionID = -1;
            int? CustomerID = null;
            DataTable detailsTable = new DataTable();

            bool found = clsSalesData.GetSaleByID(SaleID, ref TransactionID, ref CustomerID, ref detailsTable);
            clsTransaction? transaction = clsTransaction.Find(TransactionID);

            if (found && transaction != null)
            {
                clsSales sale = new clsSales();

                // نسخ خصائص الـ Transaction
                sale.TransactionID = transaction.TransactionID;
                sale.PaymentID = transaction.PaymentID;
                sale.TransactionDate = transaction.TransactionDate;
                sale.TransactionType = transaction.TransactionType;
                sale.TransactionStatus = transaction.TransactionStatus;
                sale.TotalAmount = transaction.TotalAmount;
                sale.Nots = transaction.Nots;
                sale.CreatedDate = transaction.CreatedDate;
                sale.CreatedByUserID = transaction.CreatedByUserID;
                sale.UpdatedDate = transaction.UpdatedDate;
                sale.UpdatedByUserID = transaction.UpdatedByUserID;
                sale.Mode = enMode.Update;

                // نسخ خصائص الـ Sale
                sale.SaleID = SaleID;
                sale.CustomerID = CustomerID;
                sale.SaleItems = DataTableToSaleItemsListFromDB(detailsTable);

                return sale;
            }
            else return null;
        }

        public static bool DeleteSale(int SaleID, int? UpdatedByUserID = null)
        {
            return clsSalesData.DeleteSale(SaleID, UpdatedByUserID);
        }

        public static DataTable GetAllSales()
        {
            return clsSalesData.GetAllSales();
        }

        // ── Search / Pagination ───────────────────────────────────────────────────

        public static DataTable SearchSalesPages(SalesSearchCriteria criteria)
        {
            return clsSalesData.SearchSalesPages(criteria.ToDataAccessCriteria());
        }

        public class SalesSearchCriteria
        {
            public string? SearchText { get; set; }
            public string SearchBy { get; set; } = "SaleID";        // SaleID, CustomerName
            public byte? TransactionStatus { get; set; }            // 1=InProgress, 2=Cancelled, 3=Completed, null=All
            public string? CustomerType { get; set; }               // Person, Company, null for all
            public int PageNumber { get; set; } = 1;
            public int PageSize { get; set; } = 20;
            public string SortBy { get; set; } = "TransactionDate";

            public clsSalesData.SalesSearchCriteria ToDataAccessCriteria()
            {
                return new clsSalesData.SalesSearchCriteria
                {
                    SearchText        = this.SearchText,
                    SearchBy          = this.SearchBy,
                    TransactionStatus = this.TransactionStatus,
                    CustomerType      = this.CustomerType,
                    PageNumber        = this.PageNumber,
                    PageSize          = this.PageSize,
                    SortBy            = this.SortBy
                };
            }

            public bool IsDefault()
            {
                return string.IsNullOrEmpty(SearchText) && !TransactionStatus.HasValue
                    && string.IsNullOrEmpty(CustomerType) && PageNumber == 1 && PageSize == 20;
            }
        }

        // ── DataTable ↔ List Conversion Helpers ──────────────────────────────────

        private DataTable SaleItemsListToDataTable(List<clsProductSale> items)
        {
            DataTable table = new DataTable();

            table.Columns.Add("ProductUnitID", typeof(int));
            table.Columns.Add("Quantity", typeof(decimal));
            table.Columns.Add("UnitPrice", typeof(decimal));

            foreach (clsProductSale item in items)
            {
                DataRow row = table.NewRow();

                row["ProductUnitID"] = item.ProductUnitID;
                row["Quantity"]      = item.Quantity;
                row["UnitPrice"]     = item.UnitPrice;

                table.Rows.Add(row);
            }

            return table;
        }

        private static List<clsProductSale> DataTableToSaleItemsListFromDB(DataTable table)
        {
            List<clsProductSale> list = new List<clsProductSale>();

            foreach (DataRow row in table.Rows)
            {
                clsProductSale item = new clsProductSale();

                item.ProductSaleID = row["ProductSaleID"] == DBNull.Value
                    ? -1
                    : Convert.ToInt32(row["ProductSaleID"]);

                item.SaleID = row["SaleID"] == DBNull.Value
                    ? -1
                    : Convert.ToInt32(row["SaleID"]);

                item.ProductUnitID = row["ProductUnitID"] == DBNull.Value
                    ? -1
                    : Convert.ToInt32(row["ProductUnitID"]);

                item.Quantity = row["Quantity"] == DBNull.Value
                    ? 0
                    : Convert.ToDecimal(row["Quantity"]);

                item.UnitPrice = row["UnitPrice"] == DBNull.Value
                    ? 0
                    : Convert.ToDecimal(row["UnitPrice"]);

                // البيانات قادمة من قاعدة البيانات
                item.Mode = clsProductSale.enMode.Update;

                list.Add(item);
            }

            return list;
        }
    }
}
