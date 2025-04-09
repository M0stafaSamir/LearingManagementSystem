# 🎓 Learning Management System (LMS) - ASP.NET Core MVC

This is a fully-featured **Learning Management System (LMS)** web application built using **ASP.NET Core MVC**. It allows instructors to create and manage courses, and students to register, enroll, and track progress. The system follows a layered architecture and includes user authentication, authorization, and role management.

---

## 📚 Features

- 👤 **Authentication & Authorization** (Students / Instructors / Admins)
- 🧑‍🏫 **Course Management** (Create, Update, Delete Courses)
- 📂 **Category & Subcategory Support**
- 📝 **Enrollments and Progress Tracking**
- 💬 **Reviews and Ratings**
- 🔍 **Search and Filter Courses**
- 📦 **Entity Framework Core** for database access
- 🧩 **Clean MVC Architecture**

---

## 🛠️ Tech Stack

- ASP.NET Core MVC (.NET 8)
- Entity Framework Core
- SQL Server
- Identity (Authentication & Roles)
- Bootstrap for UI
- AutoMapper
- LINQ

---

## 🧱 Project Structure

```bash
LMS/
├── Controllers/
├── Models/
├── Views/
│   ├── Courses/
│   ├── Account/
│   └── Shared/
├── Data/
│   └── ApplicationDbContext.cs
├── Services/
│   └── Interfaces and Implementations
├── wwwroot/
│   └── CSS, JS, Images
├── Program.cs
├── Startup.cs or appsettings.json
