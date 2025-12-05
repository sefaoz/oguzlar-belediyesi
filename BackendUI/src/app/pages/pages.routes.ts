import { Routes } from '@angular/router';
import { Empty } from './empty/empty';
import { PageContentComponent } from './page-content/page-content.component';

export default [
    { path: '', component: PageContentComponent },
    { path: 'empty', component: Empty },
    { path: 'news', loadComponent: () => import('./news/news').then(m => m.NewsComponent) },
    { path: 'slider', loadChildren: () => import('./slider/slider.routes').then(m => m.SLIDER_ROUTES) },
    { path: 'menus', loadComponent: () => import('./menu-management/menu-management.component').then(m => m.MenuManagementComponent) },
    { path: '**', redirectTo: '/notfound' }
] as Routes;
