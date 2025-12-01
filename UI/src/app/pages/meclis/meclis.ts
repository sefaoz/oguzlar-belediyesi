import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { PageContainerComponent } from '../../shared/components/page-container/page-container';
import { CouncilDocument } from '../../shared/models/council-document.model';
import { CouncilService } from '../../shared/services/council.service';
import { SeoService } from '../../shared/services/seo.service';

@Component({
  selector: 'app-meclis',
  standalone: true,
  imports: [CommonModule, PageContainerComponent],
  templateUrl: './meclis.html',
  styleUrl: './meclis.css',
})
export class Meclis implements OnInit {
  breadcrumbSteps = [
    { label: 'Anasayfa', url: '/' },
    { label: 'Kurumsal', url: '/kurumsal' },
    { label: 'Meclis', url: '/meclis' }
  ];

  documents: CouncilDocument[] = [];

  constructor(
    private readonly councilService: CouncilService,
    private readonly seoService: SeoService
  ) { }

  ngOnInit(): void {
    this.councilService.getDocuments().subscribe(documents => {
      this.documents = documents;
    });

    this.seoService.updateSeo({
      title: 'Belediye Meclisi',
      description: 'Oğuzlar Belediyesi Meclis kararları, gündem maddeleri ve tutanaklar.',
      slug: 'meclis'
    });
  }
}
