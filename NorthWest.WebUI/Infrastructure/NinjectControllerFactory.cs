using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Ninject;
using NorthWest.Domain.Entities;
using NorthWest.Domain.Abstract;
using Moq;

namespace NorthWest.WebUI.Infrastructure {
public class NinjectControllerFactory : DefaultControllerFactory {
    private IKernel ninjectKernel;
    public NinjectControllerFactory()
    {
        ninjectKernel = new StandardKernel();
        AddBindings();
    }
    protected override IController GetControllerInstance(RequestContext
    requestContext, Type controllerType)
    {
        return controllerType == null
        ? null
        : (IController)ninjectKernel.Get(controllerType);
    }
    private void AddBindings()
    {
        // put bindings here
        Mock<IProductRepository> mock = new Mock<IProductRepository>();
        mock.Setup(m => m.Products).Returns(new List<Product> {
             new Product { Name = "Fatface", Description = "Cheap staff, good quanlity", Price = 25 },
             new Product { Name = "T-shirt", Description = "Very expensive horrible price", Price = 179 },
             new Product { Name = "Trousers", Description = "I don't know how to descript it", Price = 95 }
}.AsQueryable());
        ninjectKernel.Bind<IProductRepository>().ToConstant(mock.Object);
    }
}
}