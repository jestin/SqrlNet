using StructureMap.Configuration.DSL;
using SqrlNet.Crypto;
using SqrlNet.Server;
using SqrlServerExample.DataAccess;

namespace SqrlServerExample
{
	public class DependencyRegistry : Registry
	{
		public DependencyRegistry()
		{
			For<ISqrlSigner>().Use<SqrlSigner>();
			For<IAesHandler>().Use<AesHandler>();
			For<ISqrlPseudoRandomNumberGenerator>().Use<SqrlPseudoRandomNumberGenerator>();
			For<ISqrlServer>().Use<SqrlServer>();
			For<IMongoDbContext>().Use<MongoDbContext>();
			For<INutRepository>().Use<NutRepository>();
			For<IUserRepository>().Use<UserRepository>();
		}
	}
}
