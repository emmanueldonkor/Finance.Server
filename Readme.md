# Expense API (.NET 7)

## Overview

This Expense API is built using .NET 7 and provides a simple yet powerful solution for managing expenses. It allows users to perform CRUD operations on expense records and offers a RESTful interface for integration with other applications.

## Features
- **User authentication**: user can create account or create with google login
- **Create:** Add new expense records to the system.
- **Read:** Retrieve information about existing expenses.
- **Update:** Modify details of existing expense records.
- **Delete:** Remove unwanted expense records from the system.
- **Search:** Find expenses based on various criteria.

## Getting Started

### Prerequisites

- [.NET 7 SDK](https://dotnet.microsoft.com/download/dotnet/7.0)
- [Database System (e.g., SQL Server, MySQL)](https://docs.microsoft.com/en-us/ef/core/providers/)

### Installation

1. Clone the repository:

    ```bash
    git clone https://github.com/emmanueldonkor/Finance.Server-api.git
    ```

2. Navigate to the project directory:

    ```bash
    cd Finanace.Server
    ```

3. Set up your database connection in the `appsettings.json` file:

    ```json
    {
      "ConnectionStrings": {
        "DefaultConnection": "YourConnectionStringHere"
      },
      // ... other configurations
      "JWT_ISSUER"
      "JWT_SECRET"
      "CLIENT_ID"
    }
    ```

4. Run database migrations:

    ```bash
    dotnet ef migrations add InitialCreate
    dotnet ef database update
    ```

5. Start the API:

    ```bash
    dotnet run
    ```

## API Endpoints
- Remember theses are just hobby project. and in production a well defined endpoints 
- **GET /Expenses:** Retrieve a list of all expenses.
- **GET /Expenses/{id}:** Retrieve details of a specific expense.
- **POST /Expenses:** Create a new expense.
- **PUT /Expenses/{id}:** Update details of a specific expense.
- **DELETE /Expenses/{id}:** Delete a specific expense.
- **POST /AUTH/signup:**  create account
- **POST /AUTH/signin:** sign in

## Usage Examples

### Create Expense

```http
POST /api/expenses
Content-Type: application/json

{
  "description": "Dinner",
  "amount": 50.00,
}
