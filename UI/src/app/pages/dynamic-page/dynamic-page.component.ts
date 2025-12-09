import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, RouterModule } from '@angular/router';
import { PageContentService } from '../../services/page-content.service';
import { PageContent } from '../../models/page-content';
import { DomSanitizer, SafeHtml, SafeResourceUrl } from '@angular/platform-browser';

import { ImageUrlPipe } from '../../shared/pipes/image-url.pipe';

@Component({
    selector: 'app-dynamic-page',
    standalone: true,
    imports: [CommonModule, RouterModule, ImageUrlPipe],
    templateUrl: './dynamic-page.component.html'
})
export class DynamicPageComponent implements OnInit {
    pageContent: PageContent | null = null;
    loading: boolean = true;
    safeContent: string | SafeHtml | null = null;

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
                    // Combine paragraphs
                    // Angular will automatically sanitize this content when bound to [innerHTML]
                    // We DO NOT bypass security here to prevent XSS
                    this.safeContent = this.pageContent.paragraphs.join('');
                }
                this.loading = false;
            },
            error: (err) => {
                console.error('Sayfa yüklenirken hata oluştu:', err);
                this.loading = false;
            }
        });
    }

    getSafeMapUrl(url: string): SafeResourceUrl | null {
        if (!url) return null;
        // Only allow Google Maps embeds
        if (url.startsWith('https://www.google.com/maps') || url.startsWith('https://maps.google.com')) {
            return this.sanitizer.bypassSecurityTrustResourceUrl(url);
        }
        return null; // Block unsafe URLs
    }
}
