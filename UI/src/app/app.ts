import { Component, OnInit, signal } from '@angular/core';
import { RouterModule } from '@angular/router';
import { SeoService } from './shared/services/seo.service';

@Component({
  selector: 'app-root',
  imports: [RouterModule],
  templateUrl: './app.html',
  styleUrl: './app.css'
})
export class App implements OnInit {
  protected readonly title = signal('oguzlar-ui');

  constructor(private readonly seoService: SeoService) { }

  ngOnInit() {
    this.seoService.updateSeo({
      title: 'Ana Sayfa',
      description: 'Oğuzlar Belediyesi resmi web sitesi. Haberler, projeler, etkinlikler ve belediye hizmetleri.',
      keywords: 'oğuzlar, belediye, çorum, oğuzlar belediyesi, haberler, projeler'
    });
  }
}
