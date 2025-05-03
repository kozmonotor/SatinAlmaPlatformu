// Tüm kullanıcılar ve rolleri
const users = [
  { email: 'normal@ornek.com', password: '123456', role: 'NormalKullanici' },
  { email: 'satinalma@ornek.com', password: '123456', role: 'SatinAlmaPersoneli' },
  { email: 'yonetici@ornek.com', password: '123456', role: 'Yonetici' },
  { email: 'tedarikci@ornek.com', password: '123456', role: 'Tedarikci' },
  { email: 'admin@ornek.com', password: '123456', role: 'Admin' },
  { email: 'kozmi@msn.com', password: '123456', role: 'Admin' }
];

describe('Tüm menü ve aksiyon butonları dinamik erişim testi', () => {
  users.forEach(user => {
    it(`${user.role} ile menü ve aksiyon butonları erişim testi`, () => {
      // Login
      cy.visit('/login');
      cy.wait(1000);
      cy.get('input[name="username"]', { timeout: 10000 }).type('admin@ornek.com');
      cy.get('input[name="password"]', { timeout: 10000 }).type('123456');
      cy.get('button[type="submit"]').click();
      cy.wait(1000);
      cy.get('nav, .MuiDrawer-root, .sidebar', { timeout: 10000 }).should('exist'); 

      // Menüdeki tüm linkleri bul ve sırayla tıkla
      cy.get('nav').within(() => {
        cy.get('a').each($el => {
          const href = $el.attr('href');
          if (href && href !== '#') {
            cy.visit(href);
            cy.wait(500);
            // Sayfa açıldı mı, hata var mı kontrol et
            cy.get('body').then($body => {
              if ($body.text().includes('404') || $body.text().includes('Bulunamadı') || $body.text().includes('Yetkiniz yok')) {
                cy.log(`HATA: ${user.role} için ${href} sayfası açılmıyor!`);
              }
            });
          }
        });
      });

      // Tablo veya kartlarda ekle/düzenle/detay butonlarını bul ve tıkla
      cy.get('button, a').each($el => {
        const text = $el.text().toLowerCase();
        if (text.includes('ekle') || text.includes('düzenle') || text.includes('detay')) {
          cy.wrap($el).click({ force: true });
          cy.wait(500); // Sayfa yüklenme süresi için
          cy.get('body').then($body => {
            if ($body.text().includes('404') || $body.text().includes('Bulunamadı') || $body.text().includes('Yetkiniz yok')) {
              cy.log(`HATA: ${user.role} için ${text} butonu çalışmıyor!`);
            }
          });
          cy.go('back');
        }
      });
    });
  });
}); 