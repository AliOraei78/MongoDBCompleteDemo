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

### Day 4: Advanced Queries
* Implemented filtering with combined conditions and MongoDB operators
* Used projections to select specific fields
* Applied ascending and descending sorting
* Implemented pagination using Skip and Limit
* Worked with advanced operators: $regex, $in, $gte, $lte, $and
* Added advanced search actions in UsersController

### Day 5: Aggregation Framework

* Introduction to the Aggregation Pipeline and its stages
* Using `$match` for initial filtering
* Using `$group` for grouping and calculations (`sum`, `avg`, `count`)
* Implementing `$lookup` to perform joins between collections (e.g., User and Post)
* Using `$project` and `$unwind` within the pipeline
* Creating statistical and analytical methods in the Repository and Controller layers

### Day 6: Indexing and Performance

* Creating Single Field Indexes, Compound Indexes, and Text Indexes
* Implementing TTL Index for automatic document expiration
* Using `Explain()` to analyze query performance
* Optimizing search queries using indexes
* Implementing `CreateIndexesAsync` method and related Controller actions

## Technologies

* .NET 10
* MongoDB.Driver
* ASP.NET Core Web API
