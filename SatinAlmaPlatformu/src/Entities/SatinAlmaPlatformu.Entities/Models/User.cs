using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using SatinAlmaPlatformu.Core.Entities;

namespace SatinAlmaPlatformu.Entities.Models;

/// <summary>
/// Kullanıcı bilgilerini temsil eden entity sınıfı
/// </summary>
public class User : BaseEntity
{
    [Required]
    [StringLength(100)]
    public string FirstName { get; set; } = null!;
    
    [Required]
    [StringLength(100)]
    public string LastName { get; set; } = null!;
    
    [NotMapped]
    public string FullName => $"{FirstName} {LastName}";
    
    [Required]
    [StringLength(150)]
    [EmailAddress]
    public string Email { get; set; } = null!;
    
    [Required]
    [StringLength(255)]
    public string PasswordHash { get; set; } = null!;
    
    [StringLength(50)]
    public string? PhoneNumber { get; set; }
    
    public int? DepartmentId { get; set; }
    
    [ForeignKey("DepartmentId")]
    public Department? Department { get; set; }
    
    // Kullanıcı Rolleri ilişkisi
    public virtual ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
    
    // Kullanıcının oluşturduğu satın alma talepleri ilişkisi
    public virtual ICollection<PurchaseRequest> Requests { get; set; } = new List<PurchaseRequest>();
} 