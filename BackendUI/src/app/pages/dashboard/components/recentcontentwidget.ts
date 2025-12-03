import { Component } from '@angular/core';
import { RippleModule } from 'primeng/ripple';
import { TableModule } from 'primeng/table';
import { ButtonModule } from 'primeng/button';
import { CommonModule } from '@angular/common';
import { TagModule } from 'primeng/tag';

interface ContentItem {
    id?: string;
    title?: string;
    type?: string;
    date?: string;
    status?: string;
}

@Component({
    standalone: true,
    selector: 'app-recent-content-widget',
    imports: [CommonModule, TableModule, ButtonModule, RippleModule, TagModule],
    template: `<div class="card mb-8!">
        <div class="font-semibold text-xl mb-4">Son Eklenen İçerikler</div>
        <p-table [value]="items" [paginator]="true" [rows]="5" responsiveLayout="scroll">
            <ng-template #header>
                <tr>
                    <th pSortableColumn="title">Başlık <p-sortIcon field="title"></p-sortIcon></th>
                    <th pSortableColumn="type">Tür <p-sortIcon field="type"></p-sortIcon></th>
                    <th pSortableColumn="date">Tarih <p-sortIcon field="date"></p-sortIcon></th>
                    <th pSortableColumn="status">Durum <p-sortIcon field="status"></p-sortIcon></th>
                    <th>İşlemler</th>
                </tr>
            </ng-template>
            <ng-template #body let-item>
                <tr>
                    <td style="width: 40%; min-width: 10rem;">{{ item.title }}</td>
                    <td style="width: 20%; min-width: 8rem;">
                        <p-tag [value]="item.type" [severity]="getTypeSeverity(item.type)"></p-tag>
                    </td>
                    <td style="width: 20%; min-width: 8rem;">{{ item.date }}</td>
                    <td style="width: 10%;">
                        <p-tag [value]="item.status" [severity]="getStatusSeverity(item.status)"></p-tag>
                    </td>
                    <td style="width: 10%;">
                        <button pButton pRipple type="button" icon="pi pi-search" class="p-button p-component p-button-text p-button-icon-only"></button>
                    </td>
                </tr>
            </ng-template>
        </p-table>
    </div>`
})
export class RecentContentWidget {
    items: ContentItem[] = [
        {
            id: '1',
            title: '29 Ekim Cumhuriyet Bayramı Kutlamaları',
            type: 'Haber',
            date: '29.10.2023',
            status: 'Yayında'
        },
        {
            id: '2',
            title: 'Su Kesintisi Hakkında Bilgilendirme',
            type: 'Duyuru',
            date: '28.10.2023',
            status: 'Yayında'
        },
        {
            id: '3',
            title: 'Halk Konseri',
            type: 'Etkinlik',
            date: '15.11.2023',
            status: 'Taslak'
        },
        {
            id: '4',
            title: 'Park Yapım İhalesi',
            type: 'İhale',
            date: '01.11.2023',
            status: 'Yayında'
        },
        {
            id: '5',
            title: 'Meclis Toplantısı Kararları',
            type: 'Haber',
            date: '25.10.2023',
            status: 'Yayında'
        }
    ];

    getStatusSeverity(status: string) {
        switch (status) {
            case 'Yayında':
                return 'success';
            case 'Taslak':
                return 'warn';
            case 'Pasif':
                return 'danger';
            default:
                return 'info';
        }
    }

    getTypeSeverity(type: string) {
        switch (type) {
            case 'Haber':
                return 'info';
            case 'Duyuru':
                return 'warn';
            case 'Etkinlik':
                return 'success';
            case 'İhale':
                return 'secondary';
            default:
                return 'contrast';
        }
    }
}
