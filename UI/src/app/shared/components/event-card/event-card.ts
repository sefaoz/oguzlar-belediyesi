import { Component, Input } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { EventItem } from '../../models/event.model';
import { ImageUrlPipe } from '../../pipes/image-url.pipe';

@Component({
  selector: 'app-event-card',
  standalone: true,
  imports: [CommonModule, RouterModule, ImageUrlPipe],
  template: `
    <div class="bg-white rounded-xl shadow-sm hover:shadow-md transition-shadow overflow-hidden border border-gray-100 group h-full flex flex-col">
      <div class="relative h-48 overflow-hidden">
        <img [src]="(event.image | imageUrl) || 'https://picsum.photos/400/300'" [alt]="event.title" 
             class="w-full h-full object-cover group-hover:scale-105 transition-transform duration-500">
        <div class="absolute top-4 left-4 bg-white/90 backdrop-blur-sm px-3 py-1 rounded-lg text-center shadow-sm">
          <div class="text-xs text-gray-500 uppercase font-bold">{{ event.eventDate | date:'MMM' }}</div>
          <div class="text-xl font-bold text-primary">{{ event.eventDate | date:'d' }}</div>
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
}
