using System;

namespace SqrlServerExample.DataAccess
{
	public interface INutRepository
	{
		void Create(string nut);
		bool Delete(string nut);
		long DeleteOlderThan(DateTime time);
	}
}
