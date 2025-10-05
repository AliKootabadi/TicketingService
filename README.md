# 🎫 Offline Ticketing System API

A simple **ASP.NET Core 8 Web API** that implements an **offline ticket management system** for internal organizational support requests.  
using **Entity Framework Core**, **JWT authentication**, and **SQLite**.

---

## 🧱 Features

- 🔐 **JWT Authentication** (Login system with roles)
- 👥 Two user roles:
  - **Employee** – Can create and view their own tickets
  - **Admin** – Can view, update, assign, and delete tickets
- 🎫 **Ticket Management**
  - Create, view, update, and delete tickets
  - Assign tickets to Admin users
  - Track ticket status (`Open`, `InProgress`, `Closed`)
  - Track priority (`Low`, `Medium`, `High`)
- 📊 **Statistics Endpoint**
  - View total ticket count per status
- 💾 **SQLite Database**
  - Runs locally, no external dependencies
- 🧪 **Swagger UI + Postman Collection**
  - For testing all endpoints easily

---

## ⚙️ Tech Stack

- **.NET 8 (ASP.NET Core Web API)**
- **Entity Framework Core 8**
- **SQLite**
- **JWT Authentication**
- **Swagger UI**
- **Role-Based Authorization**

---

## 🚀 Getting Started

### Access the API

Swagger UI:
http://localhost:5000/swagger

Base API URL:
http://localhost:5000

### Postman Collection
TicketingService.postman_collection.json

### Default Users
| Role     | Email                | Password       |
| -------- | -------------------- | -------------- |
| Admin    | `Admin@Email.com`    | `Admin`    |
| Employee | `Employee@Email.com` | `Employee` |
