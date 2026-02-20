using System;
using System.Data;
using RMS_DataAccess;
namespace RMS_Business
{
    public class clsBatch
    {
        public enum enMode { AddNew = 0, Update = 1 };
        public enMode Mode = enMode.AddNew;
        public int BatchID { get; set; }
        public int PurchaseID { get; set; }
        public int ProductUnitID { get; set; }
        public decimal TotalQuantity { get; set; }
        public decimal UniteCostPrice { get; set; }
        public DateTime? ProductionDate { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public string? BatchNumber { get; set; }
        public clsBatch()
        {
            BatchID = -1;
            PurchaseID = -1;
            ProductUnitID = -1;
            TotalQuantity = -1;
            UniteCostPrice = -1;
            ProductionDate = null;
            ExpiryDate = null;
            BatchNumber = null;
            Mode = enMode.AddNew;
        }
        public bool Save()
        {
            switch (Mode)
            {
                case enMode.AddNew:
                    var newID = clsBatchData.AddNewBatch(PurchaseID, ProductUnitID, TotalQuantity, UniteCostPrice, ProductionDate, ExpiryDate, BatchNumber);
                    if (newID != -1) { BatchID = newID; Mode = enMode.Update; return true; }
                    else return false;
                case enMode.Update:
                    return clsBatchData.UpdateBatch(BatchID, PurchaseID, ProductUnitID, TotalQuantity, UniteCostPrice, ProductionDate, ExpiryDate, BatchNumber);
            }
            return false;
        }
        public static clsBatch? Find(int BatchID)
        {
            int PurchaseID = -1;
            int ProductUnitID = -1;
            decimal TotalQuantity = -1;
            decimal UniteCostPrice = -1;
            DateTime? ProductionDate = null;
            DateTime? ExpiryDate = null;
            string? BatchNumber = null;
            bool found = clsBatchData.GetBatchByID(BatchID, ref PurchaseID, ref ProductUnitID, ref TotalQuantity, ref UniteCostPrice, ref ProductionDate, ref ExpiryDate, ref BatchNumber);
            if (found)
                return new clsBatch() { BatchID = BatchID, PurchaseID = PurchaseID, ProductUnitID = ProductUnitID, TotalQuantity = TotalQuantity, UniteCostPrice = UniteCostPrice, ProductionDate = ProductionDate, ExpiryDate = ExpiryDate, BatchNumber = BatchNumber, Mode = enMode.Update };
            else return null;
        }
        public static bool DeleteBatch(int BatchID)
        {
            return clsBatchData.DeleteBatch(BatchID );
        }
        public static DataTable GetAllBatch()
        {
            return clsBatchData.GetAllBatch();
        }
    }
}
