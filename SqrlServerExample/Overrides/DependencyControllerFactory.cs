using System;
using System.Web.Mvc;
using System.Web.Routing;
using StructureMap;

namespace SqrlServerExample.Overrides
{
	public class DependencyControllerFactory : DefaultControllerFactory
	{
		protected override IController GetControllerInstance(RequestContext requestContext, Type controllerType)
		{
			return ObjectFactory.GetInstance(controllerType) as Controller;
		}
	}
}

