using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SatinAlmaPlatformu.BusinessLogic.DTOs;
using SatinAlmaPlatformu.BusinessLogic.Services.Interfaces;
using SatinAlmaPlatformu.Core.Models;
using SatinAlmaPlatformu.Core.UnitOfWork;
using SatinAlmaPlatformu.Entities.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace SatinAlmaPlatformu.BusinessLogic.Services
{
    /// <summary>
    /// Satın alma taleplerini yöneten servis
    /// </summary>
    public class PurchaseRequestService : IPurchaseRequestService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPurchaseRequestApprovalService _approvalService;
        private readonly INotificationService _notificationService;
        private readonly ILogger<PurchaseRequestService> _logger;

        /// <summary>
        /// Constructor
        /// </summary>
        public PurchaseRequestService(
            IUnitOfWork unitOfWork,
            IPurchaseRequestApprovalService approvalService,
            INotificationService notificationService,
            ILogger<PurchaseRequestService> logger
        )
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _approvalService = approvalService ?? throw new ArgumentNullException(nameof(approvalService));
            _notificationService = notificationService ?? throw new ArgumentNullException(nameof(notificationService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Yeni bir satın alma talebi oluşturur (Taslak olarak)
        /// </summary>
        public async Task<Result<PurchaseRequestDto>> CreateRequestAsync(CreatePurchaseRequestDto createDto, int userId)
        {
            try
            {
                // Kullanıcı kontrolü
                var user = await _unitOfWork.Repository<User>().GetByIdAsync(userId);
                if (user == null)
                    return Result<PurchaseRequestDto>.Failure("Kullanıcı bulunamadı");

                // Zorunlu alanları kontrol et
                if (string.IsNullOrEmpty(createDto.Title))
                    return Result<PurchaseRequestDto>.Failure("Talep başlığı zorunludur");

                if (createDto.DepartmentId <= 0)
                    return Result<PurchaseRequestDto>.Failure("Departman bilgisi zorunludur");

                // Talep numarası oluştur (PR-YYYY-0001 formatında)
                string requestNumber = await GenerateRequestNumberAsync();

                // Yeni satın alma talebi oluştur
                var purchaseRequest = new PurchaseRequest
                {
                    RequestNumber = requestNumber,
                    RequestedById = userId,
                    DepartmentId = createDto.DepartmentId,
                    Title = createDto.Title,
                    Description = createDto.Description,
                    Justification = createDto.Justification,
                    CategoryId = createDto.CategoryId,
                    SubCategoryId = createDto.SubCategoryId,
                    Status = PurchaseRequestStatus.Draft,
                    RequestDate = DateTime.Now,
                    RequiredDate = createDto.RequiredDate,
                    EstimatedTotalCost = createDto.EstimatedTotalCost,
                    Currency = createDto.Currency ?? "TRY",
                    Priority = createDto.Priority,
                    BudgetCode = createDto.BudgetCode,
                    CreatedById = userId,
                    CreatedDate = DateTime.Now
                };

                // Talebi ekle
                await _unitOfWork.Repository<PurchaseRequest>().AddAsync(purchaseRequest);
                await _unitOfWork.SaveChangesAsync();

                // Talep kalemlerini ekle
                if (createDto.Items != null && createDto.Items.Any())
                {
                    var items = createDto.Items.Select(i => new PurchaseRequestItem
                    {
                        PurchaseRequestId = purchaseRequest.Id,
                        Name = i.Name,
                        Description = i.Description,
                        Quantity = i.Quantity,
                        UnitOfMeasure = i.UnitOfMeasure,
                        EstimatedUnitPrice = i.EstimatedUnitPrice,
                        SKU = i.SKU,
                        Specifications = i.Specifications,
                        CreatedById = userId,
                        CreatedDate = DateTime.Now
                    }).ToList();

                    await _unitOfWork.Repository<PurchaseRequestItem>().AddRangeAsync(items);
                    await _unitOfWork.SaveChangesAsync();
                }

                // Eğer talep doğrudan onaya gönderiliyorsa onay akışını başlat
                if (createDto.SubmitForApproval)
                {
                    var result = await _approvalService.InitiateApprovalFlowAsync(purchaseRequest.Id, userId);
                    if (!result.Succeeded)
                        return Result<PurchaseRequestDto>.Failure(result.ErrorMessage);
                }

                // Oluşturulan talebi dto'ya dönüştür ve dön
                var purchaseRequestDto = MapToDto(purchaseRequest);
                return Result<PurchaseRequestDto>.Success(purchaseRequestDto);
            }
            catch (Exception ex)
            {
                return Result<PurchaseRequestDto>.Failure($"Satın alma talebi oluşturulurken hata: {ex.Message}");
            }
        }

        /// <summary>
        /// Taslak durumundaki bir talebi onay sürecine gönderir
        /// </summary>
        public async Task<Result> SubmitForApprovalAsync(int purchaseRequestId, int userId)
        {
            try
            {
                // Talebi getir
                var purchaseRequest = await _unitOfWork.Repository<PurchaseRequest>().GetByIdAsync(purchaseRequestId);
                if (purchaseRequest == null)
                    return Result.Failure("Satın alma talebi bulunamadı");

                // Talep sahibi mi kontrol et
                if (purchaseRequest.RequestedById != userId)
                {
                    var user = await _unitOfWork.Repository<User>().GetByIdAsync(userId);
                    bool isAdmin = user?.Roles?.Any(r => r.Name == "Admin" || r.Name == "PurchasingManager") ?? false;
                    
                    if (!isAdmin)
                        return Result.Failure("Bu talebi onaya göndermek için yetkiniz bulunmuyor");
                }

                // Talep durumu kontrolü
                if (purchaseRequest.Status != PurchaseRequestStatus.Draft)
                    return Result.Failure("Sadece taslak durumdaki talepler onay sürecine gönderilebilir");

                // Onay akışını başlat
                return await _approvalService.InitiateApprovalFlowAsync(purchaseRequestId, userId);
            }
            catch (Exception ex)
            {
                return Result.Failure($"Talep onay sürecine gönderilirken hata: {ex.Message}");
            }
        }

        /// <summary>
        /// Talep numarası oluşturan yardımcı metod
        /// </summary>
        private async Task<string> GenerateRequestNumberAsync()
        {
            int year = DateTime.Now.Year;
            
            // Bu yıl içinde oluşturulan son talep numarasını bul
            var lastRequest = await _unitOfWork.Repository<PurchaseRequest>().GetAsync(
                predicate: r => r.RequestNumber.StartsWith($"PR-{year}"),
                orderBy: q => q.OrderByDescending(r => r.RequestNumber));

            int sequence = 1;
            
            if (lastRequest.Any())
            {
                string lastRequestNumber = lastRequest.First().RequestNumber;
                // PR-2023-00001 formatından son 5 karakteri al (00001)
                string lastSequence = lastRequestNumber.Substring(lastRequestNumber.Length - 5);
                
                if (int.TryParse(lastSequence, out int lastSequenceNumber))
                {
                    sequence = lastSequenceNumber + 1;
                }
            }
            
            return $"PR-{year}-{sequence:D5}";
        }

        /// <summary>
        /// PurchaseRequest nesnesini PurchaseRequestDto'ya dönüştürür
        /// </summary>
        private PurchaseRequestDto MapToDto(PurchaseRequest entity)
        {
            if (entity == null)
                return null;

            return new PurchaseRequestDto
            {
                Id = entity.Id,
                RequestNumber = entity.RequestNumber,
                RequestedById = entity.RequestedById,
                RequestedBy = entity.RequestedBy?.FullName,
                DepartmentId = entity.DepartmentId,
                Department = entity.Department?.Name,
                Title = entity.Title,
                Description = entity.Description,
                Justification = entity.Justification,
                CategoryId = entity.CategoryId,
                Category = entity.Category?.Name,
                SubCategoryId = entity.SubCategoryId,
                SubCategory = entity.SubCategory?.Name,
                Status = entity.Status,
                StatusText = entity.Status.ToString(),
                RequestDate = entity.RequestDate,
                RequiredDate = entity.RequiredDate,
                SubmittedDate = entity.SubmittedDate,
                CompletedDate = entity.CompletedDate,
                EstimatedTotalCost = entity.EstimatedTotalCost,
                Currency = entity.Currency,
                ApprovalFlowId = entity.ApprovalFlowId,
                CurrentApprovalStepId = entity.CurrentApprovalStepId,
                CurrentApprovalStep = entity.CurrentApprovalStep?.Name,
                Priority = entity.Priority,
                PriorityText = entity.Priority.ToString(),
                IsCancelled = entity.IsCancelled,
                CancellationReason = entity.CancellationReason,
                CreatedDate = entity.CreatedDate,
                LastUpdateNote = entity.LastUpdateNote,
                Items = entity.Items?.Select(i => new PurchaseRequestItemDto
                {
                    Id = i.Id,
                    Name = i.Name,
                    Description = i.Description,
                    Quantity = i.Quantity,
                    UnitOfMeasure = i.UnitOfMeasure,
                    EstimatedUnitPrice = i.EstimatedUnitPrice,
                    EstimatedTotalPrice = i.EstimatedTotalPrice,
                    SKU = i.SKU,
                    Specifications = i.Specifications
                }).ToList()
            };
        }

        /// <summary>
        /// Bir talep için revizyon ister
        /// </summary>
        public async Task<Result> RequestRevisionAsync(int purchaseRequestId, RevisionRequestDto revisionDto, string userId)
        {
            try
            {
                // Talebi getir
                var purchaseRequest = await _unitOfWork.PurchaseRequestRepository
                    .GetByIdAsync(purchaseRequestId, 
                        include: q => q.Include(pr => pr.ApprovalFlow)
                                      .Include(pr => pr.RequestedBy)
                                      .Include(pr => pr.CurrentApprovalStep));

                if (purchaseRequest == null)
                    return Result.Failure("Satın alma talebi bulunamadı");

                // Talep durumu uygun mu kontrol et (sadece onay sürecindeki talepler için revizyon istenebilir)
                if (purchaseRequest.Status != PurchaseRequestStatus.InReview && 
                    purchaseRequest.Status != PurchaseRequestStatus.Approved)
                    return Result.Failure("Sadece onay sürecindeki veya onaylanmış talepler için revizyon istenebilir");

                // Kullanıcı yetkisi kontrolü
                var user = await _unitOfWork.UserRepository.GetByIdAsync(userId);
                if (user == null)
                    return Result.Failure("Kullanıcı bulunamadı");

                bool isAuthorized = false;
                
                // Yönetici kontrolü
                var userRoles = await _unitOfWork.RoleRepository
                    .GetAllAsync(r => r.UserRoles.Any(ur => ur.UserId == userId));
                
                bool isAdmin = userRoles.Any(r => r.Name == "Admin" || r.Name == "PurchasingManager");
                
                // Onay adımındaki yetkili mi kontrolü
                bool isApprover = false;
                if (purchaseRequest.CurrentApprovalStepId.HasValue)
                {
                    var currentApprovals = await _unitOfWork.ApprovalStepInstanceRepository
                        .GetAllAsync(asi => asi.PurchaseRequestId == purchaseRequestId && 
                                          asi.ApprovalFlowStepId == purchaseRequest.CurrentApprovalStepId.Value &&
                                          asi.Status == ApprovalStatus.Pending);
                        
                    isApprover = currentApprovals.Any(ca => 
                        ca.ApproverId == userId ||
                        (ca.ApproverRoleId.HasValue && userRoles.Any(r => r.Id == ca.ApproverRoleId.Value)) ||
                        (ca.ApproverDepartmentId.HasValue && user.DepartmentId == ca.ApproverDepartmentId.Value && user.IsManager)
                    );
                }
                
                isAuthorized = isAdmin || isApprover;
                
                if (!isAuthorized)
                    return Result.Failure("Bu talep için revizyon isteme yetkiniz bulunmuyor");

                // Talebin durumunu güncelle
                purchaseRequest.Status = PurchaseRequestStatus.RevisionRequested;
                purchaseRequest.RejectionReason = revisionDto.Comments;
                purchaseRequest.UpdatedById = userId;
                purchaseRequest.UpdatedDate = DateTime.Now;

                // Revizyon öncesi durumu kaydet (revizyon sonrası dönüş için)
                var revisionRequest = new PurchaseRequestRevision
                {
                    PurchaseRequestId = purchaseRequestId,
                    RequestedById = userId,
                    RequestedDate = DateTime.Now,
                    Comments = revisionDto.Comments,
                    DueDate = revisionDto.DueDate,
                    PreviousStatus = purchaseRequest.Status,
                    PreviousApprovalStepId = purchaseRequest.CurrentApprovalStepId,
                    ReturnToStepId = revisionDto.ReturnToStepId,
                    IsCompleted = false
                };
                
                await _unitOfWork.PurchaseRequestRevisionRepository.AddAsync(revisionRequest);
                
                // Talebi güncelle
                await _unitOfWork.PurchaseRequestRepository.UpdateAsync(purchaseRequest);
                await _unitOfWork.SaveChangesAsync();
                
                // Talep sahibine bildirim gönder
                await _notificationService.SendRevisionRequestNotificationAsync(purchaseRequest.RequestedById.ToString(), purchaseRequestId, revisionDto.Comments);
                
                return Result.Success("Revizyon talebi başarıyla gönderildi");
            }
            catch (Exception ex)
            {
                return Result.Failure($"Revizyon isteği gönderilirken hata: {ex.Message}");
            }
        }

        /// <summary>
        /// Revizyon istenen bir talebi günceller ve yanıtlar
        /// </summary>
        public async Task<Result> RespondToRevisionAsync(int purchaseRequestId, RevisionResponseDto responseDto, string userId)
        {
            try
            {
                // Talebi getir
                var purchaseRequest = await _unitOfWork.PurchaseRequestRepository
                    .GetByIdAsync(purchaseRequestId);

                if (purchaseRequest == null)
                    return Result.Failure("Satın alma talebi bulunamadı");

                // Talep durumu kontrolü
                if (purchaseRequest.Status != PurchaseRequestStatus.RevisionRequested)
                    return Result.Failure("Sadece revizyon istenmiş talepler güncellenebilir");

                // Talep sahibi kontrolü
                if (purchaseRequest.RequestedById != userId)
                {
                    var user = await _unitOfWork.UserRepository.GetByIdAsync(userId);
                    bool isAdmin = user?.Roles?.Any(r => r.Name == "Admin" || r.Name == "PurchasingManager") ?? false;
                    
                    if (!isAdmin)
                        return Result.Failure("Bu talebi güncelleme yetkiniz bulunmuyor");
                }

                // Açık revizyon talebini bul
                var revisionRequest = await _unitOfWork.PurchaseRequestRevisionRepository
                    .GetAllAsync(pr => pr.PurchaseRequestId == purchaseRequestId && !pr.IsCompleted,
                        orderBy: q => q.OrderByDescending(pr => pr.RequestedDate));
                
                var latestRevision = revisionRequest.FirstOrDefault();
                if (latestRevision == null)
                    return Result.Failure("Açık revizyon talebi bulunamadı");

                // Talebi güncelle
                purchaseRequest.Title = responseDto.UpdatedRequest.Title;
                purchaseRequest.Description = responseDto.UpdatedRequest.Description;
                purchaseRequest.Justification = responseDto.UpdatedRequest.Justification;
                purchaseRequest.CategoryId = responseDto.UpdatedRequest.CategoryId;
                purchaseRequest.SubCategoryId = responseDto.UpdatedRequest.SubCategoryId;
                purchaseRequest.RequiredDate = responseDto.UpdatedRequest.RequiredDate;
                purchaseRequest.EstimatedTotalCost = responseDto.UpdatedRequest.EstimatedTotalCost;
                purchaseRequest.Currency = responseDto.UpdatedRequest.Currency;
                purchaseRequest.BudgetCode = responseDto.UpdatedRequest.BudgetCode;
                purchaseRequest.LastUpdateNote = responseDto.RevisionNote;
                purchaseRequest.UpdatedById = userId;
                purchaseRequest.UpdatedDate = DateTime.Now;
                
                // Talep kalemlerini güncelle
                if (responseDto.UpdatedRequest.Items != null && responseDto.UpdatedRequest.Items.Any())
                {
                    // Mevcut kalemleri getir
                    var existingItems = await _unitOfWork.PurchaseRequestItemRepository
                        .GetAllAsync(i => i.PurchaseRequestId == purchaseRequestId);
                    
                    foreach (var itemDto in responseDto.UpdatedRequest.Items)
                    {
                        if (itemDto.Id.HasValue)
                        {
                            // Mevcut kalemi güncelle
                            var existingItem = existingItems.FirstOrDefault(i => i.Id == itemDto.Id.Value);
                            if (existingItem != null)
                            {
                                if (itemDto.IsDeleted)
                                {
                                    // Kalemi sil
                                    await _unitOfWork.PurchaseRequestItemRepository.DeleteAsync(existingItem);
                                }
                                else
                                {
                                    // Kalemi güncelle
                                    existingItem.Name = itemDto.Name;
                                    existingItem.Description = itemDto.Description;
                                    existingItem.Quantity = itemDto.Quantity;
                                    existingItem.UnitOfMeasure = itemDto.UnitOfMeasure;
                                    existingItem.EstimatedUnitPrice = itemDto.EstimatedUnitPrice;
                                    existingItem.SKU = itemDto.SKU;
                                    existingItem.Specifications = itemDto.Specifications;
                                    existingItem.UpdatedById = userId;
                                    existingItem.UpdatedDate = DateTime.Now;
                                    
                                    await _unitOfWork.PurchaseRequestItemRepository.UpdateAsync(existingItem);
                                }
                            }
                        }
                        else
                        {
                            // Yeni kalem ekle
                            var newItem = new PurchaseRequestItem
                            {
                                PurchaseRequestId = purchaseRequestId,
                                Name = itemDto.Name,
                                Description = itemDto.Description,
                                Quantity = itemDto.Quantity,
                                UnitOfMeasure = itemDto.UnitOfMeasure,
                                EstimatedUnitPrice = itemDto.EstimatedUnitPrice,
                                SKU = itemDto.SKU,
                                Specifications = itemDto.Specifications,
                                CreatedById = userId,
                                CreatedDate = DateTime.Now
                            };
                            
                            await _unitOfWork.PurchaseRequestItemRepository.AddAsync(newItem);
                        }
                    }
                }
                
                // Revizyon talebini tamamlandı olarak işaretle
                latestRevision.IsCompleted = true;
                latestRevision.CompletedDate = DateTime.Now;
                latestRevision.CompletedById = userId;
                latestRevision.RevisionNote = responseDto.RevisionNote;
                
                await _unitOfWork.PurchaseRequestRevisionRepository.UpdateAsync(latestRevision);
                
                // Tekrar onaya gönderilecek mi?
                if (responseDto.SubmitForApproval)
                {
                    // Revizyon öncesi duruma göre yeni durumu belirle
                    if (latestRevision.ReturnToStepId.HasValue)
                    {
                        // Belirli bir adıma dönülecekse
                        purchaseRequest.Status = PurchaseRequestStatus.InReview;
                        purchaseRequest.CurrentApprovalStepId = latestRevision.ReturnToStepId;
                    }
                    else if (latestRevision.PreviousApprovalStepId.HasValue)
                    {
                        // Önceki adıma dönülecekse
                        purchaseRequest.Status = PurchaseRequestStatus.InReview;
                        purchaseRequest.CurrentApprovalStepId = latestRevision.PreviousApprovalStepId;
                    }
                    else
                    {
                        // Onay akışını yeniden başlat
                        purchaseRequest.Status = PurchaseRequestStatus.Draft; // Geçiçi olarak taslak yap
                        await _unitOfWork.PurchaseRequestRepository.UpdateAsync(purchaseRequest);
                        await _unitOfWork.SaveChangesAsync();
                        
                        var result = await _approvalService.InitiateApprovalFlowAsync(purchaseRequestId, userId);
                        if (!result.Succeeded)
                            return Result.Failure($"Talep güncellenmiş fakat onay akışı yeniden başlatılamadı: {result.ErrorMessage}");
                        
                        return Result.Success("Revizyon başarıyla tamamlandı ve talep yeniden onay sürecine gönderildi");
                    }
                }
                else
                {
                    // Taslak durumuna dön
                    purchaseRequest.Status = PurchaseRequestStatus.Draft;
                    purchaseRequest.CurrentApprovalStepId = null;
                }
                
                await _unitOfWork.PurchaseRequestRepository.UpdateAsync(purchaseRequest);
                await _unitOfWork.SaveChangesAsync();
                
                // Revizyon isteyen kişiye bildirim gönder
                await _notificationService.SendRevisionCompletedNotificationAsync(latestRevision.RequestedById.ToString(), purchaseRequestId, responseDto.RevisionNote);
                
                if (responseDto.SubmitForApproval)
                    return Result.Success("Revizyon başarıyla tamamlandı ve talep yeniden onay sürecine gönderildi");
                else
                    return Result.Success("Revizyon başarıyla tamamlandı ve talep taslak durumuna alındı");
            }
            catch (Exception ex)
            {
                return Result.Failure($"Revizyon yanıtlanırken hata: {ex.Message}");
            }
        }
    }
} 