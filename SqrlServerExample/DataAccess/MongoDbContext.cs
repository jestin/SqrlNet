using System;
using MongoDB.Driver;

namespace SqrlServerExample.DataAccess
{
	public class MongoDbContext : IMongoDbContext
	{
		#region IMongoDbContext implementation

		public MongoServer Server { get; set; }

		public MongoDatabase Database { get; set; }

		public bool Initialized { get; set; }

		public bool Connect (string connectionString, string databaseName)
		{
			Initialized = false;

			Server = MongoServer.Create(connectionString);

			if(Server != null)
			{
				Database = Server.GetDatabase(databaseName);

				if(Database != null)
				{
					Initialized = true;
				}
			}

			return Initialized;
		}

		#endregion
	}
}

