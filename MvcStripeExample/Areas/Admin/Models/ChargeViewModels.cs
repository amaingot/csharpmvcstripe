using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MvcStripeExample.Areas.Admin.Models
{
    public class ChargeListViewModel
    {
        public string Id { get; set; }

        public int Amount { get; set; }

        public DateTime Created { get; set; }

        public bool Paid { get; set; }

        public bool Refunded { get; set; }

        public string Status { get; set; }

        public string CustomerName { get; set; }
    }

    public class ChargeDetailViewModel : ChargeListViewModel
    {
        public string Object { get; set; }

        public bool LiveMode { get; set; }

        public bool? Captured { get; set; }

        public string Currency { get; set; }

        public string SourceCardId { get; set; }

        public string BalanceTransactionId { get; set; }

        public string CustomerId { get; set; }

        public string Description { get; set; }

        public string StatementDescriptor { get; set; }

        public string FailureCode { get; set; }

        public string FailureMessage { get; set; }

        public string InvoiceId { get; set; }

        public Dictionary<string, string> Metadata { get; set; }

        public string ReceiptEmail { get; set; }

        public string ReceiptNumber { get; set; }
    }
}