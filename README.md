# 🧾 Mini Inventory System – Interview Assignment

A lightweight, secure, and scalable backend API built with ASP.NET Core for managing Products, Customers, and Sales. Designed using clean architecture principles and modern .NET best practices.

---

## 🚀 Tech Stack

- **.NET 8** (ASP.NET Core Web API)
- **Entity Framework Core (Code-First)**
- **SQL Server**
- **JWT Authentication**
- **Swagger & Postman** for API testing
- **Async/Await** throughout

---

## ✅ Core Features

- **Product & Customer Management**
  - Full CRUD APIs
  - Soft delete support (`IsDeleted`)
  - Filtering and pagination (products)

- **Sales Module**
  - Stock validation
  - Discount (user-defined, max 100%)
  - Fixed VAT (15%)
  - 3000ms simulated processing delay
  - Max 3 concurrent sales (HTTP 429 if exceeded)

- **Sales Summary Report**
  - Total Sales, Revenue, Transaction Count by date range

- **JWT Authentication**
  - Token-based protection for all endpoints (except login)

---

## 📦 What's Included

- ✔️ Full source code
- ✔️ EF Core migration files
- ✔️ SQL script (`inventoryDb.sql`)
- ✔️ Postman collection with environment + test
- ✔️ `README.md` with setup instructions

---

## 🧑‍💻 Getting Started

### 1️⃣ Clone the Repository

```bash
git clone https://github.com/aliakramtushar/MiniInventorySystem.git
cd MiniInventorySystem
2️⃣ Configure Database
In appsettings.json:

"ConnectionStrings": {
  "DefaultConnection": "Server=YOUR_SQL_SERVER;Database=InventoryDb;Trusted_Connection=True;"
}
Replace YOUR_SQL_SERVER with your actual SQL Server name (e.g., LAPTOP\\SQLEXPRESS).

3️⃣ Set Up the Database
Option A: Run EF Core Migration

dotnet ef database update
Option B: Run SQL Script
Open DatabaseSetup.sql in SQL Server Management Studio and execute manually.

4️⃣ Run the API

dotnet run
Navigate to:
https://localhost:5001/swagger

🔐 Authentication
Login to get JWT token:


POST /api/auth/login
Credentials:
{
  "username": "admin",
  "password": "admin123"
}
Use the returned token as Bearer {token} in the Authorization header for all other endpoints.

📮 Postman Collection
File: InventorySystem.postman_collection.json

Includes environment setup (base URL & token variable)

Includes automated token generation & test scripts

📊 Sample Endpoints
Action	Method	Endpoint
Login	POST	/api/auth/login
List Products	GET	/api/products
Create Product	POST	/api/products
Create Sale	POST	/api/sales
Sales Summary Report	GET	/api/sales/summary?from=...&to=...

⚠️ Concurrency Simulation
Send 4 concurrent sale requests:

Only 3 will process

4th will return:
{
  "success": false,
  "message": "Too many concurrent sales. Try again later.",
  "statusCode": 429
}

📎 Notes
All endpoints are secured with JWT (except login).

Discount must be between 0–100%.

VAT is fixed at 15% after discount.

Sales reduce stock. If stock is insufficient, the sale is rejected.

Product & Customer list excludes soft-deleted entries.

👨‍💻 Author
Ali Akram
Senior Software Engineer
📧 aliakramtushar@gmail.com

