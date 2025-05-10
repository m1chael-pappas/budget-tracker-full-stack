#include "../include/BudgetTrackerLib.h"
#include "../include/DataManager.h"
#include <string>
#include <nlohmann/json.hpp>
#include <iostream>
#include <mutex>

/*
 * This file contains the C API for the Budget Tracker application.
 * It provides functions to manage categories, transactions, and budgets.
 * The functions are designed to be called from C or C++ code.
 */
thread_local std::string g_returnBuffer;

static std::unordered_map<void *, bool> g_managerMap;
static std::mutex g_managerMapMutex;

extern "C"
{
    void *CreateDataManager(const char *dataPath)
    {
        try
        {
            std::lock_guard<std::mutex> lock(g_managerMapMutex);
            DataManager *manager = new DataManager(dataPath);
            g_managerMap[manager] = true;
            return manager;
        }
        catch (const std::exception &e)
        {
            std::cerr << "Error creating DataManager: " << e.what() << std::endl;
            return nullptr;
        }
    }

    void DestroyDataManager(void *manager)
    {
        if (manager == nullptr)
            return;

        try
        {
            std::lock_guard<std::mutex> lock(g_managerMapMutex);
            auto it = g_managerMap.find(manager);
            if (it != g_managerMap.end() && it->second)
            {
                delete static_cast<DataManager *>(manager);
                g_managerMap[manager] = false;
                g_managerMap.erase(it);
            }
            else
            {
                std::cerr << "Warning: Attempt to destroy already destroyed or invalid DataManager" << std::endl;
            }
        }
        catch (const std::exception &e)
        {
            std::cerr << "Error destroying DataManager: " << e.what() << std::endl;
        }
    }

    // Category operations
    int AddCategory(void *manager, const char *name, const char *description, const char *color)
    {
        DataManager *dm = static_cast<DataManager *>(manager);
        Category category(0, name, description, color);
        if (dm->addCategory(category))
        {
            return category.getId();
        }
        return -1;
    }

    bool UpdateCategory(void *manager, int id, const char *name, const char *description, const char *color)
    {
        DataManager *dm = static_cast<DataManager *>(manager);
        Category category(id, name, description, color);
        return dm->updateCategory(category);
    }

    bool DeleteCategory(void *manager, int categoryId)
    {
        DataManager *dm = static_cast<DataManager *>(manager);
        return dm->deleteCategory(categoryId);
    }

    const char *GetAllCategories(void *manager)
    {
        DataManager *dm = static_cast<DataManager *>(manager);
        try
        {
            auto categories = dm->getAllCategories();

            nlohmann::json jsonArray = nlohmann::json::array();
            for (const auto &category : categories)
            {
                nlohmann::json item;
                item["id"] = category.getId();
                item["name"] = category.getName();
                item["description"] = category.getDescription();
                item["color"] = category.getColor();
                jsonArray.push_back(item);
            }

            g_returnBuffer = jsonArray.dump();
            return g_returnBuffer.c_str();
        }
        catch (const std::exception &e)
        {
            std::cerr << "Error in GetAllCategories: " << e.what() << std::endl;
            g_returnBuffer = "[]";
            return g_returnBuffer.c_str();
        }
    }

    // Transaction operations
    int AddTransaction(void *manager, const char *date, double amount, const char *description, int categoryId, bool isIncome)
    {
        DataManager *dm = static_cast<DataManager *>(manager);
        Transaction transaction(0, date, amount, description, categoryId, isIncome);
        if (dm->addTransaction(transaction))
        {
            return transaction.getId();
        }
        return -1;
    }

    bool UpdateTransaction(void *manager, int id, const char *date, double amount, const char *description, int categoryId, bool isIncome)
    {
        DataManager *dm = static_cast<DataManager *>(manager);
        Transaction transaction(id, date, amount, description, categoryId, isIncome);
        return dm->updateTransaction(transaction);
    }

    bool DeleteTransaction(void *manager, int transactionId)
    {
        DataManager *dm = static_cast<DataManager *>(manager);
        return dm->deleteTransaction(transactionId);
    }

    const char *GetAllTransactions(void *manager)
    {
        DataManager *dm = static_cast<DataManager *>(manager);
        auto transactions = dm->getAllTransactions();

        nlohmann::json jsonArray = nlohmann::json::array();
        for (const auto &transaction : transactions)
        {
            nlohmann::json item;
            item["id"] = transaction.getId();
            item["date"] = transaction.getDate();
            item["amount"] = transaction.getAmount();
            item["description"] = transaction.getDescription();
            item["categoryId"] = transaction.getCategoryId();
            item["isIncome"] = transaction.getIsIncome();

            // Add categoryName if available
            auto category = dm->getCategoryById(transaction.getCategoryId());
            if (category != nullptr)
            {
                item["categoryName"] = category->getName();
            }
            else
            {
                item["categoryName"] = "Uncategorized";
            }

            jsonArray.push_back(item);
        }

        g_returnBuffer = jsonArray.dump();
        return g_returnBuffer.c_str();
    }

    const char *GetTransactionsByMonth(void *manager, const char *monthYear)
    {
        DataManager *dm = static_cast<DataManager *>(manager);
        auto transactions = dm->getTransactionsByMonth(monthYear);

        nlohmann::json jsonArray = nlohmann::json::array();
        for (const auto &transaction : transactions)
        {
            nlohmann::json item;
            item["id"] = transaction.getId();
            item["date"] = transaction.getDate();
            item["amount"] = transaction.getAmount();
            item["description"] = transaction.getDescription();
            item["categoryId"] = transaction.getCategoryId();
            item["isIncome"] = transaction.getIsIncome();

            // Add categoryName if available
            auto category = dm->getCategoryById(transaction.getCategoryId());
            if (category != nullptr)
            {
                item["categoryName"] = category->getName();
            }
            else
            {
                item["categoryName"] = "Uncategorized";
            }

            jsonArray.push_back(item);
        }

        g_returnBuffer = jsonArray.dump();
        return g_returnBuffer.c_str();
    }

    // Analysis functions
    double GetTotalIncome(void *manager, const char *monthYear)
    {
        DataManager *dm = static_cast<DataManager *>(manager);
        return dm->getTotalIncome(monthYear);
    }

    double GetTotalExpense(void *manager, const char *monthYear)
    {
        DataManager *dm = static_cast<DataManager *>(manager);
        return dm->getTotalExpense(monthYear);
    }

    const char *GetCategoryTotals(void *manager, const char *monthYear)
    {
        DataManager *dm = static_cast<DataManager *>(manager);
        auto totals = dm->getCategoryTotals(monthYear);

        nlohmann::json jsonObject;
        for (const auto &pair : totals)
        {
            jsonObject[std::to_string(pair.first)] = pair.second;
        }

        g_returnBuffer = jsonObject.dump();
        return g_returnBuffer.c_str();
    }
}