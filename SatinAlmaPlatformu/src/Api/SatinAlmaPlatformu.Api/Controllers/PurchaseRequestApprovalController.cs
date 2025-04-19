using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SatinAlmaPlatformu.BusinessLogic.DTOs.Approval;
using SatinAlmaPlatformu.BusinessLogic.Services.Interfaces;
using SatinAlmaPlatformu.Core.Constants;
using System.Security.Claims;

namespace SatinAlmaPlatformu.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class PurchaseRequestApprovalController : ControllerBase
    {
        private readonly IPurchaseRequestApprovalService _approvalService;
        private readonly ILogger<PurchaseRequestApprovalController> _logger;

        public PurchaseRequestApprovalController(
            IPurchaseRequestApprovalService approvalService,
            ILogger<PurchaseRequestApprovalController> logger)
        {
            _approvalService = approvalService;
            _logger = logger;
        }

        [HttpGet("pending")]
        [ProducesResponseType(typeof(List<PendingApprovalDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetPendingApprovals()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _approvalService.GetPendingApprovalsForUserAsync(userId);
            return Ok(result);
        }

        [HttpPost("{approvalId}/approve")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> ApproveRequest(int approvalId, [FromBody] ApprovalActionDto approvalAction)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _approvalService.ApproveRequestAsync(approvalId, userId, approvalAction.Comments);
            
            if (!result.Success)
            {
                if (result.Error == ErrorType.NotFound)
                {
                    return NotFound(result);
                }
                else if (result.Error == ErrorType.Unauthorized)
                {
                    return Forbid();
                }
                return BadRequest(result);
            }

            return Ok(result);
        }

        [HttpPost("{approvalId}/reject")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> RejectRequest(int approvalId, [FromBody] ApprovalActionDto approvalAction)
        {
            if (string.IsNullOrEmpty(approvalAction.Comments))
            {
                ModelState.AddModelError("Comments", "Ret işlemi için açıklama gereklidir.");
                return BadRequest(ModelState);
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _approvalService.RejectRequestAsync(approvalId, userId, approvalAction.Comments);
            
            if (!result.Success)
            {
                if (result.Error == ErrorType.NotFound)
                {
                    return NotFound(result);
                }
                else if (result.Error == ErrorType.Unauthorized)
                {
                    return Forbid();
                }
                return BadRequest(result);
            }

            return Ok(result);
        }

        [HttpGet("history/{requestId}")]
        [ProducesResponseType(typeof(List<ApprovalHistoryDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetApprovalHistory(int requestId)
        {
            var result = await _approvalService.GetApprovalHistoryAsync(requestId);
            
            if (!result.Success && result.Error == ErrorType.NotFound)
            {
                return NotFound(result);
            }
            
            return Ok(result);
        }
        
        [HttpGet("status/{requestId}")]
        [ProducesResponseType(typeof(ApprovalStatusDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetApprovalStatus(int requestId)
        {
            var result = await _approvalService.GetApprovalStatusAsync(requestId);
            
            if (!result.Success && result.Error == ErrorType.NotFound)
            {
                return NotFound(result);
            }
            
            return Ok(result);
        }
    }
}

// ApprovalActionDto için model sınıfı oluşturalım
namespace SatinAlmaPlatformu.BusinessLogic.DTOs.Approval
{
    public class ApprovalActionDto
    {
        public string Comments { get; set; } = string.Empty;
    }
} 