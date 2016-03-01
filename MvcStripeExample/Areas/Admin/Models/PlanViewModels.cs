using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MvcStripeExample.Areas.Admin.Models
{
    public class PlanEditViewModel
    {
        public string Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string StatementDescriptor { get; set; }
    }

    public class PlanListViewModel
    {
        [Required]
        [Display(Name = "Plan Id")]
        public string Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public int Amount { get; set; }

        [Required]
        [DefaultValue("usd")]
        public string Currency { get; set; }

        [Required]
        [DefaultValue("month")]
        public string Interval { get; set; }

        public int IntervalCount { get; set; }

        public bool LiveMode { get; set; }
    }

    public class PlanCreateViewModel : PlanListViewModel
    {
        public int? TrialPeriodDays { get; set; }
    }

    public class PlanDetailViewModel : PlanCreateViewModel
    {
        public string StatementDescriptor { get; set; }
        public DateTime Created { get; set; }
        public Dictionary<string, string> Metadata { get; set; }
    }
}