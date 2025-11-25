import { Component, HostListener, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Router, NavigationEnd, Event } from '@angular/router';
import { filter } from 'rxjs/operators';
import { MobileMenu } from '../mobile-menu/mobile-menu';

@Component({
  selector: 'app-header',
  standalone: true,
  imports: [CommonModule, RouterModule, MobileMenu],
  templateUrl: './header.html',
  styleUrls: ['./header.css']
})
export class HeaderComponent implements OnInit {
  isMobileMenuOpen = false;
  scrolled = false;
  isHomePage = true;
  activeDropdown: string | null = null;
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
