# ASP.NET Core E-Commerce Store

A modern, responsive e-commerce web application built with **ASP.NET Core (.NET 10)**, following the **MVC** (Model-View-Controller) architecture. The project features a fully functional shopping cart, secure user authentication, an admin dashboard, and a seamless database design utilizing EF Core.

## 🚀 Key Features

*   **Product Catalog:** Browse products, filter by categories, and search by name.
*   **Stateless Shopping Cart:** Users can add items to their cart without being logged in. Cart data is safely preserved using ASP.NET Core Session (`DistributedMemoryCache`).
*   **User Roles & Authentication:** Secured through **ASP.NET Core Identity** (Roles: `Admin` and `User`).
*   **Checkout & Order History:** Secure checkout flow for logged-in users and a clean history of previous orders.
*   **Admin Dashboard:** Dedicated admin panel to manage products, update order statuses (Pending, Shipped, Delivered), permanently or temporarily ban users via Identity `Lockout`, and view sales statistics.
*   **Modern UI:** Beautiful, fully responsive layout styled exclusively via **Tailwind CSS v4** (integrated directly into the MSBuild process) and enhanced with **Tailwind Plus Elements**.

## 🛠️ Technology Stack

*   **Backend:** C#, ASP.NET Core (.NET 10), MVC pattern
*   **Database:** Microsoft SQL Server
*   **ORM:** Entity Framework Core (Code-First approach + Migrations)
*   **Authentication:** ASP.NET Core Identity
*   **Frontend:** HTML5, Razor Views (`.cshtml`), Tailwind CSS v4, Tailwind Plus Elements

## ⚙️ Getting Started (Local Setup)

To run this project locally, ensure you have the [.NET 10 SDK](https://dotnet.microsoft.com/download) and **Node.js/npm** (for Tailwind CSS) installed on your machine.

1. **Clone the repository**
   git clone https://github.com/puacnikolina/ASP.NET-project.git cd ASP.NET-project

2. **Database Migration**
   The application relies on SQL Server. Apply Entity Framework migrations to create and seed the initial database schema: dotnet ef database update
*(Note: The process includes creating a default Admin account on startup)*

3. **Install npm packages (for Tailwind CSS)**
   Required to generate the `output.css` file: 
   npm install

4. **Run the Application**
   dotnet run

The site will be available at `http://localhost:5112` or `https://localhost:7269`.

## 📂 Architecture Note
This project heavily relies on **Code-First** Entity Framework logic. The database schema (Tables: `AspNetUsers`, `Products`, `Categories`, `Orders`, `OrderItems`) is entirely mapped from C# classes dynamically. 

The shopping cart bypasses the database completely until the user proceeds to checkout; it relies on JSON serialized objects stored inside backend RAM via `IHttpContextAccessor` Sessions.