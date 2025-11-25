import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { PageContainerComponent, BreadcrumbStep } from '../shared/page-container/page-container';

@Component({
  selector: 'app-baskan-hakkinda',
  standalone: true,
  imports: [CommonModule, PageContainerComponent],
  templateUrl: './baskan-hakkinda.html',
  styleUrls: ['./baskan-hakkinda.css']
})
export class BaskanHakkindaComponent {
  breadcrumbSteps: BreadcrumbStep[] = [
    { label: 'Anasayfa', url: '/' },
    { label: 'Başkanımız' },
    { label: 'Başkan Hakkında', active: true }
  ];
}
