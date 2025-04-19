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

namespace SatinAlmaPlatformu.BusinessLogic.Services;

/// <summary>
/// IApprovalFlowService arayüzünün uygulaması
/// </summary>
public class ApprovalFlowService : IApprovalFlowService
{
    private readonly IUnitOfWork _unitOfWork;

    public ApprovalFlowService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
    }

    /// <inheritdoc/>
    public async Task<List<ApprovalFlowDto>> GetAllApprovalFlowsAsync()
    {
        var approvalFlows = await _unitOfWork.Repository<ApprovalFlow>()
            .GetAsync(
                orderBy: q => q.OrderBy(af => af.Priority),
                includes: new List<System.Linq.Expressions.Expression<Func<ApprovalFlow, object>>>
                {
                    af => af.Department,
                    af => af.Category
                });

        return approvalFlows.Select(MapToApprovalFlowDto).ToList();
    }

    /// <inheritdoc/>
    public async Task<ApprovalFlowDto> GetApprovalFlowByIdAsync(int id)
    {
        var approvalFlow = await _unitOfWork.Repository<ApprovalFlow>()
            .GetAsync(
                predicate: af => af.Id == id,
                includes: new List<System.Linq.Expressions.Expression<Func<ApprovalFlow, object>>>
                {
                    af => af.Department,
                    af => af.Category,
                    af => af.Steps.OrderBy(s => s.StepOrder)
                });

        var result = approvalFlow.FirstOrDefault();
        if (result == null)
            return null;

        return MapToApprovalFlowDto(result);
    }

    /// <inheritdoc/>
    public async Task<Result<ApprovalFlowDto>> CreateApprovalFlowAsync(CreateUpdateApprovalFlowDto dto)
    {
        if (dto == null)
            return Result<ApprovalFlowDto>.Failure("Onay akışı bilgileri boş olamaz");

        try
        {
            var approvalFlow = new ApprovalFlow
            {
                Name = dto.Name,
                Description = dto.Description ?? string.Empty,
                DepartmentId = dto.DepartmentId,
                CategoryId = dto.CategoryId,
                MinimumAmount = dto.MinAmount,
                MaximumAmount = dto.MaxAmount,
                IsActive = dto.IsActive,
                CreatedAt = DateTime.Now,
                CreatedById = 1, // TODO: Giriş yapmış kullanıcı ID'si alınacak
                Priority = 100 // Varsayılan değer
            };

            await _unitOfWork.Repository<ApprovalFlow>().AddAsync(approvalFlow);
            await _unitOfWork.SaveChangesAsync();

            // Adımları ekleyelim
            if (dto.Steps != null && dto.Steps.Any())
            {
                var steps = dto.Steps.Select((step, index) => new ApprovalFlowStep
                {
                    ApprovalFlowId = approvalFlow.Id,
                    StepOrder = step.Order,
                    Name = step.Name ?? $"Adım {step.Order}",
                    Description = $"{step.Order}. adım",
                    Type = DetermineStepType(step),
                    ApproverId = step.ApproverId,
                    ApproverRoleId = step.RoleId,
                    IsActive = step.IsRequired,
                    RequiredApprovalCount = 1
                }).ToList();

                await _unitOfWork.Repository<ApprovalFlowStep>().AddRangeAsync(steps);
                await _unitOfWork.SaveChangesAsync();
            }

            var createdFlow = await GetApprovalFlowByIdAsync(approvalFlow.Id);
            return Result<ApprovalFlowDto>.Success(createdFlow);
        }
        catch (Exception ex)
        {
            return Result<ApprovalFlowDto>.Failure($"Onay akışı oluşturulurken hata: {ex.Message}");
        }
    }

    /// <inheritdoc/>
    public async Task<Result<ApprovalFlowDto>> UpdateApprovalFlowAsync(int id, CreateUpdateApprovalFlowDto dto)
    {
        if (dto == null)
            return Result<ApprovalFlowDto>.Failure("Onay akışı bilgileri boş olamaz");

        try
        {
            var approvalFlow = await _unitOfWork.Repository<ApprovalFlow>().GetByIdAsync(id);
            if (approvalFlow == null)
                return Result<ApprovalFlowDto>.Failure("Güncellenecek onay akışı bulunamadı");

            // Ana bilgileri güncelle
            approvalFlow.Name = dto.Name;
            approvalFlow.Description = dto.Description ?? string.Empty;
            approvalFlow.DepartmentId = dto.DepartmentId;
            approvalFlow.CategoryId = dto.CategoryId;
            approvalFlow.MinimumAmount = dto.MinAmount;
            approvalFlow.MaximumAmount = dto.MaxAmount;
            approvalFlow.IsActive = dto.IsActive;
            approvalFlow.UpdatedAt = DateTime.Now;
            approvalFlow.UpdatedById = 1; // TODO: Giriş yapmış kullanıcı ID'si alınacak

            await _unitOfWork.Repository<ApprovalFlow>().UpdateAsync(approvalFlow);

            // Mevcut adımları silelim
            var existingSteps = await _unitOfWork.Repository<ApprovalFlowStep>()
                .GetAsync(s => s.ApprovalFlowId == id);
            
            if (existingSteps.Any())
            {
                await _unitOfWork.Repository<ApprovalFlowStep>().DeleteRangeAsync(existingSteps);
            }

            // Yeni adımları ekleyelim
            if (dto.Steps != null && dto.Steps.Any())
            {
                var steps = dto.Steps.Select((step, index) => new ApprovalFlowStep
                {
                    ApprovalFlowId = approvalFlow.Id,
                    StepOrder = step.Order,
                    Name = step.Name ?? $"Adım {step.Order}",
                    Description = $"{step.Order}. adım",
                    Type = DetermineStepType(step),
                    ApproverId = step.ApproverId,
                    ApproverRoleId = step.RoleId,
                    IsActive = step.IsRequired,
                    RequiredApprovalCount = 1
                }).ToList();

                await _unitOfWork.Repository<ApprovalFlowStep>().AddRangeAsync(steps);
            }

            await _unitOfWork.SaveChangesAsync();

            var updatedFlow = await GetApprovalFlowByIdAsync(id);
            return Result<ApprovalFlowDto>.Success(updatedFlow);
        }
        catch (Exception ex)
        {
            return Result<ApprovalFlowDto>.Failure($"Onay akışı güncellenirken hata: {ex.Message}");
        }
    }

    /// <inheritdoc/>
    public async Task<Result> DeleteApprovalFlowAsync(int id)
    {
        try
        {
            var approvalFlow = await _unitOfWork.Repository<ApprovalFlow>().GetByIdAsync(id);
            if (approvalFlow == null)
                return Result.Failure("Silinecek onay akışı bulunamadı");

            // İlişkili talepleri kontrol edelim
            var hasRequests = await _unitOfWork.Repository<PurchaseRequest>()
                .AnyAsync(r => r.ApprovalFlowId == id);

            if (hasRequests)
                return Result.Failure("Bu onay akışına bağlı satın alma talepleri bulunmaktadır. Silinemez.");

            // Önce adımları silelim
            var existingSteps = await _unitOfWork.Repository<ApprovalFlowStep>()
                .GetAsync(s => s.ApprovalFlowId == id);
            
            if (existingSteps.Any())
            {
                await _unitOfWork.Repository<ApprovalFlowStep>().DeleteRangeAsync(existingSteps);
            }

            // Sonra akışı silelim
            await _unitOfWork.Repository<ApprovalFlow>().DeleteAsync(approvalFlow);
            await _unitOfWork.SaveChangesAsync();

            return Result.Success();
        }
        catch (Exception ex)
        {
            return Result.Failure($"Onay akışı silinirken hata: {ex.Message}");
        }
    }

    /// <inheritdoc/>
    public async Task<Result> ToggleApprovalFlowStatusAsync(int id, bool isActive)
    {
        try
        {
            var approvalFlow = await _unitOfWork.Repository<ApprovalFlow>().GetByIdAsync(id);
            if (approvalFlow == null)
                return Result.Failure("Onay akışı bulunamadı");

            approvalFlow.IsActive = isActive;
            approvalFlow.UpdatedAt = DateTime.Now;
            approvalFlow.UpdatedById = 1; // TODO: Giriş yapmış kullanıcı ID'si alınacak

            await _unitOfWork.Repository<ApprovalFlow>().UpdateAsync(approvalFlow);
            await _unitOfWork.SaveChangesAsync();

            return Result.Success();
        }
        catch (Exception ex)
        {
            return Result.Failure($"Onay akışı durumu değiştirilirken hata: {ex.Message}");
        }
    }

    /// <inheritdoc/>
    public async Task<List<ApprovalFlowDto>> GetApprovalFlowsByCriteriaAsync(int? departmentId, int? categoryId, decimal? amount)
    {
        var query = await _unitOfWork.Repository<ApprovalFlow>()
            .GetAsync(
                predicate: af => 
                    af.IsActive &&
                    (departmentId == null || af.DepartmentId == departmentId) &&
                    (categoryId == null || af.CategoryId == categoryId) &&
                    (amount == null || 
                        (af.MinimumAmount == null || af.MinimumAmount <= amount) &&
                        (af.MaximumAmount == null || af.MaximumAmount >= amount)),
                orderBy: q => q.OrderBy(af => af.Priority),
                includes: new List<System.Linq.Expressions.Expression<Func<ApprovalFlow, object>>>
                {
                    af => af.Department,
                    af => af.Category
                });

        return query.Select(MapToApprovalFlowDto).ToList();
    }

    /// <inheritdoc/>
    public async Task<ApprovalFlowDto> DetermineApprovalFlowForRequestAsync(int purchaseRequestId)
    {
        var request = await _unitOfWork.Repository<PurchaseRequest>().GetByIdAsync(purchaseRequestId);
        if (request == null)
            return null;

        // Öncelik sırasına göre uygun olan ilk akışı bulalım
        var matchingFlows = await _unitOfWork.Repository<ApprovalFlow>()
            .GetAsync(
                predicate: af => 
                    af.IsActive &&
                    (af.DepartmentId == null || af.DepartmentId == request.DepartmentId) &&
                    (af.CategoryId == null || af.CategoryId == request.CategoryId) &&
                    (af.MinimumAmount == null || af.MinimumAmount <= request.TotalAmount) &&
                    (af.MaximumAmount == null || af.MaximumAmount >= request.TotalAmount),
                orderBy: q => q.OrderBy(af => af.Priority),
                includes: new List<System.Linq.Expressions.Expression<Func<ApprovalFlow, object>>>
                {
                    af => af.Department,
                    af => af.Category,
                    af => af.Steps.OrderBy(s => s.StepOrder)
                });

        var selectedFlow = matchingFlows.FirstOrDefault();
        if (selectedFlow == null)
            return null;

        return MapToApprovalFlowDto(selectedFlow);
    }

    #region Helper Methods

    private ApprovalFlowDto MapToApprovalFlowDto(ApprovalFlow entity)
    {
        if (entity == null)
            return null;

        var dto = new ApprovalFlowDto
        {
            Id = entity.Id,
            Name = entity.Name,
            Description = entity.Description,
            IsActive = entity.IsActive,
            DepartmentId = entity.DepartmentId,
            DepartmentName = entity.Department?.Name,
            CategoryId = entity.CategoryId,
            CategoryName = entity.Category?.Name,
            MinAmount = entity.MinimumAmount,
            MaxAmount = entity.MaximumAmount,
            Steps = new List<ApprovalFlowStepDto>()
        };

        if (entity.Steps != null)
        {
            foreach (var step in entity.Steps.OrderBy(s => s.StepOrder))
            {
                dto.Steps.Add(new ApprovalFlowStepDto
                {
                    Id = step.Id,
                    ApprovalFlowId = step.ApprovalFlowId,
                    Order = step.StepOrder,
                    Name = step.Name,
                    RoleId = step.ApproverRoleId,
                    RoleName = step.ApproverRole?.Name,
                    ApproverId = step.ApproverId,
                    ApproverName = step.Approver?.FullName,
                    IsRequired = step.IsActive
                });
            }
        }

        return dto;
    }

    private StepType DetermineStepType(CreateUpdateApprovalFlowStepDto step)
    {
        if (step.ApproverId.HasValue)
            return StepType.IndividualApproval;

        if (step.RoleId.HasValue)
            return StepType.RoleApproval;

        return StepType.AutomaticApproval;
    }

    #endregion
} 