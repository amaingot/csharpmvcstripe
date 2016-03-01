using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MvcStripeExample.Areas.Admin.Models;
using Stripe;

namespace MvcStripeExample.Areas.Admin.Controllers
{
    [Authorize(Roles = "admin")]
    public class ChargesController : Controller
    {
        // GET: Admin/Charges
        public ActionResult Index()
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

        public ActionResult Details(string id)
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
    }
}