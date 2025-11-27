import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, of } from 'rxjs';
import { NewsItem } from '../components/news-card/news-card';
import { environment } from '../../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class NewsService {
  private dummyNews: NewsItem[] = [
    {
      title: 'Oğuzlar Belediyesi Yeni Hizmet Binası Açıldı',
      description: 'İlçemize kazandırdığımız yeni hizmet binamızın açılışını büyük bir coşkuyla gerçekleştirdik.',
      date: '25 Kasım 2025',
      image: 'https://picsum.photos/800/600?random=1',
      slug: 'yeni-hizmet-binasi',
      photos: [
        'https://picsum.photos/800/600?random=1',
        'https://picsum.photos/800/600?random=11',
        'https://picsum.photos/800/600?random=12'
      ]
    },
    {
      title: 'Ceviz Festivali Hazırlıkları Başladı',
      description: 'Geleneksel Oğuzlar Ceviz Festivali için geri sayım başladı. Bu yıl sürpriz sanatçılar bizlerle olacak.',
      date: '20 Kasım 2025',
      image: 'https://picsum.photos/800/600?random=2',
      slug: 'ceviz-festivali-hazirliklari',
      photos: [
        'https://picsum.photos/800/600?random=2',
        'https://picsum.photos/800/600?random=21'
      ]
    },
    {
      title: 'Obruk Barajı Çevresi Düzenleme Çalışmaları',
      description: 'Turizme kazandırma çalışmaları kapsamında baraj çevresinde peyzaj düzenlemeleri devam ediyor.',
      date: '15 Kasım 2025',
      image: 'https://picsum.photos/800/600?random=3',
      slug: 'obruk-baraji-duzenleme',
      photos: [
        'https://picsum.photos/800/600?random=3',
        'https://picsum.photos/800/600?random=31',
        'https://picsum.photos/800/600?random=32',
        'https://picsum.photos/800/600?random=33'
      ]
    },
    {
      title: 'Kış Spor Okulları Kayıtları',
      description: 'Gençlerimiz için kış spor okulları kayıtlarımız başlamıştır. Detaylı bilgi için kültür merkezimize bekliyoruz.',
      date: '10 Kasım 2025',
      image: 'https://picsum.photos/800/600?random=4',
      slug: 'kis-spor-okullari'
    },
    {
      title: 'Yol Bakım ve Onarım Çalışmaları',
      description: 'İlçe genelinde bozulan yolların bakım ve onarım çalışmaları fen işleri ekiplerimizce sürdürülüyor.',
      date: '05 Kasım 2025',
      image: 'https://picsum.photos/800/600?random=5',
      slug: 'yol-bakim-calismalari'
    },
    {
      title: '29 Ekim Cumhuriyet Bayramı Kutlaması',
      description: 'Cumhuriyetimizin kuruluşunun 102. yıl dönümünü meydanımızda coşkuyla kutladık.',
      date: '29 Ekim 2025',
      image: 'https://picsum.photos/800/600?random=6',
      slug: 'cumhuriyet-bayrami'
    }
  ];

  constructor(private readonly http: HttpClient) { }

  getNews(): Observable<NewsItem[]> {
    // For now return dummy data, later can switch to API
    return of(this.dummyNews);
    // return this.http.get<NewsItem[]>(environment.newsApiUrl);
  }

  getNewsBySlug(slug: string): Observable<NewsItem | undefined> {
    const news = this.dummyNews.find(n => n.slug === slug);
    return of(news);
    // return this.http.get<NewsItem>(`${environment.newsApiUrl}/${slug}`);
  }
}
