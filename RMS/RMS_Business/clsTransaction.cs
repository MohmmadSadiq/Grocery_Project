using System;
using System.Data;
using RMS_DataAccess;
namespace RMS_Business
{
    public class clsTransaction
    {
        public enum enMode { AddNew = 0, Update = 1 };
        public enum enTransactionType { Unknown = 0, Sale = 1, Purchase = 2 };
        public enum enTransactionStatus { Unknown = 0, InProgress = 1, Canceld = 2, Completed = 3 };
        public enMode Mode = enMode.AddNew;
        public int TransactionID { get; set; }
        public int? PaymentID { get; set; }
        public DateTime TransactionDate { get; set; }
        public enTransactionType TransactionType { get; set; }
        public enTransactionStatus TransactionStatus { get; set; }
        public decimal TotalAmount { get; set; }
        public string? Nots { get; set; }
        public DateTime CreatedDate { get; set; }
        public int? CreatedByUserID { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public int? UpdatedByUserID { get; set; }
        public clsTransaction()
        {
            TransactionID = -1;
            PaymentID = null;
            TransactionDate = DateTime.Now;
            TransactionType = enTransactionType.Unknown;
            TransactionStatus = enTransactionStatus.Unknown;
            TotalAmount = -1;
            Nots = null;
            CreatedDate = DateTime.Now;
            CreatedByUserID = null;
            UpdatedDate = null;
            UpdatedByUserID = null;
            Mode = enMode.AddNew;
        }
        virtual public bool Save()
        {
            switch (Mode)
            {
                case enMode.AddNew:
                    var newID = clsTransactionData.AddNewTransaction(PaymentID, TransactionDate, (byte)TransactionType, (byte)TransactionStatus, TotalAmount, Nots, CreatedByUserID);
                    if (newID != -1) { TransactionID = newID; Mode = enMode.Update; return true; }
                    else return false;
                case enMode.Update:
                    return clsTransactionData.UpdateTransaction(TransactionID, PaymentID, TransactionDate, (byte)TransactionType, (byte)TransactionStatus, TotalAmount, Nots, UpdatedByUserID);
            }
            return false;
        }
        public static clsTransaction? Find(int TransactionID)
        {
            int? PaymentID = null;
            DateTime TransactionDate = DateTime.MinValue;
            byte TransactionTypeByte = 0;
            byte TransactionStatusByte = 0;
            decimal TotalAmount = -1;
            string? Nots = null;
            DateTime CreatedDate = DateTime.MinValue;
            int? CreatedByUserID = null;
            DateTime? UpdatedDate = null;
            int? UpdatedByUserID = null;
            bool found = clsTransactionData.GetTransactionByID(TransactionID, ref PaymentID, ref TransactionDate, ref TransactionTypeByte, ref TransactionStatusByte, ref TotalAmount, ref Nots, ref CreatedDate, ref CreatedByUserID, ref UpdatedDate, ref UpdatedByUserID);
            if (found)
                return new clsTransaction() { TransactionID = TransactionID, PaymentID = PaymentID, TransactionDate = TransactionDate, TransactionType = (enTransactionType)TransactionTypeByte, TransactionStatus = (enTransactionStatus)TransactionStatusByte, TotalAmount = TotalAmount, Nots = Nots, CreatedDate = CreatedDate, CreatedByUserID = CreatedByUserID, UpdatedDate = UpdatedDate, UpdatedByUserID = UpdatedByUserID, Mode = enMode.Update };
            else return null;
        }
        public static bool DeleteTransaction(int TransactionID, int? UpdatedByUserID = null)
        {
            return clsTransactionData.DeleteTransaction(TransactionID , UpdatedByUserID);
        }
        public static DataTable GetAllTransaction()
        {
            return clsTransactionData.GetAllTransaction();
        }
    }
}
