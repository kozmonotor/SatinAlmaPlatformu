# Satın Alma Platformu - İletişim Planı

## 1. İletişim Kanalları

### 1.1. Düzenli Toplantılar
| Toplantı Türü           | Sıklık           | Süre      | Katılımcılar                                 | Amaç                                                      |
|-------------------------|------------------|-----------|----------------------------------------------|-----------------------------------------------------------|
| Sprint Planlama         | 2 haftada bir    | 2 saat    | Tüm ekip + Ürün Sahibi                       | Gelecek sprintte yapılacak işlerin belirlenmesi           |
| Günlük Stand-up         | Hergün           | 15 dakika | Tüm geliştirme ekibi                         | Günlük ilerleme durumu ve engellerin belirlenmesi        |
| Sprint Review           | 2 haftada bir    | 1 saat    | Tüm ekip + Ürün Sahibi + Paydaşlar           | Tamamlanan işlerin demosunun yapılması                    |
| Sprint Retrospektif     | 2 haftada bir    | 1 saat    | Tüm ekip                                     | Süreç iyileştirme ve geri bildirim                        |
| Teknik Tasarım Toplantısı| Gerektiğinde    | 1-2 saat  | İlgili geliştiriciler + Teknik Lider         | Karmaşık özelliklerin teknik tasarımını yapmak           |
| Paydaş Bilgilendirme    | Ayda bir         | 1 saat    | Proje Yöneticisi + Üst Düzey Paydaşlar       | Proje ilerlemesini paylaşmak                             |

### 1.2. İletişim Araçları
| Araç                   | Kullanım Amacı                                                        | Hedef Kitle              |
|------------------------|-----------------------------------------------------------------------|--------------------------|
| GitHub Projects        | Görev takibi, sprint planlaması, roadmap                              | Tüm ekip                 |
| GitHub Issues          | Özellik geliştirme, bug takibi, detaylı görev tanımları              | Tüm ekip                 |
| GitHub Pull Requests   | Kod incelemeleri ve onayları                                          | Geliştiriciler           |
| Microsoft Teams        | Anlık mesajlaşma, görüntülü toplantılar, ekran paylaşımı              | Tüm ekip + Paydaşlar     |
| E-posta                | Resmi iletişim, dış paydaşlarla iletişim, belge paylaşımı             | Tüm ekip + Paydaşlar     |
| Ortak Belge Depolama   | Doküman paylaşımı, ortak çalışma (Mimari dökümanlar, gereksinimler)   | Tüm ekip + Paydaşlar     |
| Wiki Sayfaları         | Teknik dokümantasyon, kullanım kılavuzları, standartlar               | Tüm ekip                 |

## 2. İletişim Rolleri ve Sorumlulukları

### 2.1. Proje Yöneticisi
- Tüm proje iletişiminin koordinasyonu
- Paydaş toplantılarının düzenlenmesi
- İlerleme raporlarının hazırlanması
- Risklerin ve sorunların eskalasyonu
- Sprint toplantılarının organizasyonu

### 2.2. Teknik Lider
- Teknik tasarım toplantılarının yönetilmesi
- Mimari kararların dokümantasyonu
- Teknik standartların ve yönergelerin oluşturulması
- Kod inceleme sürecinin gözetimi
- Teknik engellerin çözümünün koordinasyonu

### 2.3. Takım Üyeleri
- Günlük stand-up toplantılarına katılım
- GitHub Issues üzerinde görev güncellemeleri
- Engellerin zamanında raporlanması
- Kod incelemelerine katılım
- Dokümantasyona katkıda bulunma

### 2.4. Ürün Sahibi
- Gereksinimlerin netleştirilmesi
- Sprint planlama ve review toplantılarına katılım
- Önceliklerin belirlenmesi
- Paydaşlar ile ürün vizyonunun paylaşılması

## 3. Raporlama ve Dokümantasyon

### 3.1. Düzenli Raporlar
| Rapor Türü                | Sıklık       | Sorumlu           | Dağıtım                          | İçerik                                             |
|---------------------------|--------------|-------------------|----------------------------------|---------------------------------------------------|
| Sprint İlerleme Raporu    | Haftalık     | Proje Yöneticisi  | Tüm ekip + Ürün Sahibi           | Burndown chart, tamamlanan/açık görevler          |
| Proje Durum Raporu        | Aylık        | Proje Yöneticisi  | Üst Düzey Paydaşlar              | Genel ilerleme, riskler, milestonelar             |
| Kalite Metrikleri Raporu  | 2 haftada bir| Teknik Lider      | Tüm geliştirme ekibi             | Kod kalitesi, test kapsamı, hata oranları          |

### 3.2. Dokümantasyon Standartları
- Tüm teknik tasarım dokümanları GitHub Wiki'de saklanacaktır
- API dokümantasyonu Swagger/OpenAPI ile otomatik oluşturulacaktır
- Geliştirici dokümantasyonu kod içi açıklamalar ve README dosyaları ile sağlanacaktır
- Kullanıcı kılavuzları Markdown formatında hazırlanacaktır

## 4. Karar Alma Süreci

### 4.1. Teknik Kararlar
- Küçük teknik kararlar: İlgili geliştiriciler tarafından alınır
- Orta düzey teknik kararlar: Teknik tasarım toplantılarında tartışılır ve teknik lider onayı ile kesinleşir
- Büyük mimari kararlar: Tüm ekibin katıldığı mimari toplantılarda alınır ve dokümante edilir

### 4.2. Kapsam ve Öncelik Kararları
- Ürün Sahibi, önceliklendirme ve kapsam kararlarından sorumludur
- Teknik fizibilite ve efor tahminleri geliştirme ekibi tarafından sağlanır
- Önemli kapsam değişiklikleri için resmi değişiklik talebi süreci işletilir

## 5. Sorun ve Çatışma Çözümü

### 5.1. Sorun Eskalasyon Süreci
1. Sorunlar öncelikle ekip içinde çözülmeye çalışılır
2. Çözülemezse, teknik lider veya proje yöneticisine eskalasyon yapılır
3. Teknik lider/proje yöneticisi düzeyinde çözülemezse, ürün sahibine eskalasyon yapılır
4. Gerekirse üst düzey paydaşların katılımıyla karar toplantısı düzenlenir

### 5.2. Çatışma Çözümü Yaklaşımı
- Veriye dayalı tartışma teşvik edilir
- Farklı alternatifler objektif kriterlerle değerlendirilir
- Uzlaşma sağlanamadığında sorumlu rol (teknik lider/proje yöneticisi/ürün sahibi) nihai kararı verir

## 6. İletişim Etkinliği Ölçümü

### 6.1. İletişim Başarı Kriterleri
- Toplantıların zamanında başlaması ve belirlenen süre içinde tamamlanması
- Stand-up toplantılarında tüm ekip üyelerinin aktif katılımı
- Proje dokümantasyonunun güncel tutulması
- Engellerin zamanında raporlanması ve çözülmesi

### 6.2. İyileştirme Süreci
- Her sprint retrospektifinde iletişim etkinliği değerlendirilecek
- Anketler yoluyla ekip üyelerinden geri bildirim alınacak
- İletişim planı, geri bildirimlere göre düzenli olarak güncellenecek 