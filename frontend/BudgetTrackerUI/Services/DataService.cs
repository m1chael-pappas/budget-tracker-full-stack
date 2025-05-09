using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using BudgetTrackerUI.Models;

namespace BudgetTrackerUI.Services
{
    public class DataService
    {
        private string _dataPath;
        private JsonSerializerOptions _jsonOptions;

        public DataService(string dataPath)
        {
            _dataPath = dataPath;
            _jsonOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                WriteIndented = true
            };

            // Create data directory if it doesn't exist
            if (!Directory.Exists(_dataPath))
            {
                Directory.CreateDirectory(_dataPath);
            }
        }

        #region File Paths

        private string TransactionsFilePath => Path.Combine(_dataPath, "transactions.json");
        private string CategoriesFilePath => Path.Combine(_dataPath, "categories.json");
        private string BudgetsFilePath => Path.Combine(_dataPath, "budgets.json");

        #endregion

        #region Categories

        public List<Category> GetAllCategories()
        {
            return LoadFromFile<List<Category>>(CategoriesFilePath) ?? new List<Category>();
        }

        public Category? GetCategoryById(int id)
        {
            var categories = GetAllCategories();
            return categories.FirstOrDefault(c => c.Id == id);
        }

        public bool SaveCategories(List<Category> categories)
        {
            return SaveToFile(CategoriesFilePath, categories);
        }

        public int AddCategory(Category category)
        {
            var categories = GetAllCategories();

            // Assign a new ID if needed
            if (category.Id <= 0)
            {
                category.Id = categories.Count > 0 ? categories.Max(c => c.Id) + 1 : 1;
            }

            // Check if category with this ID already exists
            if (categories.Any(c => c.Id == category.Id))
            {
                return -1; // Category ID already exists
            }

            categories.Add(category);
            SaveCategories(categories);
            return category.Id;
        }

        public bool UpdateCategory(Category category)
        {
            var categories = GetAllCategories();
            var index = categories.FindIndex(c => c.Id == category.Id);

            if (index < 0)
            {
                return false; // Category not found
            }

            categories[index] = category;
            return SaveCategories(categories);
        }

        public bool DeleteCategory(int categoryId)
        {
            var categories = GetAllCategories();
            var category = categories.FirstOrDefault(c => c.Id == categoryId);

            if (category == null)
            {
                return false; // Category not found
            }

            categories.Remove(category);
            return SaveCategories(categories);
        }

        #endregion

        #region Transactions

        public List<Transaction> GetAllTransactions()
        {
            var transactions = LoadFromFile<List<Transaction>>(TransactionsFilePath) ?? new List<Transaction>();

            // Populate category names
            var categories = GetAllCategories();
            foreach (var transaction in transactions)
            {
                var category = categories.FirstOrDefault(c => c.Id == transaction.CategoryId);
                transaction.CategoryName = category?.Name ?? "Uncategorized";
            }

            return transactions;
        }

        public Transaction? GetTransactionById(int id)
        {
            var transactions = GetAllTransactions();
            return transactions.FirstOrDefault(t => t.Id == id);
        }

        public bool SaveTransactions(List<Transaction> transactions)
        {
            return SaveToFile(TransactionsFilePath, transactions);
        }

        public int AddTransaction(Transaction transaction)
        {
            var transactions = GetAllTransactions();

            // Assign a new ID if needed
            if (transaction.Id <= 0)
            {
                transaction.Id = transactions.Count > 0 ? transactions.Max(t => t.Id) + 1 : 1;
            }

            // Check if transaction with this ID already exists
            if (transactions.Any(t => t.Id == transaction.Id))
            {
                return -1; // Transaction ID already exists
            }

            transactions.Add(transaction);
            SaveTransactions(transactions);
            return transaction.Id;
        }

        public bool UpdateTransaction(Transaction transaction)
        {
            var transactions = GetAllTransactions();
            var index = transactions.FindIndex(t => t.Id == transaction.Id);

            if (index < 0)
            {
                return false; // Transaction not found
            }

            transactions[index] = transaction;
            return SaveTransactions(transactions);
        }

        public bool DeleteTransaction(int transactionId)
        {
            var transactions = GetAllTransactions();
            var transaction = transactions.FirstOrDefault(t => t.Id == transactionId);

            if (transaction == null)
            {
                return false; // Transaction not found
            }

            transactions.Remove(transaction);
            return SaveTransactions(transactions);
        }

        public List<Transaction> GetTransactionsByCategory(int categoryId)
        {
            return GetAllTransactions().Where(t => t.CategoryId == categoryId).ToList();
        }

        public List<Transaction> GetTransactionsByMonth(string monthYear)
        {
            return GetAllTransactions().Where(t => t.Date.StartsWith(monthYear)).ToList();
        }

        #endregion

        #region Budgets

        public List<Budget> GetAllBudgets()
        {
            var budgets = LoadFromFile<List<Budget>>(BudgetsFilePath) ?? new List<Budget>();

            // Populate category names
            var categories = GetAllCategories();
            foreach (var budget in budgets)
            {
                var category = categories.FirstOrDefault(c => c.Id == budget.CategoryId);
                budget.CategoryName = category?.Name ?? "Uncategorized";
            }

            return budgets;
        }

        public Budget? GetBudget(int categoryId, string monthYear)
        {
            var budgets = GetAllBudgets();
            return budgets.FirstOrDefault(b => b.CategoryId == categoryId && b.MonthYear == monthYear);
        }

        public bool SaveBudgets(List<Budget> budgets)
        {
            return SaveToFile(BudgetsFilePath, budgets);
        }

        public bool AddBudget(Budget budget)
        {
            var budgets = GetAllBudgets();

            // Check if budget for this category and month already exists
            if (budgets.Any(b => b.CategoryId == budget.CategoryId && b.MonthYear == budget.MonthYear))
            {
                return false; // Budget already exists
            }

            budgets.Add(budget);
            return SaveBudgets(budgets);
        }

        public bool UpdateBudget(Budget budget)
        {
            var budgets = GetAllBudgets();
            var index = budgets.FindIndex(b => b.CategoryId == budget.CategoryId && b.MonthYear == budget.MonthYear);

            if (index < 0)
            {
                return false; // Budget not found
            }

            budgets[index] = budget;
            return SaveBudgets(budgets);
        }

        public bool DeleteBudget(int categoryId, string monthYear)
        {
            var budgets = GetAllBudgets();
            var budget = budgets.FirstOrDefault(b => b.CategoryId == categoryId && b.MonthYear == monthYear);

            if (budget == null)
            {
                return false; // Budget not found
            }

            budgets.Remove(budget);
            return SaveBudgets(budgets);
        }

        public List<Budget> GetBudgetsByMonth(string monthYear)
        {
            return GetAllBudgets().Where(b => b.MonthYear == monthYear).ToList();
        }

        #endregion

        #region Analysis

        public double GetTotalIncome(string monthYear)
        {
            return GetTransactionsByMonth(monthYear)
                .Where(t => t.IsIncome)
                .Sum(t => t.Amount);
        }

        public double GetTotalExpense(string monthYear)
        {
            return GetTransactionsByMonth(monthYear)
                .Where(t => !t.IsIncome)
                .Sum(t => t.Amount);
        }

        public Dictionary<int, double> GetCategoryTotals(string monthYear)
        {
            var result = new Dictionary<int, double>();
            var categories = GetAllCategories();
            var transactions = GetTransactionsByMonth(monthYear);

            // Initialize totals for all categories
            foreach (var category in categories)
            {
                result[category.Id] = 0.0;
            }

            // Add up transactions
            foreach (var transaction in transactions)
            {
                if (transaction.IsIncome)
                {
                    if (result.ContainsKey(transaction.CategoryId))
                    {
                        result[transaction.CategoryId] += transaction.Amount;
                    }
                }
                else
                {
                    if (result.ContainsKey(transaction.CategoryId))
                    {
                        result[transaction.CategoryId] -= transaction.Amount;
                    }
                }
            }

            return result;
        }

        public Dictionary<string, double> GetMonthlyTotals(bool isIncome)
        {
            var result = new Dictionary<string, double>();
            var transactions = GetAllTransactions();

            foreach (var transaction in transactions)
            {
                if (transaction.IsIncome == isIncome)
                {
                    string month = transaction.Date.Substring(0, 7); // Extract YYYY-MM

                    if (result.ContainsKey(month))
                    {
                        result[month] += transaction.Amount;
                    }
                    else
                    {
                        result[month] = transaction.Amount;
                    }
                }
            }

            return result;
        }

        #endregion

        #region File Operations

        private T? LoadFromFile<T>(string filePath)
        {
            if (!File.Exists(filePath))
            {
                return default;
            }

            try
            {
                var json = File.ReadAllText(filePath);
                return JsonSerializer.Deserialize<T>(json, _jsonOptions);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading from file: {ex.Message}");
                return default;
            }
        }

        private bool SaveToFile<T>(string filePath, T data)
        {
            try
            {
                var json = JsonSerializer.Serialize(data, _jsonOptions);
                File.WriteAllText(filePath, json);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving to file: {ex.Message}");
                return false;
            }
        }

        #endregion


        // Add this method to your DataService.cs
        public void CreateSampleData()
        {
            // Check if we already have categories
            var categories = GetAllCategories();
            if (categories.Count == 0)
            {
                // Add sample categories
                var sampleCategories = new List<Category>
        {
            new Category { Id = 1, Name = "Food", Description = "Groceries and dining out", Color = "#4CAF50" },
            new Category { Id = 2, Name = "Transportation", Description = "Gas, public transit, etc.", Color = "#2196F3" },
            new Category { Id = 3, Name = "Entertainment", Description = "Movies, games, etc.", Color = "#9C27B0" },
            new Category { Id = 4, Name = "Utilities", Description = "Electricity, water, internet, etc.", Color = "#F44336" },
            new Category { Id = 5, Name = "Salary", Description = "Income from employment", Color = "#FFC107" }
        };

                foreach (var category in sampleCategories)
                {
                    AddCategory(category);
                }
            }

            // Check if we already have transactions
            var transactions = GetAllTransactions();
            if (transactions.Count == 0)
            {
                // Add sample transactions
                var currentDate = DateTime.Now;
                var currentMonthYear = currentDate.ToString("yyyy-MM");
                var lastMonthDate = currentDate.AddMonths(-1);
                var lastMonthYear = lastMonthDate.ToString("yyyy-MM");

                var sampleTransactions = new List<Transaction>
        {
            // Current month transactions
            new Transaction {
                Id = 1,
                Date = $"{currentMonthYear}-01",
                Amount = 2000.00,
                Description = "Monthly Salary",
                CategoryId = 5,
                IsIncome = true,
                CategoryName = "Salary"
            },
            new Transaction {
                Id = 2,
                Date = $"{currentMonthYear}-05",
                Amount = 85.75,
                Description = "Grocery Shopping",
                CategoryId = 1,
                IsIncome = false,
                CategoryName = "Food"
            },
            new Transaction {
                Id = 3,
                Date = $"{currentMonthYear}-08",
                Amount = 45.00,
                Description = "Gas",
                CategoryId = 2,
                IsIncome = false,
                CategoryName = "Transportation"
            },
            new Transaction {
                Id = 4,
                Date = $"{currentMonthYear}-12",
                Amount = 120.00,
                Description = "Internet Bill",
                CategoryId = 4,
                IsIncome = false,
                CategoryName = "Utilities"
            },
            new Transaction {
                Id = 5,
                Date = $"{currentMonthYear}-15",
                Amount = 65.50,
                Description = "Movie Night",
                CategoryId = 3,
                IsIncome = false,
                CategoryName = "Entertainment"
            },

            // Last month transactions
            new Transaction {
                Id = 6,
                Date = $"{lastMonthYear}-01",
                Amount = 2000.00,
                Description = "Monthly Salary",
                CategoryId = 5,
                IsIncome = true,
                CategoryName = "Salary"
            },
            new Transaction {
                Id = 7,
                Date = $"{lastMonthYear}-10",
                Amount = 95.20,
                Description = "Grocery Shopping",
                CategoryId = 1,
                IsIncome = false,
                CategoryName = "Food"
            },
            new Transaction {
                Id = 8,
                Date = $"{lastMonthYear}-20",
                Amount = 50.00,
                Description = "Gas",
                CategoryId = 2,
                IsIncome = false,
                CategoryName = "Transportation"
            }
        };

                foreach (var transaction in sampleTransactions)
                {
                    AddTransaction(transaction);
                }
            }
        }
    }
}