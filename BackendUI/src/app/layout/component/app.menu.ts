import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { MenuItem } from 'primeng/api';
import { AppMenuitem } from './app.menuitem';

@Component({
    selector: 'app-menu',
    standalone: true,
    imports: [CommonModule, AppMenuitem, RouterModule],
    template: `<ul class="layout-menu">
        <ng-container *ngFor="let item of model; let i = index">
            <li app-menuitem *ngIf="!item.separator" [item]="item" [index]="i" [root]="true"></li>
            <li *ngIf="item.separator" class="menu-separator"></li>
        </ng-container>
    </ul> `
})
export class AppMenu {
    model: MenuItem[] = [];

    ngOnInit() {
        this.model = [
            {
                label: 'Ana Sayfa',
                items: [{ label: 'Dashboard', icon: 'pi pi-fw pi-home', routerLink: ['/'] }]
            },
            {
                label: 'İçerik Yönetimi',
                items: [
                    { label: 'Haberler', icon: 'pi pi-fw pi-megaphone', routerLink: ['/news'] },
                    { label: 'Duyurular', icon: 'pi pi-fw pi-bell', routerLink: ['/announcements'] },
                    { label: 'Etkinlikler', icon: 'pi pi-fw pi-calendar', routerLink: ['/events'] },
                    { label: 'İhaleler', icon: 'pi pi-fw pi-briefcase', routerLink: ['/tenders'] },
                    { label: 'Slider', icon: 'pi pi-fw pi-images', routerLink: ['/slider'] },
                    { label: 'Sayfa İçerikleri', icon: 'pi pi-fw pi-file-edit', routerLink: ['/pages'] }
                ]
            },
            {
                label: 'Kurumsal',
                items: [
                    { label: 'Birimler', icon: 'pi pi-fw pi-building', routerLink: ['/units'] },
                    { label: 'Araç Parkı', icon: 'pi pi-fw pi-car', routerLink: ['/vehicles'] },
                    { label: 'Meclis Kararları', icon: 'pi pi-fw pi-book', routerLink: ['/council-decisions'] },
                    { label: 'KVKK Belgeleri', icon: 'pi pi-fw pi-lock', routerLink: ['/kvkk'] }
                ]
            },
            {
                label: 'Medya',
                items: [
                    { label: 'Galeri', icon: 'pi pi-fw pi-image', routerLink: ['/gallery'] }
                ]
            },
            {
                label: 'Sistem',
                items: [
                    { label: 'Kullanıcılar', icon: 'pi pi-fw pi-users', routerLink: ['/users'] },
                    { label: 'Site Ayarları', icon: 'pi pi-fw pi-cog', routerLink: ['/site-settings'] }
                ]
            }
        ];
    }
}
