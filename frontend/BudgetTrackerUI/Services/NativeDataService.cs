using System;
using System.Runtime.InteropServices;
using System.Text.Json;
using System.Collections.Generic;
using System.IO;
using BudgetTrackerUI.Models;

namespace BudgetTrackerUI.Services
{
    public class NativeDataService : IDataService, IDisposable
    {
        private IntPtr _dataManager;
        private bool _disposed = false;
        private bool _ownsDataManager = true; // Flag to indicate if this instance owns the data manager

        // DLL imports - with safer string handling
        private const string LibraryPath = "lib/libBudgetTrackerLib.so";

        [DllImport(LibraryPath, CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr CreateDataManager([MarshalAs(UnmanagedType.LPStr)] string dataPath);

        [DllImport(LibraryPath, CallingConvention = CallingConvention.Cdecl)]
        private static extern void DestroyDataManager(IntPtr manager);

        [DllImport(LibraryPath, CallingConvention = CallingConvention.Cdecl)]
        private static extern int AddCategory(IntPtr manager,
            [MarshalAs(UnmanagedType.LPStr)] string name,
            [MarshalAs(UnmanagedType.LPStr)] string description,
            [MarshalAs(UnmanagedType.LPStr)] string color);

        [DllImport(LibraryPath, CallingConvention = CallingConvention.Cdecl)]
        private static extern bool UpdateCategory(IntPtr manager, int id,
            [MarshalAs(UnmanagedType.LPStr)] string name,
            [MarshalAs(UnmanagedType.LPStr)] string description,
            [MarshalAs(UnmanagedType.LPStr)] string color);

        [DllImport(LibraryPath, CallingConvention = CallingConvention.Cdecl)]
        private static extern bool DeleteCategory(IntPtr manager, int categoryId);

        [DllImport(LibraryPath, CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr GetAllCategories(IntPtr manager);

        [DllImport(LibraryPath, CallingConvention = CallingConvention.Cdecl)]
        private static extern int AddTransaction(IntPtr manager,
            [MarshalAs(UnmanagedType.LPStr)] string date,
            double amount,
            [MarshalAs(UnmanagedType.LPStr)] string description,
            int categoryId,
            [MarshalAs(UnmanagedType.I1)] bool isIncome);

        [DllImport(LibraryPath, CallingConvention = CallingConvention.Cdecl)]
        private static extern bool UpdateTransaction(IntPtr manager, int id,
            [MarshalAs(UnmanagedType.LPStr)] string date,
            double amount,
            [MarshalAs(UnmanagedType.LPStr)] string description,
            int categoryId,
            [MarshalAs(UnmanagedType.I1)] bool isIncome);

        [DllImport(LibraryPath, CallingConvention = CallingConvention.Cdecl)]
        private static extern bool DeleteTransaction(IntPtr manager, int transactionId);

        [DllImport(LibraryPath, CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr GetAllTransactions(IntPtr manager);

        [DllImport(LibraryPath, CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr GetTransactionsByMonth(IntPtr manager,
            [MarshalAs(UnmanagedType.LPStr)] string monthYear);

        [DllImport(LibraryPath, CallingConvention = CallingConvention.Cdecl)]
        private static extern double GetTotalIncome(IntPtr manager,
            [MarshalAs(UnmanagedType.LPStr)] string monthYear);

        [DllImport(LibraryPath, CallingConvention = CallingConvention.Cdecl)]
        private static extern double GetTotalExpense(IntPtr manager,
            [MarshalAs(UnmanagedType.LPStr)] string monthYear);

        private static string PtrToStringUTF8(IntPtr ptr)
        {
            if (ptr == IntPtr.Zero)
                return "[]";

            try
            {
                return Marshal.PtrToStringUTF8(ptr) ?? "[]";
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error converting native string: {ex.Message}");
                return "[]";
            }
        }

        public NativeDataService(string dataPath)
        {
            if (!Directory.Exists(dataPath))
            {
                Directory.CreateDirectory(dataPath);
            }

            try
            {
                Console.WriteLine($"Initializing NativeDataService with path: {dataPath}");
                _dataManager = CreateDataManager(dataPath);
                if (_dataManager == IntPtr.Zero)
                {
                    throw new InvalidOperationException("Failed to create DataManager instance");
                }
                Console.WriteLine("DataManager created successfully");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error initializing NativeDataService: {ex.Message}");
                _dataManager = IntPtr.Zero;
                throw;
            }
        }

        // Constructor 
        internal NativeDataService(IntPtr dataManager, bool takeOwnership = false)
        {
            _dataManager = dataManager;
            _ownsDataManager = takeOwnership;
            Console.WriteLine($"Created NativeDataService with existing data manager (ownership: {_ownsDataManager})");
        }

        // Category operations
        public int AddCategory(Category category)
        {
            return AddCategory(_dataManager, category.Name, category.Description, category.Color);
        }

        public bool UpdateCategory(Category category)
        {
            return UpdateCategory(_dataManager, category.Id, category.Name, category.Description, category.Color);
        }

        public bool DeleteCategory(int categoryId)
        {
            return DeleteCategory(_dataManager, categoryId);
        }

        public List<Category> GetAllCategories()
        {
            try
            {
                IntPtr resultPtr = GetAllCategories(_dataManager);
                string jsonStr = PtrToStringUTF8(resultPtr);

                if (string.IsNullOrEmpty(jsonStr))
                {
                    return new List<Category>();
                }

                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };
                return JsonSerializer.Deserialize<List<Category>>(jsonStr, options) ?? new List<Category>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetAllCategories: {ex.Message}");
                return new List<Category>();
            }
        }

        // Transaction operations
        public void AddTransaction(Transaction transaction)
        {
            int id = AddTransaction(
                _dataManager,
                transaction.Date,
                transaction.Amount,
                transaction.Description,
                transaction.CategoryId,
                transaction.IsIncome);

            transaction.Id = id;
        }

        public bool UpdateTransaction(Transaction transaction)
        {
            return UpdateTransaction(
                _dataManager,
                transaction.Id,
                transaction.Date,
                transaction.Amount,
                transaction.Description,
                transaction.CategoryId,
                transaction.IsIncome);
        }

        public bool DeleteTransaction(int transactionId)
        {
            return DeleteTransaction(_dataManager, transactionId);
        }

        public List<Transaction> GetAllTransactions()
        {
            try
            {
                IntPtr resultPtr = GetAllTransactions(_dataManager);
                string jsonStr = PtrToStringUTF8(resultPtr);

                if (string.IsNullOrEmpty(jsonStr) || jsonStr == "[]")
                {
                    return new List<Transaction>();
                }

                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };
                return JsonSerializer.Deserialize<List<Transaction>>(jsonStr, options) ?? new List<Transaction>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetAllTransactions: {ex.Message}");
                return new List<Transaction>();
            }
        }

        public List<Transaction> GetTransactionsByMonth(string monthYear)
        {
            try
            {
                IntPtr resultPtr = GetTransactionsByMonth(_dataManager, monthYear);
                string jsonStr = PtrToStringUTF8(resultPtr);

                if (string.IsNullOrEmpty(jsonStr) || jsonStr == "[]")
                {
                    return new List<Transaction>();
                }

                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };
                return JsonSerializer.Deserialize<List<Transaction>>(jsonStr, options) ?? new List<Transaction>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetTransactionsByMonth: {ex.Message}");
                return new List<Transaction>();
            }
        }


        public Transaction? GetTransactionById(int id)
        {
            var allTransactions = GetAllTransactions();
            return allTransactions.Find(t => t.Id == id);
        }

        public Category? GetCategoryById(int id)
        {
            var allCategories = GetAllCategories();
            return allCategories.Find(c => c.Id == id);
        }

        public List<Transaction> GetTransactionsByCategory(int categoryId)
        {
            var allTransactions = GetAllTransactions();
            return allTransactions.FindAll(t => t.CategoryId == categoryId);
        }

        // Analysis functions
        public double GetTotalIncome(string monthYear)
        {
            return GetTotalIncome(_dataManager, monthYear);
        }

        public double GetTotalExpense(string monthYear)
        {
            return GetTotalExpense(_dataManager, monthYear);
        }

        // Implement IDisposable
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                try
                {
                    if (_dataManager != IntPtr.Zero && _ownsDataManager)
                    {
                        Console.WriteLine("Disposing NativeDataService - destroying DataManager");
                        DestroyDataManager(_dataManager);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error disposing NativeDataService: {ex.Message}");
                }
                finally
                {
                    _dataManager = IntPtr.Zero;
                    _disposed = true;
                }
            }
        }

        ~NativeDataService()
        {
            Dispose(false);
        }
    }
}