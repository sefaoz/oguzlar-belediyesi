import { Component, OnInit } from '@angular/core';
import { CommonModule, DatePipe } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Table, TableModule } from 'primeng/table';
import { DialogModule } from 'primeng/dialog';
import { ButtonModule } from 'primeng/button';
import { RippleModule } from 'primeng/ripple';
import { ToastModule } from 'primeng/toast';
import { ToolbarModule } from 'primeng/toolbar';
import { ConfirmDialogModule } from 'primeng/confirmdialog';
import { InputTextModule } from 'primeng/inputtext';
import { TextareaModule } from 'primeng/textarea';
import { MessageService, ConfirmationService } from 'primeng/api';
import { ContactMessageService } from '../../services/contact-message.service';
import { ContactMessage } from '../../models/contact-message';
import { TagModule } from 'primeng/tag';

@Component({
    selector: 'app-contact-messages',
    standalone: true,
    imports: [
        CommonModule,
        FormsModule,
        TableModule,
        DialogModule,
        ButtonModule,
        RippleModule,
        ToastModule,
        ToolbarModule,
        ConfirmDialogModule,
        InputTextModule,
        TextareaModule,
        TagModule
    ],
    providers: [DatePipe, MessageService, ConfirmationService],
    templateUrl: './contact-messages.html',
    styleUrl: './contact-messages.scss',
})
export class ContactMessagesComponent implements OnInit {
    messageDialog: boolean = false;
    messages: ContactMessage[] = [];
    message: ContactMessage = {} as ContactMessage;
    submitted: boolean = false;

    constructor(
        private messageService: ContactMessageService,
        private messageServiceToast: MessageService,
        private confirmationService: ConfirmationService,
        private datePipe: DatePipe
    ) { }

    ngOnInit() {
        this.getMessages();
    }

    getMessages() {
        this.messageService.getAll().subscribe({
            next: (data) => {
                this.messages = data;
            },
            error: (error) => {
                console.error('Mesajlar yüklenirken hata oluştu:', error);
                this.messageServiceToast.add({ severity: 'error', summary: 'Hata', detail: 'Mesajlar yüklenemedi.' });
            }
        });
    }

    viewMessage(message: ContactMessage) {
        this.message = { ...message };
        this.messageDialog = true;

        // Mark as read logic if needed
        if (!this.message.isRead) {
            this.messageService.markAsRead(this.message.id).subscribe(() => {
                // Update local state if needed
                const index = this.messages.findIndex(m => m.id === message.id);
                if (index !== -1) {
                    this.messages[index].isRead = true;
                }
            });
        }
    }

    deleteMessage(message: ContactMessage) {
        this.confirmationService.confirm({
            message: message.name + ' isimli kişiden gelen mesajı silmek istediğinize emin misiniz?',
            header: 'Onay',
            icon: 'pi pi-exclamation-triangle',
            acceptLabel: 'Evet',
            rejectLabel: 'Hayır',
            accept: () => {
                this.messageService.delete(message.id).subscribe({
                    next: () => {
                        this.messageServiceToast.add({ severity: 'success', summary: 'Başarılı', detail: 'Mesaj silindi.', life: 3000 });
                        // Remove from local list to avoid reload flicker or fetch again
                        this.messages = this.messages.filter(m => m.id !== message.id);
                    },
                    error: (error) => {
                        console.error('Silme hatası:', error);
                        this.messageServiceToast.add({ severity: 'error', summary: 'Hata', detail: 'Mesaj silinirken hata oluştu.' });
                    }
                });
            }
        });
    }

    hideDialog() {
        this.messageDialog = false;
        this.submitted = false;
    }

    onGlobalFilter(table: Table, event: Event) {
        table.filterGlobal((event.target as HTMLInputElement).value, 'contains');
    }

    formatDate(dateString: string): string {
        if (!dateString) return '';
        const date = new Date(dateString);
        if (!isNaN(date.getTime())) {
            return this.datePipe.transform(date, 'dd.MM.yyyy HH:mm') || '';
        }
        return dateString;
    }
}
