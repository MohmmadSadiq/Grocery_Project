using System;
using System.Data;
using RMS_DataAccess;
namespace RMS_Business
{
    public class clsPaymentMethod
    {
        public enum enMode { AddNew = 0, Update = 1 };
        public enMode Mode = enMode.AddNew;
        public int PaymentMethodID { get; set; }
        public string MethodName { get; set; }
        public string? Description { get; set; }
        public bool IsActiveForSales { get; set; }
        public bool IsActiveForPurchases { get; set; }
        public clsPaymentMethod()
        {
            PaymentMethodID = -1;
            MethodName = string.Empty;
            Description = null;
            IsActiveForSales = false;
            IsActiveForPurchases = false;
            Mode = enMode.AddNew;
        }
        public bool Save()
        {
            switch (Mode)
            {
                case enMode.AddNew:
                    var newID = clsPaymentMethodData.AddNewPaymentMethod(MethodName, Description, IsActiveForSales, IsActiveForPurchases);
                    if (newID != -1) { PaymentMethodID = newID; Mode = enMode.Update; return true; }
                    else return false;
                case enMode.Update:
                    return clsPaymentMethodData.UpdatePaymentMethod(PaymentMethodID, MethodName, Description, IsActiveForSales, IsActiveForPurchases);
            }
            return false;
        }
        public static clsPaymentMethod? Find(int PaymentMethodID)
        {
            string MethodName = string.Empty;
            string? Description = null;
            bool IsActiveForSales = false;
            bool IsActiveForPurchases = false;
            bool found = clsPaymentMethodData.GetPaymentMethodByID(PaymentMethodID, ref MethodName, ref Description, ref IsActiveForSales, ref IsActiveForPurchases);
            if (found)
                return new clsPaymentMethod() { PaymentMethodID = PaymentMethodID, MethodName = MethodName, Description = Description, IsActiveForSales = IsActiveForSales, IsActiveForPurchases = IsActiveForPurchases, Mode = enMode.Update };
            else return null;
        }
        public static bool DeletePaymentMethod(int PaymentMethodID)
        {
            return clsPaymentMethodData.DeletePaymentMethod(PaymentMethodID );
        }
        public static DataTable GetAllPaymentMethod()
        {
            return clsPaymentMethodData.GetAllPaymentMethod();
        }
    }
}
