using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SatinAlmaPlatformu.BusinessLogic.DTOs.PurchaseRequest;
using SatinAlmaPlatformu.BusinessLogic.Services.Interfaces;
using SatinAlmaPlatformu.Core.Constants;
using SatinAlmaPlatformu.Core.ResultPattern;
using System.Security.Claims;

namespace SatinAlmaPlatformu.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class PurchaseRequestController : ControllerBase
    {
        private readonly IPurchaseRequestService _purchaseRequestService;
        private readonly IPurchaseRequestApprovalService _purchaseRequestApprovalService;
        private readonly ILogger<PurchaseRequestController> _logger;

        public PurchaseRequestController(
            IPurchaseRequestService purchaseRequestService,
            IPurchaseRequestApprovalService purchaseRequestApprovalService,
            ILogger<PurchaseRequestController> logger)
        {
            _purchaseRequestService = purchaseRequestService;
            _purchaseRequestApprovalService = purchaseRequestApprovalService;
            _logger = logger;
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<PurchaseRequestDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _purchaseRequestService.GetAllRequestsAsync(userId);
            return Ok(result);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(PurchaseRequestDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _purchaseRequestService.GetRequestByIdAsync(id);
            if (!result.Success)
            {
                return NotFound(result);
            }
            return Ok(result);
        }

        [HttpPost]
        [ProducesResponseType(typeof(Result<int>), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([FromBody] CreatePurchaseRequestDto request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            request.RequestedById = userId;

            var result = await _purchaseRequestService.CreateRequestAsync(request);
            if (!result.Success)
            {
                return BadRequest(result);
            }

            // Automatically start approval flow upon successful creation
            var approvalResult = await _purchaseRequestApprovalService.InitiateApprovalFlowAsync(result.Data);
            if (!approvalResult.Success)
            {
                _logger.LogWarning("Purchase request created but approval flow failed to start. RequestId: {RequestId}, Error: {Error}", 
                    result.Data, approvalResult.Message);
            }

            return CreatedAtAction(nameof(GetById), new { id = result.Data }, result);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(Result), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update(int id, [FromBody] UpdatePurchaseRequestDto request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _purchaseRequestService.UpdateRequestAsync(id, request, userId);
            
            if (!result.Success)
            {
                if (result.Error == ErrorType.NotFound)
                {
                    return NotFound(result);
                }
                return BadRequest(result);
            }

            return Ok(result);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(Result), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _purchaseRequestService.DeleteRequestAsync(id, userId);
            
            if (!result.Success)
            {
                if (result.Error == ErrorType.NotFound)
                {
                    return NotFound(result);
                }
                return BadRequest(result);
            }

            return Ok(result);
        }

        /// <summary>
        /// Talep için revizyon ister
        /// </summary>
        /// <param name="id">Talep ID</param>
        /// <param name="revisionDto">Revizyon isteği bilgileri</param>
        /// <returns>İşlem sonucu</returns>
        [HttpPost("{id}/request-revision")]
        [ProducesResponseType(typeof(Result), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> RequestRevision(int id, [FromBody] RevisionRequestDto revisionDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _purchaseRequestService.RequestRevisionAsync(id, revisionDto, userId);
            
            if (!result.Success)
            {
                if (result.Error == ErrorType.NotFound)
                {
                    return NotFound(result);
                }
                return BadRequest(result);
            }

            return Ok(result);
        }

        /// <summary>
        /// Revizyon isteğine yanıt verir
        /// </summary>
        /// <param name="id">Talep ID</param>
        /// <param name="responseDto">Revizyon yanıtı bilgileri</param>
        /// <returns>İşlem sonucu</returns>
        [HttpPost("{id}/respond-to-revision")]
        [ProducesResponseType(typeof(Result), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> RespondToRevision(int id, [FromBody] RevisionResponseDto responseDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _purchaseRequestService.RespondToRevisionAsync(id, responseDto, userId);
            
            if (!result.Success)
            {
                if (result.Error == ErrorType.NotFound)
                {
                    return NotFound(result);
                }
                return BadRequest(result);
            }

            return Ok(result);
        }
    }
} 