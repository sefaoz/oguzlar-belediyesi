import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { NewsCard, NewsItem } from '../../shared/components/news-card/news-card';
import { RouterModule } from '@angular/router';
import { PageContentModel } from '../../shared/models/page-content.model';
import { NewsService } from '../../shared/services/news.service';
import { PageContentService } from '../../shared/services/page-content.service';
import { AnnouncementService } from '../../shared/services/announcement.service';
import { EventService } from '../../shared/services/event.service';
import { TenderService } from '../../shared/services/tender.service';
import { Announcement } from '../../shared/components/announcement-card/announcement-card';
import { EventItem } from '../../shared/components/event-card/event-card';
import { Tender } from '../../shared/components/tender-card/tender-card';

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
    private readonly tenderService: TenderService
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
  }
}
