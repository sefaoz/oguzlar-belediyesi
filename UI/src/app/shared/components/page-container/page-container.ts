import { Component, Input } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';

export interface BreadcrumbStep {
  label: string;
  url?: string;
  active?: boolean;
}

@Component({
  selector: 'app-page-container',
  standalone: true,
  imports: [CommonModule, RouterModule],
  templateUrl: './page-container.html'
})
export class PageContainerComponent {
  @Input() title: string = '';
  @Input() breadcrumbSteps: BreadcrumbStep[] = [];
}
