import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, RouterModule } from '@angular/router';
import { PageContentService } from '../../services/page-content.service';
import { PageContent } from '../../models/page-content';
import { DomSanitizer, SafeHtml, SafeResourceUrl } from '@angular/platform-browser';
import { PageContainerComponent, BreadcrumbStep } from '../../shared/components/page-container/page-container';
import { MenuService } from '../../services/menu.service';
import { Menu } from '../../models/menu';
import { SeoService } from '../../shared/services/seo.service';

import { ImageUrlPipe } from '../../shared/pipes/image-url.pipe';

@Component({
    selector: 'app-dynamic-page',
    standalone: true,
    imports: [CommonModule, RouterModule, ImageUrlPipe, PageContainerComponent],
    templateUrl: './dynamic-page.component.html'
})
export class DynamicPageComponent implements OnInit {
    pageContent: PageContent | null = null;
    loading: boolean = true;
    safeContent: string | SafeHtml | null = null;
    breadcrumbSteps: BreadcrumbStep[] = [];
    pageTitle: string = '';

    constructor(
        private route: ActivatedRoute,
        private pageContentService: PageContentService,
        private menuService: MenuService,
        private seoService: SeoService,
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

        this.menuService.getAll().subscribe({
            next: (menus) => {
                const normalizedSlug = key.replace(/^\/+|\/+$/g, '');

                const currentMenu = menus.find(m => {
                    if (!m.url) return false;
                    const menuUrl = m.url.replace(/^\/+|\/+$/g, '');
                    return menuUrl === normalizedSlug || menuUrl.endsWith('/' + normalizedSlug);
                });

                if (!currentMenu) {
                    this.handleNotFound();
                    return;
                }

                this.pageContentService.getByKey(key).subscribe({
                    next: (data) => {
                        this.pageContent = data;
                        this.pageTitle = data.title;

                        this.seoService.updateSeo({
                            title: data.title,
                            description: data.subtitle || data.title,
                            slug: key
                        });

                        this.buildBreadcrumbs(menus, currentMenu);

                        if (this.pageContent.paragraphs && this.pageContent.paragraphs.length > 0) {
                            this.safeContent = this.pageContent.paragraphs.join('');
                        }
                        this.loading = false;
                    },
                    error: (err) => {
                        this.handleNotFound();
                    }
                });
            },
            error: (err) => {
                this.handleNotFound();
            }
        });
    }

    handleNotFound() {
        this.pageContent = null;
        this.pageTitle = 'Sayfa Bulunamadı';
        this.seoService.updateSeo({
            title: 'Sayfa Bulunamadı',
            description: 'Aradığınız sayfa bulunamadı.',
            slug: '404'
        });
        this.breadcrumbSteps = [];
        this.loading = false;
    }

    buildBreadcrumbs(menus: Menu[], currentMenu: Menu) {
        const steps: BreadcrumbStep[] = [];
        let current: Menu | undefined = currentMenu;

        while (current) {
            steps.unshift({
                label: current.title,
                url: current.url
            });

            if (current.parentId) {
                current = menus.find(m => m.id === current?.parentId);
            } else {
                current = undefined;
            }
        }

        this.breadcrumbSteps = [
            { label: 'Anasayfa', url: '/' },
            ...steps
        ];
    }

    getSafeMapUrl(url: string): SafeResourceUrl | null {
        if (!url) return null;
        if (url.startsWith('https://www.google.com/maps') || url.startsWith('https://maps.google.com')) {
            return this.sanitizer.bypassSecurityTrustResourceUrl(url);
        }
        return null;
    }
}
