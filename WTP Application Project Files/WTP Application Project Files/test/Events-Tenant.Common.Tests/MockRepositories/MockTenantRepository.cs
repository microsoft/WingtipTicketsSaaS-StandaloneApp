using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Events_Tenant.Common.Interfaces;
using Events_Tenant.Common.Models;

namespace Events_Tenant.Common.Tests.MockRepositories
{
    public class MockTenantRepository : ITenantRepository
    {
        #region Private Variables
        private List<CountryModel> Countries { get; set; }
        private List<CustomerModel> CustomerModels { get; set; }
        private List<EventSectionModel> EventSectionModels { get; set; }
        private List<SectionModel> SectionModels { get; set; }
        private List<TicketPurchaseModel> TicketPurchaseModels { get; set; }
        private List<TicketModel> TicketModels { get; set; }
        private List<EventModel> EventModels { get; set; }
        private List<VenueModel> VenuesModels { get; set; }
        #endregion

        public MockTenantRepository()
        {
            var country = new CountryModel
            {
                Language = "en-us",
                CountryCode = "USA",
                CountryName = "United States"
            };
            Countries = new List<CountryModel> { country };

            CustomerModels = new List<CustomerModel>();

            EventSectionModels = new List<EventSectionModel>
            {
                new EventSectionModel
                {
                    SectionId = 1,
                    EventId = 1,
                    Price = 100
                },
                new EventSectionModel
                {
                    SectionId = 2,
                    EventId = 1,
                    Price = 80
                },
                new EventSectionModel
                {
                    SectionId = 3,
                    EventId = 1,
                    Price = 60
                }
            };

            SectionModels = new List<SectionModel>
            {
                new SectionModel
                {
                    SectionId = 1,
                    SeatsPerRow = 10,
                    SectionName = "section 1",
                    StandardPrice = 100,
                    SeatRows = 4
                },
                new SectionModel
                {
                    SectionId = 2,
                    SeatsPerRow = 20,
                    SectionName = "section 2",
                    StandardPrice = 80,
                    SeatRows = 5
                }
            };

            TicketPurchaseModels = new List<TicketPurchaseModel>
            {
                new TicketPurchaseModel
                {
                    CustomerId = 1,
                    PurchaseTotal = 2,
                    TicketPurchaseId = 5,
                    PurchaseDate = DateTime.Now
                }
            };

            TicketModels = new List<TicketModel>
            {
                new TicketModel
                {
                    SectionId = 1,
                    EventId = 1,
                    TicketPurchaseId = 12,
                    SeatNumber = 50,
                    RowNumber = 2,
                    TicketId = 2
                }
            };

            EventModels = new List<EventModel>
            {
                new EventModel
                {
                    EventId = 1,
                    EventName = "Event 1",
                    Date = DateTime.Now,
                    SubTitle = "Event 1 Subtitle"
                },
                new EventModel
                {
                    EventId = 2,
                    EventName = "Event 2",
                    Date = DateTime.Now,
                    SubTitle = "Event 2 Subtitle"
                }
            };

            VenuesModels = new List<VenueModel>
            {
                new VenueModel
                {
                    CountryCode = "USA",
                    VenueType = "pop",
                    VenueName = "Contoso Concert Hall",
                    PostalCode = "123",
                    AdminEmail = "admin@email.com",
                    AdminPassword = "password",
                    VenueId = 1976168774
                }
            };
        }

        public async Task<List<CountryModel>> GetAllCountries(int tenantId)
        {
            return Countries;
        }

        public async Task<CountryModel> GetCountry(string countryCode, int tenantId)
        {
            return Countries.FirstOrDefault(i => i.CountryCode.Equals(countryCode));
        }

        public async Task<int> AddCustomer(CustomerModel customerModel, int tenantId)
        {
            CustomerModels.Add(customerModel);
            return customerModel.CustomerId;
        }

        public async Task<CustomerModel> GetCustomer(string email, int tenantId)
        {
            return CustomerModels.FirstOrDefault(i => i.Email.Equals(email));
        }

        public async Task<List<EventSectionModel>> GetEventSections(int eventId, int tenantId)
        {
            return EventSectionModels.Where(i => i.EventId == eventId).ToList();
        }

        public async Task<List<SectionModel>> GetSections(List<int> sectionIds, int tenantId)
        {
            return SectionModels.Where(i => sectionIds.Contains(i.SectionId)).ToList();
        }

        public async Task<SectionModel> GetSection(int sectionId, int tenantId)
        {
            return SectionModels.FirstOrDefault();
        }

        public async Task<int> AddTicketPurchase(TicketPurchaseModel ticketPurchaseModel, int tenantId)
        {
            TicketPurchaseModels.Add(ticketPurchaseModel);
            return ticketPurchaseModel.TicketPurchaseId;
        }

        public async Task<int> GetNumberOfTicketPurchases(int tenantId)
        {
            return TicketPurchaseModels.Count();
        }

        public async Task<bool> AddTickets(List<TicketModel> ticketModel, int tenantId)
        {
            foreach (TicketModel tkt in ticketModel)
            {
                TicketModels.Add(tkt);
            }
            return true;
        }

        public async Task<int> GetTicketsSold(int sectionId, int eventId, int tenantId)
        {
            return TicketModels.Count();
        }

        public async Task<VenueModel> GetVenue()
        {
            return VenuesModels.FirstOrDefault();
        }

        public async Task<VenueModel> GetVenueDetails(int tenantId)
        {
            return VenuesModels.FirstOrDefault();
        }

        public async Task<VenueTypeModel> GetVenueType(string venueType, int tenantId)
        {
            return new VenueTypeModel
            {
                Language = "en-us",
                VenueType = "pop",
                EventTypeShortNamePlural = "event short name",
                EventTypeName = "classic",
                VenueTypeName = "type 1",
                EventTypeShortName = "short name"
            };
        }

        public async Task<List<EventModel>> GetEventsForTenant(int tenantId)
        {
            return EventModels.ToList();
        }

        public async Task<EventModel> GetEvent(int eventId, int tenantId)
        {
            return EventModels.Where(i => i.EventId == eventId).FirstOrDefault();
        }

    }
}