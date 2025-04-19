using System;

namespace SatinAlmaPlatformu.BusinessLogic.DTOs.Approval
{
    /// <summary>
    /// DTO for representing a pending approval request
    /// </summary>
    public class PendingApprovalDto
    {
        /// <summary>
        /// ID of the approval step instance
        /// </summary>
        public int ApprovalId { get; set; }
        
        /// <summary>
        /// ID of the purchase request
        /// </summary>
        public int PurchaseRequestId { get; set; }
        
        /// <summary>
        /// Purchase request number
        /// </summary>
        public string RequestNumber { get; set; }
        
        /// <summary>
        /// Title of the purchase request
        /// </summary>
        public string RequestTitle { get; set; }
        
        /// <summary>
        /// Amount of the purchase request
        /// </summary>
        public decimal RequestAmount { get; set; }
        
        /// <summary>
        /// Currency of the purchase request
        /// </summary>
        public string Currency { get; set; }
        
        /// <summary>
        /// Name of the user who created the request
        /// </summary>
        public string RequestedBy { get; set; }
        
        /// <summary>
        /// Date when the request was created
        /// </summary>
        public DateTime RequestDate { get; set; }
        
        /// <summary>
        /// Name of the approval step
        /// </summary>
        public string ApprovalStepName { get; set; }
        
        /// <summary>
        /// Due date for the approval
        /// </summary>
        public DateTime? DueDate { get; set; }
        
        /// <summary>
        /// Whether the approval is overdue
        /// </summary>
        public bool IsOverdue { get; set; }
    }
} 