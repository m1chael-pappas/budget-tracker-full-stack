
using System.Collections.Generic;
using BudgetTrackerUI.Models;

namespace BudgetTrackerUI.Services
{
    public interface IDataService
    {
        // Category operations
        int AddCategory(Category category);
        bool UpdateCategory(Category category);
        bool DeleteCategory(int categoryId);
        List<Category> GetAllCategories();
        Category? GetCategoryById(int id);

        // Transaction operations
        void AddTransaction(Transaction transaction);
        bool UpdateTransaction(Transaction transaction);
        bool DeleteTransaction(int transactionId);
        List<Transaction> GetAllTransactions();
        Transaction? GetTransactionById(int id);
        List<Transaction> GetTransactionsByCategory(int categoryId);
        List<Transaction> GetTransactionsByMonth(string monthYear);

        // Analysis
        double GetTotalIncome(string monthYear);
        double GetTotalExpense(string monthYear);
    }
}