// BudgetTrackerLib.h
#pragma once

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

    // Core functions
    BUDGETTRACKER_API void *CreateDataManager(const char *dataPath);
    BUDGETTRACKER_API void DestroyDataManager(void *manager);

    // Category operations
    BUDGETTRACKER_API int AddCategory(void *manager, const char *name, const char *description, const char *color);
    BUDGETTRACKER_API bool UpdateCategory(void *manager, int id, const char *name, const char *description, const char *color);
    BUDGETTRACKER_API bool DeleteCategory(void *manager, int categoryId);
    BUDGETTRACKER_API const char *GetAllCategories(void *manager);

    // Transaction operations
    BUDGETTRACKER_API int AddTransaction(void *manager, const char *date, double amount, const char *description, int categoryId, bool isIncome);
    BUDGETTRACKER_API bool UpdateTransaction(void *manager, int id, const char *date, double amount, const char *description, int categoryId, bool isIncome);
    BUDGETTRACKER_API bool DeleteTransaction(void *manager, int transactionId);
    BUDGETTRACKER_API const char *GetAllTransactions(void *manager);
    BUDGETTRACKER_API const char *GetTransactionsByMonth(void *manager, const char *monthYear);

    // Budget operations
    BUDGETTRACKER_API bool AddBudget(void *manager, int categoryId, const char *monthYear, double allocatedAmount);
    BUDGETTRACKER_API bool UpdateBudget(void *manager, int categoryId, const char *monthYear, double allocatedAmount);
    BUDGETTRACKER_API bool DeleteBudget(void *manager, int categoryId, const char *monthYear);
    BUDGETTRACKER_API const char *GetAllBudgets(void *manager);

    // Analysis functions
    BUDGETTRACKER_API double GetTotalIncome(void *manager, const char *monthYear);
    BUDGETTRACKER_API double GetTotalExpense(void *manager, const char *monthYear);
    BUDGETTRACKER_API const char *GetCategoryTotals(void *manager, const char *monthYear);

#ifdef __cplusplus
}
#endif