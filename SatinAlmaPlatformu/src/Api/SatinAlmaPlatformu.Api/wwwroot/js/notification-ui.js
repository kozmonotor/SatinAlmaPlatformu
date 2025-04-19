/**
 * Satın Alma Platformu - Bildirim UI Entegrasyonu
 * Bu dosya, bildirim gösterimlerini ve etkileşimlerini yönetir
 */

class NotificationUI {
    constructor() {
        this.unreadCount = 0;
        this.notifications = [];
        this.notificationContainer = null;
        this.countBadge = null;
        this.notificationList = null;
        this.toastContainer = null;
    }

    /**
     * UI elemanlarını başlatır
     * @param {Object} config - Konfigürasyon nesnesi
     * @param {string} config.countBadgeSelector - Okunmamış bildirim sayısını gösteren etiketin seçicisi
     * @param {string} config.notificationContainerSelector - Bildirim açılır kutusunun seçicisi
     * @param {string} config.notificationListSelector - Bildirim listesinin seçicisi
     * @param {string} config.toastContainerSelector - Toast bildirimlerinin gösterileceği konteynırın seçicisi
     */
    initialize(config) {
        // UI elemanlarını bul
        this.countBadge = document.querySelector(config.countBadgeSelector);
        this.notificationContainer = document.querySelector(config.notificationContainerSelector);
        this.notificationList = document.querySelector(config.notificationListSelector);
        
        // Toast container oluştur
        if (!document.querySelector(config.toastContainerSelector)) {
            this.toastContainer = document.createElement('div');
            this.toastContainer.id = config.toastContainerSelector.replace('#', '');
            this.toastContainer.className = 'toast-container position-fixed top-0 end-0 p-3';
            document.body.appendChild(this.toastContainer);
        } else {
            this.toastContainer = document.querySelector(config.toastContainerSelector);
        }

        // Okunmamış bildirimleri yükle
        this.loadUnreadNotifications();

        // NotificationClient bağlantılarını dinle
        if (window.notificationClient) {
            window.notificationClient.onNotificationReceived(notification => {
                this.handleNewNotification(notification);
            });
        } else {
            console.error("NotificationClient bulunamadı! notification-client.js dosyasının yüklendiğinden emin olun.");
        }
    }

    /**
     * Okunmamış bildirimleri API'den yükler
     */
    async loadUnreadNotifications() {
        try {
            const response = await fetch('/api/notifications/unread');
            const result = await response.json();
            
            if (result.isSuccess) {
                this.notifications = result.data;
                this.updateNotificationCount();
                this.renderNotificationList();
            } else {
                console.error("Bildirimler yüklenirken hata:", result.message);
            }
        } catch (error) {
            console.error("Bildirimler yüklenirken hata:", error);
        }
    }

    /**
     * Bildirim sayacını günceller
     */
    updateNotificationCount() {
        this.unreadCount = this.notifications.filter(n => !n.isRead).length;
        
        if (this.countBadge) {
            this.countBadge.textContent = this.unreadCount > 0 ? this.unreadCount : '';
            this.countBadge.style.display = this.unreadCount > 0 ? 'block' : 'none';
        }
    }

    /**
     * Bildirim listesini render eder
     */
    renderNotificationList() {
        if (!this.notificationList) return;
        
        // Listeyi temizle
        this.notificationList.innerHTML = '';
        
        // Bildirim yoksa boş mesaj göster
        if (this.notifications.length === 0) {
            const emptyMessage = document.createElement('div');
            emptyMessage.className = 'text-center py-3';
            emptyMessage.textContent = 'Bildiriminiz bulunmamaktadır.';
            this.notificationList.appendChild(emptyMessage);
            return;
        }
        
        // Bildirimleri ekle
        this.notifications.slice(0, 5).forEach(notification => {
            const notificationItem = this.createNotificationElement(notification);
            this.notificationList.appendChild(notificationItem);
        });
        
        // Tümünü gör bağlantısı
        if (this.notifications.length > 5) {
            const viewAllLink = document.createElement('div');
            viewAllLink.className = 'text-center border-top py-2';
            viewAllLink.innerHTML = '<a href="/notifications" class="text-decoration-none">Tüm bildirimleri gör</a>';
            this.notificationList.appendChild(viewAllLink);
        }
    }

    /**
     * Tek bir bildirim elementi oluşturur
     * @param {Object} notification - Bildirim nesnesi
     * @returns {HTMLElement} Bildirim elementi
     */
    createNotificationElement(notification) {
        const item = document.createElement('div');
        item.className = `notification-item p-2 border-bottom ${notification.isRead ? '' : 'bg-light'}`;
        item.dataset.id = notification.id;
        
        // Tip sınıfını belirle
        let typeClass = 'text-info';
        switch(notification.type) {
            case 'Success':
            case 'RequestApproved':
                typeClass = 'text-success';
                break;
            case 'Warning':
            case 'RevisionRequest':
                typeClass = 'text-warning';
                break;
            case 'Error':
            case 'RequestRejected':
                typeClass = 'text-danger';
                break;
            default:
                typeClass = 'text-info';
        }
        
        // Tarih formatla
        const date = new Date(notification.createdAt);
        const formattedDate = `${date.toLocaleDateString()} ${date.toLocaleTimeString([], { hour: '2-digit', minute: '2-digit' })}`;
        
        // İçerik
        item.innerHTML = `
            <div class="d-flex">
                <div class="flex-shrink-0">
                    <i class="bi bi-bell-fill ${typeClass} fs-5 me-2"></i>
                </div>
                <div class="flex-grow-1 ms-2">
                    <h6 class="mb-0 fw-bold">${notification.title}</h6>
                    <p class="mb-0 small">${notification.message}</p>
                    <small class="text-muted">${formattedDate}</small>
                </div>
                <div class="ms-auto">
                    ${!notification.isRead ? '<button class="btn btn-sm mark-read" title="Okundu olarak işaretle"><i class="bi bi-check"></i></button>' : ''}
                </div>
            </div>
        `;
        
        // Tıklama olayı
        item.addEventListener('click', (e) => {
            // "Okundu olarak işaretle" düğmesine tıklandıysa
            if (e.target.closest('.mark-read')) {
                e.stopPropagation();
                this.markAsRead(notification.id);
                return;
            }
            
            // Eğer actionUrl varsa, o sayfaya yönlendir
            if (notification.actionUrl) {
                window.location.href = notification.actionUrl;
            }
            
            // Okunmamışsa, okundu olarak işaretle
            if (!notification.isRead) {
                this.markAsRead(notification.id);
            }
        });
        
        return item;
    }

    /**
     * Bildirimi okundu olarak işaretler
     * @param {number} notificationId - Bildirim ID'si
     */
    async markAsRead(notificationId) {
        try {
            const response = await fetch(`/api/notifications/${notificationId}/read`, {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                }
            });
            
            const result = await response.json();
            
            if (result.isSuccess) {
                // Bildirim durumunu güncelle
                const notification = this.notifications.find(n => n.id === notificationId);
                if (notification) {
                    notification.isRead = true;
                    this.updateNotificationCount();
                    this.renderNotificationList();
                }
            } else {
                console.error("Bildirim okundu olarak işaretlenirken hata:", result.message);
            }
        } catch (error) {
            console.error("Bildirim okundu olarak işaretlenirken hata:", error);
        }
    }

    /**
     * Tüm bildirimleri okundu olarak işaretler
     */
    async markAllAsRead() {
        const unreadIds = this.notifications.filter(n => !n.isRead).map(n => n.id);
        
        if (unreadIds.length === 0) return;
        
        try {
            const response = await fetch('/api/notifications/mark-all-read', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify(unreadIds)
            });
            
            const result = await response.json();
            
            if (result.isSuccess) {
                // Tüm bildirimleri okundu olarak işaretle
                this.notifications.forEach(n => n.isRead = true);
                this.updateNotificationCount();
                this.renderNotificationList();
            } else {
                console.error("Bildirimler okundu olarak işaretlenirken hata:", result.message);
            }
        } catch (error) {
            console.error("Bildirimler okundu olarak işaretlenirken hata:", error);
        }
    }

    /**
     * Yeni gelen bildirimi işler
     * @param {Object} notification - Yeni gelen bildirim
     */
    handleNewNotification(notification) {
        // Bildirimi listeye ekle
        this.notifications.unshift(notification);
        
        // UI'ı güncelle
        this.updateNotificationCount();
        this.renderNotificationList();
        
        // Toast bildirimi göster
        this.showToast(notification);
    }

    /**
     * Toast bildirimi gösterir
     * @param {Object} notification - Gösterilecek bildirim
     */
    showToast(notification) {
        if (!this.toastContainer) return;
        
        // Toast tipini belirle
        let bgClass = 'bg-info';
        switch(notification.type) {
            case 'Success':
            case 'RequestApproved':
                bgClass = 'bg-success';
                break;
            case 'Warning':
            case 'RevisionRequest':
                bgClass = 'bg-warning';
                break;
            case 'Error':
            case 'RequestRejected':
                bgClass = 'bg-danger';
                break;
            default:
                bgClass = 'bg-info';
        }
        
        // Toast elementi oluştur
        const toastId = `toast-${Date.now()}`;
        const toastHtml = `
            <div id="${toastId}" class="toast text-white ${bgClass}" role="alert" aria-live="assertive" aria-atomic="true">
                <div class="toast-header">
                    <strong class="me-auto">${notification.title}</strong>
                    <small>${new Date(notification.createdAt).toLocaleTimeString([], { hour: '2-digit', minute: '2-digit' })}</small>
                    <button type="button" class="btn-close" data-bs-dismiss="toast" aria-label="Kapat"></button>
                </div>
                <div class="toast-body">
                    ${notification.message}
                </div>
            </div>
        `;
        
        // Toast'u ekle
        this.toastContainer.insertAdjacentHTML('beforeend', toastHtml);
        
        // Toast'u göster
        const toastEl = document.getElementById(toastId);
        const toast = new bootstrap.Toast(toastEl, {
            autohide: true,
            delay: 5000
        });
        
        toast.show();
        
        // Toast tıklamasını dinle
        toastEl.addEventListener('click', (e) => {
            // Kapat düğmesine tıklandıysa işlem yapma
            if (e.target.classList.contains('btn-close')) return;
            
            // Eğer actionUrl varsa, o sayfaya yönlendir
            if (notification.actionUrl) {
                window.location.href = notification.actionUrl;
            }
            
            // Toast'u gizle
            toast.hide();
        });
    }
}

// Global nesne olarak dışa aktar
window.notificationUI = new NotificationUI();