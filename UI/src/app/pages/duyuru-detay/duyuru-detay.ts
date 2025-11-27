import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, RouterModule } from '@angular/router';
import { PageContainerComponent, BreadcrumbStep } from '../../shared/components/page-container/page-container';
import { Announcement } from '../../shared/components/announcement-card/announcement-card';
import { AnnouncementService } from '../../shared/services/announcement.service';

@Component({
  selector: 'app-duyuru-detay',
  standalone: true,
  imports: [CommonModule, PageContainerComponent, RouterModule],
  templateUrl: './duyuru-detay.html',
  styleUrl: './duyuru-detay.css',
})
export class DuyuruDetay implements OnInit {
  announcement?: Announcement;
  breadcrumbSteps: BreadcrumbStep[] = [];

  constructor(
    private readonly route: ActivatedRoute,
    private readonly announcementService: AnnouncementService
  ) { }

  ngOnInit(): void {
    this.route.paramMap.subscribe(params => {
      const slug = params.get('slug');
      if (slug) {
        this.announcementService.getAnnouncementBySlug(slug).subscribe({
          next: item => {
            this.announcement = item;
            this.updateBreadcrumbs();
          },
          error: () => {
            this.announcement = undefined;
            this.breadcrumbSteps = [];
          }
        });
      }
    });
  }

  updateBreadcrumbs() {
    this.breadcrumbSteps = [
      { label: 'Anasayfa', url: '/' },
      { label: 'Duyurular', url: '/duyurular' },
      { label: this.announcement?.title || 'Duyuru Detayı', url: `/duyurular/${this.announcement?.slug}` }
    ];
  }
}
