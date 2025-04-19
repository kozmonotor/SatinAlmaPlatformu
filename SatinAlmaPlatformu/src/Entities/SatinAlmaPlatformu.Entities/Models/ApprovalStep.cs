using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using SatinAlmaPlatformu.Core.Entities;

namespace SatinAlmaPlatformu.Entities.Models;

/// <summary>
/// Belirli bir talep için onay adımlarını ve durumlarını temsil eden entity sınıfı
/// </summary>
public class ApprovalStep : BaseEntity
{
    [Required]
    public int PurchaseRequestId { get; set; }
    
    [ForeignKey("PurchaseRequestId")]
    public virtual PurchaseRequest PurchaseRequest { get; set; } = null!;
    
    public int? ApprovalFlowStepId { get; set; }
    
    [ForeignKey("ApprovalFlowStepId")]
    public virtual ApprovalFlowStep? ApprovalFlowStep { get; set; }
    
    [Required]
    public int Order { get; set; }
    
    [StringLength(100)]
    public string? Name { get; set; }
    
    public int? ApproverId { get; set; }
    
    [ForeignKey("ApproverId")]
    public virtual User? Approver { get; set; }
    
    [Required]
    public ApprovalStatus Status { get; set; } = ApprovalStatus.Pending;
    
    public DateTime? ApprovedRejectedDate { get; set; }
    
    [StringLength(1000)]
    public string? Comments { get; set; }
    
    public string? RejectionReason { get; set; }
}

/// <summary>
/// Onay adımı durumunu belirten enum
/// </summary>
public enum ApprovalStatus
{
    Pending = 0,
    Approved = 1,
    Rejected = 2,
    Skipped = 3,
    NotRequired = 4,
    InProgress = 5
} 