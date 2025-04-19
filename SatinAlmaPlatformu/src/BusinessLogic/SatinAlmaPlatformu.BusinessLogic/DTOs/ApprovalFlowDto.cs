using System.ComponentModel.DataAnnotations;

namespace SatinAlmaPlatformu.BusinessLogic.DTOs;

/// <summary>
/// Onay akışı DTO sınıfı
/// </summary>
public class ApprovalFlowDto
{
    public int Id { get; set; }
    
    [Required(ErrorMessage = "Akış adı zorunludur")]
    [MaxLength(100, ErrorMessage = "Akış adı en fazla 100 karakter olabilir")]
    public string Name { get; set; } = null!;
    
    [MaxLength(500, ErrorMessage = "Açıklama en fazla 500 karakter olabilir")]
    public string? Description { get; set; }
    
    public bool IsActive { get; set; } = true;
    
    public int? DepartmentId { get; set; }
    
    public string? DepartmentName { get; set; }
    
    public int? CategoryId { get; set; }
    
    public string? CategoryName { get; set; }
    
    public decimal? MinAmount { get; set; }
    
    public decimal? MaxAmount { get; set; }
    
    public List<ApprovalFlowStepDto> Steps { get; set; } = new List<ApprovalFlowStepDto>();
}

/// <summary>
/// Onay akışı adımı DTO sınıfı
/// </summary>
public class ApprovalFlowStepDto
{
    public int Id { get; set; }
    
    public int ApprovalFlowId { get; set; }
    
    [Required(ErrorMessage = "Adım sırası zorunludur")]
    [Range(1, 100, ErrorMessage = "Adım sırası 1-100 arasında olmalıdır")]
    public int Order { get; set; }
    
    [MaxLength(100, ErrorMessage = "Adım adı en fazla 100 karakter olabilir")]
    public string? Name { get; set; }
    
    public int? RoleId { get; set; }
    
    public string? RoleName { get; set; }
    
    public int? ApproverId { get; set; }
    
    public string? ApproverName { get; set; }
    
    public int? AlternateApproverId { get; set; }
    
    public string? AlternateApproverName { get; set; }
    
    public bool IsRequired { get; set; } = true;
    
    public decimal? MinAmount { get; set; }
}

/// <summary>
/// Onay akışı oluşturma/güncelleme için kullanılan DTO sınıfı
/// </summary>
public class CreateUpdateApprovalFlowDto
{
    [Required(ErrorMessage = "Akış adı zorunludur")]
    [MaxLength(100, ErrorMessage = "Akış adı en fazla 100 karakter olabilir")]
    public string Name { get; set; } = null!;
    
    [MaxLength(500, ErrorMessage = "Açıklama en fazla 500 karakter olabilir")]
    public string? Description { get; set; }
    
    public bool IsActive { get; set; } = true;
    
    public int? DepartmentId { get; set; }
    
    public int? CategoryId { get; set; }
    
    public decimal? MinAmount { get; set; }
    
    public decimal? MaxAmount { get; set; }
    
    [Required(ErrorMessage = "En az bir onay adımı gereklidir")]
    public List<CreateUpdateApprovalFlowStepDto> Steps { get; set; } = new List<CreateUpdateApprovalFlowStepDto>();
}

/// <summary>
/// Onay akışı adımı oluşturma/güncelleme için kullanılan DTO sınıfı
/// </summary>
public class CreateUpdateApprovalFlowStepDto
{
    [Required(ErrorMessage = "Adım sırası zorunludur")]
    [Range(1, 100, ErrorMessage = "Adım sırası 1-100 arasında olmalıdır")]
    public int Order { get; set; }
    
    [MaxLength(100, ErrorMessage = "Adım adı en fazla 100 karakter olabilir")]
    public string? Name { get; set; }
    
    public int? RoleId { get; set; }
    
    public int? ApproverId { get; set; }
    
    public int? AlternateApproverId { get; set; }
    
    public bool IsRequired { get; set; } = true;
    
    public decimal? MinAmount { get; set; }
} 