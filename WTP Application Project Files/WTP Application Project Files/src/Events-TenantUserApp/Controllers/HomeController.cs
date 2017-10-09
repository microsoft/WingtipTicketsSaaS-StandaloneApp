using System;
using System.Threading.Tasks;
using Events_Tenant.Common.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Events_TenantUserApp.Controllers
{
    public class HomeController : Controller
    {
        #region Fields
        private readonly ITenantRepository _tenantRepository;
        private readonly ILogger _logger;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="HomeController" /> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        #endregion


        /// <summary>
        /// This method is hit when not passing any tenant name
        /// Will display the Events Hub page
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> Index()
        {
            try
            {
                return RedirectToAction("Index", "Events", new { tenant = "contosoconcerthall" });
            }
            catch (Exception ex)
            {
                _logger.LogError(0, ex, "Error in getting all tenants in Events Hub");
            }
            return View("Error");
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
