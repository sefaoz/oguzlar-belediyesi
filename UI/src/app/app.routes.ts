import { Routes } from '@angular/router';
import { LayoutComponent } from './layout/layout';
import { HomeComponent } from './pages/home/home';
import { BaskanHakkindaComponent } from './pages/baskan-hakkinda/baskan-hakkinda';
import { BaskanMesaji } from './pages/baskan-mesaji/baskan-mesaji';
import { BaskanaMesaj } from './pages/baskana-mesaj/baskana-mesaj';
import { IletisimComponent } from './pages/iletisim/iletisim';
import { HaberlerComponent } from './pages/haberler/haberler';
import { TarihimizComponent } from './pages/tarihimiz/tarihimiz';
import { CografiYapi } from './pages/cografi-yapi/cografi-yapi';
import { HaberDetayComponent } from './pages/haber-detay/haber-detay';
import { DuyurularComponent } from './pages/duyurular/duyurular';
import { EtkinliklerComponent } from './pages/etkinlikler/etkinlikler';
import { IhalelerComponent } from './pages/ihaleler/ihaleler';
import { DuyuruDetay } from './pages/duyuru-detay/duyuru-detay';
import { EtkinlikDetay } from './pages/etkinlik-detay/etkinlik-detay';
import { IhaleDetay } from './pages/ihale-detay/ihale-detay';
import { FotografGalerisi } from './pages/fotograf-galerisi/fotograf-galerisi';
import { Meclis } from './pages/meclis/meclis';
import { Kvkk } from './pages/kvkk/kvkk';
import { AracParki } from './pages/arac-parki/arac-parki';
import { BirimlerComponent } from './pages/birimler/birimler';

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
      { path: 'haberler/:slug', component: HaberDetayComponent },
      { path: 'ilcemizin-tarihi', component: TarihimizComponent },
      { path: 'cografi-yapi', component: CografiYapi },
      { path: 'duyurular', component: DuyurularComponent },
      { path: 'duyurular/:slug', component: DuyuruDetay },
      { path: 'etkinlikler', component: EtkinliklerComponent },
      { path: 'etkinlikler/:slug', component: EtkinlikDetay },
      { path: 'ihaleler', component: IhalelerComponent },
      { path: 'ihaleler/:slug', component: IhaleDetay },
      { path: 'fotograf-galerisi', component: FotografGalerisi },
      { path: 'fotograf-galerisi/:slug', component: FotografGalerisi },
      { path: 'meclis', component: Meclis },
      { path: 'kvkk', component: Kvkk },
      { path: 'arac-parki', component: AracParki },
      { path: 'birimler', redirectTo: 'birimler/ozel-kalem', pathMatch: 'full' },
      { path: 'birimler/:unit', component: BirimlerComponent }
    ]
  }
];
