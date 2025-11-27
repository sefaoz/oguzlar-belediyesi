using System;
using Microsoft.EntityFrameworkCore;
using OguzlarBelediyesi.Domain;
using OguzlarBelediyesi.Infrastructure.Database;
using OguzlarBelediyesi.Infrastructure.Security;

namespace OguzlarBelediyesi.Infrastructure.Data;

public sealed class DataSeeder
{
    private readonly OguzlarBelediyesiDbContext _context;
    private readonly PasswordHasher _passwordHasher;

    public DataSeeder(OguzlarBelediyesiDbContext context, PasswordHasher passwordHasher)
    {
        _context = context;
        _passwordHasher = passwordHasher;
    }

    public async Task SeedAsync()
    {
        await _context.Database.EnsureCreatedAsync();

        var shouldSave = false;

        if (!await _context.Users.AnyAsync())
        {
            var adminUser = new User
            {
                Username = "admin",
                PasswordHash = _passwordHasher.Hash("Admin123!"),
                Role = "Administrator"
            };

            await _context.Users.AddAsync(adminUser);
            shouldSave = true;
        }

        if (!await _context.Announcements.AnyAsync())
        {
            await _context.Announcements.AddRangeAsync(DefaultAnnouncements);
            shouldSave = true;
        }

        if (!await _context.Events.AnyAsync())
        {
            await _context.Events.AddRangeAsync(DefaultEvents);
            shouldSave = true;
        }

        if (!await _context.Tenders.AnyAsync())
        {
            await _context.Tenders.AddRangeAsync(DefaultTenders);
            shouldSave = true;
        }

        if (shouldSave)
        {
            await _context.SaveChangesAsync();
        }
    }

    private static readonly Announcement[] DefaultAnnouncements = new[]
    {
        new Announcement
        {
            Title = "Oğuzlar Şehir Parkı Halkın Hizmetinde",
            Summary = "Yeni düzenlenen yeşil alan, yürüyüş yolları ve oyun gruplarıyla 27 Kasım tarihinde açıldı.",
            Content = "Oğuzlar Belediyesi olarak doğayı koruyarak vatandaşlar için modern bir park alanı oluşturduk. Yeni yürüyüş yolları, dinlenme alanları ve çocuk oyun grupları bölge sakinlerini bekliyor.",
            Category = "Genel Duyuru",
            Date = "27 Kasım 2025",
            PublishedAt = new DateTime(2025, 11, 27),
            Slug = "oguzlar-sehir-parki-halkin-hizmetinde"
        },
        new Announcement
        {
            Title = "Altyapı Yenileme Çalışmaları Tamamlanıyor",
            Summary = "Ana arterlerdeki kanalizasyon ve içme suyu hatlarının yenilenmesine ilişkin çalışmalar hızlandı.",
            Content = "Fen işleri ekiplerimiz, 2025 sezonu boyunca kritik hatlarda yenileme gerçekleştirdi. Bu sayede su baskını riski azaltıldı ve altyapı dayanıklılığı artırıldı.",
            Category = "Altyapı",
            Date = "22 Kasım 2025",
            PublishedAt = new DateTime(2025, 11, 22),
            Slug = "altyapi-yenileme-calismalari-tamamlaniyor"
        },
        new Announcement
        {
            Title = "İhtiyaç Sahibi Ailelere Sosyal Yardım Desteği",
            Summary = "Sosyal hizmet birimi, kış ayları öncesi ihtiyaç sahibi ailelere gıda ve kıyafet desteği sağladı.",
            Content = "Belediyemiz sosyal yardım hattına yapılan başvurular doğrultusunda kış yardımı paketleri dağıtıldı. Ailelere bireysel destek programları hazırlanmaya devam ediyor.",
            Category = "Sosyal Hizmet",
            Date = "18 Kasım 2025",
            PublishedAt = new DateTime(2025, 11, 18),
            Slug = "sosyal-yardim-destegi-2025"
        },
        new Announcement
        {
            Title = "Yeni Kütüphane Gönüllü Okuyucularını Bekliyor",
            Summary = "Kültür merkezindeki kütüphanenin koleksiyonu genişletilerek vatandaşların hizmetine alındı.",
            Content = "Kitap bağışları ve mobil kütüphane projeleriyle desteklenen yeni kütüphanemiz, gençler için atölye ve okuma saatleri organize ediyor.",
            Category = "Kültür Etkinliği",
            Date = "15 Kasım 2025",
            PublishedAt = new DateTime(2025, 11, 15),
            Slug = "yeni-kutuphane-gonullu-okuyucu"
        }
    };

    private static readonly Event[] DefaultEvents = new[]
    {
        new Event
        {
            Title = "Geleneksel Ceviz Festivali",
            Description = "Geleneksel lezzetler, konserler ve sergilerle dolu festivalimizde tüm halkımızı bekliyoruz.",
            Location = "Oğuzlar Meydanı",
            Date = "25 Kasım 2025",
            EventDate = new DateTime(2025, 11, 25, 14, 0, 0),
            Image = "https://picsum.photos/800/600?random=101",
            Slug = "geleneksel-ceviz-festivali"
        },
        new Event
        {
            Title = "Gençlik Konseri",
            Description = "Genç müzisyenlerimizin sahne alacağı konser, kültür merkezinde düzenlenecek.",
            Location = "Kültür Merkezi",
            Date = "15 Aralık 2025",
            EventDate = new DateTime(2025, 12, 15, 19, 0, 0),
            Image = "https://picsum.photos/800/600?random=102",
            Slug = "genclik-konseri"
        },
        new Event
        {
            Title = "Doğa Yürüyüşü",
            Description = "Obruk Barajı çevresinde rehber eşliğinde doğa yürüyüşü ve fotoğrafçılık etkinliği.",
            Location = "Obruk Barajı Çevresi",
            Date = "20 Aralık 2025",
            EventDate = new DateTime(2025, 12, 20, 10, 30, 0),
            Image = "https://picsum.photos/800/600?random=103",
            Slug = "doga-yuruyusu"
        },
        new Event
        {
            Title = "Tiyatro Gösterisi: Bir Anadolu Masalı",
            Description = "Yerel sanatçılar tarafından sahnelenecek tiyatro gösterisi kültür merkezimizde izleyici ile buluşuyor.",
            Location = "Belediye Konferans Salonu",
            Date = "05 Ocak 2026",
            EventDate = new DateTime(2026, 1, 5, 18, 0, 0),
            Image = "https://picsum.photos/800/600?random=104",
            Slug = "tiyatro-gosterisi-bir-anadolu-masali"
        }
    };

    private static readonly Tender[] DefaultTenders = new[]
    {
        new Tender
        {
            Title = "Belediye Hizmet Binası Tadilat İşi",
            Description = "Hizmet binamızın bodrum katındaki tesisat ve cephe tadilat işleri ihale edilecektir.",
            Date = "10 Aralık 2025",
            PublishedAt = new DateTime(2025, 11, 10),
            RegistrationNumber = "2025/123456",
            Status = "active",
            EstimatedValue = 150000m,
            Slug = "hizmet-binasi-tadilat"
        },
        new Tender
        {
            Title = "Araç Kiralama Hizmet Alımı",
            Description = "Personel ve hizmet araçları için 12 aylık kiralama hizmeti ihalesi yapılacaktır.",
            Date = "05 Aralık 2025",
            PublishedAt = new DateTime(2025, 11, 5),
            RegistrationNumber = "2025/123457",
            Status = "active",
            EstimatedValue = 250000m,
            Slug = "arac-kiralama-hizmeti-alimi"
        },
        new Tender
        {
            Title = "Park ve Bahçeler İçin Fidan Alımı",
            Description = "Yeni düzenlenecek peyzaj alanlarında kullanılmak üzere ceviz, gül ve sedir fidanlarının alımı yapılacaktır.",
            Date = "20 Kasım 2025",
            PublishedAt = new DateTime(2025, 11, 20),
            RegistrationNumber = "2025/123450",
            Status = "completed",
            EstimatedValue = 80000m,
            Slug = "fidan-alimi-2025"
        },
        new Tender
        {
            Title = "Kırtasiye Malzemesi Alımı",
            Description = "Eğitim desteği kapsamında ihtiyaç sahibi öğrencilerimiz için kırtasiye malzemesi alımı.",
            Date = "15 Kasım 2025",
            PublishedAt = new DateTime(2025, 11, 15),
            RegistrationNumber = "2025/123449",
            Status = "passive",
            EstimatedValue = 45000m,
            Slug = "kirtasiye-malzemesi-alimi"
        }
    };
}
