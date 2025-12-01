import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { PageContainerComponent, BreadcrumbStep } from '../../shared/components/page-container/page-container';
import { ContentSidebarComponent } from '../../shared/components/content-sidebar/content-sidebar';
import { TenderCardComponent } from '../../shared/components/tender-card/tender-card';
import { TenderService } from '../../shared/services/tender.service';
import { Tender } from '../../shared/models/tender.model';
import { SeoService } from '../../shared/services/seo.service';

@Component({
  selector: 'app-ihaleler',
  standalone: true,
  imports: [CommonModule, PageContainerComponent, ContentSidebarComponent, TenderCardComponent],
  templateUrl: './ihaleler.html',
  styleUrl: './ihaleler.css'
})
export class IhalelerComponent {
  breadcrumbSteps: BreadcrumbStep[] = [
    { label: 'Anasayfa', url: '/' },
    { label: 'İhaleler', url: '/ihaleler' }
  ];

  tenders: Tender[] = [];

  constructor(
    private readonly tenderService: TenderService,
    private readonly seoService: SeoService
  ) { }

  ngOnInit() {
    this.tenderService.getTenders().subscribe(items => {
      this.tenders = items;
    });

    this.seoService.updateSeo({
      title: 'İhaleler',
      description: 'Oğuzlar Belediyesi güncel ihaleler, sonuçlanan ihaleler ve şartnameler.',
      slug: 'ihaleler'
    });
  }
}
