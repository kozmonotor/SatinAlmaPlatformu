using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SatinAlmaPlatformu.BusinessLogic.DTOs;
using SatinAlmaPlatformu.BusinessLogic.Services.Interfaces;
using SatinAlmaPlatformu.Core.Results;
using SatinAlmaPlatformu.Core.UnitOfWork;
using SatinAlmaPlatformu.Entities.Models;
using Microsoft.AspNetCore.SignalR;
using SatinAlmaPlatformu.Api.Hubs;
using SatinAlmaPlatformu.BusinessLogic.DTOs.Notification;
using SatinAlmaPlatformu.DataAccess.UnitOfWork;

namespace SatinAlmaPlatformu.BusinessLogic.Services
{
    /// <summary>
    /// Bildirim işlemlerini yöneten servis
    /// </summary>
    public class NotificationService : INotificationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<NotificationService> _logger;
        private readonly IHubContext<NotificationHub> _hubContext;

        public NotificationService(
            IUnitOfWork unitOfWork,
            ILogger<NotificationService> logger,
            IHubContext<NotificationHub> hubContext)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _hubContext = hubContext ?? throw new ArgumentNullException(nameof(hubContext));
        }
        
        /// <summary>
        /// Kullanıcıya bildirim gönderir
        /// </summary>
        public async Task<Result> SendNotificationAsync(string userId, string title, string message, 
            NotificationType type, NotificationReferenceType referenceType, int referenceId, string redirectUrl = null)
        {
            try
            {
                // Kullanıcı kontrolü
                var user = await _unitOfWork.UserRepository.GetByIdAsync(userId);
                if (user == null)
                    return Result.Failure("Kullanıcı bulunamadı");
                
                // Yeni bildirim oluştur
                var notification = new Notification
                {
                    UserId = int.Parse(userId),
                    Title = title,
                    Message = message,
                    Type = type,
                    ReferenceType = referenceType,
                    ReferenceId = referenceId,
                    RedirectUrl = redirectUrl,
                    IsRead = false,
                    CreatedDate = DateTime.Now,
                    CreatedById = 1 // Sistem kullanıcısı
                };
                
                await _unitOfWork.NotificationRepository.AddAsync(notification);
                await _unitOfWork.SaveChangesAsync();
                
                // Send real-time notification using SignalR
                await _hubContext.Clients.User(userId).SendAsync("ReceiveNotification", new 
                {
                    id = notification.Id,
                    message = notification.Message,
                    title = notification.Title,
                    type = notification.Type.ToString(),
                    createdAt = notification.CreatedDate
                });
                
                // TODO: E-posta bildirimi gönderme implementasyonu
                // 1. Bildirim tipine göre e-posta gönderme kararı ver (örn: NotificationType.RequiresEmail)
                // 2. E-posta servisini enjekte et (IEmailService) ve şablonları kullan
                // 3. Kullanıcının e-posta tercihlerini kontrol et (ayarlardan kapatmış olabilir)
                // 4. E-posta gönderimini asenkron olarak gerçekleştir
                
                return Result.Success("Bildirim başarıyla gönderildi");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Bildirim gönderilirken hata oluştu. UserId: {UserId}, Title: {Title}", userId, title);
                return Result.Failure("Bildirim gönderilirken bir hata oluştu");
            }
        }
        
        /// <summary>
        /// Birden fazla kullanıcıya aynı bildirimi gönderir
        /// </summary>
        public async Task<Result> SendBulkNotificationsAsync(List<string> userIds, string title, string message,
            NotificationType type, NotificationReferenceType referenceType, int referenceId, string redirectUrl = null)
        {
            try
            {
                if (userIds == null || !userIds.Any())
                    return Result.Success("Bildirim gönderilecek kullanıcı bulunamadı");
                
                // Kullanıcıları kontrol et
                var users = await _unitOfWork.UserRepository.GetAllAsync(u => userIds.Contains(u.Id.ToString()));
                if (!users.Any())
                    return Result.Failure("Bildirim gönderilecek kullanıcılar bulunamadı");
                
                // Bildirimleri oluştur
                var notifications = users.Select(u => new Notification
                {
                    UserId = u.Id,
                    Title = title,
                    Message = message,
                    Type = type,
                    ReferenceType = referenceType,
                    ReferenceId = referenceId,
                    RedirectUrl = redirectUrl,
                    IsRead = false,
                    CreatedDate = DateTime.Now,
                    CreatedById = 1 // Sistem kullanıcısı
                }).ToList();
                
                await _unitOfWork.NotificationRepository.AddRangeAsync(notifications);
                await _unitOfWork.SaveChangesAsync();
                
                // Send real-time notifications to each user
                foreach (var userId in userIds)
                {
                    var userNotification = notifications.First(n => n.UserId == int.Parse(userId));
                    await _hubContext.Clients.User(userId).SendAsync("ReceiveNotification", new
                    {
                        id = userNotification.Id,
                        message = userNotification.Message,
                        title = userNotification.Title,
                        type = userNotification.Type.ToString(),
                        createdAt = userNotification.CreatedDate
                    });
                }
                
                return Result.Success($"{notifications.Count} adet bildirim başarıyla gönderildi");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Toplu bildirim gönderilirken hata oluştu. UserCount: {UserCount}, Title: {Title}", 
                    userIds?.Count ?? 0, title);
                return Result.Failure("Bildirimler gönderilirken bir hata oluştu");
            }
        }
        
        /// <summary>
        /// Onay isteği bildirimi gönderir
        /// </summary>
        public async Task<Result> SendApprovalRequestNotificationAsync(string userId, int purchaseRequestId, int approvalStepId)
        {
            try
            {
                // Talep bilgilerini getir
                var purchaseRequest = await _unitOfWork.PurchaseRequestRepository.GetByIdAsync(purchaseRequestId);
                if (purchaseRequest == null)
                    return Result.Failure("Satın alma talebi bulunamadı");
                
                // Onay adımı bilgilerini getir
                var approvalStep = await _unitOfWork.ApprovalFlowStepRepository.GetByIdAsync(approvalStepId);
                if (approvalStep == null)
                    return Result.Failure("Onay adımı bulunamadı");
                
                // Bildirim gönder
                string title = "Onay Bekleyen Talep";
                string message = $"{purchaseRequest.RequestNumber} numaralı '{purchaseRequest.Title}' başlıklı satın alma talebi onayınızı bekliyor.";
                string redirectUrl = $"/approval/pending/{purchaseRequestId}";
                
                return await SendNotificationAsync(
                    userId, 
                    title, 
                    message, 
                    NotificationType.Info, 
                    NotificationReferenceType.PurchaseRequest, 
                    purchaseRequestId,
                    redirectUrl);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Onay isteği bildirimi gönderilirken hata oluştu. UserId: {UserId}, RequestId: {RequestId}", 
                    userId, purchaseRequestId);
                return Result.Failure("Onay isteği bildirimi gönderilirken bir hata oluştu");
            }
        }
        
        /// <summary>
        /// Talep onaylandığında talep sahibine bildirim gönderir
        /// </summary>
        public async Task<Result> SendRequestApprovedNotificationAsync(string userId, int purchaseRequestId)
        {
            try
            {
                // Talep bilgilerini getir
                var purchaseRequest = await _unitOfWork.PurchaseRequestRepository.GetByIdAsync(purchaseRequestId);
                if (purchaseRequest == null)
                    return Result.Failure("Satın alma talebi bulunamadı");
                
                // Bildirim gönder
                string title = "Talebiniz Onaylandı";
                string message = $"{purchaseRequest.RequestNumber} numaralı '{purchaseRequest.Title}' başlıklı satın alma talebiniz onaylandı.";
                string redirectUrl = $"/purchase-requests/details/{purchaseRequestId}";
                
                return await SendNotificationAsync(
                    userId, 
                    title, 
                    message, 
                    NotificationType.Success, 
                    NotificationReferenceType.PurchaseRequest, 
                    purchaseRequestId,
                    redirectUrl);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Talep onay bildirimi gönderilirken hata oluştu. UserId: {UserId}, RequestId: {RequestId}", 
                    userId, purchaseRequestId);
                return Result.Failure("Talep onay bildirimi gönderilirken bir hata oluştu");
            }
        }
        
        /// <summary>
        /// Talep reddedildiğinde talep sahibine bildirim gönderir
        /// </summary>
        public async Task<Result> SendRequestRejectedNotificationAsync(string userId, int purchaseRequestId, string rejectionReason)
        {
            try
            {
                // Talep bilgilerini getir
                var purchaseRequest = await _unitOfWork.PurchaseRequestRepository.GetByIdAsync(purchaseRequestId);
                if (purchaseRequest == null)
                    return Result.Failure("Satın alma talebi bulunamadı");
                
                // Bildirim gönder
                string title = "Talebiniz Reddedildi";
                string message = $"{purchaseRequest.RequestNumber} numaralı '{purchaseRequest.Title}' başlıklı satın alma talebiniz reddedildi. " +
                                $"Gerekçe: {rejectionReason}";
                string redirectUrl = $"/purchase-requests/details/{purchaseRequestId}";
                
                return await SendNotificationAsync(
                    userId, 
                    title, 
                    message, 
                    NotificationType.Error, 
                    NotificationReferenceType.PurchaseRequest, 
                    purchaseRequestId,
                    redirectUrl);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Talep red bildirimi gönderilirken hata oluştu. UserId: {UserId}, RequestId: {RequestId}", 
                    userId, purchaseRequestId);
                return Result.Failure("Talep red bildirimi gönderilirken bir hata oluştu");
            }
        }
        
        /// <summary>
        /// Revizyon istendiğinde talep sahibine bildirim gönderir
        /// </summary>
        public async Task<Result> SendRevisionRequestNotificationAsync(string userId, int purchaseRequestId, string revisionComment)
        {
            try
            {
                // Talep bilgilerini getir
                var purchaseRequest = await _unitOfWork.PurchaseRequestRepository.GetByIdAsync(purchaseRequestId);
                if (purchaseRequest == null)
                    return Result.Failure("Satın alma talebi bulunamadı");
                
                // Bildirim gönder
                string title = "Talebiniz İçin Revizyon İstendi";
                string message = $"{purchaseRequest.RequestNumber} numaralı '{purchaseRequest.Title}' başlıklı satın alma talebiniz için revizyon istendi. " +
                                $"Açıklama: {revisionComment}";
                string redirectUrl = $"/purchase-requests/edit/{purchaseRequestId}?revisionRequested=true";
                
                return await SendNotificationAsync(
                    userId, 
                    title, 
                    message, 
                    NotificationType.Warning, 
                    NotificationReferenceType.PurchaseRequest, 
                    purchaseRequestId,
                    redirectUrl);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Revizyon isteği bildirimi gönderilirken hata oluştu. UserId: {UserId}, RequestId: {RequestId}", 
                    userId, purchaseRequestId);
                return Result.Failure("Revizyon isteği bildirimi gönderilirken bir hata oluştu");
            }
        }
        
        /// <summary>
        /// Revizyon tamamlandığında revizyon isteyen kullanıcıya bildirim gönderir
        /// </summary>
        public async Task<Result> SendRevisionCompletedNotificationAsync(string userId, int purchaseRequestId, string revisionNote)
        {
            try
            {
                // Talep bilgilerini getir
                var purchaseRequest = await _unitOfWork.PurchaseRequestRepository.GetByIdAsync(purchaseRequestId);
                if (purchaseRequest == null)
                    return Result.Failure("Satın alma talebi bulunamadı");
                
                // Bildirim gönder
                string title = "Revizyon Tamamlandı";
                string message = $"{purchaseRequest.RequestNumber} numaralı '{purchaseRequest.Title}' başlıklı satın alma talebi için istediğiniz " + 
                                $"revizyon tamamlandı. Not: {revisionNote}";
                string redirectUrl = $"/purchase-requests/details/{purchaseRequestId}";
                
                return await SendNotificationAsync(
                    userId, 
                    title, 
                    message, 
                    NotificationType.Info, 
                    NotificationReferenceType.PurchaseRequest, 
                    purchaseRequestId,
                    redirectUrl);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Revizyon tamamlanma bildirimi gönderilirken hata oluştu. UserId: {UserId}, RequestId: {RequestId}", 
                    userId, purchaseRequestId);
                return Result.Failure("Revizyon tamamlanma bildirimi gönderilirken bir hata oluştu");
            }
        }
        
        /// <summary>
        /// Bildirim okundu olarak işaretler
        /// </summary>
        public async Task<Result> MarkAsReadAsync(int notificationId, string userId)
        {
            try
            {
                // Bildirimi getir
                var notification = await _unitOfWork.NotificationRepository
                    .GetAsync(n => n.Id == notificationId && n.UserId == int.Parse(userId));
                
                var userNotification = notification.FirstOrDefault();
                if (userNotification == null)
                    return Result.Failure("Bildirim bulunamadı");
                
                // Bildirim zaten okunmuşsa
                if (userNotification.IsRead)
                    return Result.Success("Bildirim zaten okunmuş");
                
                // Bildirimi okundu olarak işaretle
                userNotification.IsRead = true;
                userNotification.ReadAt = DateTime.Now;
                userNotification.UpdatedById = int.Parse(userId);
                userNotification.UpdatedDate = DateTime.Now;
                
                await _unitOfWork.NotificationRepository.UpdateAsync(userNotification);
                await _unitOfWork.SaveChangesAsync();
                
                return Result.Success("Bildirim okundu olarak işaretlendi");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Bildirim okundu olarak işaretlenirken hata oluştu. NotificationId: {NotificationId}, UserId: {UserId}", 
                    notificationId, userId);
                return Result.Failure("Bildirim okundu olarak işaretlenirken bir hata oluştu");
            }
        }
        
        /// <summary>
        /// Birden fazla bildirimi okundu olarak işaretler
        /// </summary>
        public async Task<Result> MarkMultipleAsReadAsync(List<int> notificationIds, string userId)
        {
            try
            {
                if (notificationIds == null || !notificationIds.Any())
                    return Result.Failure("Bildirim ID'leri belirtilmedi");
                
                // Bildirimleri getir
                var notifications = await _unitOfWork.NotificationRepository
                    .GetAllAsync(n => notificationIds.Contains(n.Id) && n.UserId == int.Parse(userId) && !n.IsRead);
                
                if (!notifications.Any())
                    return Result.Success("Okunacak bildirim bulunamadı");
                
                // Bildirimleri okundu olarak işaretle
                foreach (var notification in notifications)
                {
                    notification.IsRead = true;
                    notification.ReadAt = DateTime.Now;
                    notification.UpdatedById = int.Parse(userId);
                    notification.UpdatedDate = DateTime.Now;
                    
                    await _unitOfWork.NotificationRepository.UpdateAsync(notification);
                }
                
                await _unitOfWork.SaveChangesAsync();
                
                return Result.Success($"{notifications.Count} adet bildirim okundu olarak işaretlendi");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Bildirimler okundu olarak işaretlenirken hata oluştu. NotificationCount: {NotificationCount}, UserId: {UserId}", 
                    notificationIds?.Count ?? 0, userId);
                return Result.Failure("Bildirimler okundu olarak işaretlenirken bir hata oluştu");
            }
        }
        
        /// <summary>
        /// Kullanıcının okunmamış bildirimlerini getirir
        /// </summary>
        public async Task<Result<List<NotificationDto>>> GetUnreadNotificationsAsync(string userId)
        {
            try
            {
                // Okunmamış bildirimleri getir
                var unreadNotifications = await _unitOfWork.NotificationRepository
                    .GetAllAsync(
                        predicate: n => n.UserId == int.Parse(userId) && !n.IsRead,
                        orderBy: q => q.OrderByDescending(n => n.CreatedDate));
                
                // DTO'lara dönüştür
                var notificationDtos = unreadNotifications.Select(MapToDto).ToList();
                
                return Result<List<NotificationDto>>.Success(notificationDtos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Okunmamış bildirimler getirilirken hata oluştu. UserId: {UserId}", userId);
                return Result<List<NotificationDto>>.Failure("Okunmamış bildirimler getirilirken bir hata oluştu");
            }
        }
        
        /// <summary>
        /// Kullanıcının tüm bildirimlerini getirir (sayfalama destekli)
        /// </summary>
        public async Task<Result<PagedResult<NotificationDto>>> GetNotificationsAsync(string userId, int pageNumber = 1, int pageSize = 10)
        {
            try
            {
                // Toplam bildirim sayısını getir
                var totalCount = await _unitOfWork.NotificationRepository
                    .CountAsync(n => n.UserId == int.Parse(userId));
                
                // Sayfalama parametrelerini ayarla
                if (pageNumber < 1) pageNumber = 1;
                if (pageSize < 1) pageSize = 10;
                
                // Bildirimleri getir
                var notifications = await _unitOfWork.NotificationRepository
                    .GetAllAsync(
                        predicate: n => n.UserId == int.Parse(userId),
                        orderBy: q => q.OrderByDescending(n => n.CreatedDate),
                        skip: (pageNumber - 1) * pageSize,
                        take: pageSize);
                
                // DTO'lara dönüştür
                var notificationDtos = notifications.Select(MapToDto).ToList();
                
                // Sayfalama sonucunu oluştur
                var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
                var pagedResult = new PagedResult<NotificationDto>
                {
                    Items = notificationDtos,
                    TotalCount = totalCount,
                    TotalPages = totalPages,
                    CurrentPage = pageNumber,
                    PageSize = pageSize
                };
                
                return Result<PagedResult<NotificationDto>>.Success(pagedResult);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Bildirimler getirilirken hata oluştu. UserId: {UserId}, Page: {Page}, PageSize: {PageSize}", 
                    userId, pageNumber, pageSize);
                return Result<PagedResult<NotificationDto>>.Failure("Bildirimler getirilirken bir hata oluştu");
            }
        }
        
        /// <summary>
        /// Notification entity'sini NotificationDto'ya dönüştürür
        /// </summary>
        private NotificationDto MapToDto(Notification notification)
        {
            string relativeTime = GetRelativeTime(notification.CreatedDate);
            
            return new NotificationDto
            {
                Id = notification.Id,
                Title = notification.Title,
                Message = notification.Message,
                Type = notification.Type,
                ReferenceType = notification.ReferenceType,
                ReferenceId = notification.ReferenceId,
                IsRead = notification.IsRead,
                ReadAt = notification.ReadAt,
                RedirectUrl = notification.RedirectUrl,
                CreatedDate = notification.CreatedDate,
                RelativeTime = relativeTime
            };
        }
        
        /// <summary>
        /// Tarih için göreceli zaman formatını döndürür (örn: "5 dakika önce", "2 saat önce")
        /// </summary>
        private string GetRelativeTime(DateTime dateTime)
        {
            var now = DateTime.Now;
            var timeSpan = now - dateTime;
            
            if (timeSpan.TotalSeconds < 60)
                return "Az önce";
            
            if (timeSpan.TotalMinutes < 60)
                return $"{(int)timeSpan.TotalMinutes} dakika önce";
            
            if (timeSpan.TotalHours < 24)
                return $"{(int)timeSpan.TotalHours} saat önce";
            
            if (timeSpan.TotalDays < 7)
                return $"{(int)timeSpan.TotalDays} gün önce";
            
            if (timeSpan.TotalDays < 30)
                return $"{(int)(timeSpan.TotalDays / 7)} hafta önce";
            
            if (timeSpan.TotalDays < 365)
                return $"{(int)(timeSpan.TotalDays / 30)} ay önce";
            
            return $"{(int)(timeSpan.TotalDays / 365)} yıl önce";
        }
    }
} 