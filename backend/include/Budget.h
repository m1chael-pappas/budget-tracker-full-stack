/**
 * @file Budget.h
 * @brief Defines the Budget class for managing financial allocations per category.
 */
#pragma once
#include <string>

/**
 * @class Budget
 * @brief Represents a budget allocation for a specific category and time period.
 *
 * The Budget class manages financial allocations for categories within specific
 * month/year periods. This allows for tracking planned spending against actual expenses.
 */
class Budget
{
private:
    int categoryId;         /**< Identifier for the associated category */
    std::string monthYear;  /**< The month and year in "YYYY-MM" format */
    double allocatedAmount; /**< The amount allocated for this budget in the specified period */

public:
    /**
     * @brief Constructs a Budget with the specified parameters.
     *
     * @param categoryId Identifier for the associated category
     * @param monthYear Month and year in "YYYY-MM" format
     * @param allocatedAmount The amount allocated for this budget
     */
    Budget(int categoryId, const std::string &monthYear, double allocatedAmount);

    /**
     * @brief Default constructor for JSON deserialization.
     */
    Budget();

    // Getters
    /**
     * @brief Gets the category identifier.
     * @return The identifier of the associated category
     */
    int getCategoryId() const;

    /**
     * @brief Gets the month and year.
     * @return The month and year in "YYYY-MM" format
     */
    std::string getMonthYear() const;

    /**
     * @brief Gets the allocated amount.
     * @return The allocated budget amount
     */
    double getAllocatedAmount() const;

    // Setters
    /**
     * @brief Sets the category identifier.
     * @param categoryId The new category identifier
     */
    void setCategoryId(int categoryId);

    /**
     * @brief Sets the month and year.
     * @param monthYear The new month and year in "YYYY-MM" format
     */
    void setMonthYear(const std::string &monthYear);

    /**
     * @brief Sets the allocated amount.
     * @param allocatedAmount The new allocated budget amount
     */
    void setAllocatedAmount(double allocatedAmount);

    /**
     * @brief Generates a string representation of this Budget.
     * @return A string representation for debugging purposes
     */
    std::string toString() const;
};