import { Component, OnInit } from '@angular/core';
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

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [CommonModule, NewsCard, RouterModule],
  templateUrl: './home.html',
  styleUrls: ['./home.css']
})
export class HomeComponent implements OnInit {
  newsList: NewsItem[] = [];
  announcements: Announcement[] = [];
  events: EventItem[] = [];
  tenders: Tender[] = [];
  presidentMessage?: PageContentModel;

  constructor(
    private readonly newsService: NewsService,
    private readonly pageContentService: PageContentService,
    private readonly announcementService: AnnouncementService,
    private readonly eventService: EventService,
    private readonly tenderService: TenderService,
    private readonly seoService: SeoService
  ) { }

  ngOnInit(): void {
    this.newsService.getNews().subscribe(news => {
      this.newsList = news.slice(0, 3);
    });

    this.announcementService.getAnnouncements().subscribe(items => {
      this.announcements = items.slice(0, 3);
    });

    this.eventService.getEvents({ upcomingOnly: true }).subscribe(items => {
      this.events = items.slice(0, 3);
    });

    this.tenderService.getTenders({ status: 'active' }).subscribe(items => {
      this.tenders = items.slice(0, 3);
    });

    this.pageContentService.getPageContent('home-baskan-mesaji').subscribe(content => {
      this.presidentMessage = content;
    });

    this.seoService.updateSeo({
      title: 'Ana Sayfa',
      description: 'Oğuzlar Belediyesi resmi web sitesi. Haberler, projeler, etkinlikler ve belediye hizmetleri.',
      keywords: 'oğuzlar, belediye, çorum, oğuzlar belediyesi'
    });
  }
}
