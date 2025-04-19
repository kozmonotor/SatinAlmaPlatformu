using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using SatinAlmaPlatformu.Core.Entities;

namespace SatinAlmaPlatformu.Entities.Models;

/// <summary>
/// Bildirim modelini temsil eden entity sınıfı
/// </summary>
public class Notification : BaseEntity
{
    [Required]
    public int UserId { get; set; }
    
    [ForeignKey("UserId")]
    public virtual User User { get; set; } = null!;
    
    [Required]
    [StringLength(200)]
    public string Title { get; set; } = null!;
    
    [Required]
    public string Message { get; set; } = null!;
    
    [Required]
    public NotificationType Type { get; set; }
    
    public bool IsRead { get; set; } = false;
    
    public DateTime? ReadAt { get; set; }
    
    [Required]
    public NotificationReferenceType ReferenceType { get; set; }
    
    [Required]
    public int ReferenceId { get; set; }
    
    // Bildirime tıklandığında yönlendirilecek URL
    public string? RedirectUrl { get; set; }
}

/// <summary>
/// Bildirim tiplerini belirten enum
/// </summary>
public enum NotificationType
{
    Info = 0,
    Success = 1,
    Warning = 2,
    Error = 3,
    System = 4
}

/// <summary>
/// Bildirim referans tiplerini belirten enum
/// </summary>
public enum NotificationReferenceType
{
    PurchaseRequest = 0,
    ApprovalStep = 1,
    BidRequest = 2,
    Bid = 3,
    Order = 4,
    User = 5,
    System = 6
} 