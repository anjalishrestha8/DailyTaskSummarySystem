# Daily Task Summary System

An **ASP.NET Core MVC + Web API** application that enables users to submit daily work summaries and allows administrators to manage users, review submissions, and provide feedback through comments.
This project demonstrates authentication and authorization, Entity Framework Core, JWT authentication, ASP.NET Identity, Generic Repository Pattern, pagination, searching, sorting, SMTP email integration, and ASP.NET Core MVC/Web API development.

## Features

### User Features

* User registration and login
* JWT-based authentication
* Submit one task summary per day
* Edit task summaries only on the day they are created
* View task summary details
* Forgot password functionality
* Reset password through email link

### Admin Features

* View all submitted task summaries
* Review summaries grouped by date
* Manage users and roles
* Add comments to task summaries
* Monitor user activity

### Additional Features

* JWT Authentication
* Role-Based Authorization
* Generic Repository Pattern
* Pagination
* Search & Filtering
* Sorting
* SMTP Email Notifications
* AutoMapper Integration
* Entity Framework Core Migrations

---

## Technology Stack

### Backend

* **C#**
* **ASP.NET Core Web API**
* **Entity Framework Core**
* **ASP.NET Identity**
* **JWT Authentication**
* **SQL Server**
* **AutoMapper**

### Frontend

* **ASP.NET Core MVC**
* **Razor Views**
* **HTML**
* **CSS**
* **JavaScript**
* **Bootstrap**

### Design Patterns & Concepts

* Generic Repository Pattern
* Dependency Injection
* Layered Architecture
* DTO Pattern
* Authentication & Authorization

---

## Authentication

The application uses:

* ASP.NET Identity
* JWT Token Authentication
* Cookie-Based Authentication (MVC Client)
* Role-Based Authorization

---

## Email Functionality

SMTP email integration is used for:

* User registration by admin notifications
* Password setup emails
* Forgot password workflow

---

## Database

Database operations are handled using:

* Entity Framework Core
* Code-First Migrations
* SQL Server

---

## Learning Outcomes

This project helped me strengthen my understanding of:

* ASP.NET Core MVC
* ASP.NET Core Web API
* Entity Framework Core
* Repository Pattern
* JWT Authentication
* ASP.NET Identity
* AutoMapper
* SQL Server

---

## Future Enhancements

* Real-time comments using SignalR
* Dashboard analytics
* Unit Testing
* Integration Testing
* Docker Support
* CI/CD Pipeline
