using System;
using SqrlServerExample.Data;

namespace SqrlServerExample.DataAccess
{
	public interface INutRepository
	{
		void Create(string nut);
		bool IsNutActive(string nut);
		bool Validate(string nut);
		bool Delete(string nut);
		long DeleteOlderThan(DateTime time);
	}
}
