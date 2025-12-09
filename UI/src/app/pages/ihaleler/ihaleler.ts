import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { PageContainerComponent, BreadcrumbStep } from '../../shared/components/page-container/page-container';
import { ContentSidebarComponent } from '../../shared/components/content-sidebar/content-sidebar';
import { TenderCardComponent } from '../../shared/components/tender-card/tender-card';
import { TenderService } from '../../shared/services/tender.service';
import { Tender } from '../../shared/models/tender.model';
import { SeoService } from '../../shared/services/seo.service';

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

  allTenders: Tender[] = [];
  tenders: Tender[] = [];

  currentPage: number = 1;
  pageSize: number = 6;
  totalPages: number = 0;
  pages: number[] = [];

  constructor(
    private readonly tenderService: TenderService,
    private readonly seoService: SeoService
  ) { }

  ngOnInit() {
    this.tenderService.getTenders().subscribe(items => {
      this.allTenders = items.sort((a, b) => new Date(b.tenderDate).getTime() - new Date(a.tenderDate).getTime());
      this.calculatePagination();
      this.updatePage();
    });

    this.seoService.updateSeo({
      title: 'İhaleler',
      description: 'Oğuzlar Belediyesi güncel ihaleler, sonuçlanan ihaleler ve şartnameler.',
      slug: 'ihaleler'
    });
  }

  calculatePagination() {
    this.totalPages = Math.ceil(this.allTenders.length / this.pageSize);
    this.pages = Array.from({ length: this.totalPages }, (_, i) => i + 1);
  }

  updatePage() {
    const startIndex = (this.currentPage - 1) * this.pageSize;
    const endIndex = startIndex + this.pageSize;
    this.tenders = this.allTenders.slice(startIndex, endIndex);
    window.scrollTo({ top: 0, behavior: 'smooth' });
  }

  goToPage(page: number) {
    if (page >= 1 && page <= this.totalPages && page !== this.currentPage) {
      this.currentPage = page;
      this.updatePage();
    }
  }

  nextPage() {
    if (this.currentPage < this.totalPages) {
      this.currentPage++;
      this.updatePage();
    }
  }

  prevPage() {
    if (this.currentPage > 1) {
      this.currentPage--;
      this.updatePage();
    }
  }
}
