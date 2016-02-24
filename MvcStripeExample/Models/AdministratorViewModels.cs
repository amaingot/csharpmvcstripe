using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using MvcStripeExample.Misc;
using Newtonsoft.Json;
using Stripe;

namespace MvcStripeExample.Models
{
    public class AdministratorIndexViewModel
    {
        public int CustomerCount { get; set; }

        public int MonthlyCustomerValue { get; set; }

        public int DisputedChargeCount { get; set; }

        public int AccountBalance { get; set; }
    }

    #region Plan Models
    public class PlanListViewModel
    {
        [Display(Name = "Plan Id")]
        public string PlanId { get; set; }

        public string Name { get; set; }

        public int Amount { get; set; }

        [DefaultValue("usd")]
        public string Currency { get; set; }

        [DefaultValue("monthly")]
        public string Interval { get; set; }

        [DefaultValue(1)]
        public int IntervalCount { get; set; }


        public bool LiveMode { get; set; }
    }

    public class PlanCreateViewModel : PlanListViewModel
    {
        public string StatementDescriptor { get; set; }
        public int? TrialPeriodDays { get; set; }
    }

    public class PlanDetailViewModel : PlanCreateViewModel
    {
        public DateTime Created { get; set; }
        public Dictionary<string, string> Metadata { get; set; }
    }
    #endregion

    #region Coupon Models

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
    #endregion

    #region Charge Models

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

    #endregion

    #region Event Models

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

    #endregion

    #region Customer Models

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

        public StripeDiscount StripeDiscount { get; set; }

        public Dictionary<string, string> Metadata { get; set; }

        public IEnumerable<StripeCard> Cards { get; set; }

        public IEnumerable<StripeSubscription> Subscriptions { get; set; }
    }
    #endregion
}