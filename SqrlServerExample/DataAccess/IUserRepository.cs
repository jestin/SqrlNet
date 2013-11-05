using System;
using SqrlServerExample.Data;

namespace SqrlServerExample.DataAccess
{
	public interface IUserRepository
	{
		void Create(SqrlUser user);
		SqrlUser Retrieve(string id);
		bool Update(SqrlUser user);
		bool Delete(string id);
	}
}
