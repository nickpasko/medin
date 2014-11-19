using System.Data.EntityClient;
using System.Web.Mvc;
using System.Web.Routing;
using MedIn.Domain.Entities;
using MedIn.Web.App_Start;
using MedIn.Web.Controllers;
using MedIn.Web.Mvc;
using Moq;
using MvcContrib.TestHelper;
using NUnit.Framework;
using NUnit.Mocks;

namespace MedIn.WebTests.Routes
{
    [TestFixture]
    public class RouteTests
    {
        [TestFixtureSetUp]
        public void TestMethod1()
        {
            RouteTable.Routes.Clear();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }

        [Test]
        public void RootMapsToHomeIndex()
        {
            "~/".ShouldMapTo<DefaultController>(x => x.Index());
        }

        [Test]
        public void CategoryUrlMapsToCategories()
        {
            //var web = new WebContext();
            var mockContext = new Mock<IWebContext>();
            mockContext.SetupGet(x => x.ViewBag).Returns(null);
            "~/products".ShouldMapTo<ProductsController>(x => x.Details());
        }
    }
}
