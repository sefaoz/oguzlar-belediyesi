import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { PageContainerComponent, BreadcrumbStep } from '../../shared/components/page-container/page-container';
import { ContentSidebarComponent } from '../../shared/components/content-sidebar/content-sidebar';
import { EventCardComponent } from '../../shared/components/event-card/event-card';
import { EventService } from '../../shared/services/event.service';
import { EventItem } from '../../shared/models/event.model';
import { SeoService } from '../../shared/services/seo.service';

@Component({
  selector: 'app-etkinlikler',
  standalone: true,
  imports: [CommonModule, PageContainerComponent, ContentSidebarComponent, EventCardComponent],
  templateUrl: './etkinlikler.html',
  styleUrl: './etkinlikler.css'
})
export class EtkinliklerComponent {
  breadcrumbSteps: BreadcrumbStep[] = [
    { label: 'Anasayfa', url: '/' },
    { label: 'Etkinlikler', url: '/etkinlikler' }
  ];

  allEvents: EventItem[] = [];
  events: EventItem[] = [];

  currentPage = 1;
  pageSize = 9;
  totalPages = 0;
  pages: number[] = [];

  constructor(
    private readonly eventService: EventService,
    private readonly seoService: SeoService
  ) { }

  ngOnInit() {
    this.eventService.getEvents().subscribe(items => {
      this.allEvents = items;
      this.updatePagination();
    });

    this.seoService.updateSeo({
      title: 'Etkinlikler',
      description: 'Oğuzlar Belediyesi etkinlik takvimi, festivaller ve kültürel organizasyonlar.',
      slug: 'etkinlikler'
    });
  }

  updatePagination() {
    this.totalPages = Math.ceil(this.allEvents.length / this.pageSize);
    this.pages = Array.from({ length: this.totalPages }, (_, i) => i + 1);

    const startIndex = (this.currentPage - 1) * this.pageSize;
    const endIndex = startIndex + this.pageSize;
    this.events = this.allEvents.slice(startIndex, endIndex);
  }

  changePage(page: number) {
    if (page >= 1 && page <= this.totalPages) {
      this.currentPage = page;
      this.updatePagination();
      window.scrollTo(0, 0);
    }
  }

  prevPage() {
    this.changePage(this.currentPage - 1);
  }

  nextPage() {
    this.changePage(this.currentPage + 1);
  }
}
