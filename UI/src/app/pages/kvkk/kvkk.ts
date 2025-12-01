import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { PageContainerComponent } from '../../shared/components/page-container/page-container';
import { KvkkDocument } from '../../shared/models/kvkk-document.model';
import { KvkkService } from '../../shared/services/kvkk.service';
import { SeoService } from '../../shared/services/seo.service';

@Component({
  selector: 'app-kvkk',
  standalone: true,
  imports: [CommonModule, PageContainerComponent],
  templateUrl: './kvkk.html',
  styleUrl: './kvkk.css'
})
export class Kvkk implements OnInit {
  breadcrumbSteps = [
    { label: 'Anasayfa', url: '/' },
    { label: 'Kurumsal', url: '/kurumsal' },
    { label: 'KVKK', url: '/kvkk' }
  ];

  documents: KvkkDocument[] = [];

  constructor(
    private readonly kvkkService: KvkkService,
    private readonly seoService: SeoService
  ) { }

  ngOnInit(): void {
    this.kvkkService.getDocuments().subscribe(documents => {
      this.documents = documents;
    });

    this.seoService.updateSeo({
      title: 'KVKK',
      description: 'Kişisel Verilerin Korunması Kanunu (KVKK) kapsamında aydınlatma metinleri ve politikalar.',
      slug: 'kvkk'
    });
  }
}
