# Museum Web Application

## Overview

Museum Explorer is a web application created as a diploma project. The platform allows users to explore museums, view detailed information, navigate with Google Maps integration, scan QR codes for quick access to museum information, and manage content through a modern ASP.NET Core application.

The project is focused on creating an easy-to-use and responsive experience for visitors while also demonstrating practical web development skills using C#, ASP.NET Core, Entity Framework, and external APIs.

---

## Features

* User registration and authentication
* Museum listing and detailed pages
* QR code generation and scanning support
* Google Maps integration for museum locations
* Image upload and management
* Responsive user interface
* Database integration with Entity Framework Core
* Admin functionality for managing museums and content

---

## Technologies Used

### Backend

* C#
* ASP.NET Core MVC
* Entity Framework Core
* Microsoft SQL Server
* ASP.NET Identity

### Frontend

* HTML5
* CSS
* JavaScript
* Razor Views
* Bootstrap

### APIs and Services

* Google Maps API
* QR Code libraries and services

### NuGet Packages

Examples of commonly used packages in the project:

* Microsoft.EntityFrameworkCore
* Microsoft.EntityFrameworkCore.SqlServer
* Microsoft.EntityFrameworkCore.Tools
* Microsoft.AspNetCore.Identity.EntityFrameworkCore
* QRCoder
* Google Maps related libraries

---

## Project Structure

```text
Agenciq/
│
├── Controllers/
├── Models/
├── Views/
├── Data/
├── wwwroot/
│   ├── css/
│   ├── js/
│   └── images/
├── Services/
├── Migrations/
└── Program.cs
```

---

## Installation

### Requirements

* .NET SDK
* SQL Server
* Visual Studio 2022

### Setup Steps

1. Clone the repository

```bash
git clone <repository-link>
```

2. Open the project in Visual Studio

3. Configure the database connection string in:

```json
appsettings.json
```

4. Apply migrations

```bash
Update-Database
```

5. Run the application

```bash
dotnet run
```

---

## Database

The application uses SQL Server together with Entity Framework Core.

Main entities include:

* Users
* Museums
* Images
* Reviews
* QR code related data

---

## Google Maps Integration

Google Maps is used to display museum locations and improve navigation.

To use the Maps API:

1. Create a Google Cloud project
2. Enable the Google Maps JavaScript API
3. Add your API key to the application configuration

---

## QR Code Functionality

QR codes are used to provide quick access to museum pages and information.

The project uses QR code libraries integrated through NuGet packages.

---

## Screenshots

Add screenshots of:

* Home page
* Museum details page
* QR code feature
* Map integration
* Admin panel

---

## Future Improvements

* Mobile application version
* Museum ticket reservation system
* Favorites and saved locations
* Advanced search and filtering
* Multi-language support

---

## Educational Purpose

This project was created as a diploma project for demonstrating practical skills in:

* Object-oriented programming
* Web development
* Database management
* API integration
* Full-stack application development

---

## Author

Created by Aleksandar Buhtev

---

## License

Created for educational purposes.
