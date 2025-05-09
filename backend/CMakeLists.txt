# backend/CMakeLists.txt
cmake_minimum_required(VERSION 3.10)
project(BudgetTrackerBackend)

# Set C++ standard
set(CMAKE_CXX_STANDARD 17)
set(CMAKE_CXX_STANDARD_REQUIRED ON)

# Include directories
include_directories(${CMAKE_CURRENT_SOURCE_DIR}/include)

# Find nlohmann_json (system installed)
find_package(nlohmann_json 3.10.5 CONFIG)
if(NOT nlohmann_json_FOUND)
    message(STATUS "Could not find nlohmann_json, will manually include directories")
    include_directories(/usr/include/nlohmann)
endif()

# Add source files
set(SOURCES
    src/Transaction.cpp
    src/Category.cpp
    src/Budget.cpp
    src/DataManager.cpp
    src/main.cpp
)

# Create executable
add_executable(budget_tracker ${SOURCES})

# Link against nlohmann_json if found
if(nlohmann_json_FOUND)
    target_link_libraries(budget_tracker PRIVATE nlohmann_json::nlohmann_json)
endif()