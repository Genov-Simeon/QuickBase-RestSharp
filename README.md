# QuickBase API Test Automation Framework

This project demonstrates API test automation capabilities using C# and NUnit for the QuickBase API. It includes a reusable client library and comprehensive test cases for various API operations.

## Project Structure

- `QuickBaseApi.Client`: Contains the core API client implementation and models
- `QuickBaseApi.Tests`: Contains NUnit test cases for API operations

## Prerequisites

1. .NET 6.0 SDK or later
2. QuickBase Trial Account
3. QuickBase Application Token
4. QuickBase User Token

## Setup Instructions

1. Clone the repository

2. Set up your QuickBase credentials:
   - **Realm**: Found in your QuickBase URL (e.g., `quickbase-abc123` from `https://quickbase-abc123.quickbase.com`)
   - **User Token**: 
     1. Go to your QuickBase User Profile
     2. Click "Manage my user tokens for quickbase-XXX realm..."
     3. Click "+ New user token"
     4. Give your token a name
     5. Add your Application to the User Token
     6. Copy the generated token
   - **App Token**:
     1. Go to App Properties
     2. Click on Security options
     3. Click "Manage Application Token"
     4. Copy the Application Token
   - **Table ID**: The ID of the table you want to test (found in the table properties)

3. The project has two configurations called QA and DEV for easy switching of different environments. Choose the correct one and update the appsettings.*.json configuration in `QuickBaseApi.Tests` with your credentials before building the project.


## Running Tests

```bash
dotnet test
```