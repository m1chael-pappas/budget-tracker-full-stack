#include <iostream>
#include <string>
#include "../include/Transaction.h"
#include "../include/Category.h"
#include "../include/Budget.h"
#include "../include/DataManager.h"

/**
 * @brief Main function for testing the budget tracking system.
 *
 * This function creates a DataManager instance, adds categories, transactions,
 * and budgets, and performs various operations to test the functionality of the
 * system. It also demonstrates the financial analysis capabilities and data
 * persistence by reloading the data.
 */
int main()
{
    // Create a data manager with a data directory
    DataManager dataManager("./data");

    // Test category operations
    std::cout << "Testing Category Operations:" << std::endl;
    Category groceries(0, "Groceries", "Food and household items", "#4CAF50");
    Category utilities(0, "Utilities", "Bills and utilities", "#2196F3");
    Category entertainment(0, "Entertainment", "Movies, games, etc.", "#F44336");

    // Add categories
    if (dataManager.addCategory(groceries))
    {
        std::cout << "Added category: " << groceries.toString() << std::endl;
    }

    if (dataManager.addCategory(utilities))
    {
        std::cout << "Added category: " << utilities.toString() << std::endl;
    }

    if (dataManager.addCategory(entertainment))
    {
        std::cout << "Added category: " << entertainment.toString() << std::endl;
    }

    // Print all categories
    std::cout << "\nAll Categories:" << std::endl;
    for (const auto &category : dataManager.getAllCategories())
    {
        std::cout << category.toString() << std::endl;
    }

    // Test transaction operations
    std::cout << "\nTesting Transaction Operations:" << std::endl;
    Transaction groceryTrip(0, "2025-05-08", 45.67, "Weekly groceries", groceries.getId(), false);
    Transaction salary(0, "2025-05-01", 3000.00, "Monthly salary", 0, true);
    Transaction rentPayment(0, "2025-05-05", 1200.00, "Rent payment", utilities.getId(), false);

    // Add transactions
    if (dataManager.addTransaction(groceryTrip))
    {
        std::cout << "Added transaction: " << groceryTrip.toString() << std::endl;
    }

    if (dataManager.addTransaction(salary))
    {
        std::cout << "Added transaction: " << salary.toString() << std::endl;
    }

    if (dataManager.addTransaction(rentPayment))
    {
        std::cout << "Added transaction: " << rentPayment.toString() << std::endl;
    }

    // Print all transactions
    std::cout << "\nAll Transactions:" << std::endl;
    for (const auto &transaction : dataManager.getAllTransactions())
    {
        std::cout << transaction.toString() << std::endl;
    }

    // Test budget operations
    std::cout << "\nTesting Budget Operations:" << std::endl;
    Budget groceryBudget(groceries.getId(), "2025-05", 300.00);
    Budget utilitiesBudget(utilities.getId(), "2025-05", 1500.00);
    Budget entertainmentBudget(entertainment.getId(), "2025-05", 200.00);

    // Add budgets
    if (dataManager.addBudget(groceryBudget))
    {
        std::cout << "Added budget: " << groceryBudget.toString() << std::endl;
    }

    if (dataManager.addBudget(utilitiesBudget))
    {
        std::cout << "Added budget: " << utilitiesBudget.toString() << std::endl;
    }

    if (dataManager.addBudget(entertainmentBudget))
    {
        std::cout << "Added budget: " << entertainmentBudget.toString() << std::endl;
    }

    // Print all budgets
    std::cout << "\nAll Budgets:" << std::endl;
    for (const auto &budget : dataManager.getAllBudgets())
    {
        std::cout << budget.toString() << std::endl;
    }

    // Test financial analysis functions
    std::cout << "\nFinancial Analysis for May 2025:" << std::endl;
    std::cout << "Total Income: $" << dataManager.getTotalIncome("2025-05") << std::endl;
    std::cout << "Total Expenses: $" << dataManager.getTotalExpense("2025-05") << std::endl;
    std::cout << "Net: $" << (dataManager.getTotalIncome("2025-05") - dataManager.getTotalExpense("2025-05")) << std::endl;

    std::cout << "\nCategory Spending:" << std::endl;
    auto categoryTotals = dataManager.getCategoryTotals("2025-05");
    for (const auto &category : dataManager.getAllCategories())
    {
        auto it = categoryTotals.find(category.getId());
        if (it != categoryTotals.end())
        {
            std::cout << category.getName() << ": $" << it->second << std::endl;
        }
    }

    // Test data persistence by reloading
    std::cout << "\nTesting Data Persistence:" << std::endl;

    DataManager newDataManager("./data");
    std::cout << "Categories after reload: " << newDataManager.getAllCategories().size() << std::endl;
    std::cout << "Transactions after reload: " << newDataManager.getAllTransactions().size() << std::endl;
    std::cout << "Budgets after reload: " << newDataManager.getAllBudgets().size() << std::endl;

    return 0;
}