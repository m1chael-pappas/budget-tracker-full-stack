
#pragma once
#include <string>

class Category
{
private:
    int id;
    std::string name;
    std::string description;
    std::string color; // Hex color code for UI visualization

public:
    // Constructor
    Category(int id, const std::string &name,
             const std::string &description, const std::string &color);

    // Default constructor for deserialization
    Category();

    // Getters
    int getId() const;
    std::string getName() const;
    std::string getDescription() const;
    std::string getColor() const;

    // Setters
    void setId(int id);
    void setName(const std::string &name);
    void setDescription(const std::string &description);
    void setColor(const std::string &color);

    // Utility functions
    std::string toString() const; // For debugging
};