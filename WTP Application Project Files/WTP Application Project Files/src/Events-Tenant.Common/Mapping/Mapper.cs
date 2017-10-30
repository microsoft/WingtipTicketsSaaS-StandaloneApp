using System;
using Events_Tenant.Common.Models;
using Events_TenantUserApp.EF.TenantsDB;

namespace Events_Tenant.Common.Mapping
{
    public static class Mapper
    {
        #region Entity To Model Mapping

        public static CountryModel ToCountryModel(this Countries country)
        {
            return new CountryModel
            {
                CountryCode = country.CountryCode.Trim(),
                Language = country.Language.Trim(),
                CountryName = country.CountryName.Trim()
            };
        }

        public static CustomerModel ToCustomerModel(this Customers customer)
        {
            return new CustomerModel
            {
                FirstName = customer.FirstName,
                Email = customer.Email,
                PostalCode = customer.PostalCode,
                LastName = customer.LastName,
                CountryCode = customer.CountryCode,
                CustomerId = customer.CustomerId
            };
        }

        public static EventSectionModel ToEventSectionModel(this EventSections eventsection)
        {
            return new EventSectionModel
            {
                EventId = eventsection.EventId,
                Price = eventsection.Price,
                SectionId = eventsection.SectionId
            };
        }

        public static EventModel ToEventModel(this Events eventEntity)
        {
            return new EventModel
            {
                Date = eventEntity.Date,
                EventId = eventEntity.EventId,
                EventName = eventEntity.EventName.Trim(),
                SubTitle = eventEntity.Subtitle.Trim()
            };
        }

        public static SectionModel ToSectionModel(this Sections section)
        {
            return new SectionModel
            {
                SectionId = section.SectionId,
                SeatsPerRow = section.SeatsPerRow,
                SectionName = section.SectionName,
                SeatRows = section.SeatRows,
                StandardPrice = section.StandardPrice
            };
        }

        public static VenueModel ToVenueModel(this Venue venueModel)
        {
            return new VenueModel
            {
                VenueName = venueModel.VenueName.Trim(),
                AdminEmail = venueModel.AdminEmail.Trim(),
                AdminPassword = venueModel.AdminPassword,
                CountryCode = venueModel.CountryCode.Trim(),
                PostalCode = venueModel.PostalCode,
                VenueType = venueModel.VenueType.Trim(),
                VenueId = venueModel.VenueId
            };
        }

        public static VenueTypeModel ToVenueTypeModel(this VenueTypes venueType)
        {
            return new VenueTypeModel
            {
                VenueType = venueType.VenueType.Trim(),
                EventTypeName = venueType.EventTypeName.Trim(),
                EventTypeShortName = venueType.EventTypeShortName.Trim(),
                EventTypeShortNamePlural = venueType.EventTypeShortNamePlural.Trim(),
                Language = venueType.Language.Trim(),
                VenueTypeName = venueType.VenueTypeName.Trim()
            };
        }

        #endregion

        #region Model to Entity Mapping

        public static Customers ToCustomersEntity(this CustomerModel customeModel)
        {
            return new Customers
            {
                CountryCode = customeModel.CountryCode,
                Email = customeModel.Email,
                FirstName = customeModel.FirstName,
                LastName = customeModel.LastName,
                PostalCode = customeModel.PostalCode
            };
        }

        public static TicketPurchases ToTicketPurchasesEntity(this TicketPurchaseModel ticketPurchaseModel)
        {
            //password not required to save demo friction
            return new TicketPurchases
            {
                CustomerId = ticketPurchaseModel.CustomerId,
                PurchaseDate = DateTime.Now,
                PurchaseTotal = ticketPurchaseModel.PurchaseTotal
            };
        }

        public static Tickets ToTicketsEntity(this TicketModel ticketModel)
        {
            return new Tickets
            {
                TicketPurchaseId = ticketModel.TicketPurchaseId,
                SectionId = ticketModel.SectionId,
                EventId = ticketModel.EventId,
                RowNumber = ticketModel.RowNumber,
                SeatNumber = ticketModel.SeatNumber
            };
        }

        #endregion
    }
}
