using System;
using System.Threading.Tasks;
using Events_Tenant.Common.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;

namespace Events_TenantUserApp.Controllers
{
    public class EventsController : BaseController
    {
        #region Fields
        private readonly ITenantRepository _tenantRepository;
        private readonly ILogger _logger;

        #endregion

        #region Constructors

        public EventsController(ITenantRepository tenantRepository, IStringLocalizer<BaseController> baseLocalizer, ILogger<EventsController> logger, IConfiguration configuration) : base(baseLocalizer, tenantRepository, configuration)
        {
            _logger = logger;
            _tenantRepository = tenantRepository;
        }

        #endregion

        public async Task<ActionResult> Index()
        {
            try
            {
                var tenantDetails = _tenantRepository.GetVenue().Result;
                if (tenantDetails != null)
                {
                    SetTenantConfig(tenantDetails.VenueId);
                    var events = await _tenantRepository.GetEventsForTenant(tenantDetails.VenueId);
                    return View(events);
                }
                else
                {
                    return View("Error");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(0, ex, "Get events failed for tenant");
            }
            return View("Error");
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}

