using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.WebPages;
using Microsoft.AspNet.Identity;
using MvcStripeExample.Controllers;
using MvcStripeExample.Misc;
using Stripe;

namespace MvcStripeExample.Models
{
    public class Subscription
    {
        public int Id { get; set; }

        public string AdminEmail { get; set; }

        public string StripeCustomerId { get; set; }

        public SubscriptionStatus Status { get; set; }

        public string StatusDetail { get; set; }
    }
}