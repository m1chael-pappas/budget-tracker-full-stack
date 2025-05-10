/**
 * @file BudgetTrackerLib.h
 * @brief C API for the Budget Tracker application.
 *
 * This file defines the public C API for the Budget Tracker library, allowing
 * client applications to interact with the budget tracking functionality through
 * a stable ABI. This API supports operations for categories, transactions, budgets,
 * and financial analysis.
 */
#pragma once

// Define platform-specific export/import macros
#ifdef _WIN32
#ifdef BUDGETTRACKERLIB_EXPORTS
#define BUDGETTRACKER_API __declspec(dllexport)
#else
#define BUDGETTRACKER_API __declspec(dllimport)
#endif
#else
#define BUDGETTRACKER_API
#endif

#include <stdbool.h>

#ifdef __cplusplus
extern "C"
{
#endif

    /**
     * @brief Creates a new DataManager instance.
     *
     * @param dataPath Path to the directory where data will be stored
     * @return Pointer to the created DataManager, or NULL if creation fails
     */
    BUDGETTRACKER_API void *CreateDataManager(const char *dataPath);

    /**
     * @brief Destroys a DataManager instance.
     *
     * @param manager Pointer to the DataManager instance to destroy
     */
    BUDGETTRACKER_API void DestroyDataManager(void *manager);

    // Category operations
    /**
     * @brief Adds a new category.
     *
     * @param manager Pointer to the DataManager instance
     * @param name Name of the category
     * @param description Description of the category
     * @param color Color code for the category (in hex format)
     * @return The ID of the newly created category, or -1 on failure
     */
    BUDGETTRACKER_API int AddCategory(void *manager, const char *name, const char *description, const char *color);

    /**
     * @brief Updates an existing category.
     *
     * @param manager Pointer to the DataManager instance
     * @param id ID of the category to update
     * @param name New name for the category
     * @param description New description for the category
     * @param color New color code for the category
     * @return true if the category was updated successfully, false otherwise
     */
    BUDGETTRACKER_API bool UpdateCategory(void *manager, int id, const char *name, const char *description, const char *color);

    /**
     * @brief Deletes a category.
     *
     * @param manager Pointer to the DataManager instance
     * @param categoryId ID of the category to delete
     * @return true if the category was deleted successfully, false otherwise
     */
    BUDGETTRACKER_API bool DeleteCategory(void *manager, int categoryId);

    /**
     * @brief Gets all categories.
     *
     * @param manager Pointer to the DataManager instance
     * @return JSON string containing all categories, caller does not need to free this memory
     */
    BUDGETTRACKER_API const char *GetAllCategories(void *manager);

    // Transaction operations
    /**
     * @brief Adds a new transaction.
     *
     * @param manager Pointer to the DataManager instance
     * @param date Date of the transaction in "YYYY-MM-DD" format
     * @param amount Amount of the transaction
     * @param description Description of the transaction
     * @param categoryId Category ID associated with the transaction
     * @param isIncome Whether the transaction is income (true) or expense (false)
     * @return The ID of the newly created transaction, or -1 on failure
     */
    BUDGETTRACKER_API int AddTransaction(void *manager, const char *date, double amount, const char *description, int categoryId, bool isIncome);

    /**
     * @brief Updates an existing transaction.
     *
     * @param manager Pointer to the DataManager instance
     * @param id ID of the transaction to update
     * @param date New date for the transaction
     * @param amount New amount for the transaction
     * @param description New description for the transaction
     * @param categoryId New category ID for the transaction
     * @param isIncome New income status for the transaction
     * @return true if the transaction was updated successfully, false otherwise
     */
    BUDGETTRACKER_API bool UpdateTransaction(void *manager, int id, const char *date, double amount, const char *description, int categoryId, bool isIncome);

    /**
     * @brief Deletes a transaction.
     *
     * @param manager Pointer to the DataManager instance
     * @param transactionId ID of the transaction to delete
     * @return true if the transaction was deleted successfully, false otherwise
     */
    BUDGETTRACKER_API bool DeleteTransaction(void *manager, int transactionId);

    /**
     * @brief Gets all transactions.
     *
     * @param manager Pointer to the DataManager instance
     * @return JSON string containing all transactions, caller does not need to free this memory
     */
    BUDGETTRACKER_API const char *GetAllTransactions(void *manager);

    /**
     * @brief Gets transactions for a specific month.
     *
     * @param manager Pointer to the DataManager instance
     * @param monthYear Month and year in "YYYY-MM" format
     * @return JSON string containing filtered transactions, caller does not need to free this memory
     */
    BUDGETTRACKER_API const char *GetTransactionsByMonth(void *manager, const char *monthYear);

    // Budget operations
    /**
     * @brief Adds a new budget.
     *
     * @param manager Pointer to the DataManager instance
     * @param categoryId Category ID for the budget
     * @param monthYear Month and year in "YYYY-MM" format
     * @param allocatedAmount Amount allocated for the budget
     * @return true if the budget was added successfully, false otherwise
     */
    BUDGETTRACKER_API bool AddBudget(void *manager, int categoryId, const char *monthYear, double allocatedAmount);

    /**
     * @brief Updates an existing budget.
     *
     * @param manager Pointer to the DataManager instance
     * @param categoryId Category ID of the budget to update
     * @param monthYear Month and year of the budget to update
     * @param allocatedAmount New allocated amount for the budget
     * @return true if the budget was updated successfully, false otherwise
     */
    BUDGETTRACKER_API bool UpdateBudget(void *manager, int categoryId, const char *monthYear, double allocatedAmount);

    /**
     * @brief Deletes a budget.
     *
     * @param manager Pointer to the DataManager instance
     * @param categoryId Category ID of the budget to delete
     * @param monthYear Month and year of the budget to delete
     * @return true if the budget was deleted successfully, false otherwise
     */
    BUDGETTRACKER_API bool DeleteBudget(void *manager, int categoryId, const char *monthYear);

    /**
     * @brief Gets all budgets.
     *
     * @param manager Pointer to the DataManager instance
     * @return JSON string containing all budgets, caller does not need to free this memory
     */
    BUDGETTRACKER_API const char *GetAllBudgets(void *manager);

    // Analysis functions
    /**
     * @brief Gets total income for a specific month.
     *
     * @param manager Pointer to the DataManager instance
     * @param monthYear Month and year in "YYYY-MM" format
     * @return The total income for the specified month
     */
    BUDGETTRACKER_API double GetTotalIncome(void *manager, const char *monthYear);

    /**
     * @brief Gets total expenses for a specific month.
     *
     * @param manager Pointer to the DataManager instance
     * @param monthYear Month and year in "YYYY-MM" format
     * @return The total expenses for the specified month
     */
    BUDGETTRACKER_API double GetTotalExpense(void *manager, const char *monthYear);

    /**
     * @brief Gets spending totals by category for a specific month.
     *
     * @param manager Pointer to the DataManager instance
     * @param monthYear Month and year in "YYYY-MM" format
     * @return JSON string mapping category IDs to their total amounts, caller does not need to free this memory
     */
    BUDGETTRACKER_API const char *GetCategoryTotals(void *manager, const char *monthYear);

#ifdef __cplusplus
}
#endif