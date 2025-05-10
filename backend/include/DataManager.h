/**
 * @file DataManager.h
 * @brief Defines the DataManager class for handling all data operations.
 *
 * The DataManager is the central component that handles saving, loading, and
 * managing transactions, categories, and budgets. It provides methods for
 * creating, reading, updating, and deleting data, as well as analysis functions.
 */
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

/**
 * @class DataManager
 * @brief Manages all data operations for the budget tracking system.
 *
 * This class is responsible for storing and retrieving transaction, category,
 * and budget data to/from persistent storage. It also provides methods for
 * financial analysis of the stored data.
 */
class DataManager
{
private:
    std::string dataPath;                  /**< Directory path where data files are stored */
    std::vector<Transaction> transactions; /**< In-memory cache of all transactions */
    std::vector<Category> categories;      /**< In-memory cache of all categories */
    std::vector<Budget> budgets;           /**< In-memory cache of all budgets */
    int nextTransactionId;                 /**< Next available ID for new transactions */
    int nextCategoryId;                    /**< Next available ID for new categories */

    /**
     * @brief Gets the file path for transactions data.
     * @return The absolute path to the transactions JSON file
     */
    std::string getTransactionsFilePath() const;

    /**
     * @brief Gets the file path for categories data.
     * @return The absolute path to the categories JSON file
     */
    std::string getCategoriesFilePath() const;

    /**
     * @brief Gets the file path for budgets data.
     * @return The absolute path to the budgets JSON file
     */
    std::string getBudgetsFilePath() const;

    /**
     * @brief Saves JSON data to a file.
     *
     * @param filePath Path to the file where data will be saved
     * @param jsonData The JSON data to save
     * @return true if the operation was successful, false otherwise
     */
    bool saveToFile(const std::string &filePath, const json &jsonData) const;

    /**
     * @brief Loads JSON data from a file.
     *
     * @param filePath Path to the file from which to load data
     * @param jsonData Reference where the loaded JSON data will be stored
     * @return true if the operation was successful, false otherwise
     */
    bool loadFromFile(const std::string &filePath, json &jsonData) const;

public:
    /**
     * @brief Constructs a DataManager with the specified data directory.
     *
     * @param dataPath Path to the directory where data files will be stored
     */
    DataManager(const std::string &dataPath);

    // Category operations
    /**
     * @brief Adds a new category.
     *
     * @param category The category to add (ID will be assigned if it's 0)
     * @return true if the category was added successfully, false otherwise
     */
    bool addCategory(Category &category);

    /**
     * @brief Updates an existing category.
     *
     * @param category The category with updated information
     * @return true if the category was updated successfully, false otherwise
     */
    bool updateCategory(const Category &category);

    /**
     * @brief Deletes a category.
     *
     * @param categoryId ID of the category to delete
     * @return true if the category was deleted successfully, false otherwise
     */
    bool deleteCategory(int categoryId);

    /**
     * @brief Gets a category by its ID.
     *
     * @param categoryId ID of the category to retrieve
     * @return Pointer to the category, or nullptr if not found
     */
    Category *getCategoryById(int categoryId);

    /**
     * @brief Gets all categories.
     * @return Vector containing all categories
     */
    std::vector<Category> getAllCategories() const;

    // Transaction operations
    /**
     * @brief Adds a new transaction.
     *
     * @param transaction The transaction to add (ID will be assigned if it's 0)
     * @return true if the transaction was added successfully, false otherwise
     */
    bool addTransaction(Transaction &transaction);

    /**
     * @brief Updates an existing transaction.
     *
     * @param transaction The transaction with updated information
     * @return true if the transaction was updated successfully, false otherwise
     */
    bool updateTransaction(const Transaction &transaction);

    /**
     * @brief Deletes a transaction.
     *
     * @param transactionId ID of the transaction to delete
     * @return true if the transaction was deleted successfully, false otherwise
     */
    bool deleteTransaction(int transactionId);

    /**
     * @brief Gets a transaction by its ID.
     *
     * @param transactionId ID of the transaction to retrieve
     * @return Pointer to the transaction, or nullptr if not found
     */
    Transaction *getTransactionById(int transactionId);

    /**
     * @brief Gets all transactions.
     * @return Vector containing all transactions
     */
    std::vector<Transaction> getAllTransactions() const;

    /**
     * @brief Gets transactions for a specific category.
     *
     * @param categoryId ID of the category to filter by
     * @return Vector containing filtered transactions
     */
    std::vector<Transaction> getTransactionsByCategory(int categoryId) const;

    /**
     * @brief Gets transactions for a specific month.
     *
     * @param monthYear Month and year in "YYYY-MM" format
     * @return Vector containing filtered transactions
     */
    std::vector<Transaction> getTransactionsByMonth(const std::string &monthYear) const;

    // Budget operations
    /**
     * @brief Adds a new budget.
     *
     * @param budget The budget to add
     * @return true if the budget was added successfully, false otherwise
     */
    bool addBudget(Budget &budget);

    /**
     * @brief Updates an existing budget.
     *
     * @param budget The budget with updated information
     * @return true if the budget was updated successfully, false otherwise
     */
    bool updateBudget(const Budget &budget);

    /**
     * @brief Deletes a budget.
     *
     * @param categoryId Category ID of the budget to delete
     * @param monthYear Month and year of the budget to delete
     * @return true if the budget was deleted successfully, false otherwise
     */
    bool deleteBudget(int categoryId, const std::string &monthYear);

    /**
     * @brief Gets a budget by category ID and month/year.
     *
     * @param categoryId Category ID of the budget to retrieve
     * @param monthYear Month and year of the budget to retrieve
     * @return Pointer to the budget, or nullptr if not found
     */
    Budget *getBudget(int categoryId, const std::string &monthYear);

    /**
     * @brief Gets all budgets.
     * @return Vector containing all budgets
     */
    std::vector<Budget> getAllBudgets() const;

    /**
     * @brief Gets budgets for a specific month.
     *
     * @param monthYear Month and year in "YYYY-MM" format
     * @return Vector containing filtered budgets
     */
    std::vector<Budget> getBudgetsByMonth(const std::string &monthYear) const;

    // File operations
    /**
     * @brief Saves all data to persistent storage.
     * @return true if all data was saved successfully, false otherwise
     */
    bool saveAllData();

    /**
     * @brief Loads all data from persistent storage.
     * @return true if any data was loaded successfully, false otherwise
     */
    bool loadAllData();

    // Analysis functions
    /**
     * @brief Gets total income for a specific month.
     *
     * @param monthYear Month and year in "YYYY-MM" format
     * @return The total income for the specified month
     */
    double getTotalIncome(const std::string &monthYear) const;

    /**
     * @brief Gets total expenses for a specific month.
     *
     * @param monthYear Month and year in "YYYY-MM" format
     * @return The total expenses for the specified month
     */
    double getTotalExpense(const std::string &monthYear) const;

    /**
     * @brief Gets the net amount for a specific category and month.
     *
     * @param categoryId ID of the category to analyze
     * @param monthYear Month and year in "YYYY-MM" format
     * @return The net amount (income minus expenses) for the specified category and month
     */
    double getCategoryTotal(int categoryId, const std::string &monthYear) const;

    /**
     * @brief Gets spending totals by category for a specific month.
     *
     * @param monthYear Month and year in "YYYY-MM" format
     * @return Map of category IDs to their total amounts
     */
    std::map<int, double> getCategoryTotals(const std::string &monthYear) const;

    /**
     * @brief Gets monthly totals for income or expenses.
     *
     * @param isIncome Whether to get totals for income (true) or expenses (false)
     * @return Map of months ("YYYY-MM") to their total amounts
     */
    std::map<std::string, double> getMonthlyTotals(bool isIncome) const;
};

// JSON serialization helpers
/**
 * @brief Serializes a Transaction to JSON.
 *
 * @param j JSON object where the transaction will be serialized
 * @param transaction The transaction to serialize
 */
void to_json(json &j, const Transaction &transaction);

/**
 * @brief Deserializes a Transaction from JSON.
 *
 * @param j JSON object containing the serialized transaction
 * @param transaction The transaction object to populate
 */
void from_json(const json &j, Transaction &transaction);

/**
 * @brief Serializes a Category to JSON.
 *
 * @param j JSON object where the category will be serialized
 * @param category The category to serialize
 */
void to_json(json &j, const Category &category);

/**
 * @brief Deserializes a Category from JSON.
 *
 * @param j JSON object containing the serialized category
 * @param category The category object to populate
 */
void from_json(const json &j, Category &category);

/**
 * @brief Serializes a Budget to JSON.
 *
 * @param j JSON object where the budget will be serialized
 * @param budget The budget to serialize
 */
void to_json(json &j, const Budget &budget);

/**
 * @brief Deserializes a Budget from JSON.
 *
 * @param j JSON object containing the serialized budget
 * @param budget The budget object to populate
 */
void from_json(const json &j, Budget &budget);