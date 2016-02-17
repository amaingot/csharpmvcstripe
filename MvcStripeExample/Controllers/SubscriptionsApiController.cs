using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using MvcStripeExample.Models;

namespace MvcStripeExample.Controllers
{
    public class SubscriptionsApiController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/SubscriptionsApi
        public IQueryable<Subscription> GetSubscriptions()
        {
            return db.Subscriptions;
        }

        // GET: api/SubscriptionsApi/5
        [ResponseType(typeof(Subscription))]
        public async Task<IHttpActionResult> GetSubscription(int id)
        {
            Subscription subscription = await db.Subscriptions.FindAsync(id);
            if (subscription == null)
            {
                return NotFound();
            }

            return Ok(subscription);
        }

        // PUT: api/SubscriptionsApi/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutSubscription(int id, Subscription subscription)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != subscription.Id)
            {
                return BadRequest();
            }

            db.Entry(subscription).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SubscriptionExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/SubscriptionsApi
        [ResponseType(typeof(Subscription))]
        public async Task<IHttpActionResult> PostSubscription(Subscription subscription)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Subscriptions.Add(subscription);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = subscription.Id }, subscription);
        }

        // DELETE: api/SubscriptionsApi/5
        [ResponseType(typeof(Subscription))]
        public async Task<IHttpActionResult> DeleteSubscription(int id)
        {
            Subscription subscription = await db.Subscriptions.FindAsync(id);
            if (subscription == null)
            {
                return NotFound();
            }

            db.Subscriptions.Remove(subscription);
            await db.SaveChangesAsync();

            return Ok(subscription);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool SubscriptionExists(int id)
        {
            return db.Subscriptions.Count(e => e.Id == id) > 0;
        }
    }
}