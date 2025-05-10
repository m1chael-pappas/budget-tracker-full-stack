/**
 * @file Transaction.h
 * @brief Defines the Transaction class for financial transactions.
 */
#pragma once
#include <string>
#include <ctime>

/**
 * @class Transaction
 * @brief Represents a financial transaction in the budget tracking system.
 *
 * The Transaction class stores details about financial activities, including
 * the date, amount, description, associated category, and whether it represents
 * income or expense.
 */
class Transaction
{
private:
    int id;                  /**< Unique identifier for the transaction */
    std::string date;        /**< Date of the transaction in "YYYY-MM-DD" format */
    double amount;           /**< Amount of the transaction */
    std::string description; /**< Description of the transaction */
    int categoryId;          /**< ID of the category associated with the transaction */
    bool isIncome;           /**< Flag indicating if this is income (true) or expense (false) */

public:
    /**
     * @brief Constructs a Transaction with the specified parameters.
     *
     * @param id Unique identifier for the transaction
     * @param date Date of the transaction in "YYYY-MM-DD" format
     * @param amount Amount of the transaction
     * @param description Description of the transaction
     * @param categoryId ID of the associated category
     * @param isIncome Whether this is income (true) or expense (false)
     */
    Transaction(int id, const std::string &date, double amount,
                const std::string &description, int categoryId, bool isIncome);

    /**
     * @brief Default constructor for JSON deserialization.
     */
    Transaction();

    // Getters
    /**
     * @brief Gets the transaction identifier.
     * @return The unique identifier of the transaction
     */
    int getId() const;

    /**
     * @brief Gets the transaction date.
     * @return The date of the transaction in "YYYY-MM-DD" format
     */
    std::string getDate() const;

    /**
     * @brief Gets the transaction amount.
     * @return The amount of the transaction
     */
    double getAmount() const;

    /**
     * @brief Gets the transaction description.
     * @return The description of the transaction
     */
    std::string getDescription() const;

    /**
     * @brief Gets the associated category identifier.
     * @return The ID of the associated category
     */
    int getCategoryId() const;

    /**
     * @brief Gets the income status.
     * @return true if this is income, false if it's an expense
     */
    bool getIsIncome() const;

    // Setters
    /**
     * @brief Sets the transaction identifier.
     * @param id The new transaction identifier
     */
    void setId(int id);

    /**
     * @brief Sets the transaction date.
     * @param date The new transaction date in "YYYY-MM-DD" format
     */
    void setDate(const std::string &date);

    /**
     * @brief Sets the transaction amount.
     * @param amount The new transaction amount
     */
    void setAmount(double amount);

    /**
     * @brief Sets the transaction description.
     * @param description The new transaction description
     */
    void setDescription(const std::string &description);

    /**
     * @brief Sets the associated category identifier.
     * @param categoryId The new category identifier
     */
    void setCategoryId(int categoryId);

    /**
     * @brief Sets the income status.
     * @param isIncome Whether this transaction is income (true) or expense (false)
     */
    void setIsIncome(bool isIncome);

    /**
     * @brief Generates a string representation of this Transaction.
     * @return A string representation for debugging purposes
     */
    std::string toString() const;
};