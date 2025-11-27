import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { PageContainerComponent, BreadcrumbStep } from '../../shared/components/page-container/page-container';
import { ContentSidebarComponent } from '../../shared/components/content-sidebar/content-sidebar';
import { EventCardComponent, EventItem } from '../../shared/components/event-card/event-card';
import { EventService } from '../../shared/services/event.service';

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

  constructor(private readonly eventService: EventService) { }

  ngOnInit() {
    this.eventService.getEvents({ upcomingOnly: true }).subscribe(items => {
      this.events = items;
    });
  }
}
