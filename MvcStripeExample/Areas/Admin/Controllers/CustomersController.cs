using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using MvcStripeExample.Misc;
using Stripe;
using MvcStripeExample.Areas.Admin.Models;
using MvcStripeExample.Models;


namespace MvcStripeExample.Areas.Admin.Controllers
{
    [Authorize(Roles = "admin")]
    public class CustomersController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private ApplicationUserManager _userManager;

        public CustomersController()
        {
        }

        public CustomersController(ApplicationUserManager userManager)
        {
            UserManager = userManager;
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        // GET: Admin/Customers
        public ActionResult Index()
        {
            var customerService = new StripeCustomerService();
            var customers = customerService.List().Select(c =>
                new CustomerBasicListViewModel
                {
                    Id = c.Id,
                    FirstName = UserManager.FindByName(c.Email).FirstName,
                    LastName = UserManager.FindByName(c.Email).LastName,
                    PhoneNumber = UserManager.FindByName(c.Email).PhoneNumber,
                    Email = c.Email,
                    AccountBalance = c.AccountBalance,
                    Created = c.Created,
                    Deleted = c.Deleted,
                    Delinquent = c.Delinquent,
                });

            return View(customers);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(CustomerCreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                var customer = new StripeCustomerCreateOptions
                {
                    Email = model.Email,
                    Description = model.Description
                };

                var customerService = new StripeCustomerService();
                var stripeCustomer = customerService.Create(customer);

                var subscription = new Subscription
                {
                    AdminEmail = model.Email,
                    Status = SubscriptionStatus.TrialWithoutCard,
                    StatusDetail = "Admin created with no card",
                    StripeCustomerId = stripeCustomer.Id
                };

                db.Subscriptions.Add(subscription);
                await db.SaveChangesAsync();

                var user = new ApplicationUser
                {
                    UserName = model.Email,
                    Email = model.Email,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Address = model.Address,
                    Address2 = model.Address2,
                    PhoneNumber = model.PhoneNumber,
                    City = model.City,
                    State = model.State,
                    Zip = model.Zip,
                    SubscriptionId = subscription.Id
                };

                var result = await UserManager.CreateAsync(user);

                if (result.Succeeded)
                {
                    var code = await UserManager.GeneratePasswordResetTokenAsync(user.Id);
                    var callbackUrl = Url.Action("ResetPassword", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                    await UserManager.SendEmailAsync(user.Id, "Create Password", "Please create your password by clicking <a href=\"" + callbackUrl + "\">here</a>");
                    return RedirectToAction("Index");
                }
            }

            return View(model);
        }

        public ActionResult Details(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var customerService = new StripeCustomerService();
            var stripeCustomer = customerService.Get(id);
            var applicationUser = UserManager.FindByEmail(stripeCustomer.Email);

            var customer = new CustomerDetailViewModel
            {
                Id = stripeCustomer.Id,
                Email = stripeCustomer.Email,
                Description = stripeCustomer.Description,
                FirstName = applicationUser.FirstName,
                LastName = applicationUser.LastName,
                PhoneNumber = applicationUser.PhoneNumber,
                Address = applicationUser.Address,
                Address2 = applicationUser.Address2,
                City = applicationUser.City,
                State = applicationUser.State,
                Zip = applicationUser.Zip,
                LiveMode = stripeCustomer.LiveMode,
                Created = stripeCustomer.Created,
                AccountBalance = stripeCustomer.AccountBalance,
                Delinquent = stripeCustomer.Delinquent,
                Deleted = stripeCustomer.Deleted,
                Currency = stripeCustomer.Currency,
                Object = stripeCustomer.Object,
                Subscription = db.Subscriptions.Find(applicationUser.SubscriptionId),
                StripeDiscount = stripeCustomer.StripeDiscount,
                Metadata = stripeCustomer.Metadata,
                Cards = stripeCustomer.SourceList.Data,
                Subscriptions = stripeCustomer.StripeSubscriptionList.Data,
                SubscriptionUsers = UserManager.Users.Where(u =>
                                u.SubscriptionId == applicationUser.SubscriptionId
                                && u.Email != stripeCustomer.Email)
            };

            return View(customer);
        }

        public ActionResult Edit(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var customerService = new StripeCustomerService();
            var stripeCustomer = customerService.Get(id);
            var applicationUser = UserManager.FindByEmail(stripeCustomer.Email);

            var customer = new CustomerCreateViewModel
            {
                Id = stripeCustomer.Id,
                Email = stripeCustomer.Email,
                Description = stripeCustomer.Description,
                FirstName = applicationUser.FirstName,
                LastName = applicationUser.LastName,
                PhoneNumber = applicationUser.PhoneNumber,
                Address = applicationUser.Address,
                Address2 = applicationUser.Address2,
                City = applicationUser.City,
                State = applicationUser.State,
                Zip = applicationUser.Zip
            };

            return View(customer);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(CustomerCreateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var customerService = new StripeCustomerService();
            var stripeCustomer = customerService.Get(model.Id);
            var applicationUser = UserManager.FindByEmail(stripeCustomer.Email);

            var updateOptions = new StripeCustomerUpdateOptions
            {
                Description = model.Description
            };

            customerService.Update(model.Id, updateOptions);

            applicationUser.FirstName = model.FirstName;
            applicationUser.LastName = model.LastName;
            applicationUser.PhoneNumber = model.PhoneNumber;
            applicationUser.Address = model.Address;
            applicationUser.Address2 = model.Address2;
            applicationUser.City = model.City;
            applicationUser.State = model.State;
            applicationUser.Zip = model.Zip;

            UserManager.Update(applicationUser);


            return RedirectToAction("Details", new { id = model.Id });
        }

        public ActionResult ChangeEmail(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var customerService = new StripeCustomerService();
            var stripeCustomer = customerService.Get(id);

            var model = new CustomerChangeEmailViewModel
            {
                Id = id,
                OldEmail = stripeCustomer.Email,
                NewEmail = ""
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChangeEmail(CustomerChangeEmailViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            if (UserManager.Users.Any(u => u.Email == model.NewEmail))
            {
                ModelState.AddModelError("NewEmail", "That email is already in use. Please choose a different email other than" + model.NewEmail);
                return View(model);
            }

            // First change the customer stripe email address
            var options = new StripeCustomerUpdateOptions
            {
                Email = model.NewEmail
            };
            var customerService = new StripeCustomerService();
            customerService.Update(model.Id, options);

            // Next change the user account email address
            var user = UserManager.Users.First(u => u.Email == model.OldEmail);
            user.Email = model.NewEmail;
            user.UserName = model.NewEmail;
            UserManager.Update(user);

            // Finally change the Subscription entity
            var sub = db.Subscriptions.First(s => s.AdminEmail == model.OldEmail);
            sub.AdminEmail = model.NewEmail;
            db.Subscriptions.AddOrUpdate();
            db.SaveChanges();

            return RedirectToAction("Details", new { id = model.Id });
        }

        public ActionResult AddCoupon(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var customerService = new StripeCustomerService();
            var stripeCustomer = customerService.Get(id);

            var user = UserManager.Users.First(u => u.Email == stripeCustomer.Email);

            if (stripeCustomer == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var couponService = new StripeCouponService();
            var coupons = couponService.List().Select(c =>
                new SelectListItem
                {
                    Value = c.Id,
                    Text = c.Id
                });

            var model = new CustomerAddCouponViewModel
            {
                CustomerDescription = string.Format("{0} {1} ({2})", user.FirstName, user.LastName, user.Email),
                CustomerId = id,
                SelectedCoupon = "",
                Coupons = coupons
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddCoupon(CustomerAddCouponViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var options = new StripeCustomerUpdateOptions();
            options.Coupon = model.SelectedCoupon;

            var customerService = new StripeCustomerService();
            customerService.Update(model.CustomerId, options);

            return RedirectToAction("Details", new { id = model.CustomerId });
        }

        public ActionResult ChangePlan(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var customerService = new StripeCustomerService();
            var stripeCustomer = customerService.Get(id);

            var user = UserManager.Users.First(u => u.Email == stripeCustomer.Email);

            if (stripeCustomer == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var currentPlan = stripeCustomer.StripeSubscriptionList.Data.First().StripePlan;

            var planService = new StripePlanService();
            var plans = planService.List().Where(p => p.Id != currentPlan.Id).Select(p =>
                 new SelectListItem
                 {
                     Value = p.Id,
                     Text = p.Name
                 });

            var model = new CustomerChangePlanViewModel
            {
                PreviousPlan = $"{currentPlan.Name} (${currentPlan.Amount} {currentPlan.Interval})",
                CustomerDescription = $"{user.FirstName} {user.LastName} ({user.Email})",
                CustomerId = id,
                SelectedPlan = "",
                Plans = plans
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChangePlan(CustomerChangePlanViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var subscriptionService = new StripeSubscriptionService();
            var subs = subscriptionService.List(model.CustomerId);
            if (subs.Any())
            {
                foreach (var sub in subs)
                {
                    subscriptionService.Cancel(model.CustomerId, sub.Id);
                }
            }

            subscriptionService.Create(model.CustomerId, model.SelectedPlan);

            return RedirectToAction("Details", new { id = model.CustomerId });
        }

        public ActionResult AddCard(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var customerService = new StripeCustomerService();
            var stripeCustomer = customerService.Get(id);

            if (stripeCustomer == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var model = new CustomerUnsecureAddCardViewModel
            {
                CustomerId = id
            };

            return View(model);
        }

        [HttpPost]
        public ActionResult AddCard(CustomerUnsecureAddCardViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var options = new StripeCustomerUpdateOptions
            {
                Source = new StripeSourceOptions()
                {
                    Object = "card",
                    Number = model.CardNumber,
                    AddressCity = model.City,
                    AddressLine1 = model.Address,
                    AddressLine2 = model.Address2,
                    AddressState = model.State.ToString(),
                    AddressZip = model.Zip,
                    Cvc = model.Cvc,
                    Name = model.CardholderName,
                    ExpirationMonth = model.ExpirationMonth.ToString(),
                    ExpirationYear = model.ExpirationYear.ToString()
                }
            };

            var customerService = new StripeCustomerService();
            customerService.Update(model.CustomerId, options);

            return RedirectToAction("Details", new { id = model.CustomerId });
        }
    }
}