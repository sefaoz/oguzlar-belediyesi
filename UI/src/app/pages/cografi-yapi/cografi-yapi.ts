import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { PageContainerComponent, BreadcrumbStep } from '../../shared/components/page-container/page-container';
import { PageContentModel } from '../../shared/models/page-content.model';
import { PageContentService } from '../../shared/services/page-content.service';
import { SeoService } from '../../shared/services/seo.service';

import { ImageUrlPipe } from '../../shared/pipes/image-url.pipe';

@Component({
  selector: 'app-cografi-yapi',
  standalone: true,
  imports: [CommonModule, PageContainerComponent, ImageUrlPipe],
  templateUrl: './cografi-yapi.html',
  styleUrl: './cografi-yapi.css',
})
export class CografiYapi implements OnInit {
  breadcrumbSteps: BreadcrumbStep[] = [
    { label: 'Anasayfa', url: '/' },
    { label: "Oğuzlar'ı Keşfet" },
    { label: 'Coğrafi Yapı', url: '/cografi-yapi' }
  ];
  content?: PageContentModel;

  constructor(
    private readonly pageContentService: PageContentService,
    private readonly seoService: SeoService
  ) { }

  ngOnInit(): void {
    this.pageContentService.getPageContent('cografi-yapi').subscribe(content => {
      this.content = content;
      this.seoService.updateSeo({
        title: 'Coğrafi Yapı',
        description: 'Oğuzlar ilçesinin coğrafi yapısı, iklimi, bitki örtüsü ve doğal güzellikleri.',
        slug: 'cografi-yapi'
      });
    });
  }
}
