import { Component, HostListener, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Router, NavigationEnd, Event } from '@angular/router';
import { filter } from 'rxjs/operators';

@Component({
  selector: 'app-header',
  standalone: true,
  imports: [CommonModule, RouterModule],
  templateUrl: './header.html',
  styleUrls: ['./header.css']
})
export class HeaderComponent implements OnInit {
  isMobileMenuOpen = false;
  scrolled = false;
  isHomePage = true;
  activeDropdown: string | null = null;
  mobileSubmenu: string | null = null;
  hoverTimer: any;

  constructor(private router: Router) { }

  ngOnInit() {
    this.checkRoute(this.router.url);

    this.router.events.pipe(
      filter((event: Event): event is NavigationEnd => event instanceof NavigationEnd)
    ).subscribe((event: NavigationEnd) => {
      this.checkRoute(event.urlAfterRedirects || event.url);
    });
  }

  checkRoute(url: string) {
    this.isHomePage = url === '/' || url === '';
  }

  @HostListener('window:scroll', [])
  onWindowScroll() {
    this.scrolled = window.scrollY > 50;
  }

  toggleMobileMenu() {
    this.isMobileMenuOpen = !this.isMobileMenuOpen;
  }

  toggleMobileSubmenu(menuName: string) {
    this.mobileSubmenu = this.mobileSubmenu === menuName ? null : menuName;
  }

  onMouseEnter(menuName: string) {
    clearTimeout(this.hoverTimer);
    this.activeDropdown = menuName;
  }

  onMouseLeave() {
    this.hoverTimer = setTimeout(() => {
      this.activeDropdown = null;
    }, 150);
  }
}
