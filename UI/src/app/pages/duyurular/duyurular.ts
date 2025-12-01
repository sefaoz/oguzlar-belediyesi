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

  announcements: Announcement[] = [];

  constructor(
    private readonly announcementService: AnnouncementService,
    private readonly seoService: SeoService
  ) { }

  ngOnInit() {
    this.announcementService.getAnnouncements().subscribe(items => {
      this.announcements = items;
    });

    this.seoService.updateSeo({
      title: 'Duyurular',
      description: 'Oğuzlar Belediyesi resmi duyuruları, ilanlar ve bildirimler.',
      slug: 'duyurular'
    });
  }
}
