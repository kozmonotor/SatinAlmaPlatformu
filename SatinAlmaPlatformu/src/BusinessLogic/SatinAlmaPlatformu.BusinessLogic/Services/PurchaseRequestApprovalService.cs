using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SatinAlmaPlatformu.BusinessLogic.DTOs;
using SatinAlmaPlatformu.BusinessLogic.Services.Interfaces;
using SatinAlmaPlatformu.Core.Models;
using SatinAlmaPlatformu.Core.UnitOfWork;
using SatinAlmaPlatformu.Entities.Models;
using Microsoft.Extensions.Logging;

namespace SatinAlmaPlatformu.BusinessLogic.Services;

/// <summary>
/// Satın alma taleplerinin onay süreçlerini yöneten servis
/// </summary>
public class PurchaseRequestApprovalService 
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IApprovalFlowService _approvalFlowService;
    private readonly INotificationService _notificationService;
    private readonly ILogger<PurchaseRequestApprovalService> _logger;

    public PurchaseRequestApprovalService(
        IUnitOfWork unitOfWork,
        IApprovalFlowService approvalFlowService,
        INotificationService notificationService,
        ILogger<PurchaseRequestApprovalService> logger
    )
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _approvalFlowService = approvalFlowService ?? throw new ArgumentNullException(nameof(approvalFlowService));
        _notificationService = notificationService ?? throw new ArgumentNullException(nameof(notificationService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// Satın alma talebi oluşturulduğunda veya güncellendiğinde uygun onay akışını belirler ve başlatır
    /// </summary>
    /// <param name="purchaseRequestId">Satın alma talebi ID'si</param>
    /// <param name="userId">İşlemi yapan kullanıcı ID'si</param>
    /// <returns>İşlem sonucunu içeren Result nesnesi</returns>
    public async Task<Result> InitiateApprovalFlowAsync(int purchaseRequestId, int userId)
    {
        try
        {
            // Satın alma talebini getir
            var request = await _unitOfWork.Repository<PurchaseRequest>().GetAsync(
                predicate: p => p.Id == purchaseRequestId,
                includes: new List<System.Linq.Expressions.Expression<Func<PurchaseRequest, object>>>
                {
                    p => p.Department,
                    p => p.Category,
                    p => p.RequestedBy
                });

            var purchaseRequest = request.FirstOrDefault();
            if (purchaseRequest == null)
                return Result.Failure($"Satın alma talebi bulunamadı (ID: {purchaseRequestId})");

            // Talep onaya gönderilebilir durumda mı?
            if (purchaseRequest.Status != PurchaseRequestStatus.Draft)
                return Result.Failure("Sadece taslak durumdaki talepler onay sürecine gönderilebilir");

            // Uygun onay akışını belirle
            var approvalFlowDto = await _approvalFlowService.DetermineApprovalFlowForRequestAsync(purchaseRequestId);
            if (approvalFlowDto == null)
                return Result.Failure("Bu talep için uygun onay akışı bulunamadı");

            // Onay akışını ve ilk adımı talebe ata
            purchaseRequest.ApprovalFlowId = approvalFlowDto.Id;
            
            // Talep durumunu güncelle
            purchaseRequest.Status = PurchaseRequestStatus.InReview;
            purchaseRequest.SubmittedDate = DateTime.Now;
            purchaseRequest.UpdatedById = userId;
            purchaseRequest.UpdatedDate = DateTime.Now;
            
            await _unitOfWork.Repository<PurchaseRequest>().UpdateAsync(purchaseRequest);
            await _unitOfWork.SaveChangesAsync();

            // İlk onay adımını başlat
            var result = await StartFirstApprovalStepAsync(purchaseRequest.Id, approvalFlowDto.Id, userId);
            if (!result.Succeeded)
                return result;

            return Result.Success("Satın alma talebi onay sürecine gönderildi");
        }
        catch (Exception ex)
        {
            return Result.Failure($"Onay süreci başlatılırken hata: {ex.Message}");
        }
    }

    /// <summary>
    /// Onay akışının ilk adımını başlatır
    /// </summary>
    private async Task<Result> StartFirstApprovalStepAsync(int purchaseRequestId, int approvalFlowId, int initiatorUserId)
    {
        try
        {
            // Onay akışının adımlarını getir
            var approvalFlow = await _unitOfWork.Repository<ApprovalFlow>().GetAsync(
                predicate: af => af.Id == approvalFlowId, 
                includes: new List<System.Linq.Expressions.Expression<Func<ApprovalFlow, object>>>
                {
                    af => af.Steps.OrderBy(s => s.StepOrder)
                });
            
            var flow = approvalFlow.FirstOrDefault();
            if (flow == null || !flow.Steps.Any())
                return Result.Failure("Onay akışı veya adımları bulunamadı");

            // İlk adımı bul
            var firstStep = flow.Steps.OrderBy(s => s.StepOrder).First();
            
            // İlk adımı talebe ata
            var purchaseRequest = await _unitOfWork.Repository<PurchaseRequest>().GetByIdAsync(purchaseRequestId);
            if (purchaseRequest == null)
                return Result.Failure("Satın alma talebi bulunamadı");
                
            purchaseRequest.CurrentApprovalStepId = firstStep.Id;
            await _unitOfWork.Repository<PurchaseRequest>().UpdateAsync(purchaseRequest);
            
            // İlk adım otomatik onay ise direkt işlem yap
            if (firstStep.Type == StepType.AutomaticApproval)
            {
                await CreateAutomaticApproval(purchaseRequestId, firstStep.Id, initiatorUserId);
                
                // Bir sonraki adıma geç
                return await MoveToNextStepAsync(purchaseRequestId, firstStep.Id, initiatorUserId);
            }
            
            // İlk adım için onay kaydı oluştur
            var approvers = await DetermineApproversForStepAsync(firstStep.Id, purchaseRequestId);
            if (!approvers.Any())
                return Result.Failure("Bu adım için onaylayıcı bulunamadı");
            
            foreach (var approverId in approvers)
            {
                var approval = new PurchaseRequestApproval
                {
                    PurchaseRequestId = purchaseRequestId,
                    ApprovalFlowStepId = firstStep.Id,
                    ApproverId = approverId,
                    Status = ApprovalStatus.Pending,
                    ApprovalOrder = 1,
                    DueDate = DateTime.Now.AddHours(firstStep.TimeoutHours ?? 72), // Varsayılan 3 gün
                    IsActive = true,
                    CreatedById = initiatorUserId,
                    CreatedDate = DateTime.Now
                };
                
                await _unitOfWork.Repository<PurchaseRequestApproval>().AddAsync(approval);
            }
            
            await _unitOfWork.SaveChangesAsync();
            
            // Onaylayıcılara bildirim gönder
            foreach (var approverId in approvers)
            {
                await _notificationService.SendApprovalRequestNotificationAsync(approverId, purchaseRequestId, firstStep.Id);
            }
            
            return Result.Success("İlk onay adımı başlatıldı");
        }
        catch (Exception ex)
        {
            return Result.Failure($"İlk onay adımı başlatılırken hata: {ex.Message}");
        }
    }
    
    /// <summary>
    /// Belirli bir adım için onaylayıcıları belirler
    /// </summary>
    private async Task<List<int>> DetermineApproversForStepAsync(int stepId, int purchaseRequestId)
    {
        var step = await _unitOfWork.Repository<ApprovalFlowStep>().GetAsync(
            predicate: s => s.Id == stepId,
            includes: new List<System.Linq.Expressions.Expression<Func<ApprovalFlowStep, object>>>
            {
                s => s.ApproverRole,
                s => s.ApproverDepartment
            });
        
        var approvalStep = step.FirstOrDefault();
        if (approvalStep == null)
            return new List<int>();
            
        // Adım tipi bireysel onay ise
        if (approvalStep.Type == StepType.IndividualApproval && approvalStep.ApproverId.HasValue)
            return new List<int> { approvalStep.ApproverId.Value };
            
        // Adım tipi rol bazlı onay ise
        if (approvalStep.Type == StepType.RoleApproval && approvalStep.ApproverRoleId.HasValue)
        {
            var usersWithRole = await _unitOfWork.Repository<User>().GetAsync(
                predicate: u => u.Roles.Any(r => r.Id == approvalStep.ApproverRoleId.Value) && u.IsActive);
                
            return usersWithRole.Select(u => u.Id).ToList();
        }
        
        // Adım tipi departman bazlı onay ise
        if (approvalStep.Type == StepType.DepartmentApproval)
        {
            // Talebin bağlı olduğu departmanı bul
            var request = await _unitOfWork.Repository<PurchaseRequest>().GetByIdAsync(purchaseRequestId);
            if (request != null && request.DepartmentId.HasValue)
            {
                // Departman yöneticilerini bul
                var managers = await _unitOfWork.Repository<User>().GetAsync(
                    predicate: u => u.DepartmentId == request.DepartmentId && 
                                    u.IsManager && 
                                    u.IsActive);
                                    
                return managers.Select(u => u.Id).ToList();
            }
        }
        
        return new List<int>();
    }
    
    /// <summary>
    /// Otomatik onay oluşturur
    /// </summary>
    private async Task CreateAutomaticApproval(int purchaseRequestId, int stepId, int systemUserId)
    {
        var approval = new PurchaseRequestApproval
        {
            PurchaseRequestId = purchaseRequestId,
            ApprovalFlowStepId = stepId,
            Status = ApprovalStatus.Approved,
            ActionDate = DateTime.Now,
            Comments = "Sistem tarafından otomatik onaylandı",
            IsAutomaticAction = true,
            ApprovalOrder = 1,
            IsActive = true,
            CreatedById = systemUserId,
            CreatedDate = DateTime.Now
        };
        
        await _unitOfWork.Repository<PurchaseRequestApproval>().AddAsync(approval);
        await _unitOfWork.SaveChangesAsync();
    }
    
    /// <summary>
    /// Bir sonraki onay adımına geçer
    /// </summary>
    public async Task<Result> MoveToNextStepAsync(int purchaseRequestId, int currentStepId, int userId)
    {
        try
        {
            // Mevcut talebi getir
            var request = await _unitOfWork.Repository<PurchaseRequest>().GetAsync(
                predicate: p => p.Id == purchaseRequestId,
                includes: new List<System.Linq.Expressions.Expression<Func<PurchaseRequest, object>>>
                {
                    p => p.ApprovalFlow.Steps.OrderBy(s => s.StepOrder)
                });
                
            var purchaseRequest = request.FirstOrDefault();
            if (purchaseRequest == null)
                return Result.Failure($"Satın alma talebi bulunamadı (ID: {purchaseRequestId})");
                
            // Mevcut adımı bul
            var currentStep = purchaseRequest.ApprovalFlow.Steps.FirstOrDefault(s => s.Id == currentStepId);
            if (currentStep == null)
                return Result.Failure("Mevcut onay adımı bulunamadı");
                
            // Bir sonraki adımı bul
            var nextStep = purchaseRequest.ApprovalFlow.Steps
                .Where(s => s.StepOrder > currentStep.StepOrder && s.IsActive)
                .OrderBy(s => s.StepOrder)
                .FirstOrDefault();
                
            // Eğer bir sonraki adım yoksa, onay sürecini tamamla
            if (nextStep == null)
            {
                purchaseRequest.Status = PurchaseRequestStatus.Approved;
                purchaseRequest.CompletedDate = DateTime.Now;
                purchaseRequest.CurrentApprovalStepId = null;
                purchaseRequest.UpdatedById = userId;
                purchaseRequest.UpdatedDate = DateTime.Now;
                
                await _unitOfWork.Repository<PurchaseRequest>().UpdateAsync(purchaseRequest);
                await _unitOfWork.SaveChangesAsync();
                
                // Talep sahibine bildirim gönder
                await _notificationService.SendRequestApprovedNotificationAsync(purchaseRequest.RequestedById.ToString(), purchaseRequestId);
                
                return Result.Success("Onay süreci tamamlandı. Talep onaylandı.");
            }
            
            // Bir sonraki adıma geç
            purchaseRequest.CurrentApprovalStepId = nextStep.Id;
            await _unitOfWork.Repository<PurchaseRequest>().UpdateAsync(purchaseRequest);
            
            // Bir sonraki adım otomatik onay ise
            if (nextStep.Type == StepType.AutomaticApproval)
            {
                await CreateAutomaticApproval(purchaseRequestId, nextStep.Id, userId);
                return await MoveToNextStepAsync(purchaseRequestId, nextStep.Id, userId);
            }
            
            // Bir sonraki adım için onay kaydı oluştur
            var approvers = await DetermineApproversForStepAsync(nextStep.Id, purchaseRequestId);
            if (!approvers.Any())
                return Result.Failure("Bir sonraki adım için onaylayıcı bulunamadı");
                
            foreach (var approverId in approvers)
            {
                var approval = new PurchaseRequestApproval
                {
                    PurchaseRequestId = purchaseRequestId,
                    ApprovalFlowStepId = nextStep.Id,
                    ApproverId = approverId,
                    Status = ApprovalStatus.Pending,
                    ApprovalOrder = 1,
                    DueDate = DateTime.Now.AddHours(nextStep.TimeoutHours ?? 72),
                    IsActive = true,
                    CreatedById = userId,
                    CreatedDate = DateTime.Now
                };
                
                await _unitOfWork.Repository<PurchaseRequestApproval>().AddAsync(approval);
            }
            
            await _unitOfWork.SaveChangesAsync();
            
            // Onaylayıcılara bildirim gönder
            foreach (var approverId in approvers)
            {
                await _notificationService.SendApprovalRequestNotificationAsync(approverId, purchaseRequestId, nextStep.Id);
            }
            
            return Result.Success("Bir sonraki onay adımına geçildi");
        }
        catch (Exception ex)
        {
            return Result.Failure($"Bir sonraki adıma geçilirken hata: {ex.Message}");
        }
    }
    
    /// <summary>
    /// Satın alma talebini onaylar
    /// </summary>
    public async Task<Result> ApproveRequestAsync(int purchaseRequestId, int stepId, int approverId, string comments)
    {
        try
        {
            // Onay kaydını bul
            var approvals = await _unitOfWork.Repository<PurchaseRequestApproval>().GetAsync(
                predicate: a => a.PurchaseRequestId == purchaseRequestId && 
                               a.ApprovalFlowStepId == stepId && 
                               a.ApproverId == approverId && 
                               a.Status == ApprovalStatus.Pending);
                               
            var approval = approvals.FirstOrDefault();
            if (approval == null)
                return Result.Failure("Bekleyen onay kaydı bulunamadı");
                
            // Onay kaydını güncelle
            approval.Status = ApprovalStatus.Approved;
            approval.ActionDate = DateTime.Now;
            approval.Comments = comments;
            approval.ActionIpAddress = "::1"; // TODO: IP adresi alınacak
            approval.UpdatedById = approverId;
            approval.UpdatedDate = DateTime.Now;
            
            await _unitOfWork.Repository<PurchaseRequestApproval>().UpdateAsync(approval);
            await _unitOfWork.SaveChangesAsync();
            
            // Bu adım için tüm onay kayıtlarını kontrol et
            var allApprovalsForStep = await _unitOfWork.Repository<PurchaseRequestApproval>().GetAsync(
                predicate: a => a.PurchaseRequestId == purchaseRequestId && 
                               a.ApprovalFlowStepId == stepId && 
                               a.Status == ApprovalStatus.Pending);
                               
            // Eğer bekleyen onay yoksa, bir sonraki adıma geç
            if (!allApprovalsForStep.Any())
            {
                return await MoveToNextStepAsync(purchaseRequestId, stepId, approverId);
            }
            
            // Talep sahibine bildirim gönder
            await _notificationService.SendRequestApprovedNotificationAsync(purchaseRequest.RequestedById.ToString(), purchaseRequestId);
            
            return Result.Success("Onay başarıyla kaydedildi");
        }
        catch (Exception ex)
        {
            return Result.Failure($"Onay işlemi sırasında hata: {ex.Message}");
        }
    }
    
    /// <summary>
    /// Satın alma talebini reddeder
    /// </summary>
    public async Task<Result> RejectRequestAsync(int purchaseRequestId, int stepId, int approverId, string comments)
    {
        try
        {
            // Onay kaydını bul
            var approvals = await _unitOfWork.Repository<PurchaseRequestApproval>().GetAsync(
                predicate: a => a.PurchaseRequestId == purchaseRequestId && 
                               a.ApprovalFlowStepId == stepId && 
                               a.ApproverId == approverId && 
                               a.Status == ApprovalStatus.Pending);
                               
            var approval = approvals.FirstOrDefault();
            if (approval == null)
                return Result.Failure("Bekleyen onay kaydı bulunamadı");
                
            // Adımın red davranışını belirle
            var step = await _unitOfWork.Repository<ApprovalFlowStep>().GetByIdAsync(stepId);
            if (step == null)
                return Result.Failure("Onay adımı bulunamadı");
                
            // Onay kaydını güncelle
            approval.Status = ApprovalStatus.Rejected;
            approval.ActionDate = DateTime.Now;
            approval.Comments = comments;
            approval.ActionIpAddress = "::1"; // TODO: IP adresi alınacak
            approval.UpdatedById = approverId;
            approval.UpdatedDate = DateTime.Now;
            
            await _unitOfWork.Repository<PurchaseRequestApproval>().UpdateAsync(approval);
            
            // Talebi getir
            var request = await _unitOfWork.Repository<PurchaseRequest>().GetByIdAsync(purchaseRequestId);
            if (request == null)
                return Result.Failure("Satın alma talebi bulunamadı");
                
            // Red davranışını uygula
            switch (step.RejectAction)
            {
                case RejectAction.Cancel:
                    // Talebi reddet ve iptal et
                    request.Status = PurchaseRequestStatus.Rejected;
                    request.RejectionReason = comments;
                    request.CompletedDate = DateTime.Now;
                    request.CurrentApprovalStepId = null;
                    break;
                    
                case RejectAction.ReturnToRequester:
                    // Talebi talep sahibine geri gönder
                    request.Status = PurchaseRequestStatus.RevisionRequested;
                    request.RejectionReason = comments;
                    request.CurrentApprovalStepId = null;
                    break;
                    
                case RejectAction.ReturnToStep:
                    // Belirli bir adıma geri dön
                    if (step.ReturnToStepOrder.HasValue)
                    {
                        // Geri dönülecek adımı bul
                        var returnStep = await _unitOfWork.Repository<ApprovalFlow>()
                            .GetAsync(
                                predicate: af => af.Id == request.ApprovalFlowId,
                                includes: new List<System.Linq.Expressions.Expression<Func<ApprovalFlow, object>>>
                                {
                                    af => af.Steps.Where(s => s.StepOrder == step.ReturnToStepOrder.Value)
                                });
                                
                        var returnFlowStep = returnStep.FirstOrDefault()?.Steps.FirstOrDefault();
                        
                        if (returnFlowStep != null)
                        {
                            request.Status = PurchaseRequestStatus.RevisionRequested;
                            request.CurrentApprovalStepId = returnFlowStep.Id;
                            
                            // Geri dönülen adım için yeni onay kayıtları oluştur
                            var approvers = await DetermineApproversForStepAsync(returnFlowStep.Id, purchaseRequestId);
                            
                            foreach (var returnApproverId in approvers)
                            {
                                // Bu adım için daha önce yapılan onay kayıtlarını bul
                                var previousApprovals = await _unitOfWork.Repository<PurchaseRequestApproval>().GetAsync(
                                    predicate: a => a.PurchaseRequestId == purchaseRequestId && 
                                                   a.ApprovalFlowStepId == returnFlowStep.Id);
                                                   
                                // Yapılan onay sayısını belirle
                                int approvalOrder = previousApprovals.Any() ? 
                                    previousApprovals.Max(a => a.ApprovalOrder) + 1 : 1;
                                    
                                var newApproval = new PurchaseRequestApproval
                                {
                                    PurchaseRequestId = purchaseRequestId,
                                    ApprovalFlowStepId = returnFlowStep.Id,
                                    ApproverId = returnApproverId,
                                    Status = ApprovalStatus.Pending,
                                    ApprovalOrder = approvalOrder,
                                    DueDate = DateTime.Now.AddHours(returnFlowStep.TimeoutHours ?? 72),
                                    IsActive = true,
                                    CreatedById = approverId,
                                    CreatedDate = DateTime.Now
                                };
                                
                                await _unitOfWork.Repository<PurchaseRequestApproval>().AddAsync(newApproval);
                            }
                        }
                        else
                        {
                            // Geri dönülecek adım bulunamazsa iptal et
                            request.Status = PurchaseRequestStatus.Rejected;
                            request.RejectionReason = comments;
                            request.CompletedDate = DateTime.Now;
                            request.CurrentApprovalStepId = null;
                        }
                    }
                    break;
            }
            
            request.UpdatedById = approverId;
            request.UpdatedDate = DateTime.Now;
            
            await _unitOfWork.Repository<PurchaseRequest>().UpdateAsync(request);
            await _unitOfWork.SaveChangesAsync();
            
            // Bildirim gönder
            await _notificationService.SendRequestRejectedNotificationAsync(request.RequestedById.ToString(), purchaseRequestId, comments);
            
            return Result.Success("Talep başarıyla reddedildi");
        }
        catch (Exception ex)
        {
            return Result.Failure($"Red işlemi sırasında hata: {ex.Message}");
        }
    }
    
    /// <summary>
    /// Kullanıcıya ait bekleyen onay taleplerini getirir
    /// </summary>
    public async Task<List<PendingApprovalDto>> GetPendingApprovalsForUserAsync(int userId)
    {
        // Kullanıcıya ait bekleyen onayları getir
        var pendingApprovals = await _unitOfWork.Repository<PurchaseRequestApproval>().GetAsync(
            predicate: a => a.ApproverId == userId && a.Status == ApprovalStatus.Pending,
            includes: new List<System.Linq.Expressions.Expression<Func<PurchaseRequestApproval, object>>>
            {
                a => a.PurchaseRequest,
                a => a.PurchaseRequest.RequestedBy,
                a => a.ApprovalFlowStep
            },
            orderBy: q => q.OrderBy(a => a.DueDate));
            
        var result = new List<PendingApprovalDto>();
        
        foreach (var approval in pendingApprovals)
        {
            result.Add(new PendingApprovalDto
            {
                ApprovalId = approval.Id,
                PurchaseRequestId = approval.PurchaseRequestId,
                RequestNumber = approval.PurchaseRequest?.RequestNumber,
                RequestTitle = approval.PurchaseRequest?.Title,
                RequestAmount = approval.PurchaseRequest?.TotalAmount ?? 0,
                Currency = approval.PurchaseRequest?.Currency,
                RequestedBy = approval.PurchaseRequest?.RequestedBy?.FullName,
                RequestDate = approval.PurchaseRequest?.RequestDate ?? DateTime.Now,
                ApprovalStepName = approval.ApprovalFlowStep?.Name,
                DueDate = approval.DueDate,
                IsOverdue = approval.DueDate.HasValue && approval.DueDate.Value < DateTime.Now
            });
        }
        
        return result;
    }

    public async Task<Result<List<ApprovalHistoryDto>>> GetApprovalHistory(int purchaseRequestId)
    {
        try
        {
            var approvalStepInstances = await _unitOfWork.ApprovalStepInstanceRepository
                .GetAllAsync(asi => asi.PurchaseRequestId == purchaseRequestId, 
                    include: q => q.Include(asi => asi.ApprovalFlowStep)
                                  .Include(asi => asi.Approver)
                                  .Include(asi => asi.ApproverRole)
                                  .Include(asi => asi.ApproverDepartment),
                    orderBy: q => q.OrderBy(asi => asi.ApprovalFlowStep.StepOrder));

            if (approvalStepInstances == null || !approvalStepInstances.Any())
            {
                return Result<List<ApprovalHistoryDto>>.Success(new List<ApprovalHistoryDto>());
            }

            var historyList = approvalStepInstances.Select(asi => new ApprovalHistoryDto
            {
                ApprovalStepId = asi.Id,
                StepName = asi.ApprovalFlowStep.Name,
                ApproverName = asi.Approver?.FullName,
                ApproverRole = asi.ApproverRole?.Name,
                ApproverDepartment = asi.ApproverDepartment?.Name,
                Status = asi.Status.ToString(),
                ActionDate = asi.CompletedDate,
                Comments = asi.Comments,
                StepOrder = asi.ApprovalFlowStep.StepOrder
            }).ToList();

            return Result<List<ApprovalHistoryDto>>.Success(historyList);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving approval history for purchase request {RequestId}", purchaseRequestId);
            return Result<List<ApprovalHistoryDto>>.Failure("Onay geçmişi alınırken bir hata oluştu.");
        }
    }

    public async Task<Result<ApprovalStatusDto>> GetApprovalStatus(int purchaseRequestId)
    {
        try
        {
            var purchaseRequest = await _unitOfWork.PurchaseRequestRepository
                .GetByIdAsync(purchaseRequestId, 
                    include: q => q.Include(pr => pr.ApprovalFlow)
                                  .ThenInclude(af => af.ApprovalFlowSteps)
                                  .OrderBy(s => s.StepOrder));

            if (purchaseRequest == null)
            {
                return Result<ApprovalStatusDto>.Failure("Satın alma talebi bulunamadı.");
            }

            var approvalStepInstances = await _unitOfWork.ApprovalStepInstanceRepository
                .GetAllAsync(asi => asi.PurchaseRequestId == purchaseRequestId,
                    include: q => q.Include(asi => asi.ApprovalFlowStep)
                                  .Include(asi => asi.Approver)
                                  .Include(asi => asi.ApproverRole)
                                  .Include(asi => asi.ApproverDepartment),
                    orderBy: q => q.OrderBy(asi => asi.ApprovalFlowStep.StepOrder));

            // Get history for the status
            var historyList = approvalStepInstances.Select(asi => new ApprovalHistoryDto
            {
                ApprovalStepId = asi.Id,
                StepName = asi.ApprovalFlowStep.Name,
                ApproverName = asi.Approver?.FullName,
                ApproverRole = asi.ApproverRole?.Name,
                ApproverDepartment = asi.ApproverDepartment?.Name,
                Status = asi.Status.ToString(),
                ActionDate = asi.CompletedDate,
                Comments = asi.Comments,
                StepOrder = asi.ApprovalFlowStep.StepOrder
            }).ToList();

            // Get current active step
            var currentActiveStep = approvalStepInstances
                .FirstOrDefault(asi => asi.Status == ApprovalStatus.Pending);

            // Get total steps count
            var totalSteps = purchaseRequest.ApprovalFlow?.ApprovalFlowSteps?.Count ?? 0;

            // Determine if the approval process is completed
            bool isCompleted = !approvalStepInstances.Any(asi => asi.Status == ApprovalStatus.Pending);
            
            // Get completion date if completed
            DateTime? completionDate = null;
            if (isCompleted)
            {
                var lastCompletedStep = approvalStepInstances
                    .OrderByDescending(asi => asi.ApprovalFlowStep.StepOrder)
                    .FirstOrDefault();
                
                completionDate = lastCompletedStep?.CompletedDate;
            }

            // Get current approvers
            var currentApprovers = new List<string>();
            if (currentActiveStep != null)
            {
                if (currentActiveStep.ApproverId != null)
                {
                    var approver = await _unitOfWork.UserRepository.GetByIdAsync(currentActiveStep.ApproverId);
                    if (approver != null)
                    {
                        currentApprovers.Add(approver.FullName);
                    }
                }
                else if (currentActiveStep.ApproverRoleId != null)
                {
                    var usersInRole = await _unitOfWork.UserRepository
                        .GetAllAsync(u => u.UserRoles.Any(ur => ur.RoleId == currentActiveStep.ApproverRoleId));
                    
                    currentApprovers.AddRange(usersInRole.Select(u => u.FullName));
                }
                else if (currentActiveStep.ApproverDepartmentId != null)
                {
                    var departmentManagers = await _unitOfWork.UserRepository
                        .GetAllAsync(u => u.DepartmentId == currentActiveStep.ApproverDepartmentId && u.IsManager);
                    
                    currentApprovers.AddRange(departmentManagers.Select(u => u.FullName));
                }
            }

            var statusDto = new ApprovalStatusDto
            {
                RequestId = purchaseRequest.Id,
                RequestNumber = purchaseRequest.RequestNumber,
                CurrentStatus = currentActiveStep == null ? 
                    (isCompleted ? "Tamamlandı" : "Başlatılmadı") : 
                    "Onay Bekliyor",
                CurrentStepName = currentActiveStep?.ApprovalFlowStep?.Name,
                CurrentStepOrder = currentActiveStep?.ApprovalFlowStep?.StepOrder ?? 0,
                TotalSteps = totalSteps,
                IsCompleted = isCompleted,
                CompletionDate = completionDate,
                CurrentApprovers = currentApprovers,
                History = historyList
            };

            return Result<ApprovalStatusDto>.Success(statusDto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving approval status for purchase request {RequestId}", purchaseRequestId);
            return Result<ApprovalStatusDto>.Failure("Onay durumu alınırken bir hata oluştu.");
        }
    }

    public async Task<Result<List<PendingApprovalDto>>> GetPendingApprovalsForUser(string userId)
    {
        try
        {
            // Get user information to determine roles and department
            var user = await _unitOfWork.UserRepository
                .GetByIdAsync(userId, include: q => q.Include(u => u.UserRoles)
                                                    .Include(u => u.Department));
            
            if (user == null)
            {
                return Result<List<PendingApprovalDto>>.Failure("Kullanıcı bulunamadı.");
            }

            // Get user roles
            var userRoleIds = user.UserRoles.Select(ur => ur.RoleId).ToList();
            var isDepartmentManager = user.IsManager;
            var departmentId = user.DepartmentId;

            // Get all pending approval steps where:
            // 1. The user is directly assigned as the approver
            // 2. The user has a role that is assigned as the approver
            // 3. The user is a department manager and the department is assigned as the approver
            var pendingApprovals = await _unitOfWork.ApprovalStepInstanceRepository
                .GetAllAsync(
                    predicate: asi => 
                        asi.Status == ApprovalStatus.Pending && 
                        (
                            asi.ApproverId == userId || 
                            (asi.ApproverRoleId != null && userRoleIds.Contains(asi.ApproverRoleId.Value)) ||
                            (isDepartmentManager && asi.ApproverDepartmentId == departmentId)
                        ),
                    include: q => q.Include(asi => asi.PurchaseRequest)
                                  .Include(asi => asi.ApprovalFlowStep),
                    orderBy: q => q.OrderByDescending(asi => asi.DueDate));

            // Map to DTOs
            var pendingApprovalDtos = pendingApprovals.Select(pa => new PendingApprovalDto
            {
                ApprovalId = pa.Id,
                PurchaseRequestId = pa.PurchaseRequestId,
                RequestNumber = pa.PurchaseRequest.RequestNumber,
                RequestTitle = pa.PurchaseRequest.Title,
                RequestAmount = pa.PurchaseRequest.EstimatedTotalCost,
                Currency = pa.PurchaseRequest.Currency,
                RequestedBy = pa.PurchaseRequest.CreatedByName,
                RequestDate = pa.PurchaseRequest.CreatedDate,
                ApprovalStepName = pa.ApprovalFlowStep.Name,
                DueDate = pa.DueDate,
                IsOverdue = pa.DueDate.HasValue && pa.DueDate.Value < DateTime.Now
            }).ToList();

            return Result<List<PendingApprovalDto>>.Success(pendingApprovalDtos);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving pending approvals for user {UserId}", userId);
            return Result<List<PendingApprovalDto>>.Failure("Bekleyen onaylar alınırken bir hata oluştu.");
        }
    }
}

/// <summary>
/// Bekleyen onay DTO sınıfı
/// </summary>
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