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
    public class EventsController : Controller
    {
        // GET: Admin/Events
        public ActionResult Index()
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

        public ActionResult Details(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var eventService = new StripeEventService();
            var e = eventService.Get(id);

            if (e == null)
            {
                return new HttpNotFoundResult();
            }

            var model = new EventDetailsViewModel
            {
                Id = e.Id,
                Created = e.Created,
                LiveMode = e.LiveMode,
                PendingWebhooks = e.PendingWebhooks,
                Request = e.Request,
                Type = e.Type,
                UserId = e.UserId,
                Data = e.Data
            };

            return View(model);
        }
    }
}