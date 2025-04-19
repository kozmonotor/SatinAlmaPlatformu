using System.ComponentModel.DataAnnotations;

namespace SatinAlmaPlatformu.Core.Entities;

/// <summary>
/// Tüm entity'ler için ortak özellikleri içeren temel sınıf
/// </summary>
public abstract class BaseEntity : IEntity
{
    [Key]
    public int Id { get; set; }
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    public string? CreatedBy { get; set; }
    
    public DateTime? ModifiedAt { get; set; }
    
    public string? ModifiedBy { get; set; }
    
    public bool IsDeleted { get; set; } = false;
} 