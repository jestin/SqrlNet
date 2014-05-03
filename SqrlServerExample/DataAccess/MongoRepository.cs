using System;
using MongoDB.Driver;

namespace SqrlServerExample.DataAccess
{
	public abstract class MongoRepository<T>
	{
		#region Dependencies

		protected readonly IMongoDbContext _db;

		#endregion

		#region Properties

		protected string CollectionName { get; set; }

		protected virtual MongoCollection Collection
		{
			get
			{
				if(_collection == null)
				{
					_collection = _db.Database.GetCollection<T>(CollectionName);
				}

				return _collection;
			}
		}

		#endregion

		#region Member Variables

		private MongoCollection _collection;

		#endregion

		#region Constructors

		protected MongoRepository (IMongoDbContext dbContext)
		{
			_db = dbContext;

			// initialize MongoDB
			if(!_db.Initialized)
			{
				try
				{
					_db.Connect("mongodb://127.0.0.1/?safe=true", "sqrlserverexample");
				}
				catch(Exception)
				{
					Console.Error.WriteLine("Could not connect to the database");
				}
			}
		}

		#endregion
	}
}

