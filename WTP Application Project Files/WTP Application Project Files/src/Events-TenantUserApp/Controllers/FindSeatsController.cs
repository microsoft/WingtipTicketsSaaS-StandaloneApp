using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Events_Tenant.Common.Interfaces;
using Events_Tenant.Common.Models;
using Events_TenantUserApp.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;

namespace Events_TenantUserApp.Controllers
{
    [Route("/FindSeats")]
    public class FindSeatsController : BaseController
    {
        #region Private varibles

        private readonly ITenantRepository _tenantRepository;
        private readonly IStringLocalizer<FindSeatsController> _localizer;
        private readonly ILogger _logger;

        #endregion

        #region Constructor

        public FindSeatsController(ITenantRepository tenantRepository, IStringLocalizer<FindSeatsController> localizer, IStringLocalizer<BaseController> baseLocalizer, ILogger<FindSeatsController> logger, IConfiguration configuration) : base(baseLocalizer, tenantRepository, configuration)
        {
            _tenantRepository = tenantRepository;
            _localizer = localizer;
            _logger = logger;
        }

        #endregion

        [Route("FindSeats")]
        public async Task<ActionResult> FindSeats(int eventId)
        {
            try
            {
                if (eventId != 0)
                {
                    var tenantDetails = _tenantRepository.GetVenue().Result;
                    if (tenantDetails != null)
                    {
                        SetTenantConfig(tenantDetails.VenueId);

                        var eventDetails = await _tenantRepository.GetEvent(eventId, tenantDetails.VenueId);

                        if (eventDetails != null)
                        {
                            var eventSections = await _tenantRepository.GetEventSections(eventId, tenantDetails.VenueId);
                            var seatSectionIds = eventSections.Select(i => i.SectionId).ToList();

                            var seatSections = await _tenantRepository.GetSections(seatSectionIds, tenantDetails.VenueId);
                            if (seatSections != null)
                            {
                                var ticketsSold = await _tenantRepository.GetTicketsSold(seatSections[0].SectionId, eventId, tenantDetails.VenueId);

                                FindSeatViewModel viewModel = new FindSeatViewModel
                                {
                                    EventDetails = eventDetails,
                                    SeatSections = seatSections,
                                    SeatsAvailable = (seatSections[0].SeatRows * seatSections[0].SeatsPerRow) - ticketsSold
                                };

                                return View(viewModel);
                            }
                        }
                    }
                    else
                    {
                        return View("Error");
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(0, ex, "FindSeats failed for tenant");
                return View("Error");
            }
            return RedirectToAction("Index", "Events");
        }

        [Route("GetAvailableSeats")]
        public async Task<ActionResult> GetAvailableSeats(int sectionId, int eventId)
        {
            try
            {
                var tenantDetails = _tenantRepository.GetVenue().Result;
                if (tenantDetails != null)
                {
                    SetTenantConfig(tenantDetails.VenueId);

                    var sectionDetails = await _tenantRepository.GetSection(sectionId, tenantDetails.VenueId);
                    var totalNumberOfSeats = sectionDetails.SeatRows * sectionDetails.SeatsPerRow;
                    var ticketsSold = await _tenantRepository.GetTicketsSold(sectionId, eventId, tenantDetails.VenueId);

                    var availableSeats = totalNumberOfSeats - ticketsSold;
                    return Content(availableSeats.ToString());
                }
                else
                {
                    return View("Error");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(0, ex, "GetAvailableSeats failed for tenant");
                return Content("0");
            }
        }

        [HttpPost]
        [Route("PurchaseTickets")]
        public async Task<ActionResult> PurchaseTickets(int eventId, int customerId, decimal ticketPrice, int ticketCount, int sectionId)
        {
            try
            {
                bool purchaseResult = false;
                var ticketPurchaseModel = new TicketPurchaseModel
                {
                    CustomerId = customerId,
                    PurchaseTotal = ticketPrice
                };
                var tenantDetails = _tenantRepository.GetVenue().Result;
                if (tenantDetails != null)
                {
                    SetTenantConfig(tenantDetails.VenueId);
                    var purchaseTicketId = await _tenantRepository.AddTicketPurchase(ticketPurchaseModel, tenantDetails.VenueId);
                    List<TicketModel> ticketsModel = BuildTicketModel(eventId, sectionId, ticketCount, purchaseTicketId);
                    purchaseResult = await _tenantRepository.AddTickets(ticketsModel, tenantDetails.VenueId);
                    if (purchaseResult)
                        DisplayMessage(_localizer[$"You have successfully purchased {ticketCount} ticket(s)."], "Confirmation");
                    else
                        DisplayMessage(_localizer["Failed to purchase tickets."], "Error");
                }
                else
                {
                    return View("Error");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(0, ex, "Purchase tickets failed for tenant");
                return View("Error");
            }
            return RedirectToAction("Index", "Events");
        }
    }
}
