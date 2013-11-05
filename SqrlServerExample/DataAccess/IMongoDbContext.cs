using System;
using MongoDB.Driver;

namespace SqrlServerExample.DataAccess
{
	public interface IMongoDbContext
	{
		MongoServer Server { get; }
		MongoDatabase Database { get; }
		bool Initialized { get; }
		bool Connect(string connectionString, string databaseName);
	}
}

