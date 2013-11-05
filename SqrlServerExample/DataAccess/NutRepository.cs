using System;
using SqrlServerExample.Data;
using MongoDB.Driver;
using MongoDB.Driver.Builders;

namespace SqrlServerExample.DataAccess
{
	public class NutRepository : MongoRepository<NutData>, INutRepository
	{
		public NutRepository (IMongoDbContext dbContext)
			: base(dbContext)
		{
			CollectionName = "Nuts";
		}

		#region INutRepository implementation

		public void Create(string nut)
		{
			var nutData = new NutData
			{
				Nut = nut,
				Timestamp = DateTime.Now
			};

			Collection.Insert(nutData);
		}

		public bool Delete(string nut)
		{
			var result = Collection.Remove(new QueryDocument("Nut", nut));
			return result.DocumentsAffected > 0;
		}

		public long DeleteOlderThan(DateTime time)
		{
			var result = Collection.Remove(Query<NutData>.LT(p => p.Timestamp, time));
			return result.DocumentsAffected;
		}

		#endregion

	}
}
