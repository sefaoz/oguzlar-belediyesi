import { Routes } from '@angular/router';
import { Empty } from './empty/empty';
import { PageContentComponent } from './page-content/page-content.component';

export default [
    { path: '', component: PageContentComponent },
    { path: 'empty', component: Empty },
    { path: 'news', loadComponent: () => import('./news/news').then(m => m.NewsComponent) },
    { path: 'events', loadComponent: () => import('./events/events').then(m => m.EventsComponent) },
    { path: 'announcements', loadComponent: () => import('./announcements/announcements').then(m => m.AnnouncementsComponent) },
    { path: 'slider', loadChildren: () => import('./slider/slider.routes').then(m => m.SLIDER_ROUTES) },
    { path: 'menus', loadComponent: () => import('./menu-management/menu-management.component').then(m => m.MenuManagementComponent) },
    { path: 'tenders', loadComponent: () => import('./tenders/tenders').then(m => m.TendersComponent) },
    { path: 'units', loadComponent: () => import('./units/units').then(m => m.UnitsComponent) },
    { path: 'council', loadComponent: () => import('./council-decisions/council-decisions').then(m => m.CouncilDecisionsComponent) },
    { path: 'vehicles', loadComponent: () => import('./vehicles/vehicles').then(m => m.VehiclesComponent) },
    { path: 'kvkk', loadComponent: () => import('./kvkk-documents/kvkk-documents.component').then(m => m.KvkkDocumentsComponent) },
    { path: 'gallery', loadComponent: () => import('./gallery/gallery').then(m => m.GalleryComponent) },
    { path: 'users', loadComponent: () => import('./users/users').then(m => m.UsersComponent) },
    { path: 'site-settings', loadComponent: () => import('./site-settings/site-settings').then(m => m.SiteSettingsComponent) },
    { path: 'contact-messages', loadComponent: () => import('./contact-messages/contact-messages').then(m => m.ContactMessagesComponent) },
    { path: '**', redirectTo: '/notfound' }
] as Routes;
