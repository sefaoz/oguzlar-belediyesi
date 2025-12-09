import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { PageContainerComponent, BreadcrumbStep } from '../../shared/components/page-container/page-container';
import { PageContentModel } from '../../shared/models/page-content.model';
import { PageContentService } from '../../shared/services/page-content.service';
import { SeoService } from '../../shared/services/seo.service';
import { DomSanitizer, SafeHtml } from '@angular/platform-browser';

import { ImageUrlPipe } from '../../shared/pipes/image-url.pipe';

@Component({
  selector: 'app-baskan-mesaji',
  standalone: true,
  imports: [CommonModule, PageContainerComponent, ImageUrlPipe],
  templateUrl: './baskan-mesaji.html',
  styleUrl: './baskan-mesaji.css',
})
export class BaskanMesaji implements OnInit {
  breadcrumbSteps: BreadcrumbStep[] = [
    { label: 'Anasayfa', url: '/' },
    { label: 'Başkanımız' },
    { label: 'Başkanın Mesajı', url: '/baskandan-mesaj' }
  ];
  content?: PageContentModel;
  safeContent: SafeHtml | null = null;

  constructor(
    private readonly pageContentService: PageContentService,
    private readonly seoService: SeoService,
    private readonly sanitizer: DomSanitizer
  ) { }

  ngOnInit(): void {
    this.pageContentService.getPageContent('baskan-mesaji').subscribe(content => {
      this.content = content;
      if (this.content?.paragraphs) {
        this.safeContent = this.sanitizer.bypassSecurityTrustHtml(this.content.paragraphs.join(''));
      }
      this.seoService.updateSeo({
        title: 'Başkanın Mesajı',
        description: 'Oğuzlar Belediye Başkanı\'nın halka mesajı ve gelecek vizyonu.',
        slug: 'baskandan-mesaj'
      });
    });
  }
}
