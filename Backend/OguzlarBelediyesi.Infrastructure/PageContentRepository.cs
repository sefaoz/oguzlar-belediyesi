using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using OguzlarBelediyesi.Application;
using OguzlarBelediyesi.Domain;

namespace OguzlarBelediyesi.Infrastructure;

public sealed class PageContentRepository : IPageContentRepository
{
    private static readonly IReadOnlyDictionary<string, PageContent> Pages =
        new Dictionary<string, PageContent>(StringComparer.OrdinalIgnoreCase)
        {
            ["baskan-hakkinda"] = new PageContent(
                Title: "Mustafa CEBECİ",
                Subtitle: "Oğuzlar Belediye Başkanı",
                Paragraphs: new[]
                {
                    "1980 yılında Çorum'un Oğuzlar ilçesinde doğdu. İlk, orta ve lise öğrenimini Oğuzlar'da tamamladı.",
                    "Anadolu Üniversitesi İşletme Fakültesi'nden mezun oldu. Siyasi hayatına genç yaşlarda başlayan Cebeci, ilçemizin kalkınması ve gelişmesi için çeşitli sivil toplum kuruluşlarında aktif görevler aldı.",
                    "31 Mart 2024 yerel seçimlerinde Oğuzlar halkının teveccühü ile Belediye Başkanı seçildi. Evli ve 3 çocuk babasıdır.",
                    "Göreve geldiği günden bu yana \"Daha Yaşanabilir Bir Oğuzlar\" vizyonuyla çalışmalarını sürdürmekte, şeffaf, katılımcı ve halk odaklı belediyecilik anlayışını benimsemektedir."
                },
                ImageUrl: "assets/images/mustafa_cebeci.jpg"
            ),
            ["baskan-mesaji"] = new PageContent(
                Title: "Kıymetli Hemşehrilerim",
                Subtitle: "Başkanın Mesajı",
                Paragraphs: new[]
                {
                    "İsmini oğuz boyundan alan güzel ilçemiz OĞUZLAR, tarihiyle, eşsiz doğasıyla ve insanıyla bölgemizin gözde ilçelerinden biridir. Çorum'un önemli değeri olan OĞUZLAR’da, hayatın her alanında en iyiye ulaşmak için yola çıktık.",
                    "İlçemizin değerlerine sahip çıkmak, her bir insanımızın yüzünü güldürmek ve hizmetlerimizi en doğru ve ulaşılır şekilde yapmak için buradayız.",
                    "Bu kutlu vazifeyi ifa ederken, siz değerli halkımızdan gördüğümüz güven ve destek en büyük gücümüz olacaktır.",
                    "Bir şehrin Belediye Başkanı aynı zamanda o şehrin “Şehrül-Emin”idir. Üzerimizdeki bu sorumluluğun bilinciyle, görevimizi çift taraflı dayanışma anlayışı ile icra edeceğiz.",
                    "Halkımızın bize emanet ettiği bu güzel OĞUZLAR’da, daha güzel ve mutlu yarınlarda buluşmak dileği ile en kalbi muhabbetlerimi sunuyorum."
                },
                ImageUrl: "assets/images/mustafa_cebeci.jpg"
            ),
            ["baskana-mesaj"] = new PageContent(
                Title: "Başkana Mesaj Yaz",
                Subtitle: "Görüşlerinizi ve önerilerinizi bize iletin",
                Paragraphs: new[]
                {
                    "Oğuzlar Belediyesi olarak her görüşü, fikri ve öneriyi kıymetli buluyoruz. Bu form üzerinden Başkanımıza mesajınızı doğrudan ulaştırabilirsiniz.",
                    "Mesajlarınız, ekiplerimiz tarafından titizlikle incelenecek ve en kısa sürede kişisel geri dönüş yapılmaya çalışılacaktır.",
                        "KVKK onayını verdikten sonra mesajınızı iletmeyi unutmayınız."
                }
            ),
            ["home-baskan-mesaji"] = new PageContent(
                Title: "Değerli Oğuzlarlı Hemşehrilerim",
                Subtitle: "Belediye Başkanından",
                Paragraphs: new[]
                {
                    "Bu kutlu vazifeyi ifa ederken siz değerli halkımızdan gördüğümüz güven ve destek en büyük gücümüz olacaktır.",
                    "Daha yaşanabilir ve kıymetini bilen bir Oğuzlar için; her alanın birbiriyle uyumlu çalıştığı, insanına değer veren bir belediyecilik anlayışı sergiliyor; şeffaf, katılımcı ve hizmet odaklı olmanın kıymetini biliyoruz.",
                    "İlçemizin güzelliğini korumak, insanımızın yaşam kalitesini yükseltmek ve gelecek nesillere güçlü bir Oğuzlar bırakmak için yorulmadan çalışıyoruz."
                },
                ImageUrl: "assets/images/mustafa_cebeci.jpg"
            ),
            ["cografi-yapi"] = new PageContent(
                Title: "Coğrafi Yapı",
                Subtitle: "Doğal ve kültürel zenginliklerimiz",
                Paragraphs: new[]
                {
                    "Oğuzlar İlçesi Karadeniz Bölgesi'nin Orta Karadeniz Bölümünde yer alır; Çorum il merkezinin 63 km kuzeybatısındadır ve rakımı 650 metredir.",
                    "İlçemizde ana kaya genelde kalker, andezit ve gnays olup yer yer metamorfik (filit) ve efrişik kayaçlara da rastlanmaktadır.",
                    "Kızılırmak’ın etkisiyle oluşan vadiler, kıvrımlı yamaçlar ve geçiş iklimi sayesinde zengin bitki örtüsü gelişmiştir; sahil şeridinden iç kısımlara kadar geniş bir flora çeşitliliği göze çarpmaktadır.",
                    "Karasal iklimden izler taşıyan Oğuzlar, doğa tutkunları için hem yeşili hem de kültürel mirası bir arada sunar."
                },
                ImageUrl: "https://picsum.photos/1600/900?random=64"
            ),
            ["iletisim"] = new PageContent(
                Title: "İletişim",
                Subtitle: "2 adımda bize ulaşın",
                Paragraphs: new[]
                {
                    "Görüş, öneri, istek veya şikayetlerinizi Başkanımıza iletebilirsiniz. İletişim bilgileriniz gizli tutulacaktır.",
                    "Formu doldurduktan sonra ekiplerimiz en kısa sürede sizinle iletişime geçecektir."
                },
                MapEmbedUrl: "https://maps.google.com/maps?q=Oğuzlar+Belediyesi+Çorum&t=&z=15&ie=UTF8&iwloc=&output=embed",
                ContactDetails: new[]
                {
                    new ContactDetail("Adres", "Karadonlu Mahallesi Fatih Caddesi No:33/21 Oğuzlar/Çorum"),
                    new ContactDetail("Telefon", "+90 364 561 70 45"),
                    new ContactDetail("Fax", "+90 364 561 21 50"),
                    new ContactDetail("E-posta", "oguzlarbelediyesi@hotmail.com")
                }
            ),
            ["tarihimiz"] = new PageContent(
                Title: "İlçemizin Tarihi",
                Subtitle: "Geçmişten bugüne Oğuzlar",
                Paragraphs: new[]
                {
                    "Büyük Selçuklular, 1071 Malazgirt Meydan Muharebesi sonrası Anadolu’ya yerleşmeye başladıklarında Oğuzlar ve çevresini çeşitli tarihlerde iskan etmişlerdir.",
                    "Karabörk Divanı olarak da bilinen bu coğrafyada Çorum, İskilip, Osmancık gibi yerleşim birimlerinde Oğuzlar boyundan gelen birçok köy ve mevkii vardır.",
                    "1576 tarihi itibariyle Karabörk Nahiyesi’ni oluşturan köylerin nüfusu 6.340 kişi civarındaydı ve bu bölge sosyal, kültürel olarak büyük bir hareket alanına sahipti.",
                    "Kızılırmak’ın kıvrımlı vadileri, yüksek dağlarla çevrili alanları ve kendine yetebilen yerleşim birimi olması; Oğuzlar halkının binlerce yıllık kültürünün korunmasına hizmet etmiştir.",
                    "Giyim, kuşam, dil ve adetlerde görülen farklılıklar bu bölgenin içe kapalı ama kendine özgü zengin bir yaşam alanı olduğunun göstergesidir."
                },
                ImageUrl: "https://picsum.photos/1600/900?random=66"
            )
        };

    public Task<PageContent?> GetByKeyAsync(string key)
    {
        Pages.TryGetValue(key, out var content);
        return Task.FromResult(content);
    }
}
