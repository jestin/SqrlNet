using SqrlServerExample.Data;
using MongoDB.Driver.Builders;
using MongoDB.Driver;

namespace SqrlServerExample.DataAccess
{
	public class UserRepository : MongoRepository<SqrlUser>, IUserRepository
	{
		#region Constructors

		public UserRepository(IMongoDbContext context)
			: base(context)
		{
			CollectionName = "Users";
		}

		#endregion

		#region IUserRepository implementation

		public void Create(SqrlUser user)
		{
			Collection.Insert(user);
		}

		public SqrlUser Retrieve(string id)
		{
			return Collection.FindOneAs<SqrlUser>(Query.EQ("_id", id));
		}

		public bool Update(SqrlUser user)
		{
			var result = Collection.Save(user);
			return result.DocumentsAffected > 0;
		}

		public bool Delete(string id)
		{
			var result = Collection.Remove(new QueryDocument("_id", id));
			return result.DocumentsAffected > 0;
		}

		#endregion
	}
}
