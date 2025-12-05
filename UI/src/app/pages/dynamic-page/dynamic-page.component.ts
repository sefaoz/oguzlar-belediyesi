import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, RouterModule } from '@angular/router';
import { PageContentService } from '../../services/page-content.service';
import { PageContent } from '../../models/page-content';
import { DomSanitizer, SafeHtml } from '@angular/platform-browser';

@Component({
    selector: 'app-dynamic-page',
    standalone: true,
    imports: [CommonModule, RouterModule],
    templateUrl: './dynamic-page.component.html'
})
export class DynamicPageComponent implements OnInit {
    pageContent: PageContent | null = null;
    loading: boolean = true;
    safeContent: SafeHtml | null = null;

    constructor(
        private route: ActivatedRoute,
        private pageContentService: PageContentService,
        public sanitizer: DomSanitizer
    ) { }

    ngOnInit() {
        this.route.params.subscribe(params => {
            const slug = params['slug'];
            if (slug) {
                this.loadPageContent(slug);
            }
        });
    }

    loadPageContent(key: string) {
        this.loading = true;
        this.pageContentService.getByKey(key).subscribe({
            next: (data) => {
                this.pageContent = data;
                if (this.pageContent.paragraphs && this.pageContent.paragraphs.length > 0) {
                    // Combine paragraphs and sanitize
                    const combinedContent = this.pageContent.paragraphs.join('');
                    this.safeContent = this.sanitizer.bypassSecurityTrustHtml(combinedContent);
                }
                this.loading = false;
            },
            error: (err) => {
                console.error('Sayfa yüklenirken hata oluştu:', err);
                this.loading = false;
            }
        });
    }
}
