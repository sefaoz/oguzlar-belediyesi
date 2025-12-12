import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, RouterModule } from '@angular/router';
import { PageContainerComponent, BreadcrumbStep } from '../../shared/components/page-container/page-container';
import { NewsItem } from '../../shared/models/news.model';
import { NewsService } from '../../shared/services/news.service';
import { SeoService } from '../../shared/services/seo.service';

import { ImageUrlPipe } from '../../shared/pipes/image-url.pipe';

@Component({
  selector: 'app-haber-detay',
  standalone: true,
  imports: [CommonModule, PageContainerComponent, RouterModule, ImageUrlPipe],
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

    const text = this.newsItem.description.replace(/<[^>]*>/g, '');
    const wordCount = text.trim().split(/\s+/).length;

    const minutes = Math.ceil(wordCount / 200);
    return minutes > 0 ? minutes : 1;
  }

  isLightboxOpen = false;
  currentLightboxImageIndex = 0;


  openLightbox(index: number) {
    this.currentLightboxImageIndex = index;
    this.isLightboxOpen = true;
    document.body.style.overflow = 'hidden';
  }

  closeLightbox() {
    this.isLightboxOpen = false;
    document.body.style.overflow = '';
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

  share(platform: 'facebook' | 'twitter' | 'whatsapp') {
    const url = encodeURIComponent(window.location.href);
    const title = encodeURIComponent(this.newsItem?.title || document.title);

    let shareUrl = '';

    switch (platform) {
      case 'facebook':
        shareUrl = `https://www.facebook.com/sharer/sharer.php?u=${url}`;
        break;
      case 'twitter':
        shareUrl = `https://twitter.com/intent/tweet?url=${url}&text=${title}`;
        break;
      case 'whatsapp':
        shareUrl = `https://api.whatsapp.com/send?text=${title}%20${url}`;
        break;
    }

    if (shareUrl) {
      window.open(shareUrl, '_blank', 'noopener,noreferrer');
    }
  }
}
