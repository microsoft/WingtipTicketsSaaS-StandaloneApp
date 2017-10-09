using System.Threading.Tasks;
using Events_Tenant.Common.Interfaces;
using Events_Tenant.Common.Models;
using Events_TenantUserApp.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Xunit;
using Assert = Xunit.Assert;

namespace Events_TenantUserApp.Tests.ControllerTests
{
    //https://docs.microsoft.com/en-us/aspnet/core/mvc/controllers/testing
    //https://docs.microsoft.com/en-us/dotnet/articles/core/testing/unit-testing-with-dotnet-test
    //https://github.com/aspnet/Tooling/issues/664
    //http://stackoverflow.com/questions/40190679/how-to-reference-an-asp-net-core-project-from-a-full-net-framework-test-project


    [TestClass]
    public class HomeControllerTests
    {
        private readonly HomeController _homeController;

        public HomeControllerTests(ILogger<HomeController> logger)
        {
            var mockTenantRepo = new Mock<ITenantRepository>();

            mockTenantRepo.Setup(repo => repo.GetVenueById(1234646)).Returns(GetVenueDetails());
            mockTenantRepo.Setup(repo => repo.GetVenueByName("Venue Name")).Returns(GetVenueDetails());

            _homeController = new HomeController(mockTenantRepo.Object, logger);
        }


        [Fact]
        public void Index_GetAllTenantDetails()
        {
            //Act
            var result = _homeController.Index();

            // Assert
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.NotNull(redirectToActionResult.ControllerName);
            Assert.Equal("Index", redirectToActionResult.ActionName);
        }

        private async Task<VenueModel> GetVenueDetails()
        {
            return new VenueModel
            {
                AdminEmail = "adminEmail",
                AdminPassword = "Password",
                CountryCode = "USA",
                PostalCode = "123",
                VenueName = "Venue Name",
                VenueType = "classic"
            };
        }
    }
}
