using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MvcStripeExample.Areas.Admin.Models;
using Stripe;

namespace MvcStripeExample.Areas.Admin.Controllers
{
    public class DashboardController : Controller
    {
        // GET: Admin/Dashboard
        public ActionResult Index()
        {
            var customerService = new StripeCustomerService();
            var customers = customerService.List();

            var balanceService = new StripeBalanceService();
            var balance = balanceService.Get();

            var chargeService = new StripeChargeService();
            var charges = chargeService.List().Where(c => c.Dispute != null);

            var sc = customers as StripeCustomer[] ?? customers.ToArray();
            var model = new DashboardViewModel
            {
                CustomerCount = sc.Count(),
                AccountAvailableBalance = balance.Available.Sum(b => b.Amount),
                AccountPendingBalance = balance.Pending.Sum(b => b.Amount),
                MonthlyCustomerValue = sc.Sum(c => c.StripeSubscriptionList.Data.Sum(s => s.StripePlan.Amount)),
                DisputedChargeCount = charges.Sum(c => c.Dispute.Amount.GetValueOrDefault()),
                TrialCustomerCount = sc.Count(c => c.StripeSubscriptionList.Data.Any(s => s.Status.Equals("trialing"))),
                ActiveCustomerCount = sc.Count(c => c.StripeSubscriptionList.Data.Any(s => s.Status.Equals("active"))),
                PastDueCustomerCount = sc.Count(c => c.StripeSubscriptionList.Data.Any(s => s.Status.Equals("past_due"))),
                CanceledCustomerCount = sc.Count(c => c.StripeSubscriptionList.Data.Any(s => s.Status.Equals("canceled"))),
                UnpaidCustomerCount = sc.Count(c => c.StripeSubscriptionList.Data.Any(s => s.Status.Equals("unpaid"))),
            };

            return View(model);
        }
    }
}