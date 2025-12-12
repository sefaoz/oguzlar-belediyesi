import { Component, OnInit, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { NewsService } from '../../../services/news.service';
import { AnnouncementService } from '../../../services/announcement.service';
import { EventService } from '../../../services/event.service';
import { TenderService } from '../../../services/tender.service';
import { forkJoin } from 'rxjs';

@Component({
    standalone: true,
    selector: 'app-stats-widget',
    imports: [CommonModule],
    template: `
        <div class="col-span-12 lg:col-span-6 xl:col-span-3">
            <div class="card mb-0">
                <div class="flex justify-between mb-4">
                    <div>
                        <span class="block text-muted-color font-medium mb-4">Haberler</span>
                        <div class="text-surface-900 dark:text-surface-0 font-medium text-xl">{{ stats().news.total }}</div>
                    </div>
                    <div class="flex items-center justify-center bg-blue-100 dark:bg-blue-400/10 rounded-border" style="width: 2.5rem; height: 2.5rem">
                        <i class="pi pi-megaphone text-blue-500 text-xl!"></i>
                    </div>
                </div>
                <span class="text-primary font-medium">{{ stats().news.new }} yeni </span>
                <span class="text-muted-color">bu ay eklendi</span>
            </div>
        </div>
        <div class="col-span-12 lg:col-span-6 xl:col-span-3">
            <div class="card mb-0">
                <div class="flex justify-between mb-4">
                    <div>
                        <span class="block text-muted-color font-medium mb-4">Duyurular</span>
                        <div class="text-surface-900 dark:text-surface-0 font-medium text-xl">{{ stats().announcements.total }}</div>
                    </div>
                    <div class="flex items-center justify-center bg-orange-100 dark:bg-orange-400/10 rounded-border" style="width: 2.5rem; height: 2.5rem">
                        <i class="pi pi-bell text-orange-500 text-xl!"></i>
                    </div>
                </div>
                <span class="text-primary font-medium">{{ stats().announcements.new }} yeni </span>
                <span class="text-muted-color">bu hafta eklendi</span>
            </div>
        </div>
        <div class="col-span-12 lg:col-span-6 xl:col-span-3">
            <div class="card mb-0">
                <div class="flex justify-between mb-4">
                    <div>
                        <span class="block text-muted-color font-medium mb-4">Etkinlikler</span>
                        <div class="text-surface-900 dark:text-surface-0 font-medium text-xl">{{ stats().events.total }}</div>
                    </div>
                    <div class="flex items-center justify-center bg-cyan-100 dark:bg-cyan-400/10 rounded-border" style="width: 2.5rem; height: 2.5rem">
                        <i class="pi pi-calendar text-cyan-500 text-xl!"></i>
                    </div>
                </div>
                <span class="text-primary font-medium">{{ stats().events.upcoming }} yaklaşan </span>
                <span class="text-muted-color">etkinlik var</span>
            </div>
        </div>
        <div class="col-span-12 lg:col-span-6 xl:col-span-3">
            <div class="card mb-0">
                <div class="flex justify-between mb-4">
                    <div>
                        <span class="block text-muted-color font-medium mb-4">İhaleler</span>
                        <div class="text-surface-900 dark:text-surface-0 font-medium text-xl">{{ stats().tenders.total }}</div>
                    </div>
                    <div class="flex items-center justify-center bg-purple-100 dark:bg-purple-400/10 rounded-border" style="width: 2.5rem; height: 2.5rem">
                        <i class="pi pi-briefcase text-purple-500 text-xl!"></i>
                    </div>
                </div>
                <span class="text-primary font-medium">{{ stats().tenders.active }} aktif </span>
                <span class="text-muted-color">ihale mevcut</span>
            </div>
        </div>
    `
})
export class StatsWidget implements OnInit {
    private newsService = inject(NewsService);
    private announcementService = inject(AnnouncementService);
    private eventService = inject(EventService);
    private tenderService = inject(TenderService);

    stats = signal({
        news: { total: 0, new: 0 },
        announcements: { total: 0, new: 0 },
        events: { total: 0, upcoming: 0 },
        tenders: { total: 0, active: 0 }
    });

    ngOnInit() {
        const now = new Date();
        const startOfMonth = new Date(now.getFullYear(), now.getMonth(), 1);

        // Start of week calculation might need adjustment based on locale, assuming simple approach
        const startOfWeek = new Date(now);
        startOfWeek.setDate(now.getDate() - now.getDay());

        // Assuming getAll returns array observable. If services have pagination wrapped response (e.g. { items: [], total: 0 }), specific handling needed.
        // Based on NewsService.getAll returning News[], we assume others follow suite.

        forkJoin({
            news: this.newsService.getAll(),
            announcements: this.announcementService.getAll(),
            events: this.eventService.getAll(),
            tenders: this.tenderService.getAll()
        }).subscribe({
            next: ({ news, announcements, events, tenders }) => {
                this.stats.set({
                    news: {
                        total: news.length,
                        new: news.filter(n => new Date(n.date) >= startOfMonth).length
                    },
                    announcements: {
                        total: announcements.length,
                        new: announcements.filter(a => new Date(a.date) >= startOfWeek).length
                    },
                    events: {
                        total: events.length,
                        upcoming: events.filter(e => new Date(e.eventDate) >= now).length
                    },
                    tenders: {
                        total: tenders.length,
                        active: tenders.filter(t => new Date(t.tenderDate) >= now).length
                    }
                });
            },
            error: (err) => console.error('Failed to load stats', err)
        });
    }
}
