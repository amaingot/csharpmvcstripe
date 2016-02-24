using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using MvcStripeExample.Models;
using Stripe;

namespace MvcStripeExample.Controllers
{
    public class SubscriptionController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private ApplicationUserManager _userManager;


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

        // GET: Subscriptions
        public ActionResult Index()
        {
            var userId = User.Identity.GetUserId();
            var user = UserManager.FindById(userId);
            if (user.SubscriptionId == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var subscription = db.Subscriptions.Find(user.SubscriptionId);
            if (subscription == null)
            {
                return HttpNotFound();
            }

            var customerService = new StripeCustomerService();
            var stripeCustomer = customerService.Get(subscription.StripeCustomerId);

            var subscriptionview = new SubscriptionDetailsViewModel
            {
                AdminEmail = stripeCustomer.Email,
                CardExpiration = new DateTime(),
                CardLastFour = "n/a",
                MonthlyPrice = "n/a",
                SubscribedPlan = "n/a"
            };

            var subscriptionService = new StripeSubscriptionService();
            IEnumerable<StripeSubscription> stripeSubscriptions = subscriptionService.List(subscription.StripeCustomerId);
            if (stripeSubscriptions.Any())
            {
                subscriptionview.SubscribedPlan = stripeSubscriptions.FirstOrDefault().StripePlan.Name;
                subscriptionview.MonthlyPrice = stripeSubscriptions.FirstOrDefault().StripePlan.Amount.ToString();
            }

            var cardService = new StripeCardService();
            IEnumerable<StripeCard> stripeCards = cardService.List(subscription.StripeCustomerId);
            if (stripeCards.Any())
            {
                var dateString = string.Format("{1}/1/{0}", stripeCards.FirstOrDefault().ExpirationYear,
                    stripeCards.FirstOrDefault().ExpirationMonth);

                subscriptionview.CardExpiration = DateTime.Parse(dateString);
                subscriptionview.CardLastFour = "XXXX XXXX XXXX " + stripeCards.FirstOrDefault().Last4;
            }

            return View(subscriptionview);
        }

        
        public ActionResult AddCard()
        {
            return View();
        }

        //
        // POST: /Subscription/AddCard
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult AddCard(string stripeToken)
        {
            return null;
        }
    }
}
