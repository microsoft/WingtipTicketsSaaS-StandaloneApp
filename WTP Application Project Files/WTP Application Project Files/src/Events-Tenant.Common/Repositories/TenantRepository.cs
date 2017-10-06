using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Events_Tenant.Common.Interfaces;
using Events_Tenant.Common.Mapping;
using Events_Tenant.Common.Models;
using Events_Tenant.Common.Utilities;
using Events_TenantUserApp.EF.TenantsDB;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

namespace Events_Tenant.Common.Repositories
{
    public class TenantRepository : ITenantRepository
    {
        #region Private variables
        private readonly TenantDbContext _tenantDbContext;

        #endregion

        #region Constructor

        public TenantRepository(TenantDbContext tenantDbContext)
        {
            _tenantDbContext = tenantDbContext;
        }

        #endregion

        #region Countries

        public async Task<List<CountryModel>> GetAllCountries(int tenantId)
        {
            var allCountries = await _tenantDbContext.Countries.ToListAsync();
            return allCountries.Count > 0 ? allCountries.Select(country => country.ToCountryModel()).ToList() : null;
        }

        public async Task<CountryModel> GetCountry(string countryCode, int tenantId)
        {
            var country = await _tenantDbContext.Countries.Where(x => x.CountryCode == countryCode).FirstOrDefaultAsync();
            return country?.ToCountryModel();
        }

        #endregion

        #region Customers

        public async Task<int> AddCustomer(CustomerModel customeModel, int tenantId)
        {
            var customer = customeModel.ToCustomersEntity();
            customer.VenueId = tenantId;
            _tenantDbContext.Customers.Add(customer);
            await _tenantDbContext.SaveChangesAsync();
            return customer.CustomerId;
        }

        public async Task<CustomerModel> GetCustomer(string email, int tenantId)
        {
            var customer = await _tenantDbContext.Customers.Where(i => i.Email == email && i.VenueId == tenantId).FirstOrDefaultAsync();
            return customer?.ToCustomerModel();
        }

        #endregion

        #region EventSections

        public async Task<List<EventSectionModel>> GetEventSections(int eventId, int tenantId)
        {

            var eventsections = await _tenantDbContext.EventSections.Where(i => i.EventId == eventId && i.VenueId == tenantId).ToListAsync();
            return eventsections.Count > 0 ? eventsections.Select(eventSection => eventSection.ToEventSectionModel()).ToList() : null;
        }

        #endregion

        #region Events

        public async Task<List<EventModel>> GetEventsForTenant(int tenantId)
        {

            //Past events (yesterday and earlier) are not shown 
            var events = await _tenantDbContext.Events.Where(i => i.Date >= DateTime.Now && i.VenueId == tenantId).OrderBy(x => x.Date).ToListAsync();

            return events.Count > 0 ? events.Select(eventEntity => eventEntity.ToEventModel()).ToList() : null;

        }

        public async Task<EventModel> GetEvent(int eventId, int tenantId)
        {

            var eventModel = await _tenantDbContext.Events.Where(i => i.EventId == eventId && i.VenueId == tenantId).FirstOrDefaultAsync();

            return eventModel?.ToEventModel();

        }

        #endregion

        #region Sections

        public async Task<List<SectionModel>> GetSections(List<int> sectionIds, int tenantId)
        {

            var sections = await _tenantDbContext.Sections.Where(i => sectionIds.Contains(i.SectionId) && i.VenueId == tenantId).ToListAsync();

            return sections.Any() ? sections.Select(section => section.ToSectionModel()).ToList() : null;

        }

        public async Task<SectionModel> GetSection(int sectionId, int tenantId)
        {

            var section = await _tenantDbContext.Sections.Where(i => i.SectionId == sectionId && i.VenueId == tenantId).FirstOrDefaultAsync();

            return section?.ToSectionModel();

        }

        #endregion

        #region TicketPurchases

        public async Task<int> AddTicketPurchase(TicketPurchaseModel ticketPurchaseModel, int tenantId)
        {

            var ticketPurchase = ticketPurchaseModel.ToTicketPurchasesEntity();
            ticketPurchase.VenueId = tenantId;

            _tenantDbContext.TicketPurchases.Add(ticketPurchase);
            await _tenantDbContext.SaveChangesAsync();

            return ticketPurchase.TicketPurchaseId;

        }

        public async Task<int> GetNumberOfTicketPurchases(int tenantId)
        {

            var ticketPurchases = await _tenantDbContext.TicketPurchases.Where(i => i.VenueId == tenantId).ToListAsync();
            if (ticketPurchases.Any())
            {
                return ticketPurchases.Count();
            }

            return 0;
        }

        #endregion

        #region Tickets

        public async Task<bool> AddTicket(List<TicketModel> ticketModel, int tenantId)
        {

            foreach (TicketModel t in ticketModel)
            {
                t.VenueId = tenantId;
                _tenantDbContext.Tickets.Add(t.ToTicketsEntity());
            }
            await _tenantDbContext.SaveChangesAsync();

            return true;
        }

        public async Task<int> GetTicketsSold(int sectionId, int eventId, int tenantId)
        {
            var tickets = await _tenantDbContext.Tickets.Where(i => i.SectionId == sectionId && i.EventId == eventId && i.VenueId == tenantId).ToListAsync();
            if (tickets.Any())
            {
                return tickets.Count();
            }
            return 0;
        }

        #endregion

        #region Venues

        public async Task<List<VenuesModel>> GetAllVenues()
        {
            var allVenuesList = await _tenantDbContext.Venues.ToListAsync();

            if (allVenuesList.Count > 0)
            {
                return allVenuesList.Select(venue => venue.ToVenueModel()).ToList();
            }
            return null;
        }

        public async Task<VenuesModel> GetVenue(string tenantName)
        {
            var tenants = await _tenantDbContext.Venues.Where(i => Regex.Replace(i.VenueName.ToLower(), @"\s+", "") == tenantName).ToListAsync();

            if (tenants.Any())
            {
                var tenant = tenants.FirstOrDefault();
                return tenant?.ToVenueModel();
            }

            return null;
        }

        public async Task<VenuesModel> GetVenueDetails(int tenantId)
        {
            var venue = await _tenantDbContext.Venues.Where(x => x.VenueId == tenantId).FirstOrDefaultAsync();
            if (venue != null)
            {
                var venueModel = venue.ToVenueModel();
                return venueModel;
            }
            return null;
        }
        #endregion

        #region VenueTypes

        public async Task<VenueTypeModel> GetVenueType(string venueType, int tenantId)
        {
            var venueTypeDetails = await _tenantDbContext.VenueTypes.Where(i => i.VenueType == venueType).FirstOrDefaultAsync();
            return venueTypeDetails?.ToVenueTypeModel();
        }
        #endregion
    }
}
