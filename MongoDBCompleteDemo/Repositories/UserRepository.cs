using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using MongoDB.Driver;
using MongoDBCompleteDemo.Data;
using MongoDBCompleteDemo.Models;

namespace MongoDBCompleteDemo.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly IMongoCollection<User> _usersCollection;

        public UserRepository(MongoDbContext context)
        {
            _usersCollection = context.Users;
        }

        public async Task CreateUserAsync(User user)
        {
            await _usersCollection.InsertOneAsync(user);
        }

        public async Task<List<User>> GetAllUsersAsync()
        {
            return await _usersCollection.Find(_ => true).ToListAsync();
        }

        public async Task<User?> GetUserByIdAsync(string id)
        {
            return await _usersCollection.Find(u => u.Id == id).FirstOrDefaultAsync();
        }

        public async Task UpdateUserAsync(string id, User updatedUser)
        {
            await _usersCollection.ReplaceOneAsync(u => u.Id == id, updatedUser);
        }

        public async Task DeleteUserAsync(string id)
        {
            await _usersCollection.DeleteOneAsync(u => u.Id == id);
        }

        public async Task AddAddressToUserAsync(string userId, Address address)
        {
            var update = Builders<User>.Update.Set(u => u.Address, address);
            await _usersCollection.UpdateOneAsync(u => u.Id == userId, update);
        }

        public async Task<List<User>> GetUsersWithAddressAsync()
        {
            return await _usersCollection.Find(u => u.Address != null).ToListAsync();
        }
        public async Task<List<User>> SearchUsersAsync(string? name, int? minAge, int? maxAge)
        {
            var filter = Builders<User>.Filter.Empty;

            if (!string.IsNullOrEmpty(name))
                filter &= Builders<User>.Filter.Regex(u => u.FullName, new BsonRegularExpression(name, "i"));

            if (minAge.HasValue)
                filter &= Builders<User>.Filter.Gte(u => u.Age, minAge.Value);

            if (maxAge.HasValue)
                filter &= Builders<User>.Filter.Lte(u => u.Age, maxAge.Value);

            return await _usersCollection.Find(filter).ToListAsync();
        }

        public async Task<List<User>> GetUsersSortedAsync(string sortBy, bool ascending)
        {
            var sort = ascending
                ? Builders<User>.Sort.Ascending(sortBy)
                : Builders<User>.Sort.Descending(sortBy);

            return await _usersCollection.Find(_ => true).Sort(sort).ToListAsync();
        }

        public async Task<List<User>> GetUsersPagedAsync(int pageNumber, int pageSize)
        {
            var skip = (pageNumber - 1) * pageSize;
            return await _usersCollection.Find(_ => true)
                                         .Skip(skip)
                                         .Limit(pageSize)
                                         .ToListAsync();
        }

        public async Task<List<User>> GetUsersWithOperatorsAsync(List<string> tags, string emailPattern)
        {
            // English: Combine In (for arrays/lists) and Regex filters
            var filter = Builders<User>.Filter.And(
                Builders<User>.Filter.AnyIn(u => u.Tags, tags),
                Builders<User>.Filter.Regex(u => u.Email, new BsonRegularExpression(emailPattern, "i"))
            );

            return await _usersCollection.Find(filter).ToListAsync();
        }

        public async Task<User?> GetUserWithProjectionAsync(string id)
        {
            // English: Only include the fields you want. Everything else (like Address) is excluded automatically.
            var projection = Builders<User>.Projection
                .Include(u => u.Id)
                .Include(u => u.FullName)
                .Include(u => u.Email);

            return await _usersCollection.Find(u => u.Id == id)
                                         .Project<User>(projection)
                                         .FirstOrDefaultAsync();
        }
        public async Task<List<BsonDocument>> GetUserStatisticsAsync()
        {
            var pipeline = new BsonDocument[]
            {
        new BsonDocument("$match", new BsonDocument("age", new BsonDocument("$gte", 18))),
        new BsonDocument("$group", new BsonDocument
        {
            { "_id", "$age" },
            { "count", new BsonDocument("$sum", 1) },
            { "averageAge", new BsonDocument("$avg", "$age") }
        }),
        new BsonDocument("$sort", new BsonDocument("averageAge", -1))
            };

            return await _usersCollection.Aggregate<BsonDocument>(pipeline).ToListAsync();
        }

        public async Task<List<BsonDocument>> GetUsersWithPostsAsync()
        {
            var pipeline = new BsonDocument[]
            {
        new BsonDocument("$lookup", new BsonDocument
        {
            { "from", "Posts" },
            { "localField", "_id" },
            { "foreignField", "authorId" },
            { "as", "posts" }
        }),
        new BsonDocument("$project", new BsonDocument
        {
            { "fullName", 1 },
            { "email", 1 },
            { "postCount", new BsonDocument("$size", "$posts") },
            { "posts", 1 }
        })
            };

            return await _usersCollection.Aggregate<BsonDocument>(pipeline).ToListAsync();
        }

        public async Task<List<BsonDocument>> GetAgeGroupsAsync()
        {
            var pipeline = new BsonDocument[]
            {
        new BsonDocument("$group", new BsonDocument
        {
            // Calculate the age group bracket by dividing by 10 and flooring the result
            { "_id", new BsonDocument("$floor", new BsonDocument("$divide", new BsonArray { "$age", 10 })) },
            { "users", new BsonDocument("$push", "$fullName") },
            { "count", new BsonDocument("$sum", 1) }
        }),
        new BsonDocument("$sort", new BsonDocument("_id", 1))
            };

            return await _usersCollection.Aggregate<BsonDocument>(pipeline).ToListAsync();
        }
        public async Task CreateIndexesAsync()
        {
            // Single Field Index
            await _usersCollection.Indexes.CreateOneAsync(
                new CreateIndexModel<User>(
                    Builders<User>.IndexKeys.Ascending(u => u.Email)
                )
            );

            // Compound Index
            await _usersCollection.Indexes.CreateOneAsync(
                new CreateIndexModel<User>(
                    Builders<User>.IndexKeys
                        .Ascending(u => u.Age)
                        .Descending(u => u.CreatedAt)
                )
            );

            // Text Index for Full-Text Search
            await _usersCollection.Indexes.CreateOneAsync(
                new CreateIndexModel<User>(
                    Builders<User>.IndexKeys.Text(u => u.FullName)
                )
            );

            // TTL Index (Automatically delete documents after 30 days)
            await _usersCollection.Indexes.CreateOneAsync(
                new CreateIndexModel<User>(
                    Builders<User>.IndexKeys.Ascending(u => u.CreatedAt),
                    new CreateIndexOptions
                    {
                        ExpireAfter = TimeSpan.FromDays(30)
                    }
                )
            );
        }

        public async Task<List<User>> SearchUsersWithTextAsync(string searchText)
        {
            var filter = Builders<User>.Filter.Text(searchText);
            return await _usersCollection.Find(filter).ToListAsync();
        }

        public async Task<string> ExplainQueryAsync(string name)
        {
            // 1. Create your regex filter
            var filter = Builders<User>.Filter.Regex(
                u => u.FullName,
                new BsonRegularExpression(name, "i")
            );

            // 2. Render the filter correctly using the modern Driver v3.0+ RenderArgs syntax
            // English: Get the pre-configured serializer and registry from your collection instance
            var serializer = _usersCollection.DocumentSerializer;
            var registry = _usersCollection.Settings.SerializerRegistry;

            // English: Pass both parameters safely inside a single RenderArgs object
            var renderArgs = new RenderArgs<User>(serializer, registry);
            var renderedFilter = filter.Render(renderArgs);

            // 3. Construct the official 'explain' command for an aggregation pipeline
            var explainCommand = new BsonDocument
    {
        { "explain", new BsonDocument
            {
                { "aggregate", "Users" }, // Your collection name
                { "pipeline", new BsonArray
                    {
                        new BsonDocument("$match", renderedFilter)
                    }
                },
                { "cursor", new BsonDocument() }
            }
        },
        { "verbosity", "executionStats" }
    };

            // 4. Run the command directly on the database
            var database = _usersCollection.Database;
            var explanation = await database.RunCommandAsync<BsonDocument>(explainCommand);

            return explanation.ToJson();
        }
        public async Task<List<User>> GetRecentUsersAsync()
        {
            // English: Filter documents created within the last 7 days
            var filter = Builders<User>.Filter.Gte(
                u => u.CreatedAt,
                DateTime.UtcNow.AddDays(-7)
            );

            return await _usersCollection
                .Find(filter)
                .Sort(Builders<User>.Sort.Descending(u => u.CreatedAt))
                .ToListAsync();
        }
    }
}