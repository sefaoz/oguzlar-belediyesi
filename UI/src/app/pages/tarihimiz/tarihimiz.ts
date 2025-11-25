import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { PageContainerComponent, BreadcrumbStep } from '../../shared/components/page-container/page-container';

@Component({
  selector: 'app-tarihimiz',
  standalone: true,
  imports: [CommonModule, PageContainerComponent],
  templateUrl: './tarihimiz.html',
  styleUrl: './tarihimiz.css',
})
export class TarihimizComponent {
  breadcrumbSteps: BreadcrumbStep[] = [
    { label: 'Anasayfa', url: '/' },
    { label: "Oğuzlar'ı Keşfet" },
    { label: 'Oğuzlar Tarihi', url: '/oguzlar-tarihi' }
  ];
}
