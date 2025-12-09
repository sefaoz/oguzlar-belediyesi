import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, ActivatedRoute } from '@angular/router';
import { PageContainerComponent, BreadcrumbStep } from '../../shared/components/page-container/page-container';
import { MunicipalUnit } from '../../shared/models/unit.model';
import { UnitService } from '../../shared/services/unit.service';
import { SeoService } from '../../shared/services/seo.service';

import { ImageUrlPipe } from '../../shared/pipes/image-url.pipe';

@Component({
  selector: 'app-birimler',
  standalone: true,
  imports: [CommonModule, RouterModule, PageContainerComponent, ImageUrlPipe],
  templateUrl: './birimler.html',
  styleUrls: ['./birimler.css']
})
export class BirimlerComponent implements OnInit {
  breadcrumbSteps: BreadcrumbStep[] = [
    { label: 'Anasayfa', url: '/' },
    { label: 'Birimler', url: '/birimler' },
    { label: 'Birim', active: true }
  ];

  units: MunicipalUnit[] = [];
  activeUnit?: MunicipalUnit;
  private requestedUnitId = 'ozel-kalem';

  constructor(
    private readonly route: ActivatedRoute,
    private readonly unitService: UnitService,
    private readonly seoService: SeoService
  ) { }

  ngOnInit(): void {
    this.route.params.subscribe(params => {
      this.requestedUnitId = params['unit'] || 'ozel-kalem';
      this.applySelection();
    });

    this.unitService.getUnits().subscribe(units => {
      this.units = units;
      this.applySelection();
    });
  }

  isActive(unitId: string): boolean {
    return this.activeUnit?.id === unitId;
  }

  private applySelection(): void {
    if (!this.units.length) {
      return;
    }

    const unitId = this.requestedUnitId || this.units[0].id;
    this.activeUnit = this.units.find(u => u.id === unitId) ?? this.units[0];
    this.updateBreadcrumbs();

    if (this.activeUnit) {
      this.seoService.updateSeo({
        title: this.activeUnit.title,
        description: `${this.activeUnit.title} birimi hakkında bilgiler, görev ve yetkiler.`,
        slug: `birimler/${this.activeUnit.id}`
      });
    }
  }

  private updateBreadcrumbs(): void {
    this.breadcrumbSteps = [
      { label: 'Anasayfa', url: '/' },
      { label: 'Birimler', url: '/birimler' },
      { label: this.activeUnit?.title || 'Birim', active: true }
    ];
  }
}
