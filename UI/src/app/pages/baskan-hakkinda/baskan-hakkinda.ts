import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { PageContainerComponent, BreadcrumbStep } from '../../shared/components/page-container/page-container';
import { PageContentModel } from '../../shared/models/page-content.model';
import { PageContentService } from '../../shared/services/page-content.service';
import { SeoService } from '../../shared/services/seo.service';
import { DomSanitizer, SafeHtml } from '@angular/platform-browser';

@Component({
  selector: 'app-baskan-hakkinda',
  standalone: true,
  imports: [CommonModule, PageContainerComponent],
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
        this.safeContent = this.sanitizer.bypassSecurityTrustHtml(this.content.paragraphs.join(''));
      }
      this.seoService.updateSeo({
        title: 'Başkan Hakkında',
        description: 'Oğuzlar Belediye Başkanı hakkında bilgiler, özgeçmiş ve vizyon.',
        slug: 'baskan-hakkinda'
      });
    });
  }
}
