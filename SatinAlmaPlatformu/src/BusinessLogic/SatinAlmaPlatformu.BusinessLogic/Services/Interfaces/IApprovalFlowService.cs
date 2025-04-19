using SatinAlmaPlatformu.BusinessLogic.DTOs;
using SatinAlmaPlatformu.Core.Models;

namespace SatinAlmaPlatformu.BusinessLogic.Services.Interfaces;

/// <summary>
/// Onay akışı yönetimi için servis arayüzü
/// </summary>
public interface IApprovalFlowService
{
    /// <summary>
    /// Tüm onay akışlarını getirir
    /// </summary>
    Task<List<ApprovalFlowDto>> GetAllApprovalFlowsAsync();
    
    /// <summary>
    /// Belirtilen ID'ye göre onay akışını getirir
    /// </summary>
    Task<ApprovalFlowDto> GetApprovalFlowByIdAsync(int id);
    
    /// <summary>
    /// Onay akışı oluşturur
    /// </summary>
    Task<Result<ApprovalFlowDto>> CreateApprovalFlowAsync(CreateUpdateApprovalFlowDto dto);
    
    /// <summary>
    /// Onay akışını günceller
    /// </summary>
    Task<Result<ApprovalFlowDto>> UpdateApprovalFlowAsync(int id, CreateUpdateApprovalFlowDto dto);
    
    /// <summary>
    /// Onay akışını siler
    /// </summary>
    Task<Result> DeleteApprovalFlowAsync(int id);
    
    /// <summary>
    /// Onay akışını aktif veya pasif yapar
    /// </summary>
    Task<Result> ToggleApprovalFlowStatusAsync(int id, bool isActive);
    
    /// <summary>
    /// Belirli kriterlere uygun onay akışlarını getirir
    /// </summary>
    Task<List<ApprovalFlowDto>> GetApprovalFlowsByCriteriaAsync(int? departmentId, int? categoryId, decimal? amount);
    
    /// <summary>
    /// Satın alma talebi için geçerli onay akışını belirler
    /// </summary>
    Task<ApprovalFlowDto> DetermineApprovalFlowForRequestAsync(int purchaseRequestId);
} 