using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MvcStripeExample.Areas.Admin.Models
{
    public class CouponCreateViewModel
    {
        public int? PercentOff { get; set; }

        public string Currency { get; set; }

        public int? AmountOff { get; set; }

        public string Duration { get; set; }

        public string Id { get; set; }

        public int? MaxRedemptions { get; set; }

        public DateTime? RedeemBy { get; set; }

        public int? DurationInMonths { get; set; }

    }

    public class CouponListViewModel : CouponCreateViewModel
    {
        public int TimesRedeemed { get; set; }
    }

    public class CouponDetailViewModel : CouponListViewModel
    {
        public string Object { get; set; }

        public bool LiveMode { get; set; }

        public DateTime Created { get; set; }

        public bool Valid { get; set; }

        public Dictionary<string, string> Metadata { get; set; }
    }
}