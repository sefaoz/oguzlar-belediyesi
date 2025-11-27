import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, RouterModule } from '@angular/router';
import { PageContainerComponent, BreadcrumbStep } from '../../shared/components/page-container/page-container';
import { EventItem } from '../../shared/components/event-card/event-card';
import { EventService } from '../../shared/services/event.service';

@Component({
  selector: 'app-etkinlik-detay',
  standalone: true,
  imports: [CommonModule, PageContainerComponent, RouterModule],
  templateUrl: './etkinlik-detay.html',
  styleUrl: './etkinlik-detay.css',
})
export class EtkinlikDetay implements OnInit {
  event?: EventItem;
  breadcrumbSteps: BreadcrumbStep[] = [];

  constructor(
    private readonly route: ActivatedRoute,
    private readonly eventService: EventService
  ) { }

  ngOnInit(): void {
    this.route.paramMap.subscribe(params => {
      const slug = params.get('slug');
      if (slug) {
        this.eventService.getEventBySlug(slug).subscribe({
          next: item => {
            this.event = item;
            this.updateBreadcrumbs();
          },
          error: () => {
            this.event = undefined;
            this.breadcrumbSteps = [];
          }
        });
      }
    });
  }

  updateBreadcrumbs() {
    this.breadcrumbSteps = [
      { label: 'Anasayfa', url: '/' },
      { label: 'Etkinlikler', url: '/etkinlikler' },
      { label: this.event?.title || 'Etkinlik Detayı', url: `/etkinlikler/${this.event?.slug}` }
    ];
  }
}
