using System;
using System.Collections.Generic;
using System.Linq;
using RMS_Business;

internal class Program
{
    private static void Main(string[] args)
    {
        PermissionSeedValidator.Run();
    }

    private static void TestAddTransactions()
    {
        Console.WriteLine("### 0. Testing Add Transactions ###\n");

        try
        {
            // Create first transaction
            var transaction1 = new clsTransaction
            {
                TransactionDate = DateTime.Now,
                TransactionType = clsTransaction.enTransactionType.Sale,
                TransactionStatus = clsTransaction.enTransactionStatus.Completed,
                TotalAmount = 600.00m,
                Nots = "Test Sale Transaction 1",
                CreatedByUserID = null,
                Mode = clsTransaction.enMode.AddNew
            };

            Console.WriteLine("Creating Transaction 1:");
            Console.WriteLine($"   - Transaction Date: {transaction1.TransactionDate:yyyy-MM-dd HH:mm:ss}");
            Console.WriteLine($"   - Type: {transaction1.TransactionType}");
            Console.WriteLine($"   - Status: {transaction1.TransactionStatus}");
            Console.WriteLine($"   - Total Amount: {transaction1.TotalAmount:C2}");
            Console.WriteLine($"   - Notes: {transaction1.Nots}");

            if (transaction1.Save())
            {
                Console.WriteLine($"SUCCESS! Transaction 1 ID: {transaction1.TransactionID}\n");
                _testTransactionIDs.Add(transaction1.TransactionID);
            }
            else
            {
                Console.WriteLine("FAILED! Could not add Transaction 1");
                return;
            }

            // Create second transaction
            var transaction2 = new clsTransaction
            {
                TransactionDate = DateTime.Now,
                TransactionType = clsTransaction.enTransactionType.Sale,
                TransactionStatus = clsTransaction.enTransactionStatus.Completed,
                TotalAmount = 400.00m,
                Nots = "Test Sale Transaction 2",
                CreatedByUserID = null,
                Mode = clsTransaction.enMode.AddNew
            };

            Console.WriteLine("Creating Transaction 2:");
            Console.WriteLine($"   - Transaction Date: {transaction2.TransactionDate:yyyy-MM-dd HH:mm:ss}");
            Console.WriteLine($"   - Type: {transaction2.TransactionType}");
            Console.WriteLine($"   - Status: {transaction2.TransactionStatus}");
            Console.WriteLine($"   - Total Amount: {transaction2.TotalAmount:C2}");
            Console.WriteLine($"   - Notes: {transaction2.Nots}");

            if (transaction2.Save())
            {
                Console.WriteLine($"SUCCESS! Transaction 2 ID: {transaction2.TransactionID}\n");
                _testTransactionIDs.Add(transaction2.TransactionID);
                Console.WriteLine($"Total Transactions Created: {_testTransactionIDs.Count}");
            }
            else
            {
                Console.WriteLine("FAILED! Could not add Transaction 2");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"ERROR: {ex.Message}");
        }
    }

    private static void TestAddPaymentWithAllocations()
    {
        Console.WriteLine("### 1. Testing Add Payment with Allocations ###\n");

        if (_testTransactionIDs.Count == 0)
        {
            Console.WriteLine("WARNING: No Transactions created (previous add failed)");
            return;
        }

        try
        {
            // Create new payment
            var newPayment = new clsPayment
            {
                PaymentDate = DateTime.Now,
                PaymentMethodID = 4,
                PaymentAmount = 1000.00m,
                Notes = "Test payment from suppliers",
                CreatedByUserID = null,
                Mode = clsPayment.enMode.AddNew
            };

            // Add allocations using created transactions
            newPayment.Allocations.Add(new clsPaymentAllocation
            {
                TransactionID = _testTransactionIDs[0],
                Amount = 600.00m
            });

            newPayment.Allocations.Add(new clsPaymentAllocation
            {
                TransactionID = _testTransactionIDs[1],
                Amount = 400.00m
            });

            Console.WriteLine("Payment Data to Add:");
            Console.WriteLine($"   - Payment Date: {newPayment.PaymentDate:yyyy-MM-dd HH:mm:ss}");
            Console.WriteLine($"   - Payment Method ID: {newPayment.PaymentMethodID}");
            Console.WriteLine($"   - Total Amount: {newPayment.PaymentAmount:C2}");
            Console.WriteLine($"   - Notes: {newPayment.Notes}");
            Console.WriteLine($"   - Created by User ID: {newPayment.CreatedByUserID}");
            Console.WriteLine($"\n   Allocations Details:");
            foreach (var alloc in newPayment.Allocations)
            {
                Console.WriteLine($"      * Transaction ID: {alloc.TransactionID}, Amount: {alloc.Amount:C2}");
            }

            // Save payment
            Console.WriteLine("\nSaving Payment...");
            if (newPayment.Save())
            {
                Console.WriteLine($"SUCCESS! Payment ID: {newPayment.PaymentID}");
                
                // Save ID for later tests
                _testPaymentID = newPayment.PaymentID;
            }
            else
            {
                Console.WriteLine("FAILED! Could not add payment!");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"ERROR: {ex.Message}");
        }
    }

    private static void TestReadPayment()
    {
        Console.WriteLine("### 2. Testing Read Payment and Verify Details ###\n");

        if (_testPaymentID == -1)
        {
            Console.WriteLine("WARNING: No Payment ID to read (previous add failed)");
            return;
        }

        try
        {
            Console.WriteLine($"Searching for Payment ID: {_testPaymentID}...");
            
            var payment = clsPayment.Find(_testPaymentID);

            if (payment != null)
            {
                Console.WriteLine("SUCCESS! Payment found!\n");

                Console.WriteLine("Payment Data:");
                Console.WriteLine($"   - Payment ID: {payment.PaymentID}");
                Console.WriteLine($"   - Payment Date: {payment.PaymentDate:yyyy-MM-dd HH:mm:ss}");
                Console.WriteLine($"   - Payment Method ID: {payment.PaymentMethodID}");
                Console.WriteLine($"   - Total Amount: {payment.PaymentAmount:C2}");
                Console.WriteLine($"   - Notes: {payment.Notes}");
                Console.WriteLine($"   - Created Date: {payment.CreatedDate:yyyy-MM-dd HH:mm:ss}");
                Console.WriteLine($"   - Created by User ID: {payment.CreatedByUserID}");

                Console.WriteLine($"\nAllocations Count: {payment.Allocations.Count}");
                if (payment.Allocations.Count > 0)
                {
                    Console.WriteLine("   Allocations Details:");
                    decimal totalAllocated = 0;
                    foreach (var alloc in payment.Allocations)
                    {
                        Console.WriteLine($"      * Allocation ID: {alloc.AllocationID}");
                        Console.WriteLine($"        - Transaction ID: {alloc.TransactionID}");
                        Console.WriteLine($"        - Amount: {alloc.Amount:C2}");
                        Console.WriteLine($"        - Created Date: {alloc.CreatedDate:yyyy-MM-dd HH:mm:ss}");
                        totalAllocated += alloc.Amount;
                    }
                    Console.WriteLine($"\n      Total Allocated: {totalAllocated:C2}");
                    string matchStatus = totalAllocated == payment.PaymentAmount ? "MATCH with Total Amount" : "MISMATCH Warning!";
                    Console.WriteLine($"      {matchStatus}");
                }
            }
            else
            {
                Console.WriteLine("FAILED! Payment not found!");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"ERROR: {ex.Message}");
        }
    }

    private static void TestUpdatePayment()
    {
        Console.WriteLine("### 3. Testing Update Payment ###\n");

        if (_testPaymentID == -1)
        {
            Console.WriteLine("WARNING: No Payment ID to update (previous add failed)");
            return;
        }

        try
        {
            Console.WriteLine($"Searching for Payment ID: {_testPaymentID}...");
            
            var payment = clsPayment.Find(_testPaymentID);

            if (payment != null)
            {
                Console.WriteLine("SUCCESS! Payment found for update\n");

                Console.WriteLine("Old Data:");
                Console.WriteLine($"   - Amount: {payment.PaymentAmount:C2}");
                Console.WriteLine($"   - Notes: {payment.Notes}");

                // Update data
                payment.PaymentAmount = 1500.00m;
                payment.Notes = "Updated Notes - Modified payment";
                payment.UpdatedByUserID = 2;

                Console.WriteLine("\nNew Data:");
                Console.WriteLine($"   - Amount: {payment.PaymentAmount:C2}");
                Console.WriteLine($"   - Notes: {payment.Notes}");

                Console.WriteLine("\nSaving Updates...");
                if (payment.Save())
                {
                    Console.WriteLine("SUCCESS! Payment updated!\n");

                    // Verify updates by reading again
                    Console.WriteLine("Re-reading to verify updates...");
                    var updatedPayment = clsPayment.Find(_testPaymentID);
                    if (updatedPayment != null)
                    {
                        Console.WriteLine("SUCCESS! Updates verified:");
                        Console.WriteLine($"   - New Amount: {updatedPayment.PaymentAmount:C2}");
                        Console.WriteLine($"   - New Notes: {updatedPayment.Notes}");
                        Console.WriteLine($"   - Updated by User ID: {updatedPayment.UpdatedByUserID}");
                    }
                }
                else
                {
                    Console.WriteLine("FAILED! Could not update payment!");
                }
            }
            else
            {
                Console.WriteLine("FAILED! Payment not found!");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"ERROR: {ex.Message}");
        }
    }

    private static void TestDeletePayment()
    {
        Console.WriteLine("### 4. Testing Delete Payment and Verification ###\n");

        if (_testPaymentID == -1)
        {
            Console.WriteLine("WARNING: No Payment ID to delete (previous add failed)");
            return;
        }

        try
        {
            Console.WriteLine($"Searching for Payment ID: {_testPaymentID} before delete...");
            
            var paymentBefore = clsPayment.Find(_testPaymentID);
            if (paymentBefore != null)
            {
                Console.WriteLine("SUCCESS! Payment found before delete");
                Console.WriteLine($"   - ID: {paymentBefore.PaymentID}");
                Console.WriteLine($"   - Amount: {paymentBefore.PaymentAmount:C2}");
                Console.WriteLine($"   - Allocations Count: {paymentBefore.Allocations.Count}");
            }

            Console.WriteLine($"\nDeleting Payment ID: {_testPaymentID}...");
            if (clsPayment.DeletePayment(_testPaymentID, 2))
            {
                Console.WriteLine("SUCCESS! Payment deleted!\n");

                // Verify deletion by trying to read
                Console.WriteLine("Re-reading after delete to verify...");
                var paymentAfter = clsPayment.Find(_testPaymentID);
                
                if (paymentAfter == null)
                {
                    Console.WriteLine("SUCCESS! Verification passed: Payment successfully deleted from database!");
                }
                else
                {
                    Console.WriteLine("WARNING! Payment still exists in database after delete!");
                    Console.WriteLine($"   - ID: {paymentAfter.PaymentID}");
                    Console.WriteLine($"   - Amount: {paymentAfter.PaymentAmount:C2}");
                }
            }
            else
            {
                Console.WriteLine("FAILED! Could not delete payment!");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"ERROR: {ex.Message}");
        }
    }

    private static int _testPaymentID = -1;
    private static List<int> _testTransactionIDs = new List<int>();
}
