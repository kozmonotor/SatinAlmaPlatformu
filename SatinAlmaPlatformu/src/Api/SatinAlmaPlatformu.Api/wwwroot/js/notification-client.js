/**
 * Satın Alma Platformu - Bildirim Entegrasyonu
 * Bu dosya, gerçek zamanlı bildirimler için SignalR hub bağlantısını yönetir
 */

class NotificationClient {
    constructor() {
        this.hubConnection = null;
        this.reconnectInterval = 5000; // 5 saniye
        this.maxReconnectAttempts = 10;
        this.reconnectAttempts = 0;
        this.callbacks = {
            onNotificationReceived: null,
            onConnected: null,
            onDisconnected: null,
            onReconnecting: null,
            onError: null
        };
    }

    /**
     * SignalR hub bağlantısını başlatır
     * @param {string} userId - Bağlantı kurulacak kullanıcı ID
     */
    initialize(userId) {
        // SignalR hub bağlantısını oluştur
        this.hubConnection = new signalR.HubConnectionBuilder()
            .withUrl("/notificationHub")
            .withAutomaticReconnect([0, 2000, 5000, 10000, 15000, 30000]) // Otomatik tekrar bağlanma stratejisi
            .configureLogging(signalR.LogLevel.Information)
            .build();

        // "ReceiveNotification" olayını dinle
        this.hubConnection.on("ReceiveNotification", (notification) => {
            console.log("Bildirim alındı:", notification);
            if (this.callbacks.onNotificationReceived) {
                this.callbacks.onNotificationReceived(notification);
            }
        });

        // Bağlantı başarılı olduğunda
        this.hubConnection.onreconnected((connectionId) => {
            console.log("SignalR hub'a yeniden bağlandı. ConnectionId:", connectionId);
            this.reconnectAttempts = 0;
            
            // Kullanıcı grubuna katıl
            this.joinUserGroup(userId);
            
            if (this.callbacks.onConnected) {
                this.callbacks.onConnected(connectionId);
            }
        });

        // Bağlantı koptuğunda
        this.hubConnection.onclose((error) => {
            console.log("SignalR hub bağlantısı kapandı", error);
            
            if (this.callbacks.onDisconnected) {
                this.callbacks.onDisconnected(error);
            }
        });

        // Bağlantı yeniden kurulmaya çalışıldığında
        this.hubConnection.onreconnecting((error) => {
            console.log("SignalR hub'a yeniden bağlanmaya çalışılıyor", error);
            
            if (this.callbacks.onReconnecting) {
                this.callbacks.onReconnecting(error);
            }
        });

        // Bağlantıyı başlat
        this.start(userId);
    }

    /**
     * Hub bağlantısını başlatır
     * @param {string} userId - Bağlantı kurulacak kullanıcı ID
     */
    async start(userId) {
        try {
            await this.hubConnection.start();
            console.log("SignalR hub'a bağlandı.");
            this.reconnectAttempts = 0;
            
            // Kullanıcı grubuna katıl
            this.joinUserGroup(userId);
            
            if (this.callbacks.onConnected) {
                this.callbacks.onConnected();
            }
        } catch (err) {
            console.error("SignalR hub bağlantısı başlatılamadı:", err);
            
            if (this.callbacks.onError) {
                this.callbacks.onError(err);
            }
            
            // Bağlantı başarısız olduysa, yeniden deneme
            this.reconnectAttempts++;
            if (this.reconnectAttempts < this.maxReconnectAttempts) {
                setTimeout(() => this.start(userId), this.reconnectInterval);
            }
        }
    }

    /**
     * Kullanıcı grubuna katılır (kullanıcıya özel bildirimler için)
     * @param {string} userId - Katılmak istenen kullanıcı ID
     */
    async joinUserGroup(userId) {
        try {
            await this.hubConnection.invoke("JoinUserGroup", userId);
            console.log(`'${userId}' kullanıcı grubuna katıldı.`);
        } catch (err) {
            console.error("Kullanıcı grubuna katılırken hata:", err);
            
            if (this.callbacks.onError) {
                this.callbacks.onError(err);
            }
        }
    }

    /**
     * Hub bağlantısını kapatır
     */
    async disconnect() {
        if (this.hubConnection) {
            try {
                await this.hubConnection.stop();
                console.log("SignalR hub bağlantısı kapatıldı.");
            } catch (err) {
                console.error("SignalR hub bağlantısı kapatılırken hata:", err);
            }
        }
    }

    /**
     * Bildirim geldiğinde çalışacak fonksiyonu ayarlar
     * @param {function} callback - Bildirim alındığında çalışacak fonksiyon
     */
    onNotificationReceived(callback) {
        this.callbacks.onNotificationReceived = callback;
    }

    /**
     * Bağlantı başarılı olduğunda çalışacak fonksiyonu ayarlar
     * @param {function} callback - Bağlantı kurulduğunda çalışacak fonksiyon
     */
    onConnected(callback) {
        this.callbacks.onConnected = callback;
    }

    /**
     * Bağlantı koptuğunda çalışacak fonksiyonu ayarlar
     * @param {function} callback - Bağlantı koptuğunda çalışacak fonksiyon
     */
    onDisconnected(callback) {
        this.callbacks.onDisconnected = callback;
    }

    /**
     * Yeniden bağlanma girişimi sırasında çalışacak fonksiyonu ayarlar
     * @param {function} callback - Yeniden bağlanma girişiminde çalışacak fonksiyon
     */
    onReconnecting(callback) {
        this.callbacks.onReconnecting = callback;
    }

    /**
     * Hata durumunda çalışacak fonksiyonu ayarlar
     * @param {function} callback - Hata oluştuğunda çalışacak fonksiyon
     */
    onError(callback) {
        this.callbacks.onError = callback;
    }
}

// Global nesne olarak dışa aktar
window.notificationClient = new NotificationClient(); 