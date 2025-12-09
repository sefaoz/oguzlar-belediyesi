import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, RouterModule } from '@angular/router';
import { PageContainerComponent, BreadcrumbStep } from '../../shared/components/page-container/page-container';
import { NewsItem } from '../../shared/models/news.model';
import { NewsService } from '../../shared/services/news.service';
import { SeoService } from '../../shared/services/seo.service';
import { environment } from '../../../environments/environment';

@Component({
  selector: 'app-haber-detay',
  standalone: true,
  imports: [CommonModule, PageContainerComponent, RouterModule],
  templateUrl: './haber-detay.html',
  styleUrl: './haber-detay.css',
})
export class HaberDetayComponent implements OnInit {
  newsItem?: NewsItem;
  breadcrumbSteps: BreadcrumbStep[] = [];

  constructor(
    private readonly route: ActivatedRoute,
    private readonly newsService: NewsService,
    private readonly seoService: SeoService
  ) { }

  ngOnInit(): void {
    this.route.paramMap.subscribe(params => {
      const slug = params.get('slug');
      if (slug) {
        this.newsService.getNewsBySlug(slug).subscribe({
          next: item => {
            this.newsItem = item;
            this.updateBreadcrumbs();
            if (item) {
              this.seoService.updateSeo({
                title: item.title,
                description: item.description,
                image: item.image,
                slug: `haberler/${item.slug}`,
                type: 'article'
              });
            }
          },
          error: () => {
            this.newsItem = undefined;
            this.breadcrumbSteps = [];
          }
        });
      }
    });
  }

  updateBreadcrumbs() {
    this.breadcrumbSteps = [
      { label: 'Anasayfa', url: '/' },
      { label: 'Haberler', url: '/haberler' },
      { label: this.newsItem?.title || 'Haber Detayı', url: `/haberler/${this.newsItem?.slug}` }
    ];
  }

  get readingTime(): number {
    if (!this.newsItem?.description) return 1;

    // HTML etiketlerini temizle (eğer varsa) ve kelimeleri say
    const text = this.newsItem.description.replace(/<[^>]*>/g, '');
    const wordCount = text.trim().split(/\s+/).length;

    // Ortalama 200 kelime/dakika
    const minutes = Math.ceil(wordCount / 200);
    return minutes > 0 ? minutes : 1;
  }

  // Lightbox Logic
  isLightboxOpen = false;
  currentLightboxImageIndex = 0;

  openLightbox(index: number) {
    this.currentLightboxImageIndex = index;
    this.isLightboxOpen = true;
    document.body.style.overflow = 'hidden'; // Prevent scrolling
  }

  closeLightbox() {
    this.isLightboxOpen = false;
    document.body.style.overflow = ''; // Restore scrolling
  }

  nextImage(event?: Event) {
    event?.stopPropagation();
    if (this.newsItem?.photos) {
      this.currentLightboxImageIndex = (this.currentLightboxImageIndex + 1) % this.newsItem.photos.length;
    }
  }

  prevImage(event?: Event) {
    event?.stopPropagation();
    if (this.newsItem?.photos) {
      this.currentLightboxImageIndex = (this.currentLightboxImageIndex - 1 + this.newsItem.photos.length) % this.newsItem.photos.length;
    }
  }

  // Helper to get current image URL safely
  get currentLightboxImage(): string {
    const url = this.newsItem?.photos?.[this.currentLightboxImageIndex] || '';
    return this.getImageUrl(url);
  }

  getImageUrl(url: string): string {
    if (!url) return '';
    if (url.startsWith('http')) return url;
    return `${environment.imageBaseUrl}${url}`;
  }
}
