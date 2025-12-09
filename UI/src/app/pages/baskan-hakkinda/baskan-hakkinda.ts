import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { PageContainerComponent, BreadcrumbStep } from '../../shared/components/page-container/page-container';
import { PageContentModel } from '../../shared/models/page-content.model';
import { PageContentService } from '../../shared/services/page-content.service';
import { SeoService } from '../../shared/services/seo.service';
import { DomSanitizer, SafeHtml } from '@angular/platform-browser';

import { ImageUrlPipe } from '../../shared/pipes/image-url.pipe';

@Component({
  selector: 'app-baskan-hakkinda',
  standalone: true,
  imports: [CommonModule, PageContainerComponent, ImageUrlPipe],
  templateUrl: './baskan-hakkinda.html',
  styleUrls: ['./baskan-hakkinda.css']
})
export class BaskanHakkindaComponent implements OnInit {
  breadcrumbSteps: BreadcrumbStep[] = [
    { label: 'Anasayfa', url: '/' },
    { label: 'Başkanımız' },
    { label: 'Başkan Hakkında', url: '/baskan-hakkinda' }
  ];
  content?: PageContentModel;
  safeContent: SafeHtml | null = null;

  constructor(
    private readonly pageContentService: PageContentService,
    private readonly seoService: SeoService,
    private readonly sanitizer: DomSanitizer
  ) { }

  ngOnInit(): void {
    this.pageContentService.getPageContent('baskan-hakkinda').subscribe(content => {
      this.content = content;
      if (this.content?.paragraphs) {
        const decodedParagraphs = this.content.paragraphs.map(p => this.decodeHtml(p));
        this.safeContent = this.sanitizer.bypassSecurityTrustHtml(decodedParagraphs.join(''));
      }
      this.seoService.updateSeo({
        title: 'Başkan Hakkında',
        description: 'Oğuzlar Belediye Başkanı hakkında bilgiler, özgeçmiş ve vizyon.',
        slug: 'baskan-hakkinda'
      });
    });
  }

  private decodeHtml(html: string): string {
    const doc = new DOMParser().parseFromString(html, 'text/html');
    return doc.documentElement.textContent || '';
  }
}
