import { Component, OnInit, OnDestroy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { NewsCard } from '../../shared/components/news-card/news-card';
import { RouterModule } from '@angular/router';
import { PageContentModel } from '../../shared/models/page-content.model';
import { NewsService } from '../../shared/services/news.service';
import { PageContentService } from '../../shared/services/page-content.service';
import { AnnouncementService } from '../../shared/services/announcement.service';
import { EventService } from '../../shared/services/event.service';
import { TenderService } from '../../shared/services/tender.service';
import { SeoService } from '../../shared/services/seo.service';
import { NewsItem } from '../../shared/models/news.model';
import { Announcement } from '../../shared/models/announcement.model';
import { EventItem } from '../../shared/models/event.model';
import { Tender } from '../../shared/models/tender.model';
import { SliderService } from '../../services/slider.service';
import { Slider } from '../../models/slider';
import { GalleryService } from '../../shared/services/gallery.service';
import { GalleryFolder } from '../../shared/models/gallery.model';
import { ImageUrlPipe } from '../../shared/pipes/image-url.pipe';
import { SiteSettingsService } from '../../services/site-settings.service';

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [CommonModule, NewsCard, RouterModule, ImageUrlPipe],
  templateUrl: './home.html',
  styleUrls: ['./home.css']
})
export class HomeComponent implements OnInit, OnDestroy {
  newsList: NewsItem[] = [];
  announcements: Announcement[] = [];
  events: EventItem[] = [];
  tenders: Tender[] = [];
  presidentMessage?: PageContentModel;

  sliders: Slider[] = [];
  currentSlideIndex = 0;
  intervalId: any;

  featuredGalleries: GalleryFolder[] = [];
  eMunicipalityLinks: any[] = [];

  constructor(
    private readonly newsService: NewsService,
    private readonly pageContentService: PageContentService,
    private readonly announcementService: AnnouncementService,
    private readonly eventService: EventService,
    private readonly tenderService: TenderService,
    private readonly seoService: SeoService,
    private readonly sliderService: SliderService,
    private readonly galleryService: GalleryService,
    private readonly siteSettingsService: SiteSettingsService
  ) { }

  ngOnInit(): void {
    this.siteSettingsService.settings$.subscribe(() => {
      this.eMunicipalityLinks = this.siteSettingsService.getJsonSetting<any[]>('EMunicipality', 'Links') || [];
      const seo = this.siteSettingsService.getJsonSetting<any>('SEO', 'Global');
      if (seo) {
        this.seoService.updateSeo({
          title: seo.metaTitle || 'Ana Sayfa',
          description: seo.metaDescription || 'Oğuzlar Belediyesi resmi web sitesi. Haberler, projeler, etkinlikler ve belediye hizmetleri.',
          keywords: seo.metaKeywords || 'oğuzlar, belediye, çorum, oğuzlar belediyesi'
        });
      }
    });

    this.newsService.getNews().subscribe(news => {
      this.newsList = news.slice(0, 3);
    });

    this.announcementService.getAnnouncements().subscribe(items => {
      this.announcements = items.slice(0, 3);
    });

    this.galleryService.getFolders().subscribe(folders => {
      this.featuredGalleries = folders.filter(f => f.isActive && f.isFeatured).slice(0, 2);
    });

    this.eventService.getEvents({ upcomingOnly: true }).subscribe(items => {
      this.events = items.slice(0, 3);
    });

    this.tenderService.getTenders({ status: 'active' }).subscribe(items => {
      this.tenders = items.slice(0, 3);
    });

    this.pageContentService.getPageContent('home-baskan-mesaji').subscribe(content => {
      this.presidentMessage = content;
      if (this.presidentMessage?.paragraphs) {
        this.presidentMessage.paragraphs = this.presidentMessage.paragraphs.map(p => this.decodeHtml(p));
      }
    });

    this.sliderService.getSliders().subscribe(data => {
      this.sliders = data.filter(s => s.isActive).sort((a, b) => a.order - b.order);
      if (this.sliders.length > 0) {
        this.startAutoSlide();
      }
    });
    this.seoService.updateSeo({
      title: 'Ana Sayfa',
      description: 'Oğuzlar Belediyesi resmi web sitesi. Haberler, projeler, etkinlikler ve belediye hizmetleri.',
      keywords: 'oğuzlar, belediye, çorum, oğuzlar belediyesi'
    });
  }

  private decodeHtml(html: string): string {
    const doc = new DOMParser().parseFromString(html, 'text/html');
    return doc.documentElement.textContent || '';
  }

  ngOnDestroy() {
    if (this.intervalId) {
      clearInterval(this.intervalId);
    }
  }

  startAutoSlide() {
    this.intervalId = setInterval(() => {
      this.nextSlide();
    }, 5000);
  }

  nextSlide() {
    if (this.sliders.length > 0) {
      this.currentSlideIndex = (this.currentSlideIndex + 1) % this.sliders.length;
    }
  }

  prevSlide() {
    if (this.sliders.length > 0) {
      this.currentSlideIndex = (this.currentSlideIndex - 1 + this.sliders.length) % this.sliders.length;
    }
  }

  goToSlide(index: number) {
    this.currentSlideIndex = index;
    if (this.intervalId) {
      clearInterval(this.intervalId);
      this.startAutoSlide();
    }
  }

}
