using System;
using System.Collections.Generic;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using OguzlarBelediyesi.Domain;
using OguzlarBelediyesi.Infrastructure.Persistence.Database;
using OguzlarBelediyesi.Infrastructure.Persistence.Entities;
using OguzlarBelediyesi.Infrastructure.Security;
using OguzlarBelediyesi.Domain.Entities.Configuration;

namespace OguzlarBelediyesi.Infrastructure.Persistence.Data;

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

        if (!await _context.NewsItems.AnyAsync())
        {
            await _context.NewsItems.AddRangeAsync(DefaultNews);
            shouldSave = true;
        }

        if (!await _context.PageContents.AnyAsync())
        {
            await _context.PageContents.AddRangeAsync(DefaultPageContents);
            shouldSave = true;
        }

        if (!await _context.GalleryFolders.AnyAsync())
        {
            await _context.GalleryFolders.AddRangeAsync(DefaultGalleryFolders);
            shouldSave = true;
        }

        if (!await _context.GalleryImages.AnyAsync())
        {
            await _context.GalleryImages.AddRangeAsync(DefaultGalleryImages);
            shouldSave = true;
        }

        if (!await _context.CouncilDocuments.AnyAsync())
        {
            await _context.CouncilDocuments.AddRangeAsync(DefaultCouncilDocuments);
            shouldSave = true;
        }

        if (!await _context.KvkkDocuments.AnyAsync())
        {
            await _context.KvkkDocuments.AddRangeAsync(DefaultKvkkDocuments);
            shouldSave = true;
        }

        if (!await _context.MunicipalUnits.AnyAsync())
        {
            await _context.MunicipalUnits.AddRangeAsync(DefaultMunicipalUnits);
            shouldSave = true;
        }

        if (!await _context.Vehicles.AnyAsync())
        {
            await _context.Vehicles.AddRangeAsync(DefaultVehicles);
            shouldSave = true;
        }

        if (!await _context.Sliders.AnyAsync())
        {
            await _context.Sliders.AddRangeAsync(DefaultSliders);
            shouldSave = true;
        }

        if (!await _context.MenuItems.AnyAsync())
        {
            await _context.MenuItems.AddRangeAsync(DefaultMenuItems);
            shouldSave = true;
        }

        foreach (var setting in DefaultSiteSettings)
        {
            if (!await _context.SiteSettings.AnyAsync(s => s.GroupKey == setting.GroupKey && s.Key == setting.Key))
            {
                await _context.SiteSettings.AddAsync(setting);
                shouldSave = true;
            }
        }

        if (shouldSave)
        {
            await _context.SaveChangesAsync();
        }
    }

    private static readonly SiteSetting[] DefaultSiteSettings = new[]
    {
        new SiteSetting
        {
            GroupKey = "Topbar",
            Key = "Links",
            Value = JsonSerializer.Serialize(new[]
            {
                new { title = "E-Devlet / CİMER", url = "https://www.turkiye.gov.tr", target = "_blank", icon = "fas fa-landmark" },
                new { title = "Çözüm Merkezi", url = "#", target = "_self", icon = "fas fa-headset" },
                new { title = "Online Borç Ödeme", url = "#", target = "_self", icon = "fas fa-wallet" },
                new { title = "Arama", url = "#", target = "_self", icon = "fas fa-search" }
            }, JsonOptions),
            Description = "Left side topbar links"
        },
        
        new SiteSetting
        {
            GroupKey = "Footer",
            Key = "LogoContent",
            Value = "Halkımıza daha iyi hizmet sunmak için var gücümüzle çalışıyoruz. Modern, şeffaf ve katılımcı belediyecilik anlayışıyla Oğuzlar'ı geleceğe taşıyoruz.",
            Description = "Text content below the footer logo"
        },
        new SiteSetting
        {
            GroupKey = "Footer",
            Key = "QuickLinks",
            Value = JsonSerializer.Serialize(new[]
            {
                new { title = "Anasayfa", url = "/" },
                new { title = "Başkan", url = "/baskan-hakkinda" },
                new { title = "Kurumsal", url = "/kurumsal" },
                new { title = "E-Belediye", url = "/e-belediye" },
                new { title = "İletişim", url = "/iletisim" }
            }, JsonOptions),
            Description = "Quick links list in footer"
        },
        new SiteSetting
        {
            GroupKey = "Footer",
            Key = "SocialMedia",
            Value = JsonSerializer.Serialize(new
            {
                facebook = "https://facebook.com",
                twitter = "https://twitter.com",
                instagram = "https://instagram.com",
                youtube = "https://youtube.com"
            }, JsonOptions),
            Description = "Social media links"
        },

        new SiteSetting
        {
            GroupKey = "EMunicipality",
            Key = "Links",
            Value = JsonSerializer.Serialize(new[]
            {
                new { title = "Borç Ödeme", url = "#", icon = "fas fa-wallet" },
                new { title = "İlk E-Başvuru", url = "#", icon = "fas fa-pen-to-square" },
                new { title = "E-Posta Doğrulama", url = "#", icon = "fas fa-paper-plane" },
                new { title = "Güvenlik Bilgisi", url = "#", icon = "fas fa-right-from-bracket" },
                new { title = "E-Bordo", url = "#", icon = "fas fa-calendar-check" },
                new { title = "Sicil Beyanları", url = "#", icon = "fas fa-file-code" }
            }, JsonOptions),
            Description = "Homepage E-Municipality shortcut links"
        },

        new SiteSetting
        {
            GroupKey = "SEO",
            Key = "Global",
            Value = JsonSerializer.Serialize(new
            {
                metaTitle = "Oğuzlar Belediyesi - Resmi Web Sitesi",
                metaDescription = "Oğuzlar Belediyesi resmi web sitesi. Haberler, duyurular, ihaleler ve e-belediye hizmetleri.",
                metaKeywords = "oğuzlar, belediye, çorum, oğuzlar belediyesi, haberler, ihaleler"
            }, JsonOptions),
            Description = "Global SEO settings"
        }
    };

    private static readonly Announcement[] DefaultAnnouncements = new[]
    {
        new Announcement
        {
            Title = "Oğuzlar Şehir Parkı Halkın Hizmetinde",
            Summary = "Yeni düzenlenen yeşil alan, yürüyüş yolları ve oyun gruplarıyla 27 Kasım tarihinde açıldı.",
            Content = "Oğuzlar Belediyesi olarak doğayı koruyarak vatandaşlar için modern bir park alanı oluşturduk. Yeni yürüyüş yolları, dinlenme alanları ve çocuk oyun grupları bölge sakinlerini bekliyor.",
            Category = "Genel Duyuru",
            Date = new DateTime(2025, 11, 27),
            Slug = "oguzlar-sehir-parki-halkin-hizmetinde"
        },
        new Announcement
        {
            Title = "Altyapı Yenileme Çalışmaları Tamamlanıyor",
            Summary = "Ana arterlerdeki kanalizasyon ve içme suyu hatlarının yenilenmesine ilişkin çalışmalar hızlandı.",
            Content = "Fen işleri ekiplerimiz, 2025 sezonu boyunca kritik hatlarda yenileme gerçekleştirdi. Bu sayede su baskını riski azaltıldı ve altyapı dayanıklılığı artırıldı.",
            Category = "Altyapı",
            Date = new DateTime(2025, 11, 22),
            Slug = "altyapi-yenileme-calismalari-tamamlaniyor"
        },
        new Announcement
        {
            Title = "İhtiyaç Sahibi Ailelere Sosyal Yardım Desteği",
            Summary = "Sosyal hizmet birimi, kış ayları öncesi ihtiyaç sahibi ailelere gıda ve kıyafet desteği sağladı.",
            Content = "Belediyemiz sosyal yardım hattına yapılan başvurular doğrultusunda kış yardımı paketleri dağıtıldı. Ailelere bireysel destek programları hazırlanmaya devam ediyor.",
            Category = "Sosyal Hizmet",
            Date = new DateTime(2025, 11, 18),
            Slug = "sosyal-yardim-destegi-2025"
        },
        new Announcement
        {
            Title = "Yeni Kütüphane Gönüllü Okuyucularını Bekliyor",
            Summary = "Kültür merkezindeki kütüphanenin koleksiyonu genişletilerek vatandaşların hizmetine alındı.",
            Content = "Kitap bağışları ve mobil kütüphane projeleriyle desteklenen yeni kütüphanemiz, gençler için atölye ve okuma saatleri organize ediyor.",
            Category = "Kültür Etkinliği",
            Date = new DateTime(2025, 11, 15),
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

            EventDate = new DateTime(2025, 11, 25, 14, 0, 0),
            Image = "https://picsum.photos/800/600?random=101",
            Slug = "geleneksel-ceviz-festivali"
        },
        new Event
        {
            Title = "Gençlik Konseri",
            Description = "Genç müzisyenlerimizin sahne alacağı konser, kültür merkezinde düzenlenecek.",
            Location = "Kültür Merkezi",

            EventDate = new DateTime(2025, 12, 15, 19, 0, 0),
            Image = "https://picsum.photos/800/600?random=102",
            Slug = "genclik-konseri"
        },
        new Event
        {
            Title = "Doğa Yürüyüşü",
            Description = "Obruk Barajı çevresinde rehber eşliğinde doğa yürüyüşü ve fotoğrafçılık etkinliği.",
            Location = "Obruk Barajı Çevresi",

            EventDate = new DateTime(2025, 12, 20, 10, 30, 0),
            Image = "https://picsum.photos/800/600?random=103",
            Slug = "doga-yuruyusu"
        },
        new Event
        {
            Title = "Tiyatro Gösterisi: Bir Anadolu Masalı",
            Description = "Yerel sanatçılar tarafından sahnelenecek tiyatro gösterisi kültür merkezimizde izleyici ile buluşuyor.",
            Location = "Belediye Konferans Salonu",

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
            TenderDate = new DateTime(2025, 11, 10),
            RegistrationNumber = "2025/123456",
            Status = "Open",
            EstimatedValue = 150000m,
            Slug = "hizmet-binasi-tadilat",
            DocumentsJson = "[]"
        },
        new Tender
        {
            Title = "Araç Kiralama Hizmet Alımı",
            Description = "Personel ve hizmet araçları için 12 aylık kiralama hizmeti ihalesi yapılacaktır.",
            TenderDate = new DateTime(2025, 11, 5),
            RegistrationNumber = "2025/123457",
            Status = "Open",
            EstimatedValue = 250000m,
            Slug = "arac-kiralama-hizmeti-alimi",
            DocumentsJson = "[]"
        },
        new Tender
        {
            Title = "Park ve Bahçeler İçin Fidan Alımı",
            Description = "Yeni düzenlenecek peyzaj alanlarında kullanılmak üzere ceviz, gül ve sedir fidanlarının alımı yapılacaktır.",
            TenderDate = new DateTime(2025, 11, 20),
            RegistrationNumber = "2025/123450",
            Status = "Concluded",
            EstimatedValue = 80000m,
            Slug = "fidan-alimi-2025",
            DocumentsJson = "[]"
        },
        new Tender
        {
            Title = "Kırtasiye Malzemesi Alımı",
            Description = "Eğitim desteği kapsamında ihtiyaç sahibi öğrencilerimiz için kırtasiye malzemesi alımı.",
            TenderDate = new DateTime(2025, 11, 15),
            RegistrationNumber = "2025/123449",
            Status = "Cancelled",
            EstimatedValue = 45000m,
            Slug = "kirtasiye-malzemesi-alimi",
            DocumentsJson = "[]"
        }
    };

    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web);

    private static readonly NewsEntity[] DefaultNews = new[]
    {
        CreateNews("yeni-hizmet-binasi", new DateTime(2025, 11, 25), "Oğuzlar Belediyesi Yeni Hizmet Binası Açıldı",
            "Modern kütüphane, zabıta birimi ve halkla ilişkiler noktasında hizmet kalitemizi artırıyoruz.",
            "https://picsum.photos/800/600?random=1",
            new[]
            {
                "https://picsum.photos/1200/800?random=1",
                "https://picsum.photos/1200/800?random=2",
                "https://picsum.photos/1200/800?random=3"
            }),
        CreateNews("ceviz-festivali-hazirliklari", new DateTime(2025, 11, 20), "Ceviz Festivali Hazırlıkları Başladı",
            "Yerli ve yabancı misafirlerimizin buluşacağı festival alanı için sahne kurulumu tamamlanmak üzere.",
            "https://picsum.photos/800/600?random=4",
            new[]
            {
                "https://picsum.photos/1200/800?random=4",
                "https://picsum.photos/1200/800?random=5"
            }),
        CreateNews("obruk-baraji-duzenleniyor", new DateTime(2025, 11, 15), "Obruk Barajı Çevresi Düzenleniyor",
            "Çevre düzenlemeleri, yürüyüş yolları ve peyzaj çalışmaları halkımızın kullanımına hazırlanıyor.",
            "https://picsum.photos/800/600?random=6",
            new[]
            {
                "https://picsum.photos/1200/800?random=6",
                "https://picsum.photos/1200/800?random=7",
                "https://picsum.photos/1200/800?random=8"
            }),
        CreateNews("kis-spor-okullari", new DateTime(2025, 11, 10), "Köy Spor Okulları Kayıtları Açıldı",
            "Gençlerimizin sporla buluşması için kayak, masa tenisi ve yüzme kurs kayıtları başladı.",
            "https://picsum.photos/800/600?random=9"),
        CreateNews("yol-bakim-onarimlari", new DateTime(2025, 11, 5), "Yol Bakım Onarım Çalışmaları Sürüyor",
            "Fen işleri ekiplerimiz ilçe merkezinde ve kırsal mahallelerde yol bakım seferberliğini sürdürüyor.",
            "https://picsum.photos/800/600?random=10"),
        CreateNews("cumhuriyet-bayrami", new DateTime(2025, 10, 29), "29 Ekim Cumhuriyet Bayramı Coşkuyla Kutlandı",
            "Cumhuriyetimizin 102. yılı tüm birimlerin ortak kutlamasıyla gerçekleştirildi.",
            "https://picsum.photos/800/600?random=11")
    };

    private static readonly PageContentEntity[] DefaultPageContents = new[]
    {
        CreatePageContent("baskan-hakkinda", "Mustafa CEBECİ", "Oğuzlar Belediye Başkanı", new[]
        {
            "1980 yılında Çorum'un Oğuzlar ilçesinde doğdu. İlk, orta ve lise öğrenimini Oğuzlar'da tamamladı.",
            "Anadolu Üniversitesi İşletme Fakültesi'nden mezun oldu. Siyasi hayatına genç yaşlarda başlayan Cebeci, ilçemizin kalkınması ve gelişmesi için çeşitli sivil toplum kuruluşlarında aktif görevler aldı.",
            "31 Mart 2024 yerel seçimlerinde Oğuzlar halkının teveccühü ile Belediye Başkanı seçildi. Evli ve 3 çocuk babasıdır.",
            "Göreve geldiği günden bu yana \"Daha Yaşanabilir Bir Oğuzlar\" vizyonuyla çalışmalarını sürdürmekte, şeffaf, katılımcı ve halk odaklı belediyecilik anlayışını benimsemektedir."
        }, imageUrl: "assets/images/mustafa_cebeci.jpg"),
        CreatePageContent("baskan-mesaji", "Kıymetli Hemşehrilerim", "Başkanın Mesajı", new[]
        {
            "İsmini Oğuz boyundan alan güzel ilçemiz OĞUZLAR, tarihiyle, eşsiz doğasıyla ve insanıyla bölgemizin gözde ilçelerinden biridir.",
            "İlçemizin değerlerine sahip çıkmak, her bir insanımızın yüzünü güldürmek ve hizmetlerimizi en doğru ve ulaşılır şekilde yapmak için buradayız.",
            "Bu kutlu vazifeyi ifa ederken, siz değerli halkımızdan gördüğümüz güven ve destek en büyük gücümüz olacaktır.",
            "Bir şehrin Belediye Başkanı aynı zamanda o şehrin şeffaflığı için de sorumludur. Görevimizi çift taraflı dayanışma anlayışı ile icra edeceğiz.",
            "Halkımızın bize emanet ettiği bu güzel OĞUZLAR'da, daha güzel ve mutlu yarınlarda buluşmak dileğiyle en kalbi muhabbetlerimi sunuyorum."
        }, imageUrl: "assets/images/mustafa_cebeci.jpg"),
        CreatePageContent("baskana-mesaj", "Başkana Mesaj Yaz", "Görüşlerinizi iletin", new[]
        {
            "Oğuzlar Belediyesi olarak her görüşü, fikri ve öneriyi kıymetli buluyoruz. Bu form üzerinden Başkanımıza mesajınızı doğrudan ulaştırabilirsiniz.",
            "Mesajlarınız, ekiplerimiz tarafından titizlikle incelenecek ve en kısa sürede kişisel geri dönüş yapılmaya çalışılacaktır.",
            "KVKK onayını verdikten sonra mesajınızı iletmeyi unutmayınız."
        }),
        CreatePageContent("home-baskan-mesaji", "Değerli Oğuzlarlı Hemşehrilerim", "Belediye Başkanından", new[]
        {
            "Bu kutlu vazifeyi ifa ederken siz değerli halkımızdan gördüğümüz güven ve destek en büyük gücümüz olacaktır.",
            "Daha yaşanabilir ve kıymetini bilen bir Oğuzlar için; her alanın birbiriyle uyumlu çalıştığı, insanına değer veren bir belediyecilik anlayışı sergiliyor; şeffaf, katılımcı ve hizmet odaklı olmanın kıymetini biliyoruz.",
            "İlçemizin güzelliğini korumak, insanımızın yaşam kalitesini yükseltmek ve gelecek nesillere güçlü bir Oğuzlar bırakmak için yorulmadan çalışıyoruz."
        }, imageUrl: "assets/images/mustafa_cebeci.jpg"),
        CreatePageContent("cografi-yapi", "Coğrafi Yapı", "Doğal ve kültürel zenginliklerimiz", new[]
        {
            "Oğuzlar ilçesi Karadeniz Bölgesi'nin Orta Karadeniz Bölümünde yer alır; Çorum il merkezinin 63 km kuzeybatısındadır ve rakımı 650 metredir.",
            "İlçemizde ana kaya genelde kalker, andezit ve gnays olup yer yer metamorfik (filit) ve efridik kayaçlara da rastlanmaktadır.",
            "Kızılırmak’ın etkisiyle oluşan vadiler, kıvrımlı yamaçlar ve geçiş iklimi sayesinde zengin bitki örtüsü gelişmiştir; sahil yerinden iç kısımlara kadar geniş bir flora çeşitliliği göze çarpmaktadır.",
            "Karasal iklimden izler taşıyan Oğuzlar, doğa tutkunları için hem yeşili hem de kültürel miraşı bir arada sunar."
        }, imageUrl: "https://picsum.photos/1600/900?random=64"),
        CreatePageContent("iletisim", "İletişim", "2 adımda bize ulaşın", new[]
        {
            "Görüş, öneri, istek veya şikayetlerinizi Başkanımıza iletebilirsiniz. İletişim bilgileriniz gizli tutulacaktır.",
            "Formu doldurduktan sonra ekiplerimiz en kısa sürede sizinle iletişime geçecektir."
        }, mapEmbedUrl: "https://maps.google.com/maps?q=Oğuzlar+Belediyesi+Çorum&t=&z=15&ie=UTF8&iwloc=&output=embed",
            contactDetails: new[]
            {
                new ContactDetail("Adres", "Karadonlu Mahallesi Fatih Caddesi No:33/21 Oğuzlar/Çorum"),
                new ContactDetail("Telefon", "+90 364 561 70 45"),
                new ContactDetail("Fax", "+90 364 561 21 50"),
                new ContactDetail("E-posta", "oguzlarbelediyesi@hotmail.com")
            }),
        CreatePageContent("tarihimiz", "İlçemizin Tarihi", "Geçmişten Bugüne Oğuzlar", new[]
        {
            "Büyük Selçuklular, 1071 Malazgirt Meydan Muharebesi sonrasında Anadolu’ya yerleşmeye başladıklarında Oğuzlar ve çevresini çeşitli tarihlerde iskan etmişlerdir.",
            "Karabük Divanı olarak da bilinen bu coğrafyada Çorum, İskilip, Osmancık gibi yerleşim birimlerinde Oğuzlar boyundan gelen birçok köy ve mevkii vardır.",
            "1576 tarihi itibariyle Karabük Nahiyesi’ni oluşturan köylerin nüfusu 6.340 kişi civarındaydı ve bu bölge sosyal, kültürel olarak büyük bir hareket alanına sahipti.",
            "Kızılırmak’ın kıvrımlı vadileri, yüksek dağlarla çevrili alanları ve kendine yetebilen yerleşim birimi olması; Oğuzlar halkının binlerce yıllık kültürünü korumasına hizmet etmiştir.",
            "Giyim, yaşam, dil ve adetlerde görülen farklılıklar bu bölgenin içe kapalı ama kendine özgü zengin bir yaşam alanı olduğunu gösterir."
        }, imageUrl: "https://picsum.photos/1600/900?random=66")
    };

    private static readonly GalleryFolderEntity[] DefaultGalleryFolders = new[]
    {
        CreateFolder("Oguzlar Ceviz Festivali 2024", "oguzlar-ceviz-festivali-2024", "https://picsum.photos/id/10/800/600", 12, "15.10.2024"),
        CreateFolder("Altinkoz Tesisleri Açılışı", "altinkoz-tesisleri-acilisi", "https://picsum.photos/id/20/800/600", 8, "20.09.2024"),
        CreateFolder("Doğa Yürüyüşü Etkinliği", "doga-yuruyusu-etkinligi", "https://picsum.photos/id/28/800/600", 25, "05.09.2024"),
        CreateFolder("Obruk Barajı Manzaraları", "obruk-baraji-manzaralari", "https://picsum.photos/id/40/800/600", 15, "01.08.2024"),
        CreateFolder("Belediye Çalışmaları", "belediye-calismalari", "https://picsum.photos/id/50/800/600", 42, "12.07.2024"),
        CreateFolder("Köy Manzaraları", "koy-manzaralari", "https://picsum.photos/id/60/800/600", 10, "10.01.2024")
    };

    private static readonly GalleryImageEntity[] DefaultGalleryImages = BuildGalleryImages(DefaultGalleryFolders);

    private static readonly CouncilDocumentEntity[] DefaultCouncilDocuments = new[]
    {
        new CouncilDocumentEntity
        {
            Title = "2023 Mali Yıllık Faaliyet Raporu",
            Type = "Rapor",
            Date = new DateTime(2024, 1, 15),
            Description = "2023 bütçesinin kapanışı, gelir-gider tabloları ve birim bazlı analizleri sunulmaktadır.",
            FileUrl = "https://documents.oguzlar.bel.tr/faaliyet-2023.pdf"
        },
        new CouncilDocumentEntity
        {
            Title = "2022 Faaliyet Raporu",
            Type = "Rapor",
            Date = new DateTime(2023, 1, 20),
            Description = "2022 yılı boyunca yürütülen projelerin detaylı raporu."
        },
        new CouncilDocumentEntity
        {
            Title = "Meclis Üyeleri ve Görev Dağılımı",
            Type = "Liste",
            Date = new DateTime(2024, 4, 1),
            Description = "Güncel meclis üyeleri ve komisyon görevleri listesi.",
            FileUrl = "https://documents.oguzlar.bel.tr/meclis-uyeleri.pdf"
        }
    };

    private static readonly KvkkDocumentEntity[] DefaultKvkkDocuments = new[]
    {
        CreateKvkk("Oğuzlar Belediyesi KVKK Politikası", "Politika", "http://www.oguzlar.bel.tr/Upload/files/oguzlar-belediyesi-kvkk-politikasi_17.pdf"),
        CreateKvkk("Oğuzlar Belediyesi KVKK Başvuru Formu", "Form", "http://www.oguzlar.bel.tr/Upload/files/kvkk-basvuru-formu_03.pdf"),
        CreateKvkk("Oğuzlar Belediye Sosyal Medya Aydınlatma Metni", "Aydınlatma Metni", "http://www.oguzlar.bel.tr/Upload/files/sosyal-medya-aydinlatma-metni_21.pdf"),
        CreateKvkk("Oğuzlar Belediyesi Kişisel Veri Saklama ve İmha Politikası", "Politika", "http://www.oguzlar.bel.tr/Upload/files/kisisel-veri-saklama-ve-imha-politikasi_01.pdf"),
        CreateKvkk("Oğuzlar Belediyesi Aydınlatma Metni", "Aydınlatma Metni", "http://www.oguzlar.bel.tr/Upload/files/aydinlatma-metni_52.pdf"),
        CreateKvkk("Oğuzlar Belediyesi Çerez Politikası", "Politika", "http://www.oguzlar.bel.tr/Upload/files/cerez-politikasi_55.pdf"),
        CreateKvkk("Oğuzlar Belediyesi İzin Formu", "Form", "http://www.oguzlar.bel.tr/Upload/files/izin-formu_58.pdf")
    };

    private static readonly MunicipalUnitEntity[] DefaultMunicipalUnits = new[]
    {
        CreateMunicipalUnit("ozel-kalem", "Özel Kalem, Basın ve Halkla İlişkiler", "Özel Kalem... programları düzenler.", "fa-pen-fancy",
            new[]
            {
                new UnitStaff("Ahmet YILMAZ", "Özel Kalem Müdürü", "assets/images/placeholder-person.jpg")
            }),
        CreateMunicipalUnit("zabita", "Zabıta Amirliği", "Zabıta Amirliği, belde halkının esenlik...", "fa-shield-alt",
            new[]
            {
                new UnitStaff("Ali KÖRENCİ", "Zabıta Amiri", "assets/images/zabita-amiri.jpg")
            }),
        CreateMunicipalUnit("mali-hizmetler", "Mali Hizmetler Müdürlüğü", "Mali Hizmetler Müdürlüğü, belediyenin mali kaynaklarını yönetir.", "fa-coins",
            new[]
            {
                new UnitStaff("Mehmet DEMİR", "Mali Hizmetler Müdürü", "assets/images/placeholder-person.jpg")
            }),
        CreateMunicipalUnit("tahsilat-emlak", "Tahsilat ve Emlak Birimi", "Tahsilat ve Emlak Birimi, belediye gelirlerinin tahsilatını ve emlak işlemlerini yürütür.", "fa-file-invoice-dollar",
            new[]
            {
                new UnitStaff("Ayşe KAYA", "Birim Sorumlusu", "assets/images/placeholder-person.jpg")
            }),
        CreateMunicipalUnit("yazi-isleri", "Yazı İşleri Müdürlüğü", "Yazı İşleri Müdürlüğü, belediyenin resmi yazışmalarını ve meclis kararlarını takip eder.", "fa-file-signature",
            new[]
            {
                new UnitStaff("Fatma ÇELİK", "Yazı İşleri Müdürü", "assets/images/placeholder-person.jpg")
            }),
        CreateMunicipalUnit("fen-isleri", "Fen İşleri Müdürlüğü", "Fen İşleri Müdürlüğü, ilçenin altyapı ve istiyapı çalışmalarını yürütür.", "fa-hard-hat",
            new[]
            {
                new UnitStaff("Mustafa ÇAHİN", "Fen İşleri Müdürü", "assets/images/placeholder-person.jpg")
            })
    };

    private static readonly VehicleEntity[] DefaultVehicles = new[]
    {
        new VehicleEntity
        {
            Name = "JCB Kazıcı Yükleyici",
            Type = "İş Makinesi",
            Plate = "19 AA 001",
            Description = "Altyapı ve kazı çalışmalarında kullanılan çok amaçlı iş makinesi.",
            ImageUrl = "assets/images/slider.jpg"
        },
        new VehicleEntity
        {
            Name = "Ford Cargo Süpürge Kamyonu",
            Type = "Hizmet Aracı",
            Plate = "19 AA 002",
            Description = "İlçemizin temizlik hizmetlerinde kullanılan hidrolik sıkıştırmalı süpürge kamyonu.",
            ImageUrl = "assets/images/slider.jpg"
        },
        new VehicleEntity
        {
            Name = "Mercedes-Benz İtfaiye Aracı",
            Type = "Hizmet Aracı",
            Plate = "19 AA 003",
            Description = "Yangın ve kurtarma operasyonları için tam donanımlı araç.",
            ImageUrl = "assets/images/slider.jpg"
        },
        new VehicleEntity
        {
            Name = "Otokar Sultan Otobüs",
            Type = "Toplu Taşıma",
            Plate = "19 AA 004",
            Description = "Vatandaşlarımızın ulaşımı için kullanılan şehir içi yolcu otobüsü.",
            ImageUrl = "assets/images/slider.jpg"
        },
        new VehicleEntity
        {
            Name = "Caterpillar Greyder",
            Type = "İş Makinesi",
            Plate = "19 AA 005",
            Description = "Yol yapım ve bakım çalışmalarında kullanılan greyder.",
            ImageUrl = "assets/images/slider.jpg"
        },
        new VehicleEntity
        {
            Name = "Ford Transit Cenaze Aracı",
            Type = "Hizmet Aracı",
            Plate = "19 AA 006",
            Description = "Cenaze nakil hizmetlerinde kullanılan soğutuculu araç.",
            ImageUrl = "assets/images/slider.jpg"
        }
    };

    private static readonly SliderEntity[] DefaultSliders = new[]
    {
        CreateSlider("Hoş Geldiniz", "Oğuzlar Belediyesi Resmi Web Sitesi", "assets/images/slider.jpg", null, 1, true),
        CreateSlider("Kültür ve Sanat", "Etkinliklerimizden haberdar olun", "assets/images/slider2.jpg", null, 2, true)
    };

    private static readonly Guid MenuHomeId = Guid.Parse("0115d0e3-4a6a-4d5f-9a53-1a5f1ed8a9f1");
    private static readonly Guid MenuBaskanimizId = Guid.Parse("fa67a6eb-6b6a-4b66-9c3a-16fcf281e9ef");
    private static readonly Guid MenuBaskanHakkindaId = Guid.Parse("a90cbe87-5b49-4d7a-9e24-1f2c6f7ab7df");
    private static readonly Guid MenuBaskandanMesajId = Guid.Parse("16b574d8-4f7e-4c3b-9810-8c2e6f22506b");
    private static readonly Guid MenuBaskanaMesajId = Guid.Parse("2ca6b3f0-4ee5-4a4d-9700-1f8f7c151a45");
    private static readonly Guid MenuMeclisId = Guid.Parse("c56a3dbe-8a63-4c2b-8ea6-fc719b7224b3");
    private static readonly Guid MenuHaberlerId = Guid.Parse("1c2d5eab-37f7-4eb7-927c-bfe4361d8f3a");
    private static readonly Guid MenuKurumsalId = Guid.Parse("2f1d763a-1b3c-413e-8c6b-9db0bd1c8e5a");
    private static readonly Guid MenuBelediyeEncumenId = Guid.Parse("f716c5c9-7c2a-4f5e-8b72-d2b9c160ef6d");
    private static readonly Guid MenuAracParkiId = Guid.Parse("b798d1b1-8c62-405c-8c7d-3a8d5bfceb4e");
    private static readonly Guid MenuKvkkId = Guid.Parse("5d8b3a67-545d-4764-9d49-4e1df1a5f2d2");
    private static readonly Guid MenuBirimlerId = Guid.Parse("bf25a1ee-1f68-4c1e-9e37-974be48c6c01");
    private static readonly Guid MenuOzelKalemId = Guid.Parse("bc62c0c8-2eff-4a4d-bd1b-2223bde7c78c");
    private static readonly Guid MenuZabitaAmirligiId = Guid.Parse("739d1da7-47d4-4de9-8590-3a2b1f6b39d5");
    private static readonly Guid MenuMaliHizmetlerId = Guid.Parse("8ef06010-2b83-4bf7-ab0d-7a5c32a8a613");
    private static readonly Guid MenuTahsilatEmlakId = Guid.Parse("a8e56d20-b48c-4c5e-9f9a-2a226c2aa5de");
    private static readonly Guid MenuYaziIsleriId = Guid.Parse("e8638eb8-3ba1-45b5-9d19-bc5b1e3f6e0f");
    private static readonly Guid MenuFenIsleriId = Guid.Parse("03df9d1a-9f82-4db3-8d56-7d1d3d8f2f02");
    private static readonly Guid MenuOguzlariKesfetId = Guid.Parse("3a7df0f4-1f24-4a80-bfa1-d7a0a2e2bbf1");
    private static readonly Guid MenuIlcemizinTarihiId = Guid.Parse("bbf3a4d9-9ed1-4c63-9c1f-186e2f9300a4");
    private static readonly Guid MenuCografiYapisiId = Guid.Parse("ab2b1d67-4794-4aae-b044-7e59b78c0c2d");
    private static readonly Guid MenuKentRehberiId = Guid.Parse("a4183d5a-1d74-4ebc-8eb3-adeb34ef1e7a");
    private static readonly Guid MenuGezilecekYerlerId = Guid.Parse("8bf21f8f-3cea-4cb9-9622-4e69cd76763e");
    private static readonly Guid MenuTarihveKulturId = Guid.Parse("5f8d3c4b-f21a-4dfb-91e1-2ce65f3c4189");
    private static readonly Guid MenuFotografGalerisiId = Guid.Parse("52e8c1b8-0ca7-4f19-8a29-3b7d1f8b1c7c");
    private static readonly Guid MenuIletisimId = Guid.Parse("1a4a6149-7976-4f17-b6d4-8967cdb29990");

    private static readonly MenuItemEntity[] DefaultMenuItems = new[]
    {
        new MenuItemEntity
        {
            Id = MenuHomeId,
            Title = "Anasayfa",
            Url = "/",
            ParentId = null,
            Order = 1,
            IsVisible = true,
            Target = "_self"
        },
        new MenuItemEntity
        {
            Id = MenuBaskanimizId,
            Title = "Başkanımız",
            Url = "#",
            ParentId = null,
            Order = 2,
            IsVisible = true,
            Target = "_self"
        },
        new MenuItemEntity
        {
            Id = MenuBaskanHakkindaId,
            Title = "Başkan Hakkında",
            Url = "/baskan-hakkinda",
            ParentId = MenuBaskanimizId,
            Order = 1,
            IsVisible = true,
            Target = "_self"
        },
        new MenuItemEntity
        {
            Id = MenuBaskandanMesajId,
            Title = "Başkandan Mesaj",
            Url = "/baskandan-mesaj",
            ParentId = MenuBaskanimizId,
            Order = 2,
            IsVisible = true,
            Target = "_self"
        },
        new MenuItemEntity
        {
            Id = MenuBaskanaMesajId,
            Title = "Başkana Mesaj Yaz",
            Url = "/baskana-mesaj",
            ParentId = MenuBaskanimizId,
            Order = 3,
            IsVisible = true,
            Target = "_self"
        },
        new MenuItemEntity
        {
            Id = MenuMeclisId,
            Title = "Meclis",
            Url = "/meclis",
            ParentId = null,
            Order = 3,
            IsVisible = true,
            Target = "_self"
        },
        new MenuItemEntity
        {
            Id = MenuHaberlerId,
            Title = "Haberler",
            Url = "/haberler",
            ParentId = null,
            Order = 4,
            IsVisible = true,
            Target = "_self"
        },
        new MenuItemEntity
        {
            Id = MenuKurumsalId,
            Title = "Kurumsal",
            Url = "#",
            ParentId = null,
            Order = 5,
            IsVisible = true,
            Target = "_self"
        },
        new MenuItemEntity
        {
            Id = MenuBelediyeEncumenId,
            Title = "Belediye Encümeni",
            Url = "#",
            ParentId = MenuKurumsalId,
            Order = 1,
            IsVisible = true,
            Target = "_self"
        },
        new MenuItemEntity
        {
            Id = MenuAracParkiId,
            Title = "Araç Parkı",
            Url = "/arac-parki",
            ParentId = MenuKurumsalId,
            Order = 2,
            IsVisible = true,
            Target = "_self"
        },
        new MenuItemEntity
        {
            Id = MenuKvkkId,
            Title = "KVKK",
            Url = "/kvkk",
            ParentId = MenuKurumsalId,
            Order = 3,
            IsVisible = true,
            Target = "_self"
        },
        new MenuItemEntity
        {
            Id = MenuBirimlerId,
            Title = "Birimler",
            Url = "#",
            ParentId = null,
            Order = 6,
            IsVisible = true,
            Target = "_self"
        },
        new MenuItemEntity
        {
            Id = MenuOzelKalemId,
            Title = "Özel Kalem, Basın ve Halkla İlişkiler",
            Url = "/birimler/ozel-kalem",
            ParentId = MenuBirimlerId,
            Order = 1,
            IsVisible = true,
            Target = "_self"
        },
        new MenuItemEntity
        {
            Id = MenuZabitaAmirligiId,
            Title = "Zabıta Amirliği",
            Url = "/birimler/zabita",
            ParentId = MenuBirimlerId,
            Order = 2,
            IsVisible = true,
            Target = "_self"
        },
        new MenuItemEntity
        {
            Id = MenuMaliHizmetlerId,
            Title = "Mali Hizmetler Müdürlüğü",
            Url = "/birimler/mali-hizmetler",
            ParentId = MenuBirimlerId,
            Order = 3,
            IsVisible = true,
            Target = "_self"
        },
        new MenuItemEntity
        {
            Id = MenuTahsilatEmlakId,
            Title = "Tahsilat ve Emlak Birimi",
            Url = "/birimler/tahsilat-emlak",
            ParentId = MenuBirimlerId,
            Order = 4,
            IsVisible = true,
            Target = "_self"
        },
        new MenuItemEntity
        {
            Id = MenuYaziIsleriId,
            Title = "Yazı İşleri Müdürlüğü",
            Url = "/birimler/yazi-isleri",
            ParentId = MenuBirimlerId,
            Order = 5,
            IsVisible = true,
            Target = "_self"
        },
        new MenuItemEntity
        {
            Id = MenuFenIsleriId,
            Title = "Fen İşleri Müdürlüğü",
            Url = "/birimler/fen-isleri",
            ParentId = MenuBirimlerId,
            Order = 6,
            IsVisible = true,
            Target = "_self"
        },
        new MenuItemEntity
        {
            Id = MenuOguzlariKesfetId,
            Title = "Oğuzlar'ı Keşfet",
            Url = "#",
            ParentId = null,
            Order = 7,
            IsVisible = true,
            Target = "_self"
        },
        new MenuItemEntity
        {
            Id = MenuIlcemizinTarihiId,
            Title = "İlçemizin Tarihi",
            Url = "/ilcemizin-tarihi",
            ParentId = MenuOguzlariKesfetId,
            Order = 1,
            IsVisible = true,
            Target = "_self"
        },
        new MenuItemEntity
        {
            Id = MenuCografiYapisiId,
            Title = "Coğrafi Yapısı",
            Url = "/cografi-yapi",
            ParentId = MenuOguzlariKesfetId,
            Order = 2,
            IsVisible = true,
            Target = "_self"
        },
        new MenuItemEntity
        {
            Id = MenuKentRehberiId,
            Title = "Kent Rehberi",
            Url = "#",
            ParentId = MenuOguzlariKesfetId,
            Order = 3,
            IsVisible = true,
            Target = "_self"
        },
        new MenuItemEntity
        {
            Id = MenuGezilecekYerlerId,
            Title = "Gezilecek Yerler",
            Url = "#",
            ParentId = MenuOguzlariKesfetId,
            Order = 4,
            IsVisible = true,
            Target = "_self"
        },
        new MenuItemEntity
        {
            Id = MenuTarihveKulturId,
            Title = "Tarih ve Kültür",
            Url = "#",
            ParentId = MenuOguzlariKesfetId,
            Order = 5,
            IsVisible = true,
            Target = "_self"
        },
        new MenuItemEntity
        {
            Id = MenuFotografGalerisiId,
            Title = "Fotoğraf Galerisi",
            Url = "/fotograf-galerisi",
            ParentId = MenuOguzlariKesfetId,
            Order = 6,
            IsVisible = true,
            Target = "_self"
        },
        new MenuItemEntity
        {
            Id = MenuIletisimId,
            Title = "İletişim",
            Url = "/iletisim",
            ParentId = null,
            Order = 8,
            IsVisible = true,
            Target = "_self"
        }
    };

    private static SliderEntity CreateSlider(string title, string description, string imageUrl, string? link, int order, bool isActive)
    {
        return new SliderEntity
        {
            Title = title,
            Description = description,
            ImageUrl = imageUrl,
            Link = link,
            Order = order,
            IsActive = isActive
        };
    }

    private static NewsEntity CreateNews(string slug, DateTime date, string title, string description, string image, IEnumerable<string>? photos = null)
    {
        return new NewsEntity
        {
            Slug = slug,
            Date = date,
            Title = title,
            Description = description,
            Image = image,
            PhotosJson = JsonSerializer.Serialize(photos ?? Array.Empty<string>(), JsonOptions)
        };
    }

    private static PageContentEntity CreatePageContent(string key, string title, string subtitle, IEnumerable<string> paragraphs, string? imageUrl = null, string? mapEmbedUrl = null, IEnumerable<ContactDetail>? contactDetails = null)
    {
        return new PageContentEntity
        {
            Key = key,
            Title = title,
            Subtitle = subtitle,
            ParagraphsJson = JsonSerializer.Serialize(paragraphs, JsonOptions),
            ImageUrl = imageUrl,
            MapEmbedUrl = mapEmbedUrl,
            ContactDetailsJson = contactDetails is null ? null : JsonSerializer.Serialize(contactDetails, JsonOptions)
        };
    }

    private static GalleryFolderEntity CreateFolder(string title, string slug, string coverImage, int count, string date)
    {
        return new GalleryFolderEntity
        {
            Title = title,
            Slug = slug,
            CoverImage = coverImage,
            ImageCount = count,
            Date = date
        };
    }

    private static GalleryImageEntity[] BuildGalleryImages(GalleryFolderEntity[] folders)
    {
        var list = new List<GalleryImageEntity>();

        int folderIndex = 1;
        foreach (var folder in folders)
        {
            for (var i = 0; i < folder.ImageCount; i++)
            {
                var imageId = folderIndex * 10 + i;
                list.Add(new GalleryImageEntity
                {
                    FolderId = folder.Id,
                    Url = $"https://picsum.photos/id/{imageId}/1200/800",
                    ThumbnailUrl = $"https://picsum.photos/id/{imageId}/400/300",
                    Title = $"{folder.Title} - {i + 1}"
                });
            }
            folderIndex++;
        }

        return list.ToArray();
    }

    private static KvkkDocumentEntity CreateKvkk(string title, string type, string url)
    {
        return new KvkkDocumentEntity
        {
            Title = title,
            Type = type,
            FileUrl = url
        };
    }

    private static MunicipalUnitEntity CreateMunicipalUnit(string slug, string title, string? content, string icon, IEnumerable<UnitStaff> staff)
    {
        return new MunicipalUnitEntity
        {
            Slug = slug,
            Title = title,
            Content = content,
            Icon = icon,
            StaffJson = JsonSerializer.Serialize(staff, JsonOptions)
        };
    }
}
