import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { PageContainerComponent } from '../../shared/components/page-container/page-container';
import { Vehicle } from '../../shared/models/vehicle.model';
import { VehicleService } from '../../shared/services/vehicle.service';
import { SeoService } from '../../shared/services/seo.service';

import { ImageUrlPipe } from '../../shared/pipes/image-url.pipe';

@Component({
  selector: 'app-arac-parki',
  standalone: true,
  imports: [CommonModule, PageContainerComponent, ImageUrlPipe],
  templateUrl: './arac-parki.html',
  styleUrl: './arac-parki.css'
})
export class AracParki implements OnInit {
  breadcrumbSteps = [
    { label: 'Anasayfa', url: '/' },
    { label: 'Kurumsal', url: '/kurumsal' },
    { label: 'Araç Parkı', url: '/arac-parki' }
  ];

  vehicles: Vehicle[] = [];

  constructor(
    private readonly vehicleService: VehicleService,
    private readonly seoService: SeoService
  ) { }

  ngOnInit(): void {
    this.vehicleService.getVehicles().subscribe(items => {
      this.vehicles = items;
    });

    this.seoService.updateSeo({
      title: 'Araç Parkı',
      description: 'Oğuzlar Belediyesi hizmet araçları ve iş makineleri envanteri.',
      slug: 'arac-parki'
    });
  }
}
