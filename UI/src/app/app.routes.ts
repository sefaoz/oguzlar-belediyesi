import { Routes } from '@angular/router';
import { LayoutComponent } from './components/layout/layout';
import { HomeComponent } from './components/home/home';
import { BaskanHakkindaComponent } from './components/baskan-hakkinda/baskan-hakkinda';
import { BaskanMesaji } from './components/baskan-mesaji/baskan-mesaji';
import { BaskanaMesaj } from './components/baskana-mesaj/baskana-mesaj';
import { IletisimComponent } from './components/iletisim/iletisim';

export const routes: Routes = [
  {
    path: '',
    component: LayoutComponent,
    children: [
      { path: '', component: HomeComponent },
      { path: 'baskan-hakkinda', component: BaskanHakkindaComponent },
      { path: 'baskandan-mesaj', component: BaskanMesaji },
      { path: 'baskana-mesaj', component: BaskanaMesaj },
      { path: 'iletisim', component: IletisimComponent }
    ]
  }
];
