# Satın Alma Platformu - İş Kırılım Yapısı (WBS)

## 1. Proje Yönetimi (PM)
  ### 1.1. Proje Planlama
  - 1.1.1. Proje yönetim aracı seçimi ve kurulumu ✓
  - 1.1.2. İş kırılım yapısı (WBS) oluşturma
  - 1.1.3. Sprint/görev planlaması
  - 1.1.4. İletişim planı oluşturma
  - 1.1.5. Risk analizi ve risk yönetimi planı

  ### 1.2. Gereksinim Analizi
  - 1.2.1. Fonksiyonel gereksinimlerin detaylandırılması
  - 1.2.2. Rol bazlı yetkilendirme matrisinin oluşturulması
  - 1.2.3. Onay süreçleri senaryolarının belirlenmesi
  - 1.2.4. Raporlama gereksinimlerinin detaylandırılması

  ### 1.3. Proje İzleme ve Kontrol
  - 1.3.1. Haftalık durum toplantıları
  - 1.3.2. Sprint retrospektifleri
  - 1.3.3. Kalite güvence ve kod inceleme süreçleri

## 2. Mimari ve Teknik Tasarım (AT)
  ### 2.1. Temel Mimari
  - 2.1.1. Katman sorumluluklarının belirlenmesi ✓
  - 2.1.2. Dependency Injection yapısının oluşturulması ✓
  - 2.1.3. Loglama stratejisinin belirlenmesi
  - 2.1.4. Hata yönetimi stratejisinin belirlenmesi
  - 2.1.5. Object mapping stratejisinin belirlenmesi

  ### 2.2. Veritabanı Tasarımı
  - 2.2.1. Veritabanı şemasının tasarlanması ✓
  - 2.2.2. İlişkilerin ve indekslerin belirlenmesi ✓
  - 2.2.3. Stored Procedure ve View ihtiyaçlarının belirlenmesi
  - 2.2.4. Veritabanı migration stratejisinin belirlenmesi ✓

  ### 2.3. API Tasarımı
  - 2.3.1. API endpoint listesinin oluşturulması
  - 2.3.2. Request/Response yapılarının tasarlanması
  - 2.3.3. Swagger/OpenAPI dokümantasyon hazırlığı
  - 2.3.4. API güvenlik stratejisinin belirlenmesi

  ### 2.4. Diğer Teknik Tasarımlar
  - 2.4.1. Bildirim sistemi mimarisinin tasarlanması ✓
  - 2.4.2. Dosya yönetimi stratejisinin belirlenmesi ✓
  - 2.4.3. Önbellek (cache) stratejisinin belirlenmesi

## 3. Backend Geliştirme (BE)
  ### 3.1. Temel Altyapı
  - 3.1.1. Proje yapısının oluşturulması ✓
  - 3.1.2. Base entity sınıflarının oluşturulması ✓
  - 3.1.3. DbContext ve konfigürasyonların ayarlanması ✓
  - 3.1.4. İlk migration'ın oluşturulması ✓
  - 3.1.5. Loglama mekanizmasının entegrasyonu
  - 3.1.6. Hata yönetimi middleware'inin eklenmesi
  - 3.1.7. Object mapper konfigürasyonu

  ### 3.2. Kimlik Doğrulama ve Yetkilendirme
  - 3.2.1. Kimlik doğrulama mekanizmasının entegrasyonu
  - 3.2.2. JWT yapılandırması ve token üretimi
  - 3.2.3. Rol bazlı yetkilendirme mekanizmasının implementasyonu
  - 3.2.4. Kullanıcı yönetimi API'lerinin oluşturulması
  - 3.2.5. Rol ve yetki yönetimi API'lerinin oluşturulması

  ### 3.3. Talep Yönetimi
  - 3.3.1. Talep oluşturma/listeleme API'lerinin implementasyonu
  - 3.3.2. Talep güncelleme/silme API'lerinin implementasyonu
  - 3.3.3. Talep validasyon kurallarının eklenmesi
  - 3.3.4. Talep durumu yönetiminin eklenmesi

  ### 3.4. Onay Süreçleri
  - 3.4.1. Onay akışı mekanizmasının implementasyonu ✓
  - 3.4.2. Onay/red API'lerinin oluşturulması ✓
  - 3.4.3. Revizyon isteme mekanizmasının implementasyonu ✓
  - 3.4.4. Bildirim tetikleme mekanizmasının implementasyonu ✓

  ### 3.5. Tedarikçi Teklif Yönetimi
  - 3.5.1. Tedarikçi yönetimi API'lerinin implementasyonu
  - 3.5.2. Teklif isteği oluşturma mekanizmasının implementasyonu
  - 3.5.3. Teklif gönderme/listeleme API'lerinin implementasyonu
  - 3.5.4. Teklif kabul/red API'lerinin implementasyonu

  ### 3.6. Sipariş ve Takip
  - 3.6.1. Otomatik sipariş oluşturma mekanizmasının implementasyonu
  - 3.6.2. Manuel sipariş oluşturma API'sinin implementasyonu
  - 3.6.3. Sipariş listeleme/detay API'lerinin implementasyonu
  - 3.6.4. Sipariş durumu güncelleme API'sinin implementasyonu

  ### 3.7. Bildirim Sistemi
  - 3.7.1. E-posta gönderme servisinin implementasyonu ✓
  - 3.7.2. SignalR hub'ının oluşturulması ✓
  - 3.7.3. Bildirim tercihleri API'lerinin implementasyonu
  - 3.7.4. Anlık bildirim mekanizmalarının entegrasyonu ✓

  ### 3.8. Dosya Yönetimi
  - 3.8.1. Dosya yükleme/indirme API'lerinin implementasyonu
  - 3.8.2. Dosya kaydetme stratejisinin implementasyonu
  - 3.8.3. Dosya listeleme API'sinin implementasyonu

  ### 3.9. Raporlama ve Analiz
  - 3.9.1. Departman/kategori bazlı harcama raporlarının implementasyonu
  - 3.9.2. Onay süresi analiz raporlarının implementasyonu
  - 3.9.3. Talep/sipariş sayıları raporlarının implementasyonu
  - 3.9.4. Maliyet tasarrufu metrikleri raporlarının implementasyonu

## 4. Frontend Geliştirme (FE)
  ### 4.1. Temel Yapı
  - 4.1.1. Frontend framework/kütüphane seçimi ve kurulumu
  - 4.1.2. UI kütüphanesi entegrasyonu
  - 4.1.3. Proje yapısının oluşturulması
  - 4.1.4. API istemcisinin konfigürasyonu
  - 4.1.5. State management çözümünün entegrasyonu
  - 4.1.6. Routing mekanizmasının kurulumu

  ### 4.2. Temel UI Bileşenleri
  - 4.2.1. Login sayfası implementasyonu
  - 4.2.2. Ana layout implementasyonu
  - 4.2.3. Rol bazlı navigasyon yapısının oluşturulması
  - 4.2.4. Bildirim gösterim alanının implementasyonu
  - 4.2.5. Hata sayfalarının implementasyonu

  ### 4.3. Rol Bazlı Ekranlar
  - 4.3.1. Normal Kullanıcı ekranlarının implementasyonu
  - 4.3.2. Satın Alma Personeli ekranlarının implementasyonu
  - 4.3.3. Yönetici ekranlarının implementasyonu
  - 4.3.4. Tedarikçi ekranlarının implementasyonu
  - 4.3.5. Sistem Yöneticisi ekranlarının implementasyonu
  - 4.3.6. Raporlama ekranlarının implementasyonu

  ### 4.4. Genel Frontend İşlevleri
  - 4.4.1. Form validasyonlarının implementasyonu
  - 4.4.2. API çağrılarının entegrasyonu
  - 4.4.3. Loading göstergelerinin eklenmesi
  - 4.4.4. Hata mesajları gösterim mekanizmasının implementasyonu
  - 4.4.5. Bildirim gösterim mekanizmasının entegrasyonu
  - 4.4.6. Dosya yükleme bileşenlerinin entegrasyonu
  - 4.4.7. Responsive tasarım kontrolü

## 5. Test ve Kalite Güvence (TQ)
  ### 5.1. Backend Testleri
  - 5.1.1. BLL unit testlerinin yazılması
  - 5.1.2. API entegrasyon testlerinin yazılması
  - 5.1.3. DB etkileşim testlerinin yapılması

  ### 5.2. Frontend Testleri
  - 5.2.1. Component testlerinin yazılması
  - 5.2.2. End-to-End (E2E) testlerinin yazılması
  - 5.2.3. Farklı tarayıcı/cihaz testlerinin yapılması

  ### 5.3. Manuel Testler
  - 5.3.1. Rol bazlı manuel test senaryolarının oluşturulması
  - 5.3.2. Manuel testlerin yürütülmesi
  - 5.3.3. UAT testlerinin organize edilmesi

  ### 5.4. Güvenlik Testleri
  - 5.4.1. OWASP Top 10 kontrollerinin yapılması
  - 5.4.2. Yetkilendirme testlerinin yapılması
  - 5.4.3. API güvenlik testlerinin yapılması

  ### 5.5. Performans Testleri
  - 5.5.1. API yük testlerinin yapılması
  - 5.5.2. DB performans testlerinin yapılması
  - 5.5.3. Frontend performans testlerinin yapılması

## 6. Deployment ve Devreye Alma (DD)
  ### 6.1. Deployment Hazırlıkları
  - 6.1.1. Test ve production ortamlarının hazırlanması
  - 6.1.2. Ortam değişkenlerinin yapılandırılması
  - 6.1.3. Deployment scriptlerinin oluşturulması
  - 6.1.4. Veritabanı yedekleme stratejisinin uygulanması
  - 6.1.5. Monitoring araçlarının yapılandırılması

  ### 6.2. Deployment
  - 6.2.1. Staging ortamına deploy ve testler
  - 6.2.2. Production veritabanı hazırlığı
  - 6.2.3. Production ortamına deploy
  - 6.2.4. Post-deployment kontrolleri

  ### 6.3. Dokümantasyon
  - 6.3.1. API dokümantasyonunun tamamlanması
  - 6.3.2. Teknik dokümantasyonun hazırlanması
  - 6.3.3. Kullanıcı kılavuzlarının hazırlanması
  - 6.3.4. Test raporlarının hazırlanması

## 7. Devam Eden Operasyonlar ve Bakım (OM)
  ### 7.1. Sistem İzleme
  - 7.1.1. Performans izleme
  - 7.1.2. Hata izleme
  - 7.1.3. Güvenlik izleme

  ### 7.2. Kullanıcı Desteği
  - 7.2.1. Kullanıcı geri bildirim mekanizmasının kurulması
  - 7.2.2. Destek talebi sistemi entegrasyonu (opsiyonel)
  - 7.2.3. Kullanıcı eğitimlerinin hazırlanması

  ### 7.3. Bakım ve Geliştirmeler
  - 7.3.1. Periyodik bakım planının oluşturulması
  - 7.3.2. Hotfix süreçlerinin tanımlanması
  - 7.3.3. Gelecek geliştirmeler backlog'unun oluşturulması 