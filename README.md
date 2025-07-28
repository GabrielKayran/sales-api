# 🚀 Ambev Developer Evaluation - Backend API

A complete REST API developed in .NET 8 following **Clean Architecture** and **Domain-Driven Design (DDD)** principles for managing sales, products, users, and shopping cart.

## 📋 Table of Contents

- [Overview](#-overview)
- [Technologies Used](#-technologies-used)
- [Architecture](#-architecture)
- [Environment Setup](#-environment-setup)
- [Running the Project](#-running-the-project)
- [API Documentation](#-api-documentation)
- [Testing](#-testing)

## 🎯 Overview

This API was developed to meet the requirements of a sales system with the following features:

### 🛍️ **Main Features**
- **Sales Management**: Complete CRUD with business rules for discounts
- **Product Catalog**: Product management with categories and ratings
- **Shopping Cart**: Persistent cart system per user
- **Authentication and Authorization**: JWT system with different access levels
- **Pagination and Filters**: Optimized endpoints with pagination and advanced filters

### 📊 **Implemented Business Rules**
- ✅ 10% discount for purchases above 4 identical items
- ✅ 20% discount for purchases between 10 and 20 identical items
- ✅ Maximum limit of 20 identical items per sale
- ✅ Purchases below 4 items cannot have discount

## 🛠️ Technologies Used

### **Backend**
- **.NET 8.0** - Main framework
- **C# 12** - Programming language
- **Entity Framework Core** - ORM for data access
- **PostgreSQL** - Main database
- **AutoMapper** - Object mapping
- **MediatR** - Mediator pattern for CQRS
- **FluentValidation** - Data validation
- **JWT Bearer** - Authentication and authorization

### **Testing**
- **xUnit** - Unit testing framework
- **NSubstitute** - Mocking library
- **Bogus** - Fake data generation for tests

### **Documentation**
- **Swagger/OpenAPI** - Interactive API documentation

## 🏗️ Architecture

The project follows **Clean Architecture** principles with the following layers:

```
src/
├── Ambev.DeveloperEvaluation.Domain/          # Entities and business rules
├── Ambev.DeveloperEvaluation.Application/     # Use cases and application logic
├── Ambev.DeveloperEvaluation.Infrastructure/  # Data access and external services
├── Ambev.DeveloperEvaluation.WebApi/          # Controllers and API configurations
└── Ambev.DeveloperEvaluation.Common/          # Shared utilities
```

### **Implemented Patterns**
- 🎯 **CQRS** with MediatR
- 🗂️ **Repository Pattern**
- 🔄 **Unit of Work**
- 📋 **Specification Pattern**
- 🎭 **Facade Pattern**

## ⚙️ Environment Setup

### **Prerequisites**
- [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [PostgreSQL 14+](https://www.postgresql.org/download/)
- [Visual Studio 2022](https://visualstudio.microsoft.com/) or [VS Code](https://code.visualstudio.com/)

### **1. Clone the Repository**
```bash
git clone https://github.com/gabriel-dev-test/ambev-api.git
cd ambev-api
```

### **2. Configure the Database**
```bash
# Create a PostgreSQL database
createdb ambev_developer_evaluation

# Configure the connection string in appsettings.json
```

### **3. Configure appsettings.json**
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Database=ambev_developer_evaluation;Username=your_username;Password=your_password"
  },
  "JwtSettings": {
    "SecretKey": "your_super_secure_secret_key_with_at_least_32_characters",
    "Issuer": "AmbevDeveloperEvaluation",
    "Audience": "AmbevDeveloperEvaluationUsers",
    "ExpirationInMinutes": 480
  }
}
```

## 🚀 Running the Project

### **1. Restore Dependencies**
```bash
dotnet restore
```

### **2. Run Migrations**
```bash
dotnet ef database update --project src/Ambev.DeveloperEvaluation.Infrastructure --startup-project src/Ambev.DeveloperEvaluation.WebApi
```

### **3. Run the API**
```bash
cd src/Ambev.DeveloperEvaluation.WebApi
dotnet run
```

The API will be available at:
- **HTTPS**: `https://localhost:7181`
- **HTTP**: `http://localhost:5000`
- **Swagger**: `https://localhost:7181/swagger`

## 📚 API Documentation

### **🔐 Authentication**
The API uses JWT Bearer Token. To access protected endpoints:

1. **Register a user** via `POST /api/auth/register`
2. **Login** via `POST /api/auth/login`
3. **Use the token** in header: `Authorization: Bearer {your_token}`

### **📄 Main Endpoints**

#### **Authentication**
- `POST /api/auth/register` - Register user
- `POST /api/auth/login` - Login

#### **Products**
- `GET /api/products` - List products (paginated)
- `GET /api/products/{id}` - Get product
- `POST /api/products` - Create product (Admin)
- `PUT /api/products/{id}` - Update product (Admin)
- `DELETE /api/products/{id}` - Delete product (Admin)

#### **Cart**
- `GET /api/carts/my-cart` - Get my cart
- `POST /api/carts` - Create cart
- `PUT /api/carts/{id}` - Update cart
- `DELETE /api/carts/{id}` - Delete cart

#### **Sales**
- `GET /api/sales` - List sales (paginated)
- `GET /api/sales/{id}` - Get sale
- `POST /api/sales` - Create sale
- `PUT /api/sales/{id}` - Update sale
- `DELETE /api/sales/{id}` - Cancel sale

### **📊 Pagination**
All listing endpoints support pagination:

```
GET /api/products?page=2&size=10
```

**Parameters:**
- `page`: Page number (default: 1)
- `size`: Items per page (default: 10)

**Response:**
```json
{
  "data": [...],
  "totalItems": 100,
  "currentPage": 2,
  "totalPages": 10,
  "pageSize": 10,
  "hasPreviousPage": true,
  "hasNextPage": true
}
```

## 🧪 Testing

### **Run Unit Tests**
```bash
dotnet test Ambev.DeveloperEvaluation.sln --verbosity minimal
```
 
### **Run Tests with Coverage**
```bash
dotnet test Ambev.DeveloperEvaluation.sln --verbosity minimal --collect:"XPlat Code Coverage"
```

### **Test Structure**
- **Unit Tests**: `/tests/Ambev.DeveloperEvaluation.Unit.Tests`
- **Integration Tests**: `/tests/Ambev.DeveloperEvaluation.Integration.Tests`

### **Code Standards**
- Follow **SOLID** principles
- Use **Clean Architecture**
- Implement **unit tests**
- Document **public APIs**

## 📄 License

This project is under the MIT license. See the [LICENSE](LICENSE) file for more details.

---

**Developed with ❤️ for Ambev Developer Evaluation**
