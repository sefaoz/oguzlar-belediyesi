# Oğuzlar Belediyesi — Modern Kurumsal Web Sitesi Tasarım Promptu (Stitch / AI Design)

**Amaç:** Merzifon Belediyesi (https://merzifon.bel.tr) bilgi mimarisine benzeyen; ancak daha **modern, temiz, hızlı ve erişilebilir** bir belediye web sitesi arayüzü üretmek. Tasarım, **TailwindCSS** uyumlu olacak, bileşenler sistematik bir **UI Kit** yapısıyla gelecektir.

---

## 0) Markalaşma ve Varlıklar
- **Logo:** “Oğuzlar Belediyesi 1964” (turuncu çerçeve, mavi-beyaz zemin). Araca yüklenen logoyu kullan. Üst sol köşede yer alacak.
- **Renk Paleti (Logo ile uyumlu):**
  - Primary (Turuncu): `#F07C00`
  - Secondary (Mavi): `#0078B4`
  - Neutral / Surface: `#FFFFFF`, `#F7F7F7`, `#F2F4F7`, `#0F172A` (metin için koyu gri/lacivert)
  - Accent: `#10B981` (onay/başarı), `#EF4444` (uyarı/hata)
- **Tipografi:** Google Fonts – “Inter” veya “Poppins”
  - Başlık: `font-semibold` – `tracking-tight`
  - Metin: rahat satır yüksekliği `leading-relaxed`
- **İkonografi:** Lucide/Feather stili, minimalist.
- **Köşe & Gölge:** `rounded-2xl` / `shadow-lg` (hover’da yumuşak yükselme)
- **Aydınlık/Karanlık Mod:** Varsayılan aydınlık; opsiyonel karanlık mod (`dark:` sınıfları).

---

## 1) Genel Tasarım İlkeleri
- **Responsive:** Mobile-first; Tailwind breakpoints (`sm`, `md`, `lg`, `xl`, `2xl`).
- **Temizlik:** Geniş beyaz boşluk, kontrastlı başlıklar, net hiyerarşi.
- **Kullanılabilirlik:** Yapışkan (sticky) üst menü, belirgin “E‑Belediye” çağrıları.
- **Erişilebilirlik (a11y):** Kontrast ≥ WCAG AA, odak halkaları (`focus:ring`), alt metin, semantic HTML, klavye ile gezilebilir menüler.
- **Performans:** Lazy-load görseller, optimize SVG ikonlar, font-display swap, minimal LCP.
- **SEO:** Anlamlı başlıklar, meta, OpenGraph, `sitemap.xml`, `robots.txt`, yapılandırılmış veri (Organization, NewsArticle, Event, LocalBusiness).

---

## 2) Bilgi Mimarisi — Sayfalar ve Bölümler
**Toplam çekirdek sayfa:** 16+ (ana + içerik detayları).

### 2.1. Ana Sayfa
- **Hero:** Oğuzlar doğa/ilçe görseli, başlık + kısa açıklama + “E‑Belediye” ve “Duyurular” butonları.
- **Hızlı Erişim Kartları:** Vergi Ödeme, İstek/Şikayet, Nöbetçi Eczane, İhale İlanları, Meclis Kararları, İmar Planları.
- **Duyurular/Haberler Slider:** 3–6 kart, tarih rozetleri.
- **Etkinlik Takvimi Önizleme:** Yaklaşan 3 etkinlik.
- **Başkanın Mesajı (kısa)** + foto.
- **Sosyal Medya ve Bülten Aboneliği.**
- **Footer:** Adres, telefon, e‑posta, harita küçük görsel, KVKK/Künye linkleri.

### 2.2. Belediye Başkanlığı
- Biyografi, Misyon/Vizyon.
- Başkanın Mesajı (metin + opsiyonel video).
- Randevu/İletişim formu.

### 2.3. Kurumsal
- Tarihçe (1964 vurgusu), Teşkilat Şeması (kartlar/şema).
- Belediye Meclisi (üyeler: foto+isim+görev).
- Birimler/Müdürlükler (Fen, Temizlik, Park‑Bahçeler, Kültür‑Sosyal, İmar, Zabıta…).
- Stratejik Plan, Faaliyet Raporları, Bütçe (indirilebilir PDF kartları).

### 2.4. Hizmetler
- Grid kartları (6–12 hizmet). Her kartta ikon, kısa açıklama, “Detay”.
- Alt sayfalarda SSS (Accordion), başvuru şartları, evrak listesi.

### 2.5. Projeler
- **Filtreler:** Devam Eden / Tamamlanan / Planlanan.
- Proje kartı: görsel, başlık, kısa özet, etiketler, ilerleme çubuğu.
- Detay sayfası: amaç, bütçe, takvim, foto galerisi.

### 2.6. Duyurular & Haberler
- Sekmeli filtre: **Duyuru / Haber / Etkinlik**.
- Kart liste + sayfalama, arama çubuğu.
- Detay sayfası: başlık, tarih, kapak görseli, içerik, “İlgili” kartları.

### 2.7. **E‑Belediye (Öncelikli)**
- Büyük, simgeli CTA kartları: Vergi Ödeme, e‑Ruhsat, İmar Durumu Sorgu, Borç Yapılandırma, İstek‑Şikayet, Nöbetçi Eczane, Online Randevu.
- Dış linkler için “external link” ikonu.

### 2.8. İhaleler
- Aktif/Arşiv sekmeleri, arama + filtre (tarih, müdürlük, statü).
- Kart/Tablo görünümü (ilan no, konu, tarih, saat, dosya indir).
- Detay sayfası: şartname indirme butonları.

### 2.9. Meclis Kararları & Encümen Kararları
- Yıla/aya göre filtre, hızlı arama.
- Tablo veya döküman kartları (PDF ikonları).
- Detay sayfası/önizleme.

### 2.10. İmar Planları & Askı İlanları
- Harita placeholder, mahalle/pafta filtreleri.
- Plan paftası listesi, PDF/GİF önizleme.

### 2.11. İnsan Kaynakları (Kariyer)
- Güncel ilanlar listesi, başvuru formu.
- Çalışan hakları, KVKK metinleri.

### 2.12. Galeri
- Foto/Video galerisi; masonry grid, lightbox.
- Etiket bazlı filtre (festival, proje, doğa).

### 2.13. Etkinlik Takvimi
- Aylık/haftalık görünüm (kartlara düşür – takvim benzeri grid).
- Etkinlik detay: tarih/saat/konum (harita gömülü).

### 2.14. Kent Rehberi / Turizm
- Oğuzlar’a dair gezi noktaları, ulaşım, konaklama, acil numaralar.
- Harita ve rotalar (placeholder).

### 2.15. KVKK & Aydınlatma & Çerez Politikası
- Statik sayfalar (doküman şablonları).

### 2.16. İletişim
- Harita gömülü, adres/telefon/e‑posta.
- İletişim formu (ad, soyad, e‑posta, konu, mesaj).
- Çalışma saatleri, sosyal medya linkleri.

> **Opsiyonel Ek Sayfalar:** Basın Odası, Sıkça Sorulan Sorular, Anketler, Şeffaflık (açık veri), Afet Bilgilendirme, Nöbetçi Eczane detay.

---

## 3) Navigasyon & Header
- Sol: Logo. Orta: Ana navigasyon. Sağ: Arama, E‑Belediye CTA, Dil seçici, **Karanlık mod** toggle.
- **Mobil:** Hamburger menü (tam ekran drawer), birincil menüler üstte, E‑Belediye butonu belirgin.
- **Duyuru Bandı:** En üstte kapatılabilir info bar (ör: “Su kesintisi – 12:00‑18:00”).

**Menü Önerisi:**
- Ana Sayfa
- Başkanlık
- Kurumsal
- Hizmetler
- Projeler
- Duyurular
- E‑Belediye
- İhaleler
- Meclis & Encümen
- İmar
- Galeri
- Etkinlikler
- Kent Rehberi
- İletişim

---

## 4) Bileşen Kütüphanesi (TailwindCSS Sınıflarıyla)

> Aşağıdaki sınıflar örnek niteliğinde; Stitch bileşenleri üretirken benzer utility yapısını uygula.

### 4.1. Butonlar
- **Primary:** `inline-flex items-center rounded-xl bg-[#F07C00] px-5 py-3 text-white hover:bg-[#d86f00] focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-[#F07C00] transition`
- **Secondary:** `inline-flex items-center rounded-xl border border-slate-300 bg-white px-5 py-3 text-slate-800 hover:bg-slate-50 focus:ring-2 focus:ring-slate-400`
- **Ghost:** `inline-flex items-center rounded-xl px-4 py-2 hover:bg-slate-100 text-slate-700`

### 4.2. Kart
`rounded-2xl border border-slate-200 bg-white p-6 shadow-sm hover:shadow-md transition`

### 4.3. Sekme (Tabs)
- Konteyner: `flex gap-2 rounded-xl bg-slate-100 p-1`
- Buton: `px-4 py-2 rounded-lg ui-selected:bg-white ui-selected:shadow ui-selected:text-slate-900 text-slate-600`

### 4.4. Form Elemanları
- Input: `w-full rounded-xl border border-slate-300 px-4 py-3 focus:ring-2 focus:ring-[#0078B4] focus:outline-none`
- Label: `mb-1 block text-sm font-medium text-slate-700`
- Hata: `text-sm text-red-600 mt-1`

### 4.5. Uyarı/Info Banner
`rounded-xl border-l-4 p-4 bg-amber-50 border-amber-400 text-amber-800`

### 4.6. Grid ve Listeler
- Kart gridi: `grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-3 gap-6`
- Tablo: `w-full border-separate border-spacing-0 text-sm` + zebra satırları.

### 4.7. Header / Navbar
- Kap: `sticky top-0 z-50 bg-white/80 backdrop-blur border-b border-slate-200`
- İçerik: `mx-auto max-w-7xl px-4 sm:px-6 lg:px-8 h-16 flex items-center justify-between`

### 4.8. Footer
- `bg-slate-900 text-slate-200`
- Linkler: `hover:text-white`
- Sütunlar: `grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-4 gap-8 py-12`

### 4.9. Hero
- Kap: `relative overflow-hidden rounded-3xl bg-gradient-to-br from-[#0078B4] to-[#F07C00] text-white`
- İç: `max-w-7xl mx-auto px-6 py-20 flex flex-col lg:flex-row items-center gap-10`

---

## 5) Arama, Filtreleme, Listeleme
- Üst arama çubuğu: `rounded-xl bg-slate-100 px-4 py-3 focus-within:ring-2`.
- Duyuru/Haber/İhale sayfalarında **arama + çoklu filtre** (tarih aralığı, kategori, müdürlük).
- Kart–tablo görünüm arasında geçiş (toggle).

---

## 6) İçerik Detay Şablonları
- **Haber Detay:** Kapak görseli, başlık, tarih rozeti, içerik, paylaş butonları, ilgili içerikler grid’i.
- **Proje Detay:** Durum/ilerleme, bütçe, zaman çizgisi, galeri, dokümanlar.
- **İhale Detay:** Ekler (PDF/ZIP), şartname, katılım koşulları, sorumlu birim bilgisi.
- **Meclis/Encümen Detay:** Madde listesi, karar metni, dosya indir.

---

## 7) Erişilebilirlik, Hukuk ve Güven
- WCAG 2.2 AA hedefi (kontrast, odak, klavye, ARIA).
- KVKK, aydınlatma, çerez yönetimi (cookie banner + tercihler).
- Telif/üçüncü taraf görseller için lisans uyarıları.
- Harita/YouTube gömmeleri için gizlilik modu.

---

## 8) Teknik ve Meta
- **Framework:** Görsel tasarım çıktısı TailwindCSS utility sınıflarıyla uyumlu olsun.
- **Taslak Bileşen Adlandırma:** `Button`, `Card`, `Badge`, `Tabs`, `Input`, `Select`, `Table`, `Pagination`, `Modal`, `Accordion`, `Navbar`, `Footer`, `Hero`, `Stat`, `Timeline`, `Gallery`, `CalendarGrid`.
- **Karanlık Mod:** `class="dark"` kökünde çalışacak şekilde `dark:` varyantlarını ekle.
- **İçerik Yer Tutucular:** Haber, duyuru, ihale, proje için 4–6 örnek kart üret.
- **İkonlar:** Dış bağlantılarda external‑link ikonu; arama, filtre, indirme ikonları.

---

## 9) Teslim Beklentisi (Stitch’ten)
1. Ana sayfa ve tüm üst seviye sayfalar için **wireframe + high‑fidelity** varyantları.
2. Tutarlı **UI Kit / Design Tokens** (renk, tipografi, aralık, border radius, gölge).
3. Tüm bileşenlerde **TailwindCSS** sınıflarıyla uyumlu isimlendirme.
4. Mobil, tablet ve masaüstü önizlemeleri.
5. Aydınlık ve karanlık mod örnek ekranları.

---

## 10) Örnek İçerik Metinleri (yer tutucu)
- **Duyuru:** “Planlı Su Kesintisi – 12 Kasım 10:00–16:00 arası ilçe merkezinde bakım çalışması.”
- **Haber:** “Park ve Bahçeler Müdürlüğü, Sonbahar Ağaçlandırma Çalışmalarını Başlattı.”
- **İhale:** “2026/01 – Belediye Hizmet Binası Bakım-Onarım İşi – Teklif Son Tarih: 05.01.2026 10:00”
- **Proje:** “Oğuzlar Kent Meydanı Düzenlemesi – %45 tamamlandı.”

> **Not:** Tasarım dili; sade, kurumsal, vatandaş odaklı; beyaz alan kullanımı yüksek; okunaklı ve güven veren bir izlenim sunmalıdır.
