import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';

@Component({
    selector: 'app-content-sidebar',
    standalone: true,
    imports: [CommonModule, RouterModule],
    template: `
    <div class="bg-white rounded-xl shadow-md p-6 border border-gray-100 sticky top-24">
      <h3 class="text-lg font-bold text-gray-900 mb-4 pb-2 border-b border-gray-100">Hızlı Menü</h3>
      <nav class="space-y-2">
        <a routerLink="/duyurular" routerLinkActive="bg-primary text-white" 
           class="flex items-center justify-between p-3 rounded-lg text-gray-700 hover:bg-gray-50 hover:text-primary transition-all group">
           <span class="font-medium">Duyurular</span>
           <i class="fas fa-bullhorn group-hover:translate-x-1 transition-transform"></i>
        </a>
        <a routerLink="/etkinlikler" routerLinkActive="bg-primary text-white"
           class="flex items-center justify-between p-3 rounded-lg text-gray-700 hover:bg-gray-50 hover:text-primary transition-all group">
           <span class="font-medium">Etkinlikler</span>
           <i class="fas fa-calendar-alt group-hover:translate-x-1 transition-transform"></i>
        </a>
        <a routerLink="/ihaleler" routerLinkActive="bg-primary text-white"
           class="flex items-center justify-between p-3 rounded-lg text-gray-700 hover:bg-gray-50 hover:text-primary transition-all group">
           <span class="font-medium">İhaleler</span>
           <i class="fas fa-gavel group-hover:translate-x-1 transition-transform"></i>
        </a>
      </nav>
    </div>
  `
})
export class ContentSidebarComponent { }
