using SatinAlmaPlatformu.Entities.Enums;
using System;
using System.Collections.Generic;

namespace SatinAlmaPlatformu.BusinessLogic.DTOs.Approval
{
    public class PendingApprovalDto
    {
        public int ApprovalId { get; set; }
        public int PurchaseRequestId { get; set; }
        public string RequestNumber { get; set; }
        public string RequestTitle { get; set; }
        public decimal RequestAmount { get; set; }
        public string Currency { get; set; }
        public string RequestedBy { get; set; }
        public DateTime RequestDate { get; set; }
        public string ApprovalStepName { get; set; }
        public DateTime? DueDate { get; set; }
        public bool IsOverdue { get; set; }
    }

    public class ApprovalHistoryDto
    {
        public int ApprovalStepId { get; set; }
        public string StepName { get; set; }
        public string ApproverName { get; set; }
        public string ApproverRole { get; set; }
        public string ApproverDepartment { get; set; }
        public ApprovalStatus Status { get; set; }
        public string StatusText => Status.ToString();
        public DateTime? ActionDate { get; set; }
        public string Comments { get; set; }
        public int StepOrder { get; set; }
    }

    public class ApprovalStatusDto
    {
        public int RequestId { get; set; }
        public string RequestNumber { get; set; }
        public ApprovalStatus CurrentStatus { get; set; }
        public string CurrentStatusText => CurrentStatus.ToString();
        public string CurrentStepName { get; set; }
        public int CurrentStepOrder { get; set; }
        public int TotalSteps { get; set; }
        public bool IsCompleted { get; set; }
        public DateTime? CompletionDate { get; set; }
        public List<string> CurrentApprovers { get; set; } = new List<string>();
        public List<ApprovalHistoryDto> History { get; set; } = new List<ApprovalHistoryDto>();
    }

    public class ApprovalActionDto
    {
        public string Comments { get; set; } = string.Empty;
    }
} 