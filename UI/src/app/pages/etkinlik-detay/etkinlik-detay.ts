import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, RouterModule } from '@angular/router';
import { PageContainerComponent, BreadcrumbStep } from '../../shared/components/page-container/page-container';
import { EventItem } from '../../shared/models/event.model';
import { EventService } from '../../shared/services/event.service';
import { SeoService } from '../../shared/services/seo.service';

import { ImageUrlPipe } from '../../shared/pipes/image-url.pipe';

@Component({
  selector: 'app-etkinlik-detay',
  standalone: true,
  imports: [CommonModule, PageContainerComponent, RouterModule, ImageUrlPipe],
  templateUrl: './etkinlik-detay.html',
  styleUrl: './etkinlik-detay.css',
})
export class EtkinlikDetay implements OnInit {
  event?: EventItem;
  breadcrumbSteps: BreadcrumbStep[] = [];

  constructor(
    private readonly route: ActivatedRoute,
    private readonly eventService: EventService,
    private readonly seoService: SeoService
  ) { }

  ngOnInit(): void {
    this.route.paramMap.subscribe(params => {
      const slug = params.get('slug');
      if (slug) {
        this.eventService.getEventBySlug(slug).subscribe({
          next: item => {
            this.event = item;
            if (this.event && this.event.description) {
              this.event.description = this.decodeHtml(this.event.description);
            }
            this.updateBreadcrumbs();
            if (item) {
              this.seoService.updateSeo({
                title: item.title,
                description: item.description,
                image: item.image,
                slug: `etkinlikler/${item.slug}`,
                type: 'article'
              });
            }
          },
          error: () => {
            this.event = undefined;
            this.breadcrumbSteps = [];
          }
        });
      }
    });
  }

  private decodeHtml(html: string): string {
    const doc = new DOMParser().parseFromString(html, 'text/html');
    return doc.documentElement.textContent || '';
  }

  updateBreadcrumbs() {
    this.breadcrumbSteps = [
      { label: 'Anasayfa', url: '/' },
      { label: 'Etkinlikler', url: '/etkinlikler' },
      { label: this.event?.title || 'Etkinlik Detayı', url: `/etkinlikler/${this.event?.slug}` }
    ];
  }

  share(platform: 'facebook' | 'twitter' | 'whatsapp') {
    const url = encodeURIComponent(window.location.href);
    const title = encodeURIComponent(this.event?.title || document.title);

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
