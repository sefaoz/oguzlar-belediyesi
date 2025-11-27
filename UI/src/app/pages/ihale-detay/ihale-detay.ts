import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, RouterModule } from '@angular/router';
import { PageContainerComponent, BreadcrumbStep } from '../../shared/components/page-container/page-container';
import { Tender } from '../../shared/components/tender-card/tender-card';
import { TenderService } from '../../shared/services/tender.service';

@Component({
  selector: 'app-ihale-detay',
  standalone: true,
  imports: [CommonModule, PageContainerComponent, RouterModule],
  templateUrl: './ihale-detay.html',
  styleUrl: './ihale-detay.css',
})
export class IhaleDetay implements OnInit {
  tender?: Tender;
  breadcrumbSteps: BreadcrumbStep[] = [];

  constructor(
    private readonly route: ActivatedRoute,
    private readonly tenderService: TenderService
  ) { }

  ngOnInit(): void {
    this.route.paramMap.subscribe(params => {
      const slug = params.get('slug');
      if (slug) {
        this.tenderService.getTenderBySlug(slug).subscribe({
          next: item => {
            this.tender = item;
            this.updateBreadcrumbs();
          },
          error: () => {
            this.tender = undefined;
            this.breadcrumbSteps = [];
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

  getStatusLabel(status: string): string {
    switch (status) {
      case 'active': return 'Yayında';
      case 'passive': return 'Pasif';
      case 'completed': return 'Sonuçlandı';
      default: return 'Bilinmiyor';
    }
  }
}
