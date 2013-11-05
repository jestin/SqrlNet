using System;
using SqrlServerExample.Data;

namespace SqrlServerExample.DataAccess
{
	public interface INutRepository
	{
		void Create(string nut);
		bool IsNutActive(string nut);
		string IsNutValidated(string nut);
		bool Validate(string nut, string userId);
		bool Delete(string nut);
		long DeleteOlderThan(DateTime time);
	}
}
