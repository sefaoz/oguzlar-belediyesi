import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, RouterModule } from '@angular/router';
import { PageContainerComponent, BreadcrumbStep } from '../../shared/components/page-container/page-container';
import { Announcement } from '../../shared/models/announcement.model';
import { AnnouncementService } from '../../shared/services/announcement.service';
import { SeoService } from '../../shared/services/seo.service';

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
    private readonly announcementService: AnnouncementService,
    private readonly seoService: SeoService
  ) { }

  ngOnInit(): void {
    this.route.paramMap.subscribe(params => {
      const slug = params.get('slug');
      if (slug) {
        this.announcementService.getAnnouncementBySlug(slug).subscribe({
          next: item => {
            this.announcement = item;
            this.updateBreadcrumbs();
            if (item) {
              this.seoService.updateSeo({
                title: item.title,
                description: item.summary,
                slug: `duyurular/${item.slug}`,
                type: 'article'
              });
            }
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
