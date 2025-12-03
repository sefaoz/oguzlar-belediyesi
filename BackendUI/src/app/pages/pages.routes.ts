import { Routes } from '@angular/router';
import { Empty } from './empty/empty';

export default [
    { path: 'empty', component: Empty },
    { path: 'slider', loadChildren: () => import('./slider/slider.routes').then(m => m.SLIDER_ROUTES) },
    { path: '**', redirectTo: '/notfound' }
] as Routes;
