
#pragma once
#include <string>

class Budget
{
private:
    int categoryId;
    std::string monthYear; // Format: "YYYY-MM"
    double allocatedAmount;

public:
    // Constructor
    Budget(int categoryId, const std::string &monthYear, double allocatedAmount);

    // Default constructor for deserialization
    Budget();

    // Getters
    int getCategoryId() const;
    std::string getMonthYear() const;
    double getAllocatedAmount() const;

    // Setters
    void setCategoryId(int categoryId);
    void setMonthYear(const std::string &monthYear);
    void setAllocatedAmount(double allocatedAmount);

    // Utility functions
    std::string toString() const; // For debugging
};