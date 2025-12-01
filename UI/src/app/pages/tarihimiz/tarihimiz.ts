import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { PageContainerComponent, BreadcrumbStep } from '../../shared/components/page-container/page-container';
import { PageContentModel } from '../../shared/models/page-content.model';
import { PageContentService } from '../../shared/services/page-content.service';
import { SeoService } from '../../shared/services/seo.service';

@Component({
  selector: 'app-tarihimiz',
  standalone: true,
  imports: [CommonModule, PageContainerComponent],
  templateUrl: './tarihimiz.html',
  styleUrl: './tarihimiz.css',
})
export class TarihimizComponent implements OnInit {
  breadcrumbSteps: BreadcrumbStep[] = [
    { label: 'Anasayfa', url: '/' },
    { label: "Oğuzlar'ı Keşfet" },
    { label: 'İlçemizin Tarihi', url: '/tarihimiz' }
  ];
  content?: PageContentModel;

  constructor(
    private readonly pageContentService: PageContentService,
    private readonly seoService: SeoService
  ) { }

  ngOnInit(): void {
    this.pageContentService.getPageContent('tarihimiz').subscribe(content => {
      this.content = content;
      this.seoService.updateSeo({
        title: 'İlçemizin Tarihi',
        description: 'Oğuzlar ilçesinin tarihi geçmişi, kültürel mirası ve önemli olayları.',
        slug: 'ilcemizin-tarihi'
      });
    });
  }
}
