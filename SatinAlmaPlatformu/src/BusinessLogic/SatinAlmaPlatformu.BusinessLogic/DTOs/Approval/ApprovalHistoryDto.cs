using System;

namespace SatinAlmaPlatformu.BusinessLogic.DTOs.Approval
{
    /// <summary>
    /// DTO for representing an approval history item
    /// </summary>
    public class ApprovalHistoryDto
    {
        /// <summary>
        /// ID of the approval step instance
        /// </summary>
        public int ApprovalStepId { get; set; }
        
        /// <summary>
        /// Name of the approval step
        /// </summary>
        public string StepName { get; set; }
        
        /// <summary>
        /// Order of the step in the approval flow
        /// </summary>
        public int StepOrder { get; set; }
        
        /// <summary>
        /// Name of the approver if individual approver
        /// </summary>
        public string ApproverName { get; set; }
        
        /// <summary>
        /// Name of the approver role if role-based approver
        /// </summary>
        public string ApproverRole { get; set; }
        
        /// <summary>
        /// Name of the approver department if department-based approver
        /// </summary>
        public string ApproverDepartment { get; set; }
        
        /// <summary>
        /// Status of the approval step (Pending, Approved, Rejected)
        /// </summary>
        public string Status { get; set; }
        
        /// <summary>
        /// Date and time when the approval action was taken
        /// </summary>
        public DateTime? ActionDate { get; set; }
        
        /// <summary>
        /// Comments provided by the approver
        /// </summary>
        public string Comments { get; set; }
    }
} 