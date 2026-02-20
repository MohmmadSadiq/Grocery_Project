using System;
using System.Data;
using RMS_DataAccess;
namespace RMS_Business
{
    public class clsPurchase : clsTransaction
    {
        public int PurchaseID { get; set; }
        public int? SupplierID { get; set; }
        public string? InvoiceNumber { get; set; }
        public int? PurchasedByEmployeeID { get; set; }
        public List<clsBatch>? PurchaseBatches { get; set; } // the list of batches associated with this purchase
        public clsPurchase()
        {
            PurchaseID = -1;
            SupplierID = null;
            InvoiceNumber = null;
            PurchasedByEmployeeID = null;
            // Mode, TransactionID now inherited from clsTransaction
        }


        override public bool Save()
        {
            // Save or update Transaction first, but keep original Mode for Purchase logic
            var originalMode = this.Mode;
            bool transactionSaved = base.Save();
            DataTable? detailsTable = BatchListToDataTable(PurchaseBatches ?? new List<clsBatch>());
            if (!transactionSaved) return false;
            TransactionID = this.TransactionID; // ensure TransactionID is set
            this.Mode = originalMode; // restore original mode for Purchase
            switch (Mode)
            {
                case enMode.AddNew:
                    var newID = clsPurchaseData.AddNewPurchase(TransactionID, SupplierID, InvoiceNumber, PurchasedByEmployeeID, detailsTable);
                    if (newID != -1) { PurchaseID = newID; Mode = enMode.Update; return true; }
                    else return false;
                case enMode.Update:
                    return clsPurchaseData.UpdatePurchase(PurchaseID, TransactionID, SupplierID, InvoiceNumber, PurchasedByEmployeeID);
            }
            return false;
        }
        new public static clsPurchase? Find(int PurchaseID)
        {
            int TransactionID = -1;
            int? SupplierID = null;
            string? InvoiceNumber = null;
            int? PurchasedByEmployeeID = null;
            DataTable detailsTable = new DataTable();
            bool found = clsPurchaseData.GetPurchaseByID(PurchaseID, ref TransactionID, ref SupplierID, ref InvoiceNumber, ref PurchasedByEmployeeID, ref detailsTable);
            clsTransaction? transaction = clsTransaction.Find(TransactionID);
            if (found && transaction != null)
            {
                clsPurchase purchase = new clsPurchase();
                // نسخ خصائص الـ Transaction
                purchase.TransactionID = transaction.TransactionID;
                purchase.PaymentID = transaction.PaymentID;
                purchase.TransactionDate = transaction.TransactionDate;
                purchase.TransactionType = transaction.TransactionType;
                purchase.TransactionStatus = transaction.TransactionStatus;
                purchase.TotalAmount = transaction.TotalAmount;
                purchase.Nots = transaction.Nots;
                purchase.CreatedDate = transaction.CreatedDate;
                purchase.CreatedByUserID = transaction.CreatedByUserID;
                purchase.UpdatedDate = transaction.UpdatedDate;
                purchase.UpdatedByUserID = transaction.UpdatedByUserID;
                purchase.Mode = enMode.Update;
            
                // نسخ خصائص الـ Purchase
                purchase.PurchaseID = PurchaseID;
                purchase.SupplierID = SupplierID;
                purchase.InvoiceNumber = InvoiceNumber;
                purchase.PurchasedByEmployeeID = PurchasedByEmployeeID;
                purchase.PurchaseBatches = purchase.DataTableToBatchListFromDB(detailsTable);
            
                return purchase;
            }
            else return null;
        }
        public static bool DeletePurchase(int PurchaseID , int? UpdatedByUserID = null)
        {
            return clsPurchaseData.DeletePurchase(PurchaseID , UpdatedByUserID);
        }
        public static DataTable GetAllPurchase()
        {
            return clsPurchaseData.GetAllPurchase();
        }

        private List<clsBatch> DataTableToBatchList(DataTable table)
        {
            List<clsBatch> list = new List<clsBatch>();
        
            foreach (DataRow row in table.Rows)
            {
                clsBatch batch = new clsBatch();
        
                batch.ProductUnitID  = row["ProductUnitID"] == DBNull.Value ? -1 : Convert.ToInt32(row["ProductUnitID"]);
                batch.TotalQuantity  = row["TotalQuantity"] == DBNull.Value ? 0  : Convert.ToDecimal(row["TotalQuantity"]);
                batch.UniteCostPrice = row["UniteCostPrice"] == DBNull.Value ? 0  : Convert.ToDecimal(row["UniteCostPrice"]);
        
                batch.ProductionDate = row["ProductionDate"] == DBNull.Value
                                        ? (DateTime?)null
                                        : Convert.ToDateTime(row["ProductionDate"]);
        
                batch.ExpiryDate = row["ExpiryDate"] == DBNull.Value
                                        ? (DateTime?)null
                                        : Convert.ToDateTime(row["ExpiryDate"]);
        
                batch.BatchNumber = row["BatchNumber"] == DBNull.Value
                                        ? null
                                        : row["BatchNumber"].ToString();
        
                batch.Mode = clsBatch.enMode.AddNew;   // لأنها عادة تأتي من TVP / إدخال جديد
        
                list.Add(batch);
            }
        
            return list;
        }

        private DataTable BatchListToDataTable(List<clsBatch> batches)
        {
            DataTable table = new DataTable();

            table.Columns.Add("ProductUnitID", typeof(int));
            table.Columns.Add("TotalQuantity", typeof(decimal));
            table.Columns.Add("UniteCostPrice", typeof(decimal));
            table.Columns.Add("ProductionDate", typeof(DateTime));
            table.Columns.Add("ExpiryDate", typeof(DateTime));
            table.Columns.Add("BatchNumber", typeof(string));

            foreach (clsBatch batch in batches)
            {
                DataRow row = table.NewRow();

                row["ProductUnitID"]  = batch.ProductUnitID;
                row["TotalQuantity"]  = batch.TotalQuantity;
                row["UniteCostPrice"] = batch.UniteCostPrice;

                row["ProductionDate"] = batch.ProductionDate.HasValue
                                            ? batch.ProductionDate.Value
                                            : (object)DBNull.Value;

                row["ExpiryDate"] = batch.ExpiryDate.HasValue
                                            ? batch.ExpiryDate.Value
                                            : (object)DBNull.Value;

                row["BatchNumber"] = string.IsNullOrEmpty(batch.BatchNumber)
                                            ? (object)DBNull.Value
                                            : batch.BatchNumber;

                table.Rows.Add(row);
            }

            return table;
        }

        private List<clsBatch> DataTableToBatchListFromDB(DataTable table)
        {
            List<clsBatch> list = new List<clsBatch>();

            foreach (DataRow row in table.Rows)
            {
                clsBatch batch = new clsBatch();

                batch.BatchID = row["BatchID"] == DBNull.Value
                    ? -1
                    : Convert.ToInt32(row["BatchID"]);

                batch.PurchaseID = row["PurchaseID"] == DBNull.Value
                    ? -1
                    : Convert.ToInt32(row["PurchaseID"]);

                batch.ProductUnitID = row["ProductUnitID"] == DBNull.Value
                    ? -1
                    : Convert.ToInt32(row["ProductUnitID"]);

                batch.TotalQuantity = row["TotalQuantity"] == DBNull.Value
                    ? 0
                    : Convert.ToDecimal(row["TotalQuantity"]);

                batch.UniteCostPrice = row["UniteCostPrice"] == DBNull.Value
                    ? 0
                    : Convert.ToDecimal(row["UniteCostPrice"]);

                batch.ProductionDate = row["ProductionDate"] == DBNull.Value
                    ? (DateTime?)null
                    : Convert.ToDateTime(row["ProductionDate"]);

                batch.ExpiryDate = row["ExpiryDate"] == DBNull.Value
                    ? (DateTime?)null
                    : Convert.ToDateTime(row["ExpiryDate"]);

                batch.BatchNumber = row["BatchNumber"] == DBNull.Value
                    ? null
                    : row["BatchNumber"].ToString();

                // هذه مهمة لأن البيانات قادمة من قاعدة البيانات
                batch.Mode = clsBatch.enMode.Update;

                list.Add(batch);
            }

            return list;
        }
        
    }
}
