
#include "../include/Budget.h"

#include <sstream>
#include <iomanip>

// Constructor implementation
Budget::Budget(int categoryId, const std::string &monthYear, double allocatedAmount)
    : categoryId(categoryId), monthYear(monthYear), allocatedAmount(allocatedAmount) {}

// Default constructor
Budget::Budget()
    : categoryId(0), monthYear(""), allocatedAmount(0.0) {}

// Getters implementation
int Budget::getCategoryId() const { return categoryId; }
std::string Budget::getMonthYear() const { return monthYear; }
double Budget::getAllocatedAmount() const { return allocatedAmount; }

// Setters implementation
void Budget::setCategoryId(int categoryId) { this->categoryId = categoryId; }
void Budget::setMonthYear(const std::string &monthYear) { this->monthYear = monthYear; }
void Budget::setAllocatedAmount(double allocatedAmount) { this->allocatedAmount = allocatedAmount; }

// Utility function implementation
std::string Budget::toString() const
{
    std::ostringstream oss;
    oss << "Budget [Category ID: " << categoryId
        << ", Month/Year: " << monthYear
        << ", Allocated Amount: " << std::fixed << std::setprecision(2) << allocatedAmount << "]";
    return oss.str();
}