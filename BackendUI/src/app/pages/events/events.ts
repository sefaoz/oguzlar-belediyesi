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
import { FileUploadModule } from 'primeng/fileupload';
import { ImageModule } from 'primeng/image';
import { EditorModule } from 'primeng/editor';
import { BlockUIModule } from 'primeng/blockui';
import { ProgressBarModule } from 'primeng/progressbar';
import { MessageService, ConfirmationService } from 'primeng/api';
import { EventService } from '../../services/event.service';
import { EventItem } from '../../models/event';
import { environment } from '../../../environments/environment';

@Component({
    selector: 'app-events',
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
        FileUploadModule,
        ImageModule,
        EditorModule,
        BlockUIModule,
        ProgressBarModule
    ],
    providers: [ConfirmationService, DatePipe],
    templateUrl: './events.html',
    styleUrl: './events.scss',
})
export class EventsComponent implements OnInit {
    eventDialog: boolean = false;
    eventsList: EventItem[] = [];
    eventItem: EventItem = {} as EventItem;
    submitted: boolean = false;
    selectedImage: File | undefined;
    selectedImagePreview: string | undefined;
    originalImageUrl: string | undefined;

    constructor(
        private eventService: EventService,
        private messageService: MessageService,
        private confirmationService: ConfirmationService,
        private datePipe: DatePipe
    ) { }

    ngOnInit() {
        this.getEvents();
    }

    getEvents() {
        this.eventService.getAll().subscribe({
            next: (data) => {
                this.eventsList = data;
            },
            error: (error) => {
                console.error('Etkinlikler yüklenirken hata oluştu:', error);
                this.messageService.add({ severity: 'error', summary: 'Hata', detail: 'Etkinlikler yüklenemedi.' });
            }
        });
    }

    openNew() {
        this.eventItem = {} as EventItem;
        // Varsayılan tarih olarak bugünü ata
        const today = new Date();
        // input type="date" formatı: yyyy-MM-dd
        this.eventItem.eventDate = this.datePipe.transform(today, 'yyyy-MM-dd') || '';

        this.submitted = false;
        this.eventDialog = true;
        this.selectedImage = undefined;
        this.selectedImagePreview = undefined;
        this.originalImageUrl = undefined;
    }

    editEvent(item: EventItem) {
        this.eventItem = { ...item };
        if (this.eventItem.eventDate) {
            // Backend sends ISO string, likely standard format. input type="date" needs yyyy-MM-dd
            this.eventItem.eventDate = this.datePipe.transform(this.eventItem.eventDate, 'yyyy-MM-dd') || '';
        }
        this.originalImageUrl = this.eventItem.image;
        this.selectedImagePreview = undefined;
        this.selectedImage = undefined;

        this.eventDialog = true;
    }

    deleteEvent(item: EventItem) {
        this.confirmationService.confirm({
            message: '"' + item.title + '" başlıklı etkinliği silmek istediğinize emin misiniz?',
            header: 'Onay',
            icon: 'pi pi-exclamation-triangle',
            acceptLabel: 'Evet',
            rejectLabel: 'Hayır',
            accept: () => {
                this.eventService.delete(item.id).subscribe({
                    next: () => {
                        this.messageService.add({ severity: 'success', summary: 'Başarılı', detail: 'Etkinlik silindi.', life: 3000 });
                        this.getEvents();
                    },
                    error: (error) => {
                        console.error('Silme hatası:', error);
                        this.messageService.add({ severity: 'error', summary: 'Hata', detail: 'Etkinlik silinirken hata oluştu.' });
                    }
                });
            }
        });
    }

    hideDialog() {
        this.eventDialog = false;
        this.submitted = false;
    }

    onFileSelected(event: any) {
        const file = event.target.files[0];
        if (file) {
            this.selectedImage = file;
            const reader = new FileReader();
            reader.onload = (e) => {
                this.selectedImagePreview = e.target?.result as string;
            };
            reader.readAsDataURL(file);
        }
    }

    isLoading: boolean = false;
    progressValue: number = 0;
    progressInterval: any;

    saveEvent() {
        this.submitted = true;

        if (this.eventItem.title?.trim() && this.eventItem.location?.trim() && this.eventItem.eventDate) {
            this.isLoading = true;
            this.progressValue = 0;
            this.startTimer();

            const finalizeCallback = () => {
                this.isLoading = false;
                this.stopTimer();
            };

            if (this.eventItem.id) {
                // Güncelleme
                const { id, ...eventData } = this.eventItem;
                this.eventService.update(id, eventData, this.selectedImage).subscribe({
                    next: () => {
                        this.messageService.add({ severity: 'success', summary: 'Başarılı', detail: 'Etkinlik güncellendi.', life: 3000 });
                        this.getEvents();
                        this.eventDialog = false;
                        this.eventItem = {} as EventItem;
                    },
                    error: (error) => {
                        console.error('Güncelleme hatası:', error);
                        this.messageService.add({ severity: 'error', summary: 'Hata', detail: 'Etkinlik güncellenirken hata oluştu.' });
                    },
                    complete: finalizeCallback
                });
            } else {
                // Yeni Kayıt
                this.eventService.create(this.eventItem, this.selectedImage).subscribe({
                    next: () => {
                        this.messageService.add({ severity: 'success', summary: 'Başarılı', detail: 'Etkinlik oluşturuldu.', life: 3000 });
                        this.getEvents();
                        this.eventDialog = false;
                        this.eventItem = {} as EventItem;
                    },
                    error: (error) => {
                        console.error('Oluşturma hatası:', error);
                        this.messageService.add({ severity: 'error', summary: 'Hata', detail: 'Etkinlik oluşturulurken hata oluştu.' });
                    },
                    complete: finalizeCallback
                });
            }
        }
    }

    onGlobalFilter(table: Table, event: Event) {
        table.filterGlobal((event.target as HTMLInputElement).value, 'contains');
    }

    formatDate(dateString: string): string {
        if (!dateString) return '';
        const date = new Date(dateString);
        if (!isNaN(date.getTime())) {
            return this.datePipe.transform(date, 'dd.MM.yyyy') || '';
        }
        return dateString;
    }

    getImageUrl(url: string): string {
        if (!url) return '';
        if (url.startsWith('http')) return url;
        return `${environment.imageBaseUrl}${url}`;
    }

    startTimer() {
        this.progressInterval = setInterval(() => {
            this.progressValue += 1;
            if (this.progressValue >= 90) {
                // ...
            }
        }, 100);
    }

    stopTimer() {
        if (this.progressInterval) {
            clearInterval(this.progressInterval);
            this.progressInterval = null;
        }
    }
}
