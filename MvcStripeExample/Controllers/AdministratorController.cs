using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using MvcStripeExample.Misc;
using MvcStripeExample.Models;
using Stripe;

namespace MvcStripeExample.Controllers
{
    public class AdministratorController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private ApplicationUserManager _userManager;

        public AdministratorController()
        {
        }

        public AdministratorController(ApplicationUserManager userManager)
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

        // GET: Administrator
        public ActionResult Index()
        {
            return View();
        }

        // GET: Administrator/Plans
        public ActionResult Plans()
        {
            var planService = new StripePlanService();
            var plans = planService.List().Select(p => 
                new PlanListViewModel
                {
                    PlanId = p.Id,
                    Name = p.Name,
                    Amount = p.Amount,
                    Currency = p.Currency,
                    Interval = p.Interval,
                    IntervalCount = p.IntervalCount,
                    LiveMode = p.LiveMode
                }); 
            return View(plans);
        }

        // GET: Administrator/CreatePlan
        public ActionResult CreatePlan()
        {
            return View();
        }

        // POST: /Administrator/CreatePlan
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreatePlan(PlanCreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                var options = new StripePlanCreateOptions
                {
                    Id = model.PlanId,
                    Amount = model.Amount,
                    Currency = model.Currency,
                    Interval = model.Interval,
                    IntervalCount = model.IntervalCount,
                    Name = model.Name,
                    TrialPeriodDays = model.TrialPeriodDays,
                    StatementDescriptor = model.StatementDescriptor
                };

                var planService = new StripePlanService();
                var plan = planService.Create(options);

                return RedirectToAction("Plans");
            }
            return View(model);
        }

        // GET: /Administrator/Coupons
        public ActionResult Coupons()
        {
            var couponService = new StripeCouponService();
            var coupons = couponService.List().Select(c => 
                new CouponListViewModel
                {
                    Id = c.Id,
                    AmountOff = c.AmountOff,
                    Currency = c.Currency,
                    Duration = c.Duration,
                    DurationInMonths = c.DurationInMonths,
                    MaxRedemptions = c.MaxRedemptions,
                    PercentOff = c.PercentOff,
                    RedeemBy = c.RedeemBy,
                    TimesRedeemed = c.TimesRedeemed
                });  
            return View(coupons);
        }

        // GET: /Administrator/CreateCoupon
        public ActionResult CreateCoupon()
        {
            return View();
        }

        // GET: /Administrator/CouponDetail/{id}
        public ActionResult CouponDetail(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var couponService = new StripeCouponService();
            var stripeCoupon = couponService.Get(id);
            var coupon = new CouponDetailViewModel
            {
                AmountOff = stripeCoupon.AmountOff,
                Currency = stripeCoupon.Currency,
                Created = stripeCoupon.Created,
                Duration = stripeCoupon.Duration,
                DurationInMonths = stripeCoupon.DurationInMonths,
                Id = stripeCoupon.Id,
                LiveMode = stripeCoupon.LiveMode,
                MaxRedemptions = stripeCoupon.MaxRedemptions,
                Metadata = stripeCoupon.Metadata,
                Object = stripeCoupon.Object,
                Valid = stripeCoupon.Valid,
                TimesRedeemed = stripeCoupon.TimesRedeemed,
                RedeemBy = stripeCoupon.RedeemBy,
                PercentOff = stripeCoupon.PercentOff
            };

            if (coupon == null)
            {
                return HttpNotFound();
            }
            return View(coupon);
        }

        // POST: /Administrator/CreateCoupon
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateCoupon(CouponCreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                var options = new StripeCouponCreateOptions
                {
                    Id = model.Id,
                    AmountOff = model.AmountOff,
                    Currency = model.Currency,
                    Duration = model.Duration,
                    DurationInMonths = model.DurationInMonths,
                    MaxRedemptions = model.MaxRedemptions,
                    PercentOff = model.PercentOff,
                    RedeemBy = model.RedeemBy
                };

                var couponService = new StripeCouponService();
                var coupon = couponService.Create(options);

                return RedirectToAction("Coupons");
            }

            return View(model);
        }

        // GET: /Administrator/DeleteCoupon/{id}
        public ActionResult DeleteCoupon(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var couponService = new StripeCouponService();
            var stripeCoupon = couponService.Get(id);
            var coupon = new CouponDetailViewModel
            {
                AmountOff = stripeCoupon.AmountOff,
                Currency = stripeCoupon.Currency,
                Created = stripeCoupon.Created,
                Duration = stripeCoupon.Duration,
                DurationInMonths = stripeCoupon.DurationInMonths,
                Id = stripeCoupon.Id,
                LiveMode = stripeCoupon.LiveMode,
                MaxRedemptions = stripeCoupon.MaxRedemptions,
                Metadata = stripeCoupon.Metadata,
                Object = stripeCoupon.Object,
                Valid = stripeCoupon.Valid,
                TimesRedeemed = stripeCoupon.TimesRedeemed,
                RedeemBy = stripeCoupon.RedeemBy,
                PercentOff = stripeCoupon.PercentOff
            };
            if (coupon == null)
            {
                return HttpNotFound();
            }
            return View(coupon);
        }

        // POST: /Administrator/DeleteCoupon/{id}
        [HttpPost, ActionName("DeleteCoupon")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteCouponConfirmed(string id)
        {
            var couponService = new StripeCouponService();
            couponService.Delete(id);

            return RedirectToAction("Coupons");
        }

        // GET: /Administrator/Charges
        public ActionResult Charges()
        {
            var chargeService = new StripeChargeService();
            var charges = chargeService.List().Select(c =>
                new ChargeListViewModel
                {
                    Id = c.Id,
                    Amount = c.Amount,
                    Created = c.Created,
                    CustomerName = c.Customer.Email,
                    Paid = c.Paid,
                    Refunded = c.Refunded,
                    Status = c.Status
                });
            return View(charges);
        }

        // GET: /Administrator/ChargeDetails/{id}
        public ActionResult ChargeDetail(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var chargeService = new StripeChargeService();
            var stripeCharge = chargeService.Get(id);
            var charge = new ChargeDetailViewModel
            {
                Id = stripeCharge.Id,
                Amount = stripeCharge.Amount,
                Created = stripeCharge.Created,
                CustomerName = stripeCharge.Customer.Email,
                Paid = stripeCharge.Paid,
                Refunded = stripeCharge.Refunded,
                Status = stripeCharge.Status,
                BalanceTransactionId = stripeCharge.BalanceTransactionId,
                Captured = stripeCharge.Captured,
                Currency = stripeCharge.Currency,
                CustomerId = stripeCharge.CustomerId,
                Object = stripeCharge.Object,
                LiveMode = stripeCharge.LiveMode,
                SourceCardId = stripeCharge.Source.Id,
                Description = stripeCharge.Description,
                StatementDescriptor = stripeCharge.StatementDescriptor,
                FailureCode = stripeCharge.FailureCode,
                FailureMessage = stripeCharge.FailureMessage,
                InvoiceId = stripeCharge.InvoiceId,
                Metadata = stripeCharge.Metadata,
                ReceiptEmail = stripeCharge.ReceiptEmail,
                ReceiptNumber = stripeCharge.ReceiptNumber
            };

            return View(charge);
        }

        // GET: /Administrator/Events
        public ActionResult Events()
        {
            var eventService = new StripeEventService();
            var events = eventService.List().Select(e => 
                new EventListViewModel
                {
                    Id = e.Id,
                    Created = e.Created,
                    LiveMode = e.LiveMode,
                    PendingWebhooks = e.PendingWebhooks,
                    Request = e.Request,
                    Type = e.Type,
                    UserId = e.UserId
                });
            return View(events);
        }

        // GET: /Administrator/Customers
        public ActionResult Customers()
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

        // GET: /Administrator/CreateCustomer
        public ActionResult CreateCustomer()
        {
            return View();
        }

        // POST: /Administrator/CreateCustomer
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateCustomer(CustomerCreateViewModel model)
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
                    return RedirectToAction("Customers");
                }
            }
            
            return View(model);
        }

        // GET: /Administrator/CustomerDetail/{id}
        public ActionResult CustomerDetail(string id)
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

        // GET: /Administrator/EditCustomer/{id}
        // POST: /Administrator/EditCustomer/{id}

        // GET: /Administrator/AddCustomerCoupon/{id}
        // POST: /Administrator/AddCustomerCoupon/{id}

        // GET: /Administrator/CustomerPlan/{id}
        // POST: /Administrator/CustomerPlan/{id}

        // GET: /Administrator/AddCustomerCard/{id}
        // POST: /Administrator/AddCustomerCard/{id}

        // GET: /Administrator/DeleteCustomerCard/{id}
        // POST: /Administrator/DeleteCustomerCard/{id}

        // GET: /Administrator/DeactivateCustomer/{id}
        // POST: /Administrator/DeactivateCustomer/{id}

    }
}