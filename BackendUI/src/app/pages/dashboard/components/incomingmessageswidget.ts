import { Component, OnInit, inject, signal } from '@angular/core';
import { CommonModule, DatePipe } from '@angular/common';
import { TableModule } from 'primeng/table';
import { ButtonModule } from 'primeng/button';
import { RippleModule } from 'primeng/ripple';
import { TagModule } from 'primeng/tag';
import { ContactMessageService } from '../../../services/contact-message.service';
import { ContactMessage } from '../../../models/contact-message';
import { MessageService } from 'primeng/api';
import { TooltipModule } from 'primeng/tooltip';
import { DialogModule } from 'primeng/dialog';

@Component({
    standalone: true,
    selector: 'app-incoming-messages-widget',
    imports: [CommonModule, TableModule, ButtonModule, RippleModule, TagModule, TooltipModule, DialogModule],
    template: `
        <div class="card mb-8!">
            <div class="font-semibold text-xl mb-4">Gelen Mesajlar</div>
            <p-table [value]="messages()" [paginator]="true" [rows]="5" responsiveLayout="scroll">
                <ng-template #header>
                    <tr>
                        <th pSortableColumn="name">Ad Soyad <p-sortIcon field="name"></p-sortIcon></th>
                        <th pSortableColumn="messageType">Tür <p-sortIcon field="messageType"></p-sortIcon></th>
                        <th pSortableColumn="createdAt">Tarih <p-sortIcon field="createdAt"></p-sortIcon></th>
                        <th pSortableColumn="isRead">Durum <p-sortIcon field="isRead"></p-sortIcon></th>
                        <th class="text-right">İşlemler</th>
                    </tr>
                </ng-template>
                <ng-template #body let-message>
                    <tr>
                        <td>{{ message.name }}</td>
                        <td>
                            <p-tag [value]="message.messageType === 'MayorMessage' ? 'Başkana Mesaj' : 'İletişim'" 
                                   [severity]="message.messageType === 'MayorMessage' ? 'warn' : 'info'">
                            </p-tag>
                        </td>
                        <td>{{ message.createdAt | date:'dd.MM.yyyy HH:mm' }}</td>
                        <td>
                            <p-tag [value]="message.isRead ? 'Okundu' : 'Okunmadı'" 
                                   [severity]="message.isRead ? 'success' : 'danger'">
                            </p-tag>
                        </td>
                        <td class="text-right">
                             <button pButton pRipple type="button" icon="pi pi-eye" 
                                    class="p-button-rounded p-button-text p-button-info mr-2" 
                                    (click)="viewMessage(message)" 
                                    pTooltip="Görüntüle" tooltipPosition="top"></button>
                            <button pButton pRipple type="button" icon="pi pi-check" 
                                    class="p-button-rounded p-button-text p-button-success mr-2" 
                                    *ngIf="!message.isRead"
                                    (click)="markAsRead(message)" 
                                    pTooltip="Okundu İşaretle" tooltipPosition="top"></button>
                            <button pButton pRipple type="button" icon="pi pi-trash" 
                                    class="p-button-rounded p-button-text p-button-danger" 
                                    (click)="deleteMessage(message)" 
                                    pTooltip="Sil" tooltipPosition="top"></button>
                        </td>
                    </tr>
                </ng-template>
            </p-table>
        </div>

        <p-dialog [(visible)]="visible" [style]="{width: '500px'}" header="Mesaj Detayı" [modal]="true" styleClass="p-fluid" [draggable]="false" [resizable]="false">
            <ng-template pTemplate="content">
                <div *ngIf="selectedMessage" class="flex flex-col gap-4">
                    <div class="flex items-center gap-3 p-3 bg-surface-50 dark:bg-surface-800 rounded-lg border border-surface-200 dark:border-surface-700">
                        <div class="flex items-center justify-center w-12 h-12 rounded-full bg-primary-100 dark:bg-primary-900/30 text-primary-600 dark:text-primary-400">
                            <i class="pi pi-user text-xl"></i>
                        </div>
                        <div class="flex-1">
                            <div class="text-sm text-muted-color">Gönderen</div>
                            <div class="font-semibold text-lg">{{ selectedMessage.name }}</div>
                        </div>
                        <div class="text-right">
                            <p-tag [value]="selectedMessage.messageType === 'MayorMessage' ? 'Başkana Mesaj' : 'İletişim Formu'"
                                   [severity]="selectedMessage.messageType === 'MayorMessage' ? 'warn' : 'info'"></p-tag>
                        </div>
                    </div>

                    <div class="grid grid-cols-2 gap-4">
                        <div class="p-3 bg-surface-50 dark:bg-surface-800 rounded-lg border border-surface-200 dark:border-surface-700">
                            <div class="text-xs text-muted-color mb-1 uppercase font-medium">E-posta</div>
                            <div class="font-medium text-surface-900 dark:text-surface-0 break-all">{{ selectedMessage.email }}</div>
                        </div>
                        <div class="p-3 bg-surface-50 dark:bg-surface-800 rounded-lg border border-surface-200 dark:border-surface-700">
                            <div class="text-xs text-muted-color mb-1 uppercase font-medium">Telefon</div>
                            <div class="font-medium text-surface-900 dark:text-surface-0">{{ selectedMessage.phone }}</div>
                        </div>
                    </div>

                     <div class="p-3 bg-surface-50 dark:bg-surface-800 rounded-lg border border-surface-200 dark:border-surface-700">
                        <div class="flex items-center gap-2 mb-2">
                            <i class="pi pi-calendar text-muted-color"></i>
                            <span class="text-xs text-muted-color uppercase font-medium">Tarih</span>
                        </div>
                        <div class="font-medium text-surface-900 dark:text-surface-0">{{ selectedMessage.createdAt | date:'dd MMMM yyyy HH:mm' }}</div>
                    </div>

                    <div class="p-4 bg-surface-50 dark:bg-surface-800 rounded-lg border border-surface-200 dark:border-surface-700">
                        <div class="text-xs text-muted-color mb-2 uppercase font-medium">Mesaj İçeriği</div>
                        <div class="text-surface-900 dark:text-surface-0 leading-relaxed whitespace-pre-wrap">{{ selectedMessage.message }}</div>
                    </div>
                </div>
            </ng-template>
            <ng-template pTemplate="footer">
                <button pButton pRipple label="Kapat" icon="pi pi-times" class="p-button-text" (click)="visible = false"></button>
            </ng-template>
        </p-dialog>
    `
})
export class IncomingMessagesWidget implements OnInit {
    private service = inject(ContactMessageService);
    // Note: Assuming MessageService is provided at root or parent
    private messageService = inject(MessageService);

    messages = signal<ContactMessage[]>([]);
    visible: boolean = false;
    selectedMessage: ContactMessage | null = null;

    ngOnInit() {
        this.loadMessages();
    }

    loadMessages() {
        this.service.getAll().subscribe({
            next: (data) => this.messages.set(data),
            error: (err) => console.error('Mesajlar yüklenirken hata:', err)
        });
    }

    viewMessage(message: ContactMessage) {
        this.selectedMessage = message;
        this.visible = true;
    }

    markAsRead(message: ContactMessage) {
        this.service.markAsRead(message.id).subscribe({
            next: () => {
                this.messageService.add({ severity: 'success', summary: 'Başarılı', detail: 'Mesaj okundu olarak işaretlendi.' });
                this.loadMessages();
            },
            error: () => {
                this.messageService.add({ severity: 'error', summary: 'Hata', detail: 'İşlem başarısız.' });
            }
        });
    }

    deleteMessage(message: ContactMessage) {
        // Simple confirmation could be added here, but for now just delete
        this.service.delete(message.id).subscribe({
            next: () => {
                this.messageService.add({ severity: 'success', summary: 'Başarılı', detail: 'Mesaj silindi.' });
                this.loadMessages();
            },
            error: () => {
                this.messageService.add({ severity: 'error', summary: 'Hata', detail: 'Silme işlemi başarısız.' });
            }
        });
    }
}
