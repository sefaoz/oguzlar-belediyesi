import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { NewsCard, NewsItem } from '../../shared/components/news-card/news-card';
import { PageContainerComponent } from '../../shared/components/page-container/page-container';

import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-haberler',
  standalone: true,
  imports: [CommonModule, NewsCard, PageContainerComponent, FormsModule],
  templateUrl: './haberler.html',
  styleUrl: './haberler.css',
})
export class HaberlerComponent {
  breadcrumbSteps = [
    { label: 'Anasayfa', url: '/' },
    { label: 'Haberler', url: '/haberler' }
  ];

  searchTerm: string = '';

  newsList: NewsItem[] = [
    {
      image: 'https://picsum.photos/400/250?image=20',
      date: '12 Kasım 2025',
      title: 'Yeni Park Alanı Halkın Hizmetine Açıldı',
      description: 'Çocuklar ve aileler için tasarlanan modern oyun ve dinlenme alanları büyük beğeni topladı.',
      link: '#'
    },
    {
      image: 'https://picsum.photos/400/250?image=30',
      date: '11 Kasım 2025',
      title: 'Altyapı Yenileme Çalışmaları Tamamlandı',
      description: 'İlçemizin ana caddelerindeki su ve kanalizasyon hatları tamamen yenilendi.',
      link: '#'
    },
    {
      image: 'https://picsum.photos/400/250?image=40',
      date: '10 Kasım 2025',
      title: 'Sokak Hayvanları İçin Yeni Barınak',
      description: 'Modern ve hijyenik koşullara sahip yeni hayvan barınağımız hizmete girdi.',
      link: '#'
    },
    {
      image: 'https://picsum.photos/400/250?image=50',
      date: '08 Kasım 2025',
      title: 'Kültür Merkezi İnşaatı Hızla Devam Ediyor',
      description: 'İlçemize değer katacak yeni kültür merkezinin kaba inşaatı tamamlandı.',
      link: '#'
    },
    {
      image: 'https://picsum.photos/400/250?image=60',
      date: '05 Kasım 2025',
      title: 'Geleneksel Oğuzlar Festivali Başlıyor',
      description: 'Bu yıl 15.si düzenlenecek olan festivalimiz renkli görüntülere sahne olacak.',
      link: '#'
    },
    {
      image: 'https://picsum.photos/400/250?image=70',
      date: '01 Kasım 2025',
      title: 'Belediyemizden Öğrencilere Kırtasiye Desteği',
      description: 'İlçemizdeki ihtiyaç sahibi öğrencilere kırtasiye seti dağıtımı yapıldı.',
      link: '#'
    }
  ];

  get filteredNews(): NewsItem[] {
    if (!this.searchTerm) {
      return this.newsList;
    }
    const term = this.searchTerm.toLowerCase();
    return this.newsList.filter(news =>
      news.title.toLowerCase().includes(term) ||
      news.description.toLowerCase().includes(term)
    );
  }
}
