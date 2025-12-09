import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { NewsCard } from '../../shared/components/news-card/news-card';
import { PageContainerComponent, BreadcrumbStep } from '../../shared/components/page-container/page-container';
import { FormsModule } from '@angular/forms';

import { NewsService } from '../../shared/services/news.service';
import { SeoService } from '../../shared/services/seo.service';
import { NewsItem } from '../../shared/models/news.model';

@Component({
  selector: 'app-haberler',
  standalone: true,
  imports: [CommonModule, NewsCard, PageContainerComponent, FormsModule],
  templateUrl: './haberler.html',
  styleUrl: './haberler.css',
})
export class HaberlerComponent implements OnInit {
  breadcrumbSteps: BreadcrumbStep[] = [
    { label: 'Anasayfa', url: '/' },
    { label: 'Haberler', url: '/haberler' }
  ];

  searchTerm: string = '';

  newsList: NewsItem[] = [];

  // Filters
  selectedMonth: number | null = null;
  selectedYear: number | null = null;

  months = [
    { value: 1, label: 'Ocak' },
    { value: 2, label: 'Şubat' },
    { value: 3, label: 'Mart' },
    { value: 4, label: 'Nisan' },
    { value: 5, label: 'Mayıs' },
    { value: 6, label: 'Haziran' },
    { value: 7, label: 'Temmuz' },
    { value: 8, label: 'Ağustos' },
    { value: 9, label: 'Eylül' },
    { value: 10, label: 'Ekim' },
    { value: 11, label: 'Kasım' },
    { value: 12, label: 'Aralık' }
  ];

  years: number[] = [];

  constructor(
    private readonly newsService: NewsService,
    private readonly seoService: SeoService
  ) {
    const now = new Date();
    this.selectedMonth = now.getMonth() + 1;
    this.selectedYear = now.getFullYear();

    const currentYear = now.getFullYear();
    for (let i = currentYear; i >= 2020; i--) {
      this.years.push(i);
    }
  }

  ngOnInit(): void {
    this.newsService.getNews().subscribe(news => {
      this.newsList = news;
    });

    this.seoService.updateSeo({
      title: 'Haberler',
      description: 'Oğuzlar Belediyesi güncel haberler, duyurular ve gelişmeler.',
      slug: 'haberler'
    });
  }

  monthMap: { [key: string]: number } = {
    'Ocak': 1, 'Şubat': 2, 'Mart': 3, 'Nisan': 4, 'Mayıs': 5, 'Haziran': 6,
    'Temmuz': 7, 'Ağustos': 8, 'Eylül': 9, 'Ekim': 10, 'Kasım': 11, 'Aralık': 12
  };

  get filteredNews(): NewsItem[] {
    let filtered = this.newsList;

    // Search Filter
    if (this.searchTerm) {
      const term = this.searchTerm.toLowerCase();
      filtered = filtered.filter(news =>
        news.title.toLowerCase().includes(term) ||
        news.description.toLowerCase().includes(term)
      );
    } else {
      // Month Filter
      if (this.selectedMonth) {
        filtered = filtered.filter(news => {
          const d = new Date(news.date);
          if (!isNaN(d.getTime())) {
            return (d.getMonth() + 1) === +this.selectedMonth!;
          }

          // Try DD.MM.YYYY
          let parts = news.date.split('.');
          if (parts.length === 3) {
            return parseInt(parts[1], 10) === +this.selectedMonth!;
          }

          // Try DD MonthName YYYY (e.g. 12 Kasım 2025)
          parts = news.date.split(' ');
          if (parts.length >= 3) {
            const monthName = parts[1];
            return this.monthMap[monthName] === +this.selectedMonth!;
          }
          return false;
        });
      }

      // Year Filter
      if (this.selectedYear) {
        filtered = filtered.filter(news => {
          const d = new Date(news.date);
          if (!isNaN(d.getTime())) {
            return d.getFullYear() === +this.selectedYear!;
          }

          // Try DD.MM.YYYY
          let parts = news.date.split('.');
          if (parts.length === 3) {
            return parseInt(parts[2], 10) === +this.selectedYear!;
          }

          // Try DD MonthName YYYY (e.g. 12 Kasım 2025)
          parts = news.date.split(' ');
          if (parts.length >= 3) {
            const year = parts[2];
            return parseInt(year, 10) === +this.selectedYear!;
          }
          return false;
        });
      }
    }

    return filtered;
  }

  // Pagination
  currentPage: number = 1;
  itemsPerPage: number = 6;

  get paginatedNews(): NewsItem[] {
    const startIndex = (this.currentPage - 1) * this.itemsPerPage;
    return this.filteredNews.slice(startIndex, startIndex + this.itemsPerPage);
  }

  get totalPages(): number {
    return Math.ceil(this.filteredNews.length / this.itemsPerPage);
  }

  get pages(): number[] {
    const total = this.totalPages;
    let start = Math.max(1, this.currentPage - 2);
    let end = Math.min(total, this.currentPage + 2);

    if (total <= 5) {
      start = 1;
      end = total;
    } else {
      if (this.currentPage <= 3) {
        end = 5;
      } else if (this.currentPage >= total - 2) {
        start = total - 4;
      }
    }

    const pages = [];
    for (let i = start; i <= end; i++) {
      pages.push(i);
    }
    return pages;
  }

  changePage(page: number) {
    if (page >= 1 && page <= this.totalPages) {
      this.currentPage = page;
      window.scrollTo(0, 0);
    }
  }

  // Trigger page reset when filters change
  onFilterChange() {
    this.currentPage = 1;
  }
  clearFilters() {
    this.searchTerm = '';
    const now = new Date();
    this.selectedMonth = now.getMonth() + 1;
    this.selectedYear = now.getFullYear();
    this.onFilterChange();
  }
}
