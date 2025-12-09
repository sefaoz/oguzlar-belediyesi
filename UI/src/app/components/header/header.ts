import { Component, HostListener, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Router, NavigationEnd, Event } from '@angular/router';
import { filter } from 'rxjs/operators';
import { MobileMenu } from '../mobile-menu/mobile-menu';
import { MenuService } from '../../services/menu.service';
import { Menu } from '../../models/menu';
import { SiteSettingsService } from '../../services/site-settings.service';

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
  menus: Menu[] = [];
  topbarLinks: any[] = [];

  constructor(
    private router: Router,
    private menuService: MenuService,
    private siteSettingsService: SiteSettingsService
  ) { }

  ngOnInit() {
    this.siteSettingsService.settings$.subscribe(() => {
      this.topbarLinks = this.siteSettingsService.getJsonSetting<any[]>('Topbar', 'Links') || [];
    });
    this.checkRoute(this.router.url);

    this.router.events.pipe(
      filter((event: Event): event is NavigationEnd => event instanceof NavigationEnd)
    ).subscribe((event: NavigationEnd) => {
      this.checkRoute(event.urlAfterRedirects || event.url);
    });
    this.loadMenus();
  }

  loadMenus() {
    this.menuService.getAll().subscribe(data => {
      // Filter visible menus and sort by order
      const visibleMenus = data.filter(m => m.isVisible).sort((a, b) => a.order - b.order);
      this.menus = this.buildTree(visibleMenus);
    });
  }

  buildTree(menus: Menu[]): Menu[] {
    const map = new Map<string, Menu>();
    const roots: Menu[] = [];

    // Initialize map and children array
    menus.forEach(menu => {
      map.set(menu.id, { ...menu, children: [] });
    });

    // Connect children to parents
    menus.forEach(menu => {
      const node = map.get(menu.id);
      if (menu.parentId && map.has(menu.parentId)) {
        map.get(menu.parentId)!.children!.push(node!);
      } else {
        roots.push(node!);
      }
    });

    // Sort children
    const sortChildren = (items: Menu[]) => {
      items.forEach(item => {
        if (item.children && item.children.length > 0) {
          item.children.sort((a, b) => a.order - b.order);
          sortChildren(item.children);
        }
      });
    };
    sortChildren(roots);

    return roots;
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
