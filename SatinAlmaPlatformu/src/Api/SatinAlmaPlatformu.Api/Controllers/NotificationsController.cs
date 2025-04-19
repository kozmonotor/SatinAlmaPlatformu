using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SatinAlmaPlatformu.BusinessLogic.DTOs;
using SatinAlmaPlatformu.BusinessLogic.Services.Interfaces;
using SatinAlmaPlatformu.Core.Results;

namespace SatinAlmaPlatformu.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class NotificationsController : ControllerBase
    {
        private readonly INotificationService _notificationService;
        private readonly ILogger<NotificationsController> _logger;

        public NotificationsController(
            INotificationService notificationService,
            ILogger<NotificationsController> logger)
        {
            _notificationService = notificationService ?? throw new ArgumentNullException(nameof(notificationService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Okunmamış bildirimleri getirir
        /// </summary>
        [HttpGet("unread")]
        [ProducesResponseType(typeof(Result<List<NotificationDto>>), 200)]
        public async Task<ActionResult<Result<List<NotificationDto>>>> GetUnreadNotifications()
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var result = await _notificationService.GetUnreadNotificationsAsync(userId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Okunmamış bildirimler getirilirken hata oluştu");
                return StatusCode(500, Result.Failure<List<NotificationDto>>("Bildirimler getirilirken bir hata oluştu"));
            }
        }

        /// <summary>
        /// Kullanıcının bildirimlerini sayfalı şekilde getirir
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(Result<PagedResult<NotificationDto>>), 200)]
        public async Task<ActionResult<Result<PagedResult<NotificationDto>>>> GetNotifications(
            [FromQuery] int pageNumber = 1, 
            [FromQuery] int pageSize = 10)
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var result = await _notificationService.GetNotificationsAsync(userId, pageNumber, pageSize);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Bildirimler getirilirken hata oluştu. PageNumber: {PageNumber}, PageSize: {PageSize}", 
                    pageNumber, pageSize);
                return StatusCode(500, Result.Failure<PagedResult<NotificationDto>>("Bildirimler getirilirken bir hata oluştu"));
            }
        }

        /// <summary>
        /// Bildirimi okundu olarak işaretler
        /// </summary>
        [HttpPut("{id}/read")]
        [ProducesResponseType(typeof(Result), 200)]
        public async Task<ActionResult<Result>> MarkAsRead(int id)
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var result = await _notificationService.MarkAsReadAsync(id, userId);
                
                if (!result.IsSuccess)
                    return BadRequest(result);
                
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Bildirim okundu olarak işaretlenirken hata oluştu. NotificationId: {NotificationId}", id);
                return StatusCode(500, Result.Failure("Bildirim okundu olarak işaretlenirken bir hata oluştu"));
            }
        }

        /// <summary>
        /// Birden çok bildirimi okundu olarak işaretler
        /// </summary>
        [HttpPut("read")]
        [ProducesResponseType(typeof(Result), 200)]
        public async Task<ActionResult<Result>> MarkMultipleAsRead([FromBody] List<int> notificationIds)
        {
            try
            {
                if (notificationIds == null || notificationIds.Count == 0)
                    return BadRequest(Result.Failure("Bildirim ID'leri boş olamaz"));
                
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var result = await _notificationService.MarkMultipleAsReadAsync(notificationIds, userId);
                
                if (!result.IsSuccess)
                    return BadRequest(result);
                
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Bildirimler okundu olarak işaretlenirken hata oluştu. Count: {Count}", 
                    notificationIds?.Count ?? 0);
                return StatusCode(500, Result.Failure("Bildirimler okundu olarak işaretlenirken bir hata oluştu"));
            }
        }

        [HttpPost("subscribe")]
        [Authorize]
        public async Task<ActionResult<Result>> SubscribeToNotifications()
        {
            // This API endpoint is just for documentation purposes.
            // The actual subscription happens client-side when connecting to the SignalR hub
            return Ok(Result.Success("NotificationHub bağlantısı yapılabilir durumdadır. Client tarafında SignalR bağlantısı yaparak bildirimlere abone olabilirsiniz."));
        }
    }
} 