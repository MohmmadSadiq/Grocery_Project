using System;
using System.Data;
using RMS_DataAccess;
namespace RMS_Business
{
    public class clsPaymentAllocation
    {
        public enum enMode { AddNew = 0, Update = 1 };
        public enMode Mode = enMode.AddNew;
        public int AllocationID { get; set; }
        public int PaymentID { get; set; }
        public int TransactionID { get; set; }
        public decimal Amount { get; set; }
        public DateTime CreatedDate { get; set; }
        public int? CreatedByUserID { get; set; }
        public clsPaymentAllocation()
        {
            AllocationID = -1;
            PaymentID = -1;
            TransactionID = -1;
            Amount = -1;
            CreatedDate = DateTime.MinValue;
            CreatedByUserID = null;
            Mode = enMode.AddNew;
        }
        public bool Save()
        {
            switch (Mode)
            {
                case enMode.AddNew:
                    var newID = clsPaymentAllocationData.AddNewAllocation(PaymentID, TransactionID, Amount, CreatedByUserID);
                    if (newID != -1) { AllocationID = newID; Mode = enMode.Update; return true; }
                    else return false;
                case enMode.Update:
                    return clsPaymentAllocationData.UpdateAllocation(AllocationID, PaymentID, TransactionID, Amount);
            }
            return false;
        }
        public static clsPaymentAllocation? Find(int AllocationID)
        {
            int PaymentID = -1;
            int TransactionID = -1;
            decimal Amount = -1;
            DateTime CreatedDate = DateTime.MinValue;
            int? CreatedByUserID = null;
            bool found = clsPaymentAllocationData.GetAllocationByID(AllocationID, ref PaymentID, ref TransactionID, ref Amount, ref CreatedDate, ref CreatedByUserID);
            if (found)
                return new clsPaymentAllocation() { AllocationID = AllocationID, PaymentID = PaymentID, TransactionID = TransactionID, Amount = Amount, CreatedDate = CreatedDate, CreatedByUserID = CreatedByUserID, Mode = enMode.Update };
            else return null;
        }
        public static bool DeleteAllocation(int AllocationID)
        {
            return clsPaymentAllocationData.DeleteAllocation(AllocationID );
        }
        public static DataTable GetAllAllocation()
        {
            return clsPaymentAllocationData.GetAllAllocation();
        }
    }
}
