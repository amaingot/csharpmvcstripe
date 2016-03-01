using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MvcStripeExample.Misc;
using MvcStripeExample.Models;
using Stripe;

namespace MvcStripeExample.Areas.Admin.Models
{
    public class CustomerBasicListViewModel
    {
        public string Id { get; set; }

        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        public string Email { get; set; }

        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }

        public int AccountBalance { get; set; }

        public bool Delinquent { get; set; }

        public DateTime Created { get; set; }

        public bool? Deleted { get; set; }
    }

    public class CustomerBasicViewModel
    {
        public string Id { get; set; }

        public string Email { get; set; }

        public string Description { get; set; }
    }

    public class CustomerCreateViewModel : CustomerBasicViewModel
    {
        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required]
        [Display(Name = "Phone Number")]
        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }

        [Required]
        public string Address { get; set; }

        [Display(Name = "Address (line 2)")]
        public string Address2 { get; set; }

        [Required]
        public string City { get; set; }

        [Required]
        public State State { get; set; }

        [Required]
        [DataType(DataType.PostalCode)]
        public int Zip { get; set; }
    }

    public class CustomerListViewModel : CustomerCreateViewModel
    {
        public bool LiveMode { get; set; }

        public DateTime Created { get; set; }

        public int AccountBalance { get; set; }

        public bool Delinquent { get; set; }

        public bool? Deleted { get; set; }

        public string Currency { get; set; }
    }

    public class CustomerDetailViewModel : CustomerListViewModel
    {
        public string Object { get; set; }

        public Subscription Subscription { get; set; }

        public StripeDiscount StripeDiscount { get; set; }

        public Dictionary<string, string> Metadata { get; set; }

        public IEnumerable<StripeCard> Cards { get; set; }

        public IEnumerable<StripeSubscription> Subscriptions { get; set; }

        public IEnumerable<ApplicationUser> SubscriptionUsers { get; set; }
    }

    public class CustomerChangeEmailViewModel
    {
        public string Id { get; set; }

        [EmailAddress]
        public string OldEmail { get; set; }

        [Required]
        [EmailAddress]
        public string NewEmail { get; set; }
    }

    public class CustomerAddCouponViewModel
    {
        public string CustomerId { get; set; }

        public string CustomerDescription { get; set; }

        [Display(Name = "Coupon to Add")]
        public string SelectedCoupon { get; set; }

        public IEnumerable<SelectListItem> Coupons { get; set; }
    }

    public class CustomerChangePlanViewModel
    {
        public string CustomerId { get; set; }

        public string CustomerDescription { get; set; }

        public string PreviousPlan { get; set; }

        [Display(Name = "New Plan")]
        public string SelectedPlan { get; set; }

        public IEnumerable<SelectListItem> Plans { get; set; }
    }

    public class CustomerAddCardViewModel
    {
        public string CustomerId { get; set; }

        public string StripeToken { get; set; }
    }

    public class CustomerUnsecureAddCardViewModel
    {
        public string CustomerId { get; set; }

        [Required]
        [DataType(DataType.CreditCard)]
        public string CardNumber { get; set; }

        [Required]
        [Range(16,99)]
        public int ExpirationYear { get; set; }

        [Required]
        [Range(1,12)]
        public int ExpirationMonth { get; set; }

        public string Address { get; set; }

        public string Address2 { get; set; }

        public string City { get; set; }

        public State State { get; set; }

        [DataType(DataType.PostalCode)]
        public string Zip { get; set; }

        public string CardholderName { get; set; }

        [Range(0,999)]
        public string Cvc { get; set; }
    }
}