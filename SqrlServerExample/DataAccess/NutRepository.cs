using System;
using System.Linq;
using SqrlServerExample.Data;
using MongoDB.Driver;
using MongoDB.Driver.Builders;

namespace SqrlServerExample.DataAccess
{
	public class NutRepository : MongoRepository<NutRecord>, INutRepository
	{
		public NutRepository (IMongoDbContext dbContext)
			: base(dbContext)
		{
			CollectionName = "Nuts";
		}

		#region INutRepository implementation

		public void Create(string nut)
		{
			var nutData = new NutRecord
			{
				Id = nut,
				Timestamp = DateTime.Now,
				Validated = false
			};

			Collection.Insert(nutData);
		}

		public bool IsNutActive(string nut)
		{
			var nutRecord = Collection.FindAllAs<NutRecord>().FirstOrDefault(x => x.Id == nut);
			return (nutRecord != null) && !nutRecord.Validated && (nutRecord.Timestamp > DateTime.Now.AddMinutes(-2));
		}

		public bool Validate(string nut)
		{
			var nutRecord = Collection.FindAllAs<NutRecord>().FirstOrDefault(x => x.Id == nut);

			if(nutRecord == null)
			{
				return false;
			}

			nutRecord.Validated = true;

			var result = Collection.Save(nutRecord);
			return result.DocumentsAffected > 0;
		}

		public bool Delete(string nut)
		{
			var result = Collection.Remove(new QueryDocument("_id", nut));
			return result.DocumentsAffected > 0;
		}

		public long DeleteOlderThan(DateTime time)
		{
			var result = Collection.Remove(Query<NutRecord>.LT(p => p.Timestamp, time));
			return result.DocumentsAffected;
		}

		#endregion

	}
}
