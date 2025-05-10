/**
 * @file Category.h
 * @brief Defines the Category class for classifying transactions.
 */
#pragma once
#include <string>

/**
 * @class Category
 * @brief Represents a category for transactions in the budget tracking system.
 *
 * Categories allow the grouping and organization of financial transactions
 * for better tracking and analysis of spending patterns.
 */
class Category
{
private:
    int id;                  /**< Unique identifier for the category */
    std::string name;        /**< Name of the category */
    std::string description; /**< Description of what the category includes */
    std::string color;       /**< Hex color code for UI visualization of the category */

public:
    /**
     * @brief Constructs a Category with the specified parameters.
     *
     * @param id Unique identifier for the category
     * @param name Name of the category
     * @param description Description of the category
     * @param color Hex color code for UI visualization
     */
    Category(int id, const std::string &name,
             const std::string &description, const std::string &color);

    /**
     * @brief Default constructor for JSON deserialization.
     */
    Category();

    // Getters
    /**
     * @brief Gets the category identifier.
     * @return The unique identifier of the category
     */
    int getId() const;

    /**
     * @brief Gets the category name.
     * @return The name of the category
     */
    std::string getName() const;

    /**
     * @brief Gets the category description.
     * @return The description of the category
     */
    std::string getDescription() const;

    /**
     * @brief Gets the category color.
     * @return The hex color code for the category
     */
    std::string getColor() const;

    // Setters
    /**
     * @brief Sets the category identifier.
     * @param id The new category identifier
     */
    void setId(int id);

    /**
     * @brief Sets the category name.
     * @param name The new category name
     */
    void setName(const std::string &name);

    /**
     * @brief Sets the category description.
     * @param description The new category description
     */
    void setDescription(const std::string &description);

    /**
     * @brief Sets the category color.
     * @param color The new hex color code
     */
    void setColor(const std::string &color);

    /**
     * @brief Generates a string representation of this Category.
     * @return A string representation for debugging purposes
     */
    std::string toString() const;
};