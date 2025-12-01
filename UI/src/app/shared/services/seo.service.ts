import { Injectable } from '@angular/core';
import { Meta, Title } from '@angular/platform-browser';
import { Router, NavigationEnd } from '@angular/router';
import { filter } from 'rxjs/operators';

export interface SeoConfig {
    title: string;
    description?: string;
    keywords?: string;
    image?: string;
    type?: string; // website, article, etc.
    slug?: string; // for canonical url
}

@Injectable({
    providedIn: 'root'
})
export class SeoService {
    private readonly defaultTitle = 'Oğuzlar Belediyesi';
    private readonly defaultDescription = 'Oğuzlar Belediyesi Resmi Web Sitesi. Haberler, duyurular, projeler ve belediye hizmetleri hakkında güncel bilgiler.';
    private readonly defaultImage = 'assets/images/logo.png'; // Varsayılan logo veya görsel
    private readonly baseUrl = 'https://oguzlar.bel.tr'; // Canlı domain buraya gelecek

    constructor(
        private readonly titleService: Title,
        private readonly metaService: Meta,
        private readonly router: Router
    ) {
        // Router değişikliklerini dinleyip canonical URL güncelleyebiliriz
        this.router.events.pipe(
            filter(event => event instanceof NavigationEnd)
        ).subscribe(() => {
            this.updateCanonicalUrl();
        });
    }

    updateSeo(config: SeoConfig) {
        // Title
        const title = config.title ? `${config.title} | ${this.defaultTitle}` : this.defaultTitle;
        this.titleService.setTitle(title);

        // Description
        const description = config.description || this.defaultDescription;
        this.metaService.updateTag({ name: 'description', content: description });

        // Keywords
        if (config.keywords) {
            this.metaService.updateTag({ name: 'keywords', content: config.keywords });
        }

        // Open Graph (Facebook, LinkedIn, etc.)
        this.metaService.updateTag({ property: 'og:title', content: title });
        this.metaService.updateTag({ property: 'og:description', content: description });
        this.metaService.updateTag({ property: 'og:type', content: config.type || 'website' });
        this.metaService.updateTag({ property: 'og:url', content: this.createUrl(config.slug) });
        this.metaService.updateTag({ property: 'og:image', content: config.image || this.defaultImage });
        this.metaService.updateTag({ property: 'og:site_name', content: this.defaultTitle });
        this.metaService.updateTag({ property: 'og:locale', content: 'tr_TR' });

        // Twitter
        this.metaService.updateTag({ name: 'twitter:card', content: 'summary_large_image' });
        this.metaService.updateTag({ name: 'twitter:title', content: title });
        this.metaService.updateTag({ name: 'twitter:description', content: description });
        this.metaService.updateTag({ name: 'twitter:image', content: config.image || this.defaultImage });

        // Canonical URL
        this.updateCanonicalUrl(config.slug);
    }

    private createUrl(slug?: string): string {
        if (slug) {
            // Slug başında / varsa kaldır, yoksa olduğu gibi kullan
            const cleanSlug = slug.startsWith('/') ? slug.substring(1) : slug;
            return `${this.baseUrl}/${cleanSlug}`;
        }
        return this.baseUrl + this.router.url;
    }

    private updateCanonicalUrl(slug?: string) {
        const url = this.createUrl(slug);
        let link: HTMLLinkElement | null = document.querySelector("link[rel='canonical']");
        if (!link) {
            link = document.createElement('link');
            link.setAttribute('rel', 'canonical');
            document.head.appendChild(link);
        }
        link.setAttribute('href', url);
    }
}
