using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SatinAlmaPlatformu.BusinessLogic.DTOs.Approval;
using SatinAlmaPlatformu.BusinessLogic.Services.Interfaces;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SatinAlmaPlatformu.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ApprovalController : ControllerBase
    {
        private readonly IPurchaseRequestApprovalService _approvalService;

        public ApprovalController(IPurchaseRequestApprovalService approvalService)
        {
            _approvalService = approvalService;
        }

        /// <summary>
        /// Retrieves the approval history for a purchase request
        /// </summary>
        /// <param name="purchaseRequestId">ID of the purchase request</param>
        /// <returns>List of approval history items</returns>
        [HttpGet("history/{purchaseRequestId}")]
        public async Task<ActionResult<List<ApprovalHistoryDto>>> GetApprovalHistory(int purchaseRequestId)
        {
            var result = await _approvalService.GetApprovalHistory(purchaseRequestId);
            
            if (!result.Succeeded)
                return BadRequest(result.Errors);
                
            return Ok(result.Data);
        }

        /// <summary>
        /// Retrieves the current approval status for a purchase request
        /// </summary>
        /// <param name="purchaseRequestId">ID of the purchase request</param>
        /// <returns>Approval status details</returns>
        [HttpGet("status/{purchaseRequestId}")]
        public async Task<ActionResult<ApprovalStatusDto>> GetApprovalStatus(int purchaseRequestId)
        {
            var result = await _approvalService.GetApprovalStatus(purchaseRequestId);
            
            if (!result.Succeeded)
                return BadRequest(result.Errors);
                
            return Ok(result.Data);
        }

        /// <summary>
        /// Retrieves all pending approvals for the current user
        /// </summary>
        /// <returns>List of pending approval requests</returns>
        [HttpGet("pending")]
        public async Task<ActionResult<List<PendingApprovalDto>>> GetPendingApprovals()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _approvalService.GetPendingApprovalsForUser(userId);
            
            if (!result.Succeeded)
                return BadRequest(result.Errors);
                
            return Ok(result.Data);
        }

        /// <summary>
        /// Approves a purchase request at the current step
        /// </summary>
        /// <param name="approvalStepId">ID of the approval step instance</param>
        /// <param name="comments">Optional approval comments</param>
        /// <returns>Result of the approval operation</returns>
        [HttpPost("approve/{approvalStepId}")]
        public async Task<IActionResult> ApproveRequest(int approvalStepId, [FromBody] string comments = null)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _approvalService.ApproveAsync(approvalStepId, userId, comments);
            
            if (!result.Succeeded)
                return BadRequest(result.Errors);
                
            return Ok();
        }

        /// <summary>
        /// Rejects a purchase request at the current step
        /// </summary>
        /// <param name="approvalStepId">ID of the approval step instance</param>
        /// <param name="rejectDto">Rejection details</param>
        /// <returns>Result of the rejection operation</returns>
        [HttpPost("reject/{approvalStepId}")]
        public async Task<IActionResult> RejectRequest(int approvalStepId, [FromBody] RejectRequestDto rejectDto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _approvalService.RejectAsync(approvalStepId, userId, rejectDto.Comments);
            
            if (!result.Succeeded)
                return BadRequest(result.Errors);
                
            return Ok();
        }
    }

    public class RejectRequestDto
    {
        public string Comments { get; set; }
    }
} 