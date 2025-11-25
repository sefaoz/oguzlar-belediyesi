import { Routes } from '@angular/router';
import { LayoutComponent } from './layout/layout';
import { HomeComponent } from './pages/home/home';
import { BaskanHakkindaComponent } from './pages/baskan-hakkinda/baskan-hakkinda';
import { BaskanMesaji } from './pages/baskan-mesaji/baskan-mesaji';
import { BaskanaMesaj } from './pages/baskana-mesaj/baskana-mesaj';
import { IletisimComponent } from './pages/iletisim/iletisim';
import { HaberlerComponent } from './pages/haberler/haberler';
import { TarihimizComponent } from './pages/tarihimiz/tarihimiz';

export const routes: Routes = [
  {
    path: '',
    component: LayoutComponent,
    children: [
      { path: '', component: HomeComponent },
      { path: 'baskan-hakkinda', component: BaskanHakkindaComponent },
      { path: 'baskandan-mesaj', component: BaskanMesaji },
      { path: 'baskana-mesaj', component: BaskanaMesaj },
      { path: 'iletisim', component: IletisimComponent },
      { path: 'haberler', component: HaberlerComponent },
      { path: 'oguzlar-tarihi', component: TarihimizComponent }
    ]
  }
];
