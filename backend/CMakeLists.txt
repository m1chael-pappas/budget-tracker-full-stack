
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

# Common source files (without main.cpp)
set(COMMON_SOURCES
    src/Transaction.cpp
    src/Category.cpp
    src/Budget.cpp
    src/DataManager.cpp
)

# Library-specific source
set(LIB_SOURCES
    ${COMMON_SOURCES}
    src/BudgetTrackerLib.cpp  # This is the new file we'll create
)

# Add shared library target
add_library(BudgetTrackerLib SHARED ${LIB_SOURCES})
target_compile_definitions(BudgetTrackerLib PRIVATE BUDGETTRACKERLIB_EXPORTS)

# Create executable
add_executable(budget_tracker ${COMMON_SOURCES} src/main.cpp)

# Link against nlohmann_json if found
if(nlohmann_json_FOUND)
    target_link_libraries(budget_tracker PRIVATE nlohmann_json::nlohmann_json)
    target_link_libraries(BudgetTrackerLib PRIVATE nlohmann_json::nlohmann_json)
endif()

# Set library output directory to match where the C# app will look
set_target_properties(BudgetTrackerLib PROPERTIES 
    LIBRARY_OUTPUT_DIRECTORY "${CMAKE_BINARY_DIR}/lib"
    RUNTIME_OUTPUT_DIRECTORY "${CMAKE_BINARY_DIR}/bin"  # For Windows DLL
)