import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { PageContainerComponent, BreadcrumbStep } from '../shared/page-container/page-container';

@Component({
  selector: 'app-baskan-mesaji',
  standalone: true,
  imports: [CommonModule, PageContainerComponent],
  templateUrl: './baskan-mesaji.html',
  styleUrl: './baskan-mesaji.css',
})
export class BaskanMesaji {
  breadcrumbSteps: BreadcrumbStep[] = [
    { label: 'Anasayfa', url: '/' },
    { label: 'Başkanımız' },
    { label: 'Başkanın Mesajı', active: true }
  ];
}
