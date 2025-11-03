using VelocipedeUtils.Shared.Models.Business.Customers;
using VelocipedeUtils.Shared.Models.Business.InformationSystem;
using VelocipedeUtils.Shared.Models.Business.Monetary;
using VelocipedeUtils.Shared.Models.Business.Responsibilities;

namespace VelocipedeUtils.Shared.Models.Business.BusinessDocuments
{
    /// <summary>
    /// Employment contract.
    /// </summary>
    public class EmploymentContract : WfBusinessEntity, IWfBusinessEntity, ITemporalBusinessEntity
    {
        /// <summary>
        /// Contract.
        /// </summary>
        public Contract? Contract { get; set; }

        /// <summary>
        /// Employment contract type.
        /// </summary>
        public EmploymentContractType? EmploymentContractType { get; set; }

        /// <summary>
        /// Expired employment contract.
        /// </summary>
        public EmploymentContract? ExpiredEmploymentContract { get; set; }

        /// <summary>
        /// Company.
        /// </summary>
        public Company? Company { get; set; }

        /// <summary>
        /// Employee.
        /// </summary>
        public Employee? Employee { get; set; }

        /// <summary>
        /// Pay rate.
        /// </summary>
        public PayRate? PayRate { get; set; }

        /// <summary>
        /// Employee responsibilities.
        /// </summary>
        [Obsolete("It's better to use EmploymentContractResponsibility object")]
        public required ICollection<EmployeeResponsibility> EmployeeResponsibilities { get; set; }

        /// <summary>
        /// Employer responsibilities.
        /// </summary>
        [Obsolete("It's better to use EmploymentContractResponsibility object")]
        public required ICollection<EmployerResponsibility> EmployerResponsibilities { get; set; }
        
        /// <summary>
        /// Employee benifits.
        /// </summary>
        [Obsolete("It's better to use EmploymentContractBenefit object")]
        public required ICollection<EmployeeBenefit> EmployeeBenifits { get; set; }
        
        /// <summary>
        /// Schedule.
        /// </summary>
        public Schedule? Schedule { get; set; }

        /// <summary>
        /// Non-compete clause.
        /// </summary>
        public NonCompeteClause? NonCompeteClause { get; set; }

        /// <summary>
        /// Status of the employment contract.
        /// </summary>
        public EmploymentContractStatus? Status { get; set; }

        /// <summary>
        /// Internal termination reason.
        /// </summary>
        public EmploymentTerminationReason? InternalTerminationReason { get; set; }

        /// <summary>
        /// Official termination reason.
        /// </summary>
        public EmploymentTerminationReason? OfficialTerminationReason { get; set; }
        
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