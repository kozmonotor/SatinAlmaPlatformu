using System;
using System.Collections.Generic;

namespace SatinAlmaPlatformu.BusinessLogic.DTOs.Approval
{
    /// <summary>
    /// DTO for representing the overall approval status of a purchase request
    /// </summary>
    public class ApprovalStatusDto
    {
        /// <summary>
        /// ID of the purchase request
        /// </summary>
        public int RequestId { get; set; }
        
        /// <summary>
        /// Purchase request number
        /// </summary>
        public string RequestNumber { get; set; }
        
        /// <summary>
        /// Current status of the approval workflow (e.g., "Onay Bekliyor", "Tamamlandı", "Başlatılmadı")
        /// </summary>
        public string CurrentStatus { get; set; }
        
        /// <summary>
        /// Name of the current approval step
        /// </summary>
        public string CurrentStepName { get; set; }
        
        /// <summary>
        /// Order of the current step in the approval flow
        /// </summary>
        public int CurrentStepOrder { get; set; }
        
        /// <summary>
        /// Total number of steps in the approval flow
        /// </summary>
        public int TotalSteps { get; set; }
        
        /// <summary>
        /// Whether the approval process is completed
        /// </summary>
        public bool IsCompleted { get; set; }
        
        /// <summary>
        /// Date and time when the approval process was completed
        /// </summary>
        public DateTime? CompletionDate { get; set; }
        
        /// <summary>
        /// Names of current approvers for the active step
        /// </summary>
        public List<string> CurrentApprovers { get; set; }
        
        /// <summary>
        /// Complete approval history for this purchase request
        /// </summary>
        public List<ApprovalHistoryDto> History { get; set; }
    }
}