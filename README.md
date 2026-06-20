# MongoDB Complete Demo - .NET Backend Sample Project

A complete educational MongoDB project using the C# Driver to demonstrate backend development skills.

## Current Progress

### Day 1: Introduction and Environment Setup

* Create an ASP.NET Core Web API project in Visual Studio
* Install the `MongoDB.Driver` package
* Implement `MongoDbContext` for database connectivity
* Define the initial `User` model
* Configure `appsettings.json` and Dependency Injection
* Create a test controller to verify the connection

### Day 2: Basic CRUD Operations

* Create the `IUserRepository` interface and implement `UserRepository`
* Implement full CRUD operations (Create, Read, Update, Delete)
* Register the repository in `Program.cs` with Singleton lifetime
* Create `UsersController` with RESTful actions
* Test operations using Swagger UI or Postman

### Day 3: Data Modeling and Mapping

* Use MongoDB attributes (`[BsonId]`, `[BsonElement]`, `[BsonRepresentation]`, `[BsonDefaultValue]`)
* Implement embedded documents (e.g., `Address` class inside `User`)
* Implement references between collections (e.g., `Post` model with `AuthorId`)
* Create `PostRepository` and `PostsController`
* Add new methods for working with embedded documents

## Technologies

* .NET 10
* MongoDB.Driver
* ASP.NET Core Web API
