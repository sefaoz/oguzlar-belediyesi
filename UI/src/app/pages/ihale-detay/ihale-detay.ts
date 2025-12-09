import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, RouterModule } from '@angular/router';
import { PageContainerComponent, BreadcrumbStep } from '../../shared/components/page-container/page-container';
import { Tender, TenderDocument } from '../../shared/models/tender.model';
import { TenderService } from '../../shared/services/tender.service';
import { SeoService } from '../../shared/services/seo.service';
import { environment } from '../../../environments/environment';

@Component({
  selector: 'app-ihale-detay',
  standalone: true,
  imports: [CommonModule, PageContainerComponent, RouterModule],
  templateUrl: './ihale-detay.html',
  styleUrl: './ihale-detay.css',
})
export class IhaleDetay implements OnInit {
  tender?: Tender;
  documents: TenderDocument[] = [];
  breadcrumbSteps: BreadcrumbStep[] = [];

  constructor(
    private readonly route: ActivatedRoute,
    private readonly tenderService: TenderService,
    private readonly seoService: SeoService
  ) { }

  ngOnInit(): void {
    this.route.paramMap.subscribe(params => {
      const slug = params.get('slug');
      if (slug) {
        this.tenderService.getTenderBySlug(slug).subscribe({
          next: item => {
            this.tender = item;
            this.updateBreadcrumbs();
            this.parseDocuments();
            if (item) {
              this.seoService.updateSeo({
                title: item.title,
                description: item.description,
                slug: `ihaleler/${item.slug}`,
                type: 'article'
              });
            }
          },
          error: () => {
            this.tender = undefined;
            this.breadcrumbSteps = [];
            this.documents = [];
          }
        });
      }
    });
  }

  updateBreadcrumbs() {
    this.breadcrumbSteps = [
      { label: 'Anasayfa', url: '/' },
      { label: 'İhaleler', url: '/ihaleler' },
      { label: this.tender?.title || 'İhale Detayı', url: `/ihaleler/${this.tender?.slug}` }
    ];
  }

  parseDocuments() {
    if (this.tender?.documentsJson) {
      try {
        this.documents = JSON.parse(this.tender.documentsJson);
      } catch (e) {
        console.error('Error parsing documents JSON', e);
        this.documents = [];
      }
    } else {
      this.documents = [];
    }
  }

  getFileUrl(url: string): string {
    if (!url) return '';
    if (url.startsWith('http')) return url;
    // Ensure there is a slash between base and url
    const baseUrl = environment.imageBaseUrl.endsWith('/') ? environment.imageBaseUrl : `${environment.imageBaseUrl}/`;
    const cleanUrl = url.startsWith('/') ? url.substring(1) : url;
    return `${baseUrl}${cleanUrl}`;
  }

  getFileIcon(fileName: string): string {
    const ext = fileName.split('.').pop()?.toLowerCase();
    if (ext === 'pdf') return 'far fa-file-pdf';
    if (ext === 'doc' || ext === 'docx') return 'far fa-file-word';
    if (ext === 'xls' || ext === 'xlsx') return 'far fa-file-excel';
    return 'far fa-file-alt';
  }

  getFileColorClass(fileName: string): string {
    const ext = fileName.split('.').pop()?.toLowerCase();
    if (ext === 'pdf') return 'bg-red-100 text-red-500';
    if (ext === 'doc' || ext === 'docx') return 'bg-blue-100 text-blue-500';
    if (ext === 'xls' || ext === 'xlsx') return 'bg-green-100 text-green-500';
    return 'bg-gray-100 text-gray-500';
  }

  getStatusLabel(status: string): string {
    const s = status?.toLowerCase();
    if (s === 'open' || s === 'active' || s === 'yayında') return 'Yayında';
    if (s === 'closed' || s === 'passive' || s === 'pasif') return 'Pasif';
    if (s === 'completed' || s === 'sonuçlandı') return 'Sonuçlandı';
    return s || 'Bilinmiyor';
  }

  share(platform: 'facebook' | 'twitter' | 'whatsapp') {
    const url = encodeURIComponent(window.location.href);
    const title = encodeURIComponent(this.tender?.title || document.title);

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
