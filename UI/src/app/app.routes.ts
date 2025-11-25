import { Routes } from '@angular/router';
import { LayoutComponent } from './components/layout/layout';
import { HomeComponent } from './components/home/home';
import { BaskanHakkindaComponent } from './components/baskan-hakkinda/baskan-hakkinda';

export const routes: Routes = [
  {
    path: '',
    component: LayoutComponent,
    children: [
      { path: '', component: HomeComponent },
      { path: 'baskan-hakkinda', component: BaskanHakkindaComponent }
    ]
  }
];
