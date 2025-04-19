using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SatinAlmaPlatformu.BusinessLogic.DTOs;
using SatinAlmaPlatformu.Core.Models;
using SatinAlmaPlatformu.Core.UnitOfWork;
using SatinAlmaPlatformu.Entities.Models;

namespace SatinAlmaPlatformu.BusinessLogic.Services;

/// <summary>
/// Onay akış şablonları ve konfigürasyonları yönetim servisi.
/// Bu servis genel onay akış şablonlarını oluşturma, 
/// kategorilere veya departmanlara göre özelleştirme ve
/// dinamik onay akışlarını yapılandırma işlemlerini sağlar.
/// </summary>
public class ApprovalFlowConfigurationService
{
    private readonly IUnitOfWork _unitOfWork;

    public ApprovalFlowConfigurationService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
    }

    /// <summary>
    /// Standart onay akış şablonları oluşturur
    /// </summary>
    public async Task<Result<List<ApprovalFlowDto>>> CreateDefaultTemplatesAsync(int createdById)
    {
        try
        {
            var templates = new List<ApprovalFlow>
            {
                // Düşük bütçeli satın almalar için basit onay şablonu
                new ApprovalFlow
                {
                    Name = "Düşük Bütçe Onay Akışı",
                    Description = "10.000 TL'ye kadar olan talepler için basitleştirilmiş onay akışı",
                    MinimumAmount = 0,
                    MaximumAmount = 10000,
                    Currency = "TRY",
                    Priority = 10,
                    IsActive = true,
                    CreatedAt = DateTime.Now,
                    CreatedById = createdById,
                    Steps = new List<ApprovalFlowStep>
                    {
                        new ApprovalFlowStep
                        {
                            StepOrder = 1,
                            Name = "Departman Yöneticisi Onayı",
                            Description = "Talep sahibinin bağlı olduğu departman yöneticisinin onayı",
                            Type = StepType.DepartmentApproval,
                            IsActive = true,
                            TimeoutHours = 48,
                            TimeoutAction = TimeoutAction.SendReminder,
                            RejectAction = RejectAction.Cancel
                        },
                        new ApprovalFlowStep
                        {
                            StepOrder = 2,
                            Name = "Satın Alma Uzmanı Kontrolü",
                            Description = "Satın alma birimi tarafından inceleme",
                            Type = StepType.RoleApproval,
                            IsActive = true,
                            TimeoutHours = 24,
                            TimeoutAction = TimeoutAction.SendReminder,
                            RejectAction = RejectAction.ReturnToRequester
                        }
                    }
                },

                // Orta bütçeli satın almalar için onay şablonu
                new ApprovalFlow
                {
                    Name = "Orta Bütçe Onay Akışı",
                    Description = "10.000 TL - 50.000 TL arası talepler için onay akışı",
                    MinimumAmount = 10000.01m,
                    MaximumAmount = 50000,
                    Currency = "TRY",
                    Priority = 20,
                    IsActive = true,
                    CreatedAt = DateTime.Now,
                    CreatedById = createdById,
                    Steps = new List<ApprovalFlowStep>
                    {
                        new ApprovalFlowStep
                        {
                            StepOrder = 1,
                            Name = "Departman Yöneticisi Onayı",
                            Description = "Talep sahibinin bağlı olduğu departman yöneticisinin onayı",
                            Type = StepType.DepartmentApproval,
                            IsActive = true,
                            TimeoutHours = 48,
                            TimeoutAction = TimeoutAction.SendReminder,
                            RejectAction = RejectAction.Cancel
                        },
                        new ApprovalFlowStep
                        {
                            StepOrder = 2,
                            Name = "Satın Alma Uzmanı İncelemesi",
                            Description = "Satın alma uzmanı tarafından teknik ve fiyat incelemesi",
                            Type = StepType.RoleApproval,
                            IsActive = true,
                            TimeoutHours = 72,
                            TimeoutAction = TimeoutAction.SendReminder,
                            RejectAction = RejectAction.ReturnToRequester,
                            AutomatedAction = AutomatedAction.SendRfqToSuppliers
                        },
                        new ApprovalFlowStep
                        {
                            StepOrder = 3,
                            Name = "Satın Alma Müdürü Onayı",
                            Description = "Satın alma müdürü onayı",
                            Type = StepType.RoleApproval,
                            IsActive = true,
                            TimeoutHours = 24,
                            TimeoutAction = TimeoutAction.SendReminder,
                            RejectAction = RejectAction.ReturnToStep,
                            ReturnToStepOrder = 2
                        },
                        new ApprovalFlowStep
                        {
                            StepOrder = 4,
                            Name = "Bütçe Kontrolü",
                            Description = "Bütçe kontrolü ve onayı",
                            Type = StepType.AutomaticApproval,
                            IsActive = true,
                            AutomatedAction = AutomatedAction.BudgetCheck
                        }
                    }
                },

                // Yüksek bütçeli satın almalar için kapsamlı onay şablonu
                new ApprovalFlow
                {
                    Name = "Yüksek Bütçe Onay Akışı",
                    Description = "50.000 TL üzeri talepler için kapsamlı onay akışı",
                    MinimumAmount = 50000.01m,
                    Currency = "TRY",
                    Priority = 30,
                    IsActive = true,
                    CreatedAt = DateTime.Now,
                    CreatedById = createdById,
                    Steps = new List<ApprovalFlowStep>
                    {
                        new ApprovalFlowStep
                        {
                            StepOrder = 1,
                            Name = "Departman Yöneticisi Onayı",
                            Description = "Talep sahibinin bağlı olduğu departman yöneticisinin onayı",
                            Type = StepType.DepartmentApproval,
                            IsActive = true,
                            TimeoutHours = 24,
                            TimeoutAction = TimeoutAction.SendReminder,
                            RejectAction = RejectAction.Cancel
                        },
                        new ApprovalFlowStep
                        {
                            StepOrder = 2,
                            Name = "Bölüm Direktörü Onayı",
                            Description = "İlgili bölüm direktörünün onayı",
                            Type = StepType.RoleApproval,
                            IsActive = true,
                            TimeoutHours = 48,
                            TimeoutAction = TimeoutAction.SendReminder,
                            RejectAction = RejectAction.ReturnToRequester
                        },
                        new ApprovalFlowStep
                        {
                            StepOrder = 3,
                            Name = "Satın Alma Uzmanı İncelemesi",
                            Description = "Satın alma uzmanı tarafından teknik ve fiyat incelemesi",
                            Type = StepType.RoleApproval,
                            IsActive = true,
                            TimeoutHours = 72,
                            TimeoutAction = TimeoutAction.SendReminder,
                            RejectAction = RejectAction.ReturnToRequester,
                            AutomatedAction = AutomatedAction.SendRfqToSuppliers
                        },
                        new ApprovalFlowStep
                        {
                            StepOrder = 4,
                            Name = "Satın Alma Müdürü Onayı",
                            Description = "Satın alma müdürü onayı",
                            Type = StepType.RoleApproval,
                            IsActive = true,
                            TimeoutHours = 48,
                            TimeoutAction = TimeoutAction.SendReminder,
                            RejectAction = RejectAction.ReturnToStep,
                            ReturnToStepOrder = 3
                        },
                        new ApprovalFlowStep
                        {
                            StepOrder = 5,
                            Name = "Bütçe Kontrolü",
                            Description = "Finans departmanı tarafından bütçe kontrolü",
                            Type = StepType.RoleApproval,
                            IsActive = true,
                            TimeoutHours = 48,
                            TimeoutAction = TimeoutAction.SendReminder,
                            RejectAction = RejectAction.ReturnToStep,
                            ReturnToStepOrder = 3,
                            AutomatedAction = AutomatedAction.BudgetCheck
                        },
                        new ApprovalFlowStep
                        {
                            StepOrder = 6,
                            Name = "Genel Müdür Onayı",
                            Description = "Genel müdür onayı",
                            Type = StepType.IndividualApproval,
                            IsActive = true,
                            TimeoutHours = 72,
                            TimeoutAction = TimeoutAction.Escalate,
                            RejectAction = RejectAction.Cancel
                        }
                    }
                }
            };

            // Veritabanına kaydedelim
            await _unitOfWork.Repository<ApprovalFlow>().AddRangeAsync(templates);
            await _unitOfWork.SaveChangesAsync();

            // DTO'lara dönüştürelim
            var approvalFlowRepository = _unitOfWork.Repository<ApprovalFlow>();
            var createdTemplates = await approvalFlowRepository.GetAsync(
                predicate: af => templates.Select(t => t.Id).Contains(af.Id),
                includes: new List<System.Linq.Expressions.Expression<Func<ApprovalFlow, object>>>
                {
                    af => af.Steps.OrderBy(s => s.StepOrder)
                }
            );

            var dtos = createdTemplates.Select(MapToApprovalFlowDto).ToList();
            return Result<List<ApprovalFlowDto>>.Success(dtos);
        }
        catch (Exception ex)
        {
            return Result<List<ApprovalFlowDto>>.Failure($"Varsayılan şablonlar oluşturulurken hata: {ex.Message}");
        }
    }

    /// <summary>
    /// Belirli bir kategori için özelleştirilmiş onay akışı oluşturur
    /// </summary>
    public async Task<Result<ApprovalFlowDto>> CreateCategorySpecificFlow(int categoryId, int createdById, decimal? minimumAmount = null, decimal? maximumAmount = null)
    {
        try
        {
            // Önce kategoriyi kontrol edelim
            var category = await _unitOfWork.Repository<Category>().GetByIdAsync(categoryId);
            if (category == null)
                return Result<ApprovalFlowDto>.Failure($"Kategori bulunamadı (ID: {categoryId})");

            // Kategori tipine göre adımları belirleyelim
            var steps = new List<ApprovalFlowStep>();

            // Kategori tipine göre varsayılan adımları oluşturalım
            steps.Add(new ApprovalFlowStep
            {
                StepOrder = 1,
                Name = "Departman Yöneticisi Onayı",
                Description = "Talep sahibinin bağlı olduğu departman yöneticisinin onayı",
                Type = StepType.DepartmentApproval,
                IsActive = true,
                TimeoutHours = 48,
                TimeoutAction = TimeoutAction.SendReminder,
                RejectAction = RejectAction.Cancel
            });

            // BT kategorisi ise BT onayı ekleyelim
            if (category.Name.Contains("BT") || category.Name.Contains("Bilgi Teknolojileri") || category.Name.Contains("IT"))
            {
                steps.Add(new ApprovalFlowStep
                {
                    StepOrder = 2,
                    Name = "BT Departmanı Onayı",
                    Description = "BT departmanı tarafından teknik onay",
                    Type = StepType.DepartmentApproval,
                    IsActive = true,
                    TimeoutHours = 72,
                    TimeoutAction = TimeoutAction.SendReminder,
                    RejectAction = RejectAction.ReturnToRequester
                });

                steps.Add(new ApprovalFlowStep
                {
                    StepOrder = 3,
                    Name = "Satın Alma Uzmanı İncelemesi",
                    Description = "Satın alma uzmanı tarafından teknik ve fiyat incelemesi",
                    Type = StepType.RoleApproval,
                    IsActive = true,
                    TimeoutHours = 48,
                    TimeoutAction = TimeoutAction.SendReminder,
                    RejectAction = RejectAction.ReturnToRequester,
                    AutomatedAction = AutomatedAction.SendRfqToSuppliers
                });
            }
            // Genel kategorilerde satın alma adımı
            else
            {
                steps.Add(new ApprovalFlowStep
                {
                    StepOrder = 2,
                    Name = "Satın Alma Uzmanı İncelemesi",
                    Description = "Satın alma uzmanı tarafından teknik ve fiyat incelemesi",
                    Type = StepType.RoleApproval,
                    IsActive = true,
                    TimeoutHours = 48,
                    TimeoutAction = TimeoutAction.SendReminder,
                    RejectAction = RejectAction.ReturnToRequester,
                    AutomatedAction = AutomatedAction.SendRfqToSuppliers
                });
            }

            // En son adım olarak satın alma yönetici onayı
            steps.Add(new ApprovalFlowStep
            {
                StepOrder = steps.Count + 1,
                Name = "Satın Alma Müdürü Onayı",
                Description = "Satın alma müdürü onayı",
                Type = StepType.RoleApproval,
                IsActive = true,
                TimeoutHours = 24,
                TimeoutAction = TimeoutAction.SendReminder,
                RejectAction = RejectAction.ReturnToStep,
                ReturnToStepOrder = 2
            });

            // Akışı oluşturalım
            var approvalFlow = new ApprovalFlow
            {
                Name = $"{category.Name} Kategori Onay Akışı",
                Description = $"{category.Name} kategorisine özel olarak oluşturulmuş onay akışı",
                CategoryId = categoryId,
                MinimumAmount = minimumAmount,
                MaximumAmount = maximumAmount,
                Currency = "TRY",
                Priority = 5, // Kategori bazlı akışlar yüksek öncelikli
                IsActive = true,
                CreatedAt = DateTime.Now,
                CreatedById = createdById,
                Steps = steps
            };

            // Veritabanına kaydedelim
            await _unitOfWork.Repository<ApprovalFlow>().AddAsync(approvalFlow);
            await _unitOfWork.SaveChangesAsync();

            // Kaydedilen akışı yükleyelim (ilişkiler dahil)
            var createdFlow = await _unitOfWork.Repository<ApprovalFlow>().GetAsync(
                predicate: af => af.Id == approvalFlow.Id,
                includes: new List<System.Linq.Expressions.Expression<Func<ApprovalFlow, object>>>
                {
                    af => af.Category,
                    af => af.Steps.OrderBy(s => s.StepOrder)
                }
            );

            return Result<ApprovalFlowDto>.Success(MapToApprovalFlowDto(createdFlow.FirstOrDefault()));
        }
        catch (Exception ex)
        {
            return Result<ApprovalFlowDto>.Failure($"Kategori için onay akışı oluşturulurken hata: {ex.Message}");
        }
    }

    /// <summary>
    /// Belirli bir departman için özelleştirilmiş onay akışı oluşturur
    /// </summary>
    public async Task<Result<ApprovalFlowDto>> CreateDepartmentSpecificFlow(int departmentId, int createdById, decimal? minimumAmount = null, decimal? maximumAmount = null)
    {
        try
        {
            // Önce departmanı kontrol edelim
            var department = await _unitOfWork.Repository<Department>().GetByIdAsync(departmentId);
            if (department == null)
                return Result<ApprovalFlowDto>.Failure($"Departman bulunamadı (ID: {departmentId})");

            // Departman bazlı standart adımları oluşturalım
            var steps = new List<ApprovalFlowStep>
            {
                new ApprovalFlowStep
                {
                    StepOrder = 1,
                    Name = $"{department.Name} Yöneticisi Onayı",
                    Description = $"{department.Name} departmanı yöneticisinin onayı",
                    Type = StepType.DepartmentApproval,
                    IsActive = true,
                    TimeoutHours = 48,
                    TimeoutAction = TimeoutAction.SendReminder,
                    RejectAction = RejectAction.Cancel
                },
                new ApprovalFlowStep
                {
                    StepOrder = 2,
                    Name = "Satın Alma Uzmanı İncelemesi",
                    Description = "Satın alma uzmanı tarafından teknik ve fiyat incelemesi",
                    Type = StepType.RoleApproval,
                    IsActive = true,
                    TimeoutHours = 72,
                    TimeoutAction = TimeoutAction.SendReminder,
                    RejectAction = RejectAction.ReturnToRequester,
                    AutomatedAction = AutomatedAction.SendRfqToSuppliers
                },
                new ApprovalFlowStep
                {
                    StepOrder = 3,
                    Name = "Satın Alma Müdürü Onayı",
                    Description = "Satın alma müdürü onayı",
                    Type = StepType.RoleApproval,
                    IsActive = true,
                    TimeoutHours = 24,
                    TimeoutAction = TimeoutAction.SendReminder,
                    RejectAction = RejectAction.ReturnToStep,
                    ReturnToStepOrder = 2
                }
            };

            // Akışı oluşturalım
            var approvalFlow = new ApprovalFlow
            {
                Name = $"{department.Name} Departmanı Onay Akışı",
                Description = $"{department.Name} departmanına özel oluşturulmuş onay akışı",
                DepartmentId = departmentId,
                MinimumAmount = minimumAmount,
                MaximumAmount = maximumAmount,
                Currency = "TRY",
                Priority = 5, // Departman bazlı akışlar yüksek öncelikli
                IsActive = true,
                CreatedAt = DateTime.Now,
                CreatedById = createdById,
                Steps = steps
            };

            // Veritabanına kaydedelim
            await _unitOfWork.Repository<ApprovalFlow>().AddAsync(approvalFlow);
            await _unitOfWork.SaveChangesAsync();

            // Kaydedilen akışı yükleyelim (ilişkiler dahil)
            var createdFlow = await _unitOfWork.Repository<ApprovalFlow>().GetAsync(
                predicate: af => af.Id == approvalFlow.Id,
                includes: new List<System.Linq.Expressions.Expression<Func<ApprovalFlow, object>>>
                {
                    af => af.Department,
                    af => af.Steps.OrderBy(s => s.StepOrder)
                }
            );

            return Result<ApprovalFlowDto>.Success(MapToApprovalFlowDto(createdFlow.FirstOrDefault()));
        }
        catch (Exception ex)
        {
            return Result<ApprovalFlowDto>.Failure($"Departman için onay akışı oluşturulurken hata: {ex.Message}");
        }
    }

    /// <summary>
    /// Mevcut bir onay akışını kopyalayarak yeni bir flow oluşturur
    /// </summary>
    public async Task<Result<ApprovalFlowDto>> CloneApprovalFlowAsync(int sourceFlowId, string newName, int createdById, int? departmentId = null, int? categoryId = null)
    {
        try
        {
            // Kaynak akışı yükleyelim
            var sourceFlow = await _unitOfWork.Repository<ApprovalFlow>().GetAsync(
                predicate: af => af.Id == sourceFlowId,
                includes: new List<System.Linq.Expressions.Expression<Func<ApprovalFlow, object>>>
                {
                    af => af.Steps.OrderBy(s => s.StepOrder)
                }
            );

            var flow = sourceFlow.FirstOrDefault();
            if (flow == null)
                return Result<ApprovalFlowDto>.Failure($"Kaynak onay akışı bulunamadı (ID: {sourceFlowId})");

            // Yeni akış nesnesi oluşturalım
            var newFlow = new ApprovalFlow
            {
                Name = newName,
                Description = $"{flow.Name} akışından kopyalanmıştır.",
                DepartmentId = departmentId,
                CategoryId = categoryId,
                MinimumAmount = flow.MinimumAmount,
                MaximumAmount = flow.MaximumAmount,
                Currency = flow.Currency,
                Priority = flow.Priority,
                IsActive = true,
                CreatedAt = DateTime.Now,
                CreatedById = createdById,
                Steps = new List<ApprovalFlowStep>()
            };

            // Adımları kopyalayalım
            if (flow.Steps != null && flow.Steps.Any())
            {
                foreach (var step in flow.Steps.OrderBy(s => s.StepOrder))
                {
                    newFlow.Steps.Add(new ApprovalFlowStep
                    {
                        StepOrder = step.StepOrder,
                        Name = step.Name,
                        Description = step.Description,
                        Type = step.Type,
                        ApproverId = step.ApproverId,
                        ApproverRoleId = step.ApproverRoleId,
                        ApproverDepartmentId = step.ApproverDepartmentId,
                        RequiredApprovalCount = step.RequiredApprovalCount,
                        TimeoutHours = step.TimeoutHours,
                        TimeoutAction = step.TimeoutAction,
                        AutomatedAction = step.AutomatedAction,
                        IsActive = step.IsActive,
                        RejectAction = step.RejectAction,
                        ReturnToStepOrder = step.ReturnToStepOrder
                    });
                }
            }

            // Veritabanına kaydedelim
            await _unitOfWork.Repository<ApprovalFlow>().AddAsync(newFlow);
            await _unitOfWork.SaveChangesAsync();

            // Kaydedilen akışı yükleyelim (ilişkiler dahil)
            var createdFlow = await _unitOfWork.Repository<ApprovalFlow>().GetAsync(
                predicate: af => af.Id == newFlow.Id,
                includes: new List<System.Linq.Expressions.Expression<Func<ApprovalFlow, object>>>
                {
                    af => af.Department,
                    af => af.Category,
                    af => af.Steps.OrderBy(s => s.StepOrder)
                }
            );

            return Result<ApprovalFlowDto>.Success(MapToApprovalFlowDto(createdFlow.FirstOrDefault()));
        }
        catch (Exception ex)
        {
            return Result<ApprovalFlowDto>.Failure($"Onay akışı kopyalanırken hata: {ex.Message}");
        }
    }

    /// <summary>
    /// Belirli bir talep için en uygun onay akışını belirler
    /// </summary>
    public async Task<Result<ApprovalFlowDto>> DetermineBestFlowForRequest(int purchaseRequestId)
    {
        try
        {
            // Talebi yükleyelim
            var request = await _unitOfWork.Repository<PurchaseRequest>().GetByIdAsync(purchaseRequestId);
            if (request == null)
                return Result<ApprovalFlowDto>.Failure($"Satın alma talebi bulunamadı (ID: {purchaseRequestId})");

            // Kriterlere göre sıralı olarak akış arama:
            // 1. Önce kategori ve departman her ikisi de eşleşen akış
            // 2. Sadece kategori eşleşen
            // 3. Sadece departman eşleşen
            // 4. Son olarak genel akış (tutar bazlı)

            var matchingFlows = await _unitOfWork.Repository<ApprovalFlow>()
                .GetAsync(
                    predicate: af => 
                        af.IsActive &&
                        (
                            // 1. Kategori ve departman eşleşen
                            (af.CategoryId == request.CategoryId && af.DepartmentId == request.DepartmentId) ||
                            // 2. Sadece kategori eşleşen
                            (af.CategoryId == request.CategoryId && af.DepartmentId == null) ||
                            // 3. Sadece departman eşleşen
                            (af.DepartmentId == request.DepartmentId && af.CategoryId == null) ||
                            // 4. Genel akış (her ikisi de null)
                            (af.CategoryId == null && af.DepartmentId == null)
                        ) &&
                        (af.MinimumAmount == null || af.MinimumAmount <= request.TotalAmount) &&
                        (af.MaximumAmount == null || af.MaximumAmount >= request.TotalAmount),
                    orderBy: q => q.OrderBy(af => af.Priority),
                    includes: new List<System.Linq.Expressions.Expression<Func<ApprovalFlow, object>>>
                    {
                        af => af.Department,
                        af => af.Category,
                        af => af.Steps.OrderBy(s => s.StepOrder)
                    }
                );

            var selectedFlow = matchingFlows.FirstOrDefault();
            if (selectedFlow == null)
                return Result<ApprovalFlowDto>.Failure("Bu talep için uygun onay akışı bulunamadı");

            return Result<ApprovalFlowDto>.Success(MapToApprovalFlowDto(selectedFlow));
        }
        catch (Exception ex)
        {
            return Result<ApprovalFlowDto>.Failure($"Onay akışı belirlenirken hata: {ex.Message}");
        }
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

    #endregion
} 