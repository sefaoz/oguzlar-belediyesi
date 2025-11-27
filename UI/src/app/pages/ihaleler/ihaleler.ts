import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { PageContainerComponent, BreadcrumbStep } from '../../shared/components/page-container/page-container';
import { ContentSidebarComponent } from '../../shared/components/content-sidebar/content-sidebar';
import { TenderCardComponent, Tender } from '../../shared/components/tender-card/tender-card';
import { TenderService } from '../../shared/services/tender.service';

@Component({
  selector: 'app-ihaleler',
  standalone: true,
  imports: [CommonModule, PageContainerComponent, ContentSidebarComponent, TenderCardComponent],
  templateUrl: './ihaleler.html',
  styleUrl: './ihaleler.css'
})
export class IhalelerComponent {
  breadcrumbSteps: BreadcrumbStep[] = [
    { label: 'Anasayfa', url: '/' },
    { label: 'İhaleler', url: '/ihaleler' }
  ];

  tenders: Tender[] = [];

  constructor(private readonly tenderService: TenderService) { }

  ngOnInit() {
    this.tenderService.getTenders().subscribe(items => {
      this.tenders = items;
    });
  }
}
