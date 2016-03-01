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
    public class CouponsController : Controller
    {
        // GET: Admin/Coupons
        public ActionResult Index()
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

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CouponCreateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

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

            return RedirectToAction("Index");
        }

        public ActionResult Details(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var couponService = new StripeCouponService();
            var stripeCoupon = couponService.Get(id);

            if (stripeCoupon == null)
            {
                return HttpNotFound();
            }

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

            return View(coupon);
        }

        public ActionResult Delete(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var couponService = new StripeCouponService();
            var stripeCoupon = couponService.Get(id);

            if (stripeCoupon == null)
            {
                return HttpNotFound();
            }

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

            return View(coupon);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var couponService = new StripeCouponService();
            couponService.Delete(id);

            return RedirectToAction("Index");
        }
    }
}