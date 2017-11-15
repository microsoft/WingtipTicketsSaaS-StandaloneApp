using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Events_Tenant.Common.Interfaces;
using Events_Tenant.Common.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;

namespace Events_TenantUserApp.Controllers
{
    [Route("/Account")]
    public class AccountController : BaseController
    {
        #region Fields

        private readonly ITenantRepository _tenantRepository;
        private readonly IStringLocalizer<AccountController> _localizer;
        private readonly ILogger _logger;

        #endregion

        #region Constructors

        public AccountController(IStringLocalizer<AccountController> localizer, IStringLocalizer<BaseController> baseLocalizer, ITenantRepository tenantRepository, ILogger<AccountController> logger, IConfiguration configuration)
            : base(baseLocalizer, tenantRepository, configuration)
        {
            _localizer = localizer;
            _tenantRepository = tenantRepository;
            _logger = logger;
        }

        #endregion

        [HttpPost]
        [Route("Login")]
        public async Task<ActionResult> Login(string regEmail)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(regEmail))
                {
                    var message = _localizer["Please type your email."];
                    DisplayMessage(message, "Error");
                }
                else
                {
                    var tenantDetails = _tenantRepository.GetVenue().Result;

                    if (tenantDetails != null)
                    {
                        SetTenantConfig(tenantDetails.VenueId);

                        var customer = await _tenantRepository.GetCustomer(regEmail, tenantDetails.VenueId);

                        if (customer != null)
                        {
                            customer.TenantName = tenantDetails.VenueName;

                            var userSessions = HttpContext.Session.GetObjectFromJson<List<CustomerModel>>("SessionUsers");
                            if (userSessions == null)
                            {
                                userSessions = new List<CustomerModel>
                                {
                                    customer
                                };
                                HttpContext.Session.SetObjectAsJson("SessionUsers", userSessions);
                            }
                            else
                            {
                                userSessions.Add(customer);
                                HttpContext.Session.SetObjectAsJson("SessionUsers", userSessions);
                            }
                        }
                        else
                        {
                            var message = _localizer["The user does not exist."];
                            DisplayMessage(message, "Error");
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
                _logger.LogError(0, ex, "Login failed for tenant");
                return View("Error");
            }
            return Redirect(Request.Headers["Referer"].ToString());
        }

        [Route("Logout")]
        public ActionResult Logout(string email)
        {
            try
            {
                var userSessions = HttpContext.Session.GetObjectFromJson<List<CustomerModel>>("SessionUsers");
                if (userSessions != null)
                {
                    userSessions.Remove(userSessions.First(a => a.Email.ToUpper() == email.ToUpper()));
                    HttpContext.Session.SetObjectAsJson("SessionUsers", userSessions);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(0, ex, "Log out failed for tenant");
                return View("Error");
            }
            return RedirectToAction("Index", "Events");
        }

        [HttpPost]
        [Route("Register")]
        public async Task<ActionResult> Register(CustomerModel customerModel)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return RedirectToAction("Index", "Events");
                }

                var tenantDetails = _tenantRepository.GetVenue().Result;
                if (tenantDetails != null)
                {
                    SetTenantConfig(tenantDetails.VenueId);

                    //check if customer already exists
                    var customer = (_tenantRepository.GetCustomer(customerModel.Email, tenantDetails.VenueId)).Result;

                    if (customer == null)
                    {
                        var customerId = await _tenantRepository.AddCustomer(customerModel, tenantDetails.VenueId);
                        customerModel.CustomerId = customerId;
                        customerModel.TenantName = tenantDetails.VenueName;

                        var userSessions = HttpContext.Session.GetObjectFromJson<List<CustomerModel>>("SessionUsers");
                        if (userSessions == null)
                        {
                            userSessions = new List<CustomerModel>
                        {
                            customerModel
                        };
                            HttpContext.Session.SetObjectAsJson("SessionUsers", userSessions);
                        }
                        else
                        {
                            userSessions.Add(customerModel);
                            HttpContext.Session.SetObjectAsJson("SessionUsers", userSessions);
                        }
                    }
                    else
                    {
                        var message = _localizer["User already exists."];
                        DisplayMessage(message, "Error");
                    }
                }
                else
                {
                    return View("Error");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(0, ex, "Registration failed for tenant ");
                return View("Error");
            }
            return Redirect(Request.Headers["Referer"].ToString());
        }
    }
}
