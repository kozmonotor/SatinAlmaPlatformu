using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SatinAlmaPlatformu.BusinessLogic.DTOs;
using SatinAlmaPlatformu.Core.Models;
using SatinAlmaPlatformu.BusinessLogic.DTOs.Approval;
using SatinAlmaPlatformu.Core.Results;

namespace SatinAlmaPlatformu.BusinessLogic.Services.Interfaces;

/// <summary>
/// Satın alma taleplerinin onay süreçlerini yöneten servis arayüzü
/// </summary>
public interface IPurchaseRequestApprovalService
{
    /// <summary>
    /// Satın alma talebi oluşturulduğunda veya güncellendiğinde uygun onay akışını belirler ve başlatır
    /// </summary>
    /// <param name="purchaseRequestId">Satın alma talebi ID'si</param>
    /// <param name="userId">İşlemi yapan kullanıcı ID'si</param>
    /// <returns>İşlem sonucunu içeren Result nesnesi</returns>
    Task<Result> InitiateApprovalFlowAsync(int purchaseRequestId, int userId);
    
    /// <summary>
    /// Bir sonraki onay adımına geçer
    /// </summary>
    /// <param name="purchaseRequestId">Satın alma talebi ID'si</param>
    /// <param name="currentStepId">Mevcut adım ID'si</param>
    /// <param name="userId">İşlemi yapan kullanıcı ID'si</param>
    /// <returns>İşlem sonucunu içeren Result nesnesi</returns>
    Task<Result> MoveToNextStepAsync(int purchaseRequestId, int currentStepId, int userId);
    
    /// <summary>
    /// Satın alma talebini onaylar
    /// </summary>
    /// <param name="purchaseRequestId">Satın alma talebi ID'si</param>
    /// <param name="stepId">Onay adımı ID'si</param>
    /// <param name="approverId">Onaylayan kullanıcı ID'si</param>
    /// <param name="comments">Onay yorumu</param>
    /// <returns>İşlem sonucunu içeren Result nesnesi</returns>
    Task<Result> ApproveRequestAsync(int purchaseRequestId, int stepId, int approverId, string comments);
    
    /// <summary>
    /// Satın alma talebini reddeder
    /// </summary>
    /// <param name="purchaseRequestId">Satın alma talebi ID'si</param>
    /// <param name="stepId">Onay adımı ID'si</param>
    /// <param name="approverId">Reddeden kullanıcı ID'si</param>
    /// <param name="comments">Red yorumu</param>
    /// <returns>İşlem sonucunu içeren Result nesnesi</returns>
    Task<Result> RejectRequestAsync(int purchaseRequestId, int stepId, int approverId, string comments);
    
    /// <summary>
    /// Kullanıcıya ait bekleyen onay taleplerini getirir
    /// </summary>
    /// <param name="userId">Kullanıcı ID'si</param>
    /// <returns>Bekleyen onay taleplerinin listesi</returns>
    Task<List<PendingApprovalDto>> GetPendingApprovalsForUserAsync(int userId);

    /// <summary>
    /// Retrieves all pending approval requests for a specific user.
    /// </summary>
    /// <param name="userId">The ID of the user.</param>
    /// <returns>A list of pending approval requests.</returns>
    Task<Result<List<PendingApprovalDto>>> GetPendingApprovalsForUser(string userId);
    
    /// <summary>
    /// Retrieves the approval history for a purchase request.
    /// </summary>
    /// <param name="purchaseRequestId">The ID of the purchase request.</param>
    /// <returns>A list of approval history records.</returns>
    Task<Result<List<ApprovalHistoryDto>>> GetApprovalHistory(int purchaseRequestId);
    
    /// <summary>
    /// Retrieves the current approval status for a purchase request.
    /// </summary>
    /// <param name="purchaseRequestId">The ID of the purchase request.</param>
    /// <returns>The current approval status information.</returns>
    Task<Result<ApprovalStatusDto>> GetApprovalStatus(int purchaseRequestId);
}