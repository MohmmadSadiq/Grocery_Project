using System;
using System.Data;
using System.Collections.Generic;
using RMS_DataAccess;
namespace RMS_Business
{
    public class clsPayment
    {
        public enum enMode { AddNew = 0, Update = 1 };
        public enMode Mode = enMode.AddNew;
        public int PaymentID { get; set; }
        public DateTime PaymentDate { get; set; }
        public int PaymentMethodID { get; set; }
        public decimal PaymentAmount { get; set; }
        public string? Notes { get; set; }
        public DateTime CreatedDate { get; set; }
        public int? CreatedByUserID { get; set; }
        public DateTime UpdatedDate { get; set; }
        public int? UpdatedByUserID { get; set; }
        
        // List of Payment Allocations
        public List<clsPaymentAllocation> Allocations { get; set; } = new List<clsPaymentAllocation>();
        public clsPayment()
        {
            PaymentID = -1;
            PaymentDate = DateTime.MinValue;
            PaymentMethodID = -1;
            PaymentAmount = -1;
            Notes = null;
            CreatedDate = DateTime.MinValue;
            CreatedByUserID = null;
            UpdatedDate = DateTime.MinValue;
            UpdatedByUserID = null;
            Mode = enMode.AddNew;
        }

        private DataTable _ConvertAllocationsToDataTable()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("TransactionID", typeof(int));
            dt.Columns.Add("Amount", typeof(decimal));

            foreach (var allocation in Allocations)
            {
                dt.Rows.Add(allocation.TransactionID, allocation.Amount);
            }

            return dt;
        }

        private void _LoadAllocationsFromDataTable(DataTable? allocations)
        {
            if (allocations != null && allocations.Rows.Count > 0)
            {
                foreach (DataRow row in allocations.Rows)
                {
                    var allocation = new clsPaymentAllocation
                    {
                        AllocationID = (int)row["AllocationID"],
                        PaymentID = (int)row["PaymentID"],
                        TransactionID = (int)row["TransactionID"],
                        Amount = (decimal)row["Amount"],
                        CreatedDate = (DateTime)row["CreatedDate"],
                        CreatedByUserID = row["CreatedByUserID"] != DBNull.Value ? (int?)row["CreatedByUserID"] : null
                    };
                    this.Allocations.Add(allocation);
                }
            }
        }

        public bool Save()
        {
            switch (Mode)
            {
                case enMode.AddNew:
                    DataTable allocationsTable = _ConvertAllocationsToDataTable();
                    var newID = clsPaymentData.AddNewPayment(PaymentDate, PaymentMethodID, PaymentAmount, Notes, CreatedByUserID, allocationsTable);
                    if (newID != -1) { PaymentID = newID; Mode = enMode.Update; return true; }
                    else return false;
                case enMode.Update:
                    return clsPaymentData.UpdatePayment(PaymentID, PaymentDate, PaymentMethodID, PaymentAmount, Notes, UpdatedByUserID);
            }
            return false;
        }
        public static clsPayment? Find(int PaymentID)
        {
            DateTime PaymentDate = DateTime.MinValue;
            int PaymentMethodID = -1;
            decimal PaymentAmount = -1;
            string? Notes = null;
            DateTime CreatedDate = DateTime.MinValue;
            int? CreatedByUserID = null;
            DateTime UpdatedDate = DateTime.MinValue;
            int? UpdatedByUserID = null;
            DataTable? allocations = null;
            
            bool found = clsPaymentData.GetPaymentByID(PaymentID, ref PaymentDate, ref PaymentMethodID, ref PaymentAmount, ref Notes, ref CreatedDate, ref CreatedByUserID, ref UpdatedDate, ref UpdatedByUserID, ref allocations);
            
            if (found)
            {
                var payment = new clsPayment() 
                { 
                    PaymentID = PaymentID, 
                    PaymentDate = PaymentDate, 
                    PaymentMethodID = PaymentMethodID, 
                    PaymentAmount = PaymentAmount, 
                    Notes = Notes, 
                    CreatedDate = CreatedDate, 
                    CreatedByUserID = CreatedByUserID, 
                    UpdatedDate = UpdatedDate, 
                    UpdatedByUserID = UpdatedByUserID, 
                    Mode = enMode.Update 
                };

                // Load Allocations into List
                payment._LoadAllocationsFromDataTable(allocations);

                return payment;
            }
            else 
                return null;
        }
        public static bool DeletePayment(int PaymentID, int? UpdatedByUserID = null)
        {
            return clsPaymentData.DeletePayment(PaymentID , UpdatedByUserID);
        }
        public static DataTable GetAllPayment()
        {
            return clsPaymentData.GetAllPayment();
        }
    }
}
