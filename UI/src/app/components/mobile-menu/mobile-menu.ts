import { Component, Output, EventEmitter } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';

@Component({
  selector: 'app-mobile-menu',
  standalone: true,
  imports: [CommonModule, RouterModule],
  templateUrl: './mobile-menu.html',
  styleUrl: './mobile-menu.css',
})
export class MobileMenu {
  @Output() closeMenu = new EventEmitter<void>();
  mobileSubmenu: string | null = null;

  toggleMobileSubmenu(menuName: string) {
    this.mobileSubmenu = this.mobileSubmenu === menuName ? null : menuName;
  }

  onLinkClick() {
    this.closeMenu.emit();
  }
}
