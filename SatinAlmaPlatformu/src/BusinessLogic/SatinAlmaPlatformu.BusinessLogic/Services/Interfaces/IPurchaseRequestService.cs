using System.Collections.Generic;
using System.Threading.Tasks;
using SatinAlmaPlatformu.BusinessLogic.DTOs;
using SatinAlmaPlatformu.Core.Models;
using SatinAlmaPlatformu.Core.Results;

namespace SatinAlmaPlatformu.BusinessLogic.Services.Interfaces
{
    /// <summary>
    /// Satın alma taleplerini yöneten servis arayüzü
    /// </summary>
    public interface IPurchaseRequestService
    {
        /// <summary>
        /// Yeni bir satın alma talebi oluşturur (Taslak olarak)
        /// </summary>
        /// <param name="createDto">Oluşturulacak talep bilgileri</param>
        /// <param name="userId">İşlemi yapan kullanıcı ID'si</param>
        /// <returns>Oluşturulan talep bilgilerini içeren Result nesnesi</returns>
        Task<Result<PurchaseRequestDto>> CreateRequestAsync(CreatePurchaseRequestDto createDto, int userId);
        
        /// <summary>
        /// Taslak durumundaki bir talebi onay sürecine gönderir
        /// </summary>
        /// <param name="purchaseRequestId">Satın alma talebi ID'si</param>
        /// <param name="userId">İşlemi yapan kullanıcı ID'si</param>
        /// <returns>İşlem sonucunu içeren Result nesnesi</returns>
        Task<Result> SubmitForApprovalAsync(int purchaseRequestId, int userId);
        
        /// <summary>
        /// ID'ye göre satın alma talebini getirir
        /// </summary>
        /// <param name="purchaseRequestId">Satın alma talebi ID'si</param>
        /// <returns>Talep bilgilerini içeren Result nesnesi</returns>
        Task<Result<PurchaseRequestDto>> GetRequestByIdAsync(int purchaseRequestId);
        
        /// <summary>
        /// Kullanıcıya göre satın alma taleplerini listeler
        /// </summary>
        /// <param name="userId">Kullanıcı ID'si</param>
        /// <returns>Talep listesini içeren Result nesnesi</returns>
        Task<Result<List<PurchaseRequestDto>>> GetAllRequestsAsync(string userId);
        
        /// <summary>
        /// Satın alma talebini günceller
        /// </summary>
        /// <param name="purchaseRequestId">Satın alma talebi ID'si</param>
        /// <param name="updateDto">Güncellenecek talep bilgileri</param>
        /// <param name="userId">İşlemi yapan kullanıcı ID'si</param>
        /// <returns>İşlem sonucunu içeren Result nesnesi</returns>
        Task<Result> UpdateRequestAsync(int purchaseRequestId, UpdatePurchaseRequestDto updateDto, string userId);
        
        /// <summary>
        /// Satın alma talebini siler
        /// </summary>
        /// <param name="purchaseRequestId">Satın alma talebi ID'si</param>
        /// <param name="userId">İşlemi yapan kullanıcı ID'si</param>
        /// <returns>İşlem sonucunu içeren Result nesnesi</returns>
        Task<Result> DeleteRequestAsync(int purchaseRequestId, string userId);
        
        /// <summary>
        /// Bir talep için revizyon ister
        /// </summary>
        /// <param name="purchaseRequestId">Satın alma talebi ID'si</param>
        /// <param name="revisionDto">Revizyon isteği bilgileri</param>
        /// <param name="userId">İşlemi yapan kullanıcı ID'si</param>
        /// <returns>İşlem sonucunu içeren Result nesnesi</returns>
        Task<Result> RequestRevisionAsync(int purchaseRequestId, RevisionRequestDto revisionDto, string userId);
        
        /// <summary>
        /// Revizyon istenen bir talebi günceller ve yanıtlar
        /// </summary>
        /// <param name="purchaseRequestId">Satın alma talebi ID'si</param>
        /// <param name="responseDto">Revizyon yanıtı bilgileri</param>
        /// <param name="userId">İşlemi yapan kullanıcı ID'si</param>
        /// <returns>İşlem sonucunu içeren Result nesnesi</returns>
        Task<Result> RespondToRevisionAsync(int purchaseRequestId, RevisionResponseDto responseDto, string userId);
    }
} 