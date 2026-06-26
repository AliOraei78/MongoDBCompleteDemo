using Bogus;
using MongoDB.Bson;
using MongoDBCompleteDemo.Data;
using MongoDBCompleteDemo.Models;
using MongoDBCompleteDemo.Repositories;

namespace MongoDBCompleteDemo.Utilities
{
    public static class DatabaseSeeder
    {
        public static async Task SeedDataAsync(MongoDbContext context)
        {
            // English: Automatically spin up repository to guarantee required collection indexes exist
            var userRepository = new UserRepository(context);
            await userRepository.CreateIndexesAsync();

            // Check if data already exists to avoid duplicate seeding on every startup
            var usersExist = await context.Users.EstimatedDocumentCountAsync() > 0;
            if (usersExist)
            {
                return; // Database is already seeded
            }

            // Configure Bogus Faker for Users
            var userFaker = new Faker<User>()
                .RuleFor(u => u.Id, f => ObjectId.GenerateNewId().ToString())
                .RuleFor(u => u.FullName, f => f.Name.FullName())
                .RuleFor(u => u.Email, (f, u) => f.Internet.Email(u.FullName))
                .RuleFor(u => u.Age, f => f.Random.Number(18, 70))
                .RuleFor(u => u.Version, f => 1) // English: Seed initial baseline version constraint
                .RuleFor(u => u.Balance, f => Math.Round(f.Random.Decimal(500, 5000), 2)) // English: Seed realistic starting financial accounts
                .RuleFor(u => u.CreatedAt, f => f.Date.Past(1))
                .RuleFor(u => u.Address, f => new Address
                {
                    City = f.Address.City(),
                    Street = f.Address.StreetAddress(),
                    PostalCode = f.Address.ZipCode()
                })
                // Generate 2 to 4 random tech-related tags
                .RuleFor(u => u.Tags, f => f.Make(f.Random.Number(2, 4), () => f.Hacker.Noun()).ToList());

            // Generate exactly 30 Users
            var fakeUsers = userFaker.Generate(30);

            // Configure Bogus Faker for Posts
            // Note: We need the fake users list so we can map real AuthorIds
            var postFaker = new Faker<Post>()
                .RuleFor(p => p.Id, f => ObjectId.GenerateNewId().ToString())
                .RuleFor(p => p.Title, f => f.Lorem.Sentence(5))
                .RuleFor(p => p.Content, f => f.Lorem.Paragraphs(2))
                // Pick a random user from our generated list to establish the relationship
                .RuleFor(p => p.AuthorId, f => f.PickRandom(fakeUsers).Id)
                .RuleFor(p => p.CreatedAt, f => f.Date.Past(1));

            // Generate exactly 30 Posts
            var fakePosts = postFaker.Generate(30);

            // Insert everything into MongoDB in bulk for high performance
            await context.Users.InsertManyAsync(fakeUsers);

            // English: Use the public property you already created in MongoDbContext
            await context.Posts.InsertManyAsync(fakePosts);
        }
    }
}