import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { PageContainerComponent, BreadcrumbStep } from '../../shared/components/page-container/page-container';
import { ContentSidebarComponent } from '../../shared/components/content-sidebar/content-sidebar';
import { AnnouncementCardComponent } from '../../shared/components/announcement-card/announcement-card';
import { AnnouncementService } from '../../shared/services/announcement.service';
import { Announcement } from '../../shared/models/announcement.model';
import { SeoService } from '../../shared/services/seo.service';

@Component({
  selector: 'app-duyurular',
  standalone: true,
  imports: [CommonModule, PageContainerComponent, ContentSidebarComponent, AnnouncementCardComponent],
  templateUrl: './duyurular.html',
  styleUrl: './duyurular.css'
})
export class DuyurularComponent {
  breadcrumbSteps: BreadcrumbStep[] = [
    { label: 'Anasayfa', url: '/' },
    { label: 'Duyurular', url: '/duyurular' }
  ];

  allAnnouncements: Announcement[] = [];
  announcements: Announcement[] = [];

  currentPage = 1;
  pageSize = 6;
  totalPages = 0;
  pages: number[] = [];

  constructor(
    private readonly announcementService: AnnouncementService,
    private readonly seoService: SeoService
  ) { }

  ngOnInit() {
    this.announcementService.getAnnouncements().subscribe(items => {
      this.allAnnouncements = items;
      this.updatePagination();
    });

    this.seoService.updateSeo({
      title: 'Duyurular',
      description: 'Oğuzlar Belediyesi resmi duyuruları, ilanlar ve bildirimler.',
      slug: 'duyurular'
    });
  }

  updatePagination() {
    this.totalPages = Math.ceil(this.allAnnouncements.length / this.pageSize);
    this.pages = Array.from({ length: this.totalPages }, (_, i) => i + 1);

    const startIndex = (this.currentPage - 1) * this.pageSize;
    const endIndex = startIndex + this.pageSize;
    this.announcements = this.allAnnouncements.slice(startIndex, endIndex);
  }

  changePage(page: number) {
    if (page >= 1 && page <= this.totalPages) {
      this.currentPage = page;
      this.updatePagination();
      window.scrollTo(0, 0);
    }
  }

  prevPage() {
    this.changePage(this.currentPage - 1);
  }

  nextPage() {
    this.changePage(this.currentPage + 1);
  }
}
