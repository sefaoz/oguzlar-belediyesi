import { Component, Input } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';

export interface EventItem {
    title: string;
    date: string;
    location: string;
    image?: string;
    slug: string;
    description?: string;
    publishedAt?: string;
}

@Component({
    selector: 'app-event-card',
    standalone: true,
    imports: [CommonModule, RouterModule],
    template: `
    <div class="bg-white rounded-xl shadow-sm hover:shadow-md transition-shadow overflow-hidden border border-gray-100 group h-full flex flex-col">
      <div class="relative h-48 overflow-hidden">
        <img [src]="event.image || 'https://picsum.photos/400/300'" [alt]="event.title" 
             class="w-full h-full object-cover group-hover:scale-105 transition-transform duration-500">
        <div class="absolute top-4 left-4 bg-white/90 backdrop-blur-sm px-3 py-1 rounded-lg text-center shadow-sm">
          <div class="text-xs text-gray-500 uppercase font-bold">{{ getMonth(event.date) }}</div>
          <div class="text-xl font-bold text-primary">{{ getDay(event.date) }}</div>
        </div>
      </div>
      <div class="p-6 flex flex-col flex-grow">
        <div class="flex items-center text-xs text-gray-500 mb-3">
          <i class="fas fa-map-marker-alt mr-1.5 text-accent"></i>
          {{ event.location }}
        </div>
        <h3 class="text-lg font-bold text-gray-900 mb-3 line-clamp-2 group-hover:text-primary transition-colors">
          <a [routerLink]="['/etkinlikler', event.slug]">{{ event.title }}</a>
        </h3>
        <a [routerLink]="['/etkinlikler', event.slug]" class="mt-auto text-primary font-medium text-sm hover:text-accent transition-colors inline-flex items-center">
          Etkinlik Detayı <i class="fas fa-arrow-right ml-2 text-xs"></i>
        </a>
      </div>
    </div>
  `
})
export class EventCardComponent {
    @Input() event!: EventItem;

    getMonth(dateStr: string): string {
        // Simple mock parsing, ideally use a date library or proper Date object
        // Assuming format "DD.MM.YYYY" or similar text
        return 'KASIM';
    }

    getDay(dateStr: string): string {
        return '25';
    }
}
