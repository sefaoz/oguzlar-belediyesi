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

  events: EventItem[] = [];

  constructor(
    private readonly eventService: EventService,
    private readonly seoService: SeoService
  ) { }

  ngOnInit() {
    this.eventService.getEvents({ upcomingOnly: true }).subscribe(items => {
      this.events = items;
    });

    this.seoService.updateSeo({
      title: 'Etkinlikler',
      description: 'Oğuzlar Belediyesi etkinlik takvimi, festivaller ve kültürel organizasyonlar.',
      slug: 'etkinlikler'
    });
  }
}
