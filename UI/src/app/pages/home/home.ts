import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { NewsCard, NewsItem } from '../../shared/components/news-card/news-card';

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [CommonModule, NewsCard],
  templateUrl: './home.html',
  styleUrls: ['./home.css']
})
export class HomeComponent {
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
    }
  ];
}
