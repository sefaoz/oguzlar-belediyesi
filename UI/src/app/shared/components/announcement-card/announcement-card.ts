import { Component, Input } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { Announcement } from '../../models/announcement.model';

@Component({
    selector: 'app-announcement-card',
    standalone: true,
    imports: [CommonModule, RouterModule],
    template: `
    <div class="bg-white rounded-xl shadow-sm hover:shadow-md transition-shadow p-6 border border-gray-100 flex flex-col h-full">
      <div class="flex items-center text-sm text-gray-500 mb-3">
        <i class="far fa-calendar-alt mr-2 text-accent"></i>
        {{ announcement.date }}
      </div>
      <h3 class="text-lg font-bold text-gray-900 mb-3 line-clamp-2 hover:text-primary transition-colors">
        <a [routerLink]="['/duyurular', announcement.slug]">{{ announcement.title }}</a>
      </h3>
      <p class="text-gray-600 text-sm mb-4 line-clamp-3 flex-grow">
        {{ announcement.summary }}
      </p>
      <a [routerLink]="['/duyurular', announcement.slug]" class="text-primary font-medium text-sm hover:text-accent transition-colors inline-flex items-center mt-auto">
        Detayları İncele <i class="fas fa-arrow-right ml-2 text-xs"></i>
      </a>
    </div>
  `
})
export class AnnouncementCardComponent {
    @Input() announcement!: Announcement;
}
