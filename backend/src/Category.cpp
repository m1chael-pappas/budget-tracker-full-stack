
#include "../include/Category.h"
#include <sstream>

// Constructor implementation
Category::Category(int id, const std::string &name,
                   const std::string &description, const std::string &color)
    : id(id), name(name), description(description), color(color) {}

// Default constructor
Category::Category()
    : id(0), name(""), description(""), color("#000000") {}

// Getters implementation
int Category::getId() const { return id; }
std::string Category::getName() const { return name; }
std::string Category::getDescription() const { return description; }
std::string Category::getColor() const { return color; }

// Setters implementation
void Category::setId(int id) { this->id = id; }
void Category::setName(const std::string &name) { this->name = name; }
void Category::setDescription(const std::string &description) { this->description = description; }
void Category::setColor(const std::string &color) { this->color = color; }

// Utility function implementation
std::string Category::toString() const
{
    std::ostringstream oss;
    oss << "Category [ID: " << id
        << ", Name: " << name
        << ", Description: " << description
        << ", Color: " << color << "]";
    return oss.str();
}