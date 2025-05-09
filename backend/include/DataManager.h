
#pragma once
#include <string>
#include <vector>
#include <fstream>
#include <memory>
#include "Transaction.h"
#include "Category.h"
#include "Budget.h"

#include <nlohmann/json.hpp>

using json = nlohmann::json;

class DataManager
{
private:
    std::string dataPath;
    std::vector<Transaction> transactions;
    std::vector<Category> categories;
    std::vector<Budget> budgets;
    int nextTransactionId;
    int nextCategoryId;

    // File paths
    std::string getTransactionsFilePath() const;
    std::string getCategoriesFilePath() const;
    std::string getBudgetsFilePath() const;

    // File operations
    bool saveToFile(const std::string &filePath, const json &jsonData) const;
    bool loadFromFile(const std::string &filePath, json &jsonData) const;

public:
    DataManager(const std::string &dataPath);

    // Category operations
    bool addCategory(Category &category);
    bool updateCategory(const Category &category);
    bool deleteCategory(int categoryId);
    Category *getCategoryById(int categoryId);
    std::vector<Category> getAllCategories() const;

    // Transaction operations
    bool addTransaction(Transaction &transaction);
    bool updateTransaction(const Transaction &transaction);
    bool deleteTransaction(int transactionId);
    Transaction *getTransactionById(int transactionId);
    std::vector<Transaction> getAllTransactions() const;
    std::vector<Transaction> getTransactionsByCategory(int categoryId) const;
    std::vector<Transaction> getTransactionsByMonth(const std::string &monthYear) const;

    // Budget operations
    bool addBudget(Budget &budget);
    bool updateBudget(const Budget &budget);
    bool deleteBudget(int categoryId, const std::string &monthYear);
    Budget *getBudget(int categoryId, const std::string &monthYear);
    std::vector<Budget> getAllBudgets() const;
    std::vector<Budget> getBudgetsByMonth(const std::string &monthYear) const;

    // File operations
    bool saveAllData();
    bool loadAllData();

    // Analysis functions
    double getTotalIncome(const std::string &monthYear) const;
    double getTotalExpense(const std::string &monthYear) const;
    double getCategoryTotal(int categoryId, const std::string &monthYear) const;
    std::map<int, double> getCategoryTotals(const std::string &monthYear) const;
    std::map<std::string, double> getMonthlyTotals(bool isIncome) const;
};

// JSON Serialization helpers - these functions will be outside the class
void to_json(json &j, const Transaction &transaction);
void from_json(const json &j, Transaction &transaction);

void to_json(json &j, const Category &category);
void from_json(const json &j, Category &category);

void to_json(json &j, const Budget &budget);
void from_json(const json &j, Budget &budget);