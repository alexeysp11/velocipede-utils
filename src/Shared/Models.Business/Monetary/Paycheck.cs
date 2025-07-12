using WorkflowLib.Shared.Models.Business;
using WorkflowLib.Shared.Models.Business.InformationSystem;

namespace WorkflowLib.Shared.Models.Business.Monetary
{
    /// <summary>
    /// Paycheck.
    /// </summary>
    public class Paycheck : WfBusinessEntity, IWfBusinessEntity, ITemporalBusinessEntity
    {
        /// <summary>
        /// Date.
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// Working hours.
        /// </summary>
        public WorkingHours WorkingHours { get; set; }

        /// <summary>
        /// Days off.
        /// </summary>
        public DaysOff? DaysOff { get; set; }

        /// <summary>
        /// Sub total payments.
        /// </summary>
        public SubTotalPayments SubTotalPayments { get; set; }

        /// <summary>
        /// Total amount.
        /// </summary>
        public decimal? Total { get; set; }

        /// <summary>
        /// Employee.
        /// </summary>
        public virtual Employee Employee { get; set; }
        
        /// <summary>
        /// Actual start date.
        /// </summary>
        public DateTime? DateStartActual { get; set; }
        
        /// <summary>
        /// Actual end date.
        /// </summary>
        public DateTime? DateEndActual { get; set; }
        
        /// <summary>
        /// Expected start date.
        /// </summary>
        public DateTime? DateStartExpected { get; set; }
        
        /// <summary>
        /// Expected end date.
        /// </summary>
        public DateTime? DateEndExpected { get; set; }
    }
}