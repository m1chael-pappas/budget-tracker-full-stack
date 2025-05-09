
#pragma once
#include <string>
#include <ctime>

class Transaction
{
private:
    int id;
    std::string date;
    double amount;
    std::string description;
    int categoryId;
    bool isIncome;

public:
    // Constructor
    Transaction(int id, const std::string &date, double amount,
                const std::string &description, int categoryId, bool isIncome);

    // Default constructor for deserialization
    Transaction();

    // Getters
    int getId() const;
    std::string getDate() const;
    double getAmount() const;
    std::string getDescription() const;
    int getCategoryId() const;
    bool getIsIncome() const;

    // Setters
    void setId(int id);
    void setDate(const std::string &date);
    void setAmount(double amount);
    void setDescription(const std::string &description);
    void setCategoryId(int categoryId);
    void setIsIncome(bool isIncome);

    // Utility functions
    std::string toString() const; // For debugging
};