# SpendSmart

## Overview

SpendSmart is a project that allows users to Book store. It supports multiple database providers, including SQL Server and PostgreSQL. The application allows users to create, view, update, and delete expenses. Images associated with expenses are resized and stored in different resolutions.

## Features

- Manage books  with details like value, description, and images.
- Support for both SQL Server and PostgreSQL databases.
- Dynamic image resizing for better performance and storage optimization.

## Technologies

- **ASP.NET Core**: A cross-platform framework for building modern, cloud-based, internet-connected applications.
- **Entity Framework Core**: An Object-Relational Mapper (ORM) for .NET applications.
- **SQL Server**: A relational database management system developed by Microsoft.
- **PostgreSQL**: An open-source relational database management system.

## Packages

- `system.drawing.common` version 8.0.8
- `microsoft.entityframeworkcore` version 8.0.8
- `microsoft.entityframeworkcore.sqlserver` version 8.0.8
- `microsoft.entityframeworkcore.design` version 8.0.8
- `microsoft.net.http.headers` version 8.0.8
- `npgsql.entityframeworkcore.postgresql` version 8.0.4
- `microsoft.entityframeworkcore.inmemory` version 8.0.8
- `npgsql` version 8.0.3

## Frameworks

- `Microsoft.AspNetCore.App.Ref` version 8.0.5
- `Microsoft.NETCore.App.Ref` version 8.0.5

## Setup
## Install Dependencies:

Ensure you have the .NET SDK installed.
Run the following command to restore the NuGet packages:

dotnet restore

## Configure the Database:

Update the appsettings.json file with your database connection strings and the desired DatabaseProvider.



