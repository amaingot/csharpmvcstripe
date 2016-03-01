using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Stripe;

namespace MvcStripeExample.Areas.Admin.Models
{
    public class EventListViewModel
    {
        public string Id { get; set; }

        public string Type { get; set; }

        public DateTime? Created { get; set; }

        public bool LiveMode { get; set; }

        public string UserId { get; set; }

        public int PendingWebhooks { get; set; }

        public string Request { get; set; }
    }

    public class EventDetailsViewModel : EventListViewModel
    {
        public StripeEventData Data { get; set; }
    }
}