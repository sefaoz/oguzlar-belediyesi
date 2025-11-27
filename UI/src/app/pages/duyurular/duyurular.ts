import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { PageContainerComponent, BreadcrumbStep } from '../../shared/components/page-container/page-container';
import { ContentSidebarComponent } from '../../shared/components/content-sidebar/content-sidebar';
import { AnnouncementCardComponent, Announcement } from '../../shared/components/announcement-card/announcement-card';
import { AnnouncementService } from '../../shared/services/announcement.service';

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

  constructor(private readonly announcementService: AnnouncementService) { }

  ngOnInit() {
    this.announcementService.getAnnouncements().subscribe(items => {
      this.announcements = items;
    });
  }
}
