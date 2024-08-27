## SpendSmart
Overview
SpendSmart is an application designed for managing a book store. It supports multiple database providers, including SQL Server and PostgreSQL, and enables users to create, view, update, and delete expenses. The application also handles image resizing for optimized storage and performance. Data synchronization between different databases is facilitated using Hangfire for background task management.

## Features
Book Management: Add, view, update, and delete book expenses with details such as value, description, and images.
Database Support: Seamless integration with both SQL Server and PostgreSQL.
Image Resizing: Automatically resize images to different resolutions for better performance and storage efficiency.
Data Synchronization: Regular synchronization of data between SQL Server and PostgreSQL.

## Technologies
ASP.NET Core 8.0: A cross-platform framework for building modern, cloud-based, internet-connected applications.

Entity Framework Core 8.0.8: An Object-Relational Mapper (ORM) for .NET applications.

SQL Server: A relational database management system developed by Microsoft.

PostgreSQL: An open-source relational database management system.

Hangfire 1.8.14: Used for scheduling and managing background tasks.

Npgsql 8.0.3: PostgreSQL database provider for Entity Framework Core.

System.Drawing.Common 8.0.8: Used for image processing.

## Installation
NuGet Packages
Add the following NuGet packages to your project:

System.Drawing.Common (8.0.8)

Hangfire (1.8.14)

Microsoft.EntityFrameworkCore (8.0.8)

Microsoft.EntityFrameworkCore.SqlServer (8.0.8)

Microsoft.EntityFrameworkCore.Design (8.0.8)

Hangfire.SqlServer (1.8.14)

Hangfire.AspNetCore (1.8.14)

Hangfire.Core (1.8.14)

Hangfire.NetCore (1.8.14)

Npgsql.EntityFrameworkCore.PostgreSQL (8.0.4)

Microsoft.EntityFrameworkCore.InMemory (8.0.8)

Npgsql (8.0.3)

Hangfire.PostgreSql (1.20.9)

SDK Packs
Ensure the following .NET SDK packs are installed:

Microsoft.AspNetCore.App.Ref 8.0.5: Located at C:\Program Files\dotnet\packs\Microsoft.AspNetCore.App.Ref\8.0.5\

Microsoft.NETCore.App.Ref 8.0.5: Located at C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\8.0.5\



## Dependencies
Ensure the following dependencies are installed and properly configured for the application to function correctly:

.NET 8.0
Entity Framework Core
Hangfire

## License
This project is licensed under the MIT License - see the LICENSE file for details.

Configuration
