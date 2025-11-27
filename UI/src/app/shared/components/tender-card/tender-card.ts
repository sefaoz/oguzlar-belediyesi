import { Component, Input } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';

export interface Tender {
    title: string;
    date: string;
    status: 'active' | 'passive' | 'completed';
    registrationNumber: string;
    slug: string;
    description?: string;
    estimatedValue?: number;
    publishedAt?: string;
}

@Component({
    selector: 'app-tender-card',
    standalone: true,
    imports: [CommonModule, RouterModule],
    template: `
    <div class="bg-white rounded-xl shadow-sm hover:shadow-md transition-shadow p-6 border border-gray-100 flex flex-col h-full relative overflow-hidden">
      <div class="absolute top-0 right-0 w-2 h-full" [ngClass]="{
        'bg-green-500': tender.status === 'active',
        'bg-red-500': tender.status === 'passive' || tender.status === 'completed'
      }"></div>
      
      <div class="flex justify-between items-start mb-4 pr-4">
        <span class="inline-block px-3 py-1 rounded-full text-xs font-semibold" [ngClass]="{
          'bg-green-100 text-green-700': tender.status === 'active',
          'bg-red-100 text-red-700': tender.status !== 'active'
        }">
          {{ getStatusLabel(tender.status) }}
        </span>
        <span class="text-xs text-gray-400 font-mono">{{ tender.registrationNumber }}</span>
      </div>

      <h3 class="text-lg font-bold text-gray-900 mb-3 line-clamp-2 hover:text-primary transition-colors">
        <a [routerLink]="['/ihaleler', tender.slug]">{{ tender.title }}</a>
      </h3>

      <div class="mt-auto pt-4 border-t border-gray-50 flex items-center justify-between text-sm">
        <div class="text-gray-500">
          <i class="far fa-calendar-alt mr-1.5"></i> {{ tender.date }}
        </div>
        <a [routerLink]="['/ihaleler', tender.slug]" class="text-primary font-medium hover:text-accent transition-colors">
          İncele
        </a>
      </div>
    </div>
  `
})
export class TenderCardComponent {
    @Input() tender!: Tender;

    getStatusLabel(status: string): string {
        switch (status) {
            case 'active': return 'Yayında';
            case 'passive': return 'Pasif';
            case 'completed': return 'Sonuçlandı';
            default: return 'Bilinmiyor';
        }
    }
}
