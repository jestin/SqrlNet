using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using SqrlServerExample.Overrides;
using StructureMap;
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
			For<ISqrlServer>().Use<SqrlServer>();
			For<IMongoDbContext>().Use<MongoDbContext>();
			For<INutRepository>().Use<NutRepository>();
		}
	}
}
