import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { PageContainerComponent, BreadcrumbStep } from '../../shared/components/page-container/page-container';
import { PageContentModel } from '../../shared/models/page-content.model';
import { PageContentService } from '../../shared/services/page-content.service';

@Component({
  selector: 'app-baskan-mesaji',
  standalone: true,
  imports: [CommonModule, PageContainerComponent],
  templateUrl: './baskan-mesaji.html',
  styleUrl: './baskan-mesaji.css',
})
export class BaskanMesaji implements OnInit {
  breadcrumbSteps: BreadcrumbStep[] = [
    { label: 'Anasayfa', url: '/' },
    { label: 'Başkanımız' },
    { label: 'Başkanın Mesajı', url: '/baskandan-mesaj' }
  ];
  content?: PageContentModel;

  constructor(private readonly pageContentService: PageContentService) {}

  ngOnInit(): void {
    this.pageContentService.getPageContent('baskan-mesaji').subscribe(content => {
      this.content = content;
    });
  }
}
