
#include "../include/Transaction.h"
#include <sstream>
#include <iomanip>

// Constructor implementation
Transaction::Transaction(int id, const std::string &date, double amount,
                         const std::string &description, int categoryId, bool isIncome)
    : id(id), date(date), amount(amount), description(description),
      categoryId(categoryId), isIncome(isIncome) {}

// Default constructor
Transaction::Transaction()
    : id(0), date(""), amount(0.0), description(""), categoryId(0), isIncome(false) {}

// Getters implementation
int Transaction::getId() const { return id; }
std::string Transaction::getDate() const { return date; }
double Transaction::getAmount() const { return amount; }
std::string Transaction::getDescription() const { return description; }
int Transaction::getCategoryId() const { return categoryId; }
bool Transaction::getIsIncome() const { return isIncome; }

// Setters implementation
void Transaction::setId(int id) { this->id = id; }
void Transaction::setDate(const std::string &date) { this->date = date; }
void Transaction::setAmount(double amount) { this->amount = amount; }
void Transaction::setDescription(const std::string &description) { this->description = description; }
void Transaction::setCategoryId(int categoryId) { this->categoryId = categoryId; }
void Transaction::setIsIncome(bool isIncome) { this->isIncome = isIncome; }

// Utility function implementation
std::string Transaction::toString() const
{
    std::ostringstream oss;
    oss << "Transaction [ID: " << id
        << ", Date: " << date
        << ", Amount: " << std::fixed << std::setprecision(2) << amount
        << ", Description: " << description
        << ", Category ID: " << categoryId
        << ", Type: " << (isIncome ? "Income" : "Expense") << "]";
    return oss.str();
}