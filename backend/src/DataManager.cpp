
#include "../include/DataManager.h"
#include <iostream>
#include <filesystem>
#include <map>

// JSON serialization for Transaction
void to_json(json &j, const Transaction &transaction)
{
    j = json{
        {"id", transaction.getId()},
        {"date", transaction.getDate()},
        {"amount", transaction.getAmount()},
        {"description", transaction.getDescription()},
        {"categoryId", transaction.getCategoryId()},
        {"isIncome", transaction.getIsIncome()}};
}

void from_json(const json &j, Transaction &transaction)
{
    transaction.setId(j.at("id").get<int>());
    transaction.setDate(j.at("date").get<std::string>());
    transaction.setAmount(j.at("amount").get<double>());
    transaction.setDescription(j.at("description").get<std::string>());
    transaction.setCategoryId(j.at("categoryId").get<int>());
    transaction.setIsIncome(j.at("isIncome").get<bool>());
}

// JSON serialization for Category
void to_json(json &j, const Category &category)
{
    j = json{
        {"id", category.getId()},
        {"name", category.getName()},
        {"description", category.getDescription()},
        {"color", category.getColor()}};
}

void from_json(const json &j, Category &category)
{
    category.setId(j.at("id").get<int>());
    category.setName(j.at("name").get<std::string>());
    category.setDescription(j.at("description").get<std::string>());
    category.setColor(j.at("color").get<std::string>());
}

// JSON serialization for Budget
void to_json(json &j, const Budget &budget)
{
    j = json{
        {"categoryId", budget.getCategoryId()},
        {"monthYear", budget.getMonthYear()},
        {"allocatedAmount", budget.getAllocatedAmount()}};
}

void from_json(const json &j, Budget &budget)
{
    budget.setCategoryId(j.at("categoryId").get<int>());
    budget.setMonthYear(j.at("monthYear").get<std::string>());
    budget.setAllocatedAmount(j.at("allocatedAmount").get<double>());
}

// DataManager implementation
DataManager::DataManager(const std::string &dataPath)
    : dataPath(dataPath), nextTransactionId(1), nextCategoryId(1)
{

    // Create data directory if it doesn't exist
    std::filesystem::create_directories(dataPath);

    // Load existing data if available
    loadAllData();
}

std::string DataManager::getTransactionsFilePath() const
{
    return dataPath + "/transactions.json";
}

std::string DataManager::getCategoriesFilePath() const
{
    return dataPath + "/categories.json";
}

std::string DataManager::getBudgetsFilePath() const
{
    return dataPath + "/budgets.json";
}

bool DataManager::saveToFile(const std::string &filePath, const json &jsonData) const
{
    try
    {
        std::ofstream file(filePath);
        if (!file.is_open())
        {
            std::cerr << "Failed to open file for writing: " << filePath << std::endl;
            return false;
        }

        file << std::setw(4) << jsonData << std::endl;
        return true;
    }
    catch (const std::exception &e)
    {
        std::cerr << "Error saving to file: " << e.what() << std::endl;
        return false;
    }
}

bool DataManager::loadFromFile(const std::string &filePath, json &jsonData) const
{
    try
    {
        std::ifstream file(filePath);
        if (!file.is_open())
        {
            // File might not exist yet, which is not an error
            jsonData = json::array(); // Initialize as empty array
            return true;
        }

        // Check if file is empty
        file.seekg(0, std::ios::end);
        if (file.tellg() == 0)
        {
            // File is empty, return empty array
            jsonData = json::array();
            return true;
        }

        // Reset to start of file
        file.seekg(0, std::ios::beg);

        // Parse JSON
        file >> jsonData;
        return true;
    }
    catch (const std::exception &e)
    {
        std::cerr << "Error loading from file: " << e.what() << std::endl;
        jsonData = json::array(); // Initialize as empty array in case of error
        return false;
    }
}

// Category operations
bool DataManager::addCategory(Category &category)
{
    // Assign a new ID if the category doesn't have one
    if (category.getId() == 0)
    {
        category.setId(nextCategoryId++);
    }

    // Check if a category with this ID already exists
    for (size_t i = 0; i < categories.size(); i++)
    {
        if (categories[i].getId() == category.getId())
        {
            return false; // Category ID already exists
        }
    }

    categories.push_back(category);
    return saveAllData();
}

bool DataManager::updateCategory(const Category &category)
{
    for (size_t i = 0; i < categories.size(); i++)
    {
        if (categories[i].getId() == category.getId())
        {
            categories[i] = category;
            return saveAllData();
        }
    }
    return false; // Category not found
}

bool DataManager::deleteCategory(int categoryId)
{
    for (size_t i = 0; i < categories.size(); i++)
    {
        if (categories[i].getId() == categoryId)
        {
            categories.erase(categories.begin() + i);
            return saveAllData();
        }
    }
    return false; // Category not found
}

Category *DataManager::getCategoryById(int categoryId)
{
    for (size_t i = 0; i < categories.size(); i++)
    {
        if (categories[i].getId() == categoryId)
        {
            return &categories[i];
        }
    }
    return nullptr; // Category not found
}

std::vector<Category> DataManager::getAllCategories() const
{
    return categories;
}

// Transaction operations
bool DataManager::addTransaction(Transaction &transaction)
{
    // Assign a new ID if the transaction doesn't have one
    if (transaction.getId() == 0)
    {
        transaction.setId(nextTransactionId++);
    }

    // Check if a transaction with this ID already exists
    for (size_t i = 0; i < transactions.size(); i++)
    {
        if (transactions[i].getId() == transaction.getId())
        {
            return false; // Transaction ID already exists
        }
    }

    transactions.push_back(transaction);
    return saveAllData();
}

bool DataManager::updateTransaction(const Transaction &transaction)
{
    for (size_t i = 0; i < transactions.size(); i++)
    {
        if (transactions[i].getId() == transaction.getId())
        {
            transactions[i] = transaction;
            return saveAllData();
        }
    }
    return false; // Transaction not found
}

bool DataManager::deleteTransaction(int transactionId)
{
    for (size_t i = 0; i < transactions.size(); i++)
    {
        if (transactions[i].getId() == transactionId)
        {
            transactions.erase(transactions.begin() + i);
            return saveAllData();
        }
    }
    return false; // Transaction not found
}

Transaction *DataManager::getTransactionById(int transactionId)
{
    for (size_t i = 0; i < transactions.size(); i++)
    {
        if (transactions[i].getId() == transactionId)
        {
            return &transactions[i];
        }
    }
    return nullptr; // Transaction not found
}

std::vector<Transaction> DataManager::getAllTransactions() const
{
    return transactions;
}

std::vector<Transaction> DataManager::getTransactionsByCategory(int categoryId) const
{
    std::vector<Transaction> result;
    for (const auto &transaction : transactions)
    {
        if (transaction.getCategoryId() == categoryId)
        {
            result.push_back(transaction);
        }
    }
    return result;
}

std::vector<Transaction> DataManager::getTransactionsByMonth(const std::string &monthYear) const
{
    std::vector<Transaction> result;
    for (const auto &transaction : transactions)
    {
        // Extract month-year from transaction date (format: YYYY-MM-DD)
        if (transaction.getDate().substr(0, 7) == monthYear)
        {
            result.push_back(transaction);
        }
    }
    return result;
}

// Budget operations
bool DataManager::addBudget(Budget &budget)
{
    // Check if a budget for this category and month already exists
    for (size_t i = 0; i < budgets.size(); i++)
    {
        if (budgets[i].getCategoryId() == budget.getCategoryId() &&
            budgets[i].getMonthYear() == budget.getMonthYear())
        {
            return false; // Budget already exists
        }
    }

    budgets.push_back(budget);
    return saveAllData();
}

bool DataManager::updateBudget(const Budget &budget)
{
    for (size_t i = 0; i < budgets.size(); i++)
    {
        if (budgets[i].getCategoryId() == budget.getCategoryId() &&
            budgets[i].getMonthYear() == budget.getMonthYear())
        {
            budgets[i] = budget;
            return saveAllData();
        }
    }
    return false; // Budget not found
}

bool DataManager::deleteBudget(int categoryId, const std::string &monthYear)
{
    for (size_t i = 0; i < budgets.size(); i++)
    {
        if (budgets[i].getCategoryId() == categoryId &&
            budgets[i].getMonthYear() == monthYear)
        {
            budgets.erase(budgets.begin() + i);
            return saveAllData();
        }
    }
    return false; // Budget not found
}

Budget *DataManager::getBudget(int categoryId, const std::string &monthYear)
{
    for (size_t i = 0; i < budgets.size(); i++)
    {
        if (budgets[i].getCategoryId() == categoryId &&
            budgets[i].getMonthYear() == monthYear)
        {
            return &budgets[i];
        }
    }
    return nullptr; // Budget not found
}

std::vector<Budget> DataManager::getAllBudgets() const
{
    return budgets;
}

std::vector<Budget> DataManager::getBudgetsByMonth(const std::string &monthYear) const
{
    std::vector<Budget> result;
    for (const auto &budget : budgets)
    {
        if (budget.getMonthYear() == monthYear)
        {
            result.push_back(budget);
        }
    }
    return result;
}

// File operations
bool DataManager::saveAllData()
{
    bool success = true;

    // Save transactions
    json transactionsJson = json::array();
    for (const auto &transaction : transactions)
    {
        transactionsJson.push_back(transaction);
    }
    success &= saveToFile(getTransactionsFilePath(), transactionsJson);

    // Save categories
    json categoriesJson = json::array();
    for (const auto &category : categories)
    {
        categoriesJson.push_back(category);
    }
    success &= saveToFile(getCategoriesFilePath(), categoriesJson);

    // Save budgets
    json budgetsJson = json::array();
    for (const auto &budget : budgets)
    {
        budgetsJson.push_back(budget);
    }
    success &= saveToFile(getBudgetsFilePath(), budgetsJson);

    return success;
}

bool DataManager::loadAllData()
{
    json transactionsJson, categoriesJson, budgetsJson;
    bool anyLoaded = false;

    // Load transactions
    if (loadFromFile(getTransactionsFilePath(), transactionsJson))
    {
        transactions.clear();
        nextTransactionId = 1;

        for (const auto &item : transactionsJson)
        {
            Transaction transaction;
            from_json(item, transaction);
            transactions.push_back(transaction);

            // Update next ID
            if (transaction.getId() >= nextTransactionId)
            {
                nextTransactionId = transaction.getId() + 1;
            }
        }
        anyLoaded = true;
    }

    // Load categories
    if (loadFromFile(getCategoriesFilePath(), categoriesJson))
    {
        categories.clear();
        nextCategoryId = 1;

        for (const auto &item : categoriesJson)
        {
            Category category;
            from_json(item, category);
            categories.push_back(category);

            // Update next ID
            if (category.getId() >= nextCategoryId)
            {
                nextCategoryId = category.getId() + 1;
            }
        }
        anyLoaded = true;
    }

    // Load budgets
    if (loadFromFile(getBudgetsFilePath(), budgetsJson))
    {
        budgets.clear();

        for (const auto &item : budgetsJson)
        {
            Budget budget;
            from_json(item, budget);
            budgets.push_back(budget);
        }
        anyLoaded = true;
    }

    return anyLoaded;
}

// Analysis functions
double DataManager::getTotalIncome(const std::string &monthYear) const
{
    double total = 0.0;
    for (const auto &transaction : transactions)
    {
        if (transaction.getIsIncome() && transaction.getDate().substr(0, 7) == monthYear)
        {
            total += transaction.getAmount();
        }
    }
    return total;
}

double DataManager::getTotalExpense(const std::string &monthYear) const
{
    double total = 0.0;
    for (const auto &transaction : transactions)
    {
        if (!transaction.getIsIncome() && transaction.getDate().substr(0, 7) == monthYear)
        {
            total += transaction.getAmount();
        }
    }
    return total;
}

double DataManager::getCategoryTotal(int categoryId, const std::string &monthYear) const
{
    double total = 0.0;
    for (const auto &transaction : transactions)
    {
        if (transaction.getCategoryId() == categoryId &&
            transaction.getDate().substr(0, 7) == monthYear)
        {
            if (transaction.getIsIncome())
            {
                total += transaction.getAmount();
            }
            else
            {
                total -= transaction.getAmount();
            }
        }
    }
    return total;
}

std::map<int, double> DataManager::getCategoryTotals(const std::string &monthYear) const
{
    std::map<int, double> totals;

    // Initialize totals for all categories
    for (const auto &category : categories)
    {
        totals[category.getId()] = 0.0;
    }

    // Add up transactions
    for (const auto &transaction : transactions)
    {
        if (transaction.getDate().substr(0, 7) == monthYear)
        {
            if (transaction.getIsIncome())
            {
                totals[transaction.getCategoryId()] += transaction.getAmount();
            }
            else
            {
                totals[transaction.getCategoryId()] -= transaction.getAmount();
            }
        }
    }

    return totals;
}

std::map<std::string, double> DataManager::getMonthlyTotals(bool isIncome) const
{
    std::map<std::string, double> totals;

    for (const auto &transaction : transactions)
    {
        std::string month = transaction.getDate().substr(0, 7); // Extract YYYY-MM

        if (transaction.getIsIncome() == isIncome)
        {
            totals[month] += transaction.getAmount();
        }
    }

    return totals;
}