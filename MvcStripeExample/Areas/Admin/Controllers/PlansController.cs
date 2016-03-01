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
    public class PlansController : Controller
    {
        // GET: Admin/Plans
        public ActionResult Index()
        {
            var planService = new StripePlanService();
            var plans = planService.List().Select(p =>
                new PlanListViewModel
                {
                    Id = p.Id,
                    Name = p.Name,
                    Amount = p.Amount,
                    Currency = p.Currency,
                    Interval = p.Interval,
                    IntervalCount = p.IntervalCount,
                    LiveMode = p.LiveMode
                });
            return View(plans);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(PlanCreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                var options = new StripePlanCreateOptions
                {
                    Id = model.Id,
                    Amount = model.Amount,
                    Currency = model.Currency,
                    Interval = model.Interval,
                    IntervalCount = model.IntervalCount,
                    Name = model.Name,
                    TrialPeriodDays = model.TrialPeriodDays
                };

                var planService = new StripePlanService();
                var plan = planService.Create(options);

                return RedirectToAction("Index");
            }
            return View(model);
        }

        public ActionResult Details(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var planService = new StripePlanService();
            var p = planService.Get(id);
            var plan = new PlanDetailViewModel
            {
                Id = p.Id,
                Name = p.Name,
                Amount = p.Amount,
                Currency = p.Currency,
                Interval = p.Interval,
                IntervalCount = p.IntervalCount,
                LiveMode = p.LiveMode,
                TrialPeriodDays = p.TrialPeriodDays,
                StatementDescriptor = p.StatementDescriptor,
                Created = p.Created,
                Metadata = p.Metadata
            };
            return View(plan);
        }

        public ActionResult Edit(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var planService = new StripePlanService();
            var p = planService.Get(id);
            var plan = new PlanEditViewModel
            {
                Id = p.Id,
                Name = p.Name,
                StatementDescriptor = p.StatementDescriptor
            };
            return View(plan);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(PlanEditViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var planService = new StripePlanService();
            var options = new StripePlanUpdateOptions
            {
                Name = model.Name,
                StatementDescriptor = model.StatementDescriptor
            };
            planService.Update(model.Id, options);

            return RedirectToAction("Details", new {id = model.Id});
        }

        public ActionResult Delete(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var planService = new StripePlanService();
            var p = planService.Get(id);
            var plan = new PlanDetailViewModel
            {
                Id = p.Id,
                Name = p.Name,
                Amount = p.Amount,
                Currency = p.Currency,
                Interval = p.Interval,
                IntervalCount = p.IntervalCount,
                LiveMode = p.LiveMode,
                TrialPeriodDays = p.TrialPeriodDays,
                StatementDescriptor = p.StatementDescriptor,
                Created = p.Created,
                Metadata = p.Metadata
            };
            return View(plan);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            if(string.IsNullOrEmpty(id))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var planService = new StripePlanService();
            planService.Delete(id);

            return RedirectToAction("Index");
        }
    }
}