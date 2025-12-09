import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
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
import { AnnouncementService } from '../../services/announcement.service';
import { Announcement } from '../../models/announcement';
import { environment } from '../../../environments/environment';

@Component({
    selector: 'app-announcements',
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
    providers: [MessageService, ConfirmationService, DatePipe],
    templateUrl: './announcements.html',
    styleUrl: './announcements.scss',
})
export class AnnouncementsComponent implements OnInit {
    announcementDialog: boolean = false;
    announcementsList: Announcement[] = [];
    announcement: Announcement = {} as Announcement;
    submitted: boolean = false;

    constructor(
        private announcementService: AnnouncementService,
        private messageService: MessageService,
        private confirmationService: ConfirmationService,
        private datePipe: DatePipe,
        private cdr: ChangeDetectorRef
    ) { }

    ngOnInit() {
        this.getAnnouncements();
    }

    getAnnouncements() {
        this.announcementService.getAll().subscribe({
            next: (data) => {
                this.announcementsList = data;
            },
            error: (error) => {
                console.error('Duyurular yüklenirken hata oluştu:', error);
                this.messageService.add({ severity: 'error', summary: 'Hata', detail: 'Duyurular yüklenemedi.' });
            }
        });
    }

    openNew() {
        this.announcement = {} as Announcement;
        // Varsayılan tarih olarak bugünü ata
        const today = new Date();
        this.announcement.date = this.datePipe.transform(today, 'yyyy-MM-dd') || '';

        this.submitted = false;
        this.announcementDialog = true;
    }

    editAnnouncement(announcementItem: Announcement) {
        try {
            this.announcement = { ...announcementItem };
            if (this.announcement.date) {
                // Handle potential Turkish date format "dd Month yyyy"
                let dateValue: Date | string = this.announcement.date;

                if (typeof dateValue === 'string' && dateValue.includes(' ')) {
                    const parts = dateValue.split(' ');
                    if (parts.length === 3) {
                        const day = parseInt(parts[0], 10);
                        const monthStr = parts[1].toLowerCase();
                        const year = parseInt(parts[2], 10);

                        const turkishMonths: { [key: string]: number } = {
                            'ocak': 0, 'şubat': 1, 'mart': 2, 'nisan': 3, 'mayıs': 4, 'haziran': 5,
                            'temmuz': 6, 'ağustos': 7, 'eylül': 8, 'ekim': 9, 'kasım': 10, 'aralık': 11,
                            'kasim': 10, 'subat': 1, 'agustos': 7, 'eylul': 8, 'aralik': 11
                        };

                        if (turkishMonths.hasOwnProperty(monthStr)) {
                            // Create date in UTC to avoid timezone shifts when pipe transforms it
                            dateValue = new Date(year, turkishMonths[monthStr], day);
                        }
                    }
                }

                // If conversion to Date object was successful or it was already a standard string, transform it
                try {
                    const transformedDate = this.datePipe.transform(dateValue, 'yyyy-MM-dd');
                    this.announcement.date = transformedDate || '';
                } catch (pipeError) {
                    console.warn('DatePipe transform failed, trying direct ISO slice', pipeError);
                    // Fallback: if it's an ISO string but DatePipe failed slightly (unlikely if valid), or just clear it
                    if (typeof dateValue === 'string' && dateValue.includes('T')) {
                        this.announcement.date = dateValue.split('T')[0];
                    } else {
                        // Keep original if we can't parse, though <input type="date"> wont show it
                    }
                }
            }
            this.submitted = false;
            this.announcementDialog = true;
            this.cdr.detectChanges(); // Ensure UI updates
        } catch (e) {
            console.error('Edit error:', e);
            this.messageService.add({ severity: 'error', summary: 'Hata', detail: 'Düzenleme penceresi açılamadı.' });
        }
    }

    deleteAnnouncement(announcementItem: Announcement) {
        this.confirmationService.confirm({
            message: '"' + announcementItem.title + '" başlıklı duyuruyu silmek istediğinize emin misiniz?',
            header: 'Onay',
            icon: 'pi pi-exclamation-triangle',
            acceptLabel: 'Evet',
            rejectLabel: 'Hayır',
            accept: () => {
                this.announcementService.delete(announcementItem.id).subscribe({
                    next: () => {
                        this.messageService.add({ severity: 'success', summary: 'Başarılı', detail: 'Duyuru silindi.', life: 3000 });
                        this.getAnnouncements();
                    },
                    error: (error) => {
                        console.error('Silme hatası:', error);
                        this.messageService.add({ severity: 'error', summary: 'Hata', detail: 'Duyuru silinirken hata oluştu.' });
                    }
                });
            }
        });
    }

    hideDialog() {
        this.announcementDialog = false;
        this.submitted = false;
    }

    isLoading: boolean = false;
    progressValue: number = 0;
    progressInterval: any;

    startTimer() {
        this.progressInterval = setInterval(() => {
            this.progressValue += 1;
            if (this.progressValue >= 90) {
                // Do not reach 100% automatically
            }
        }, 100);
    }

    stopTimer() {
        if (this.progressInterval) {
            clearInterval(this.progressInterval);
            this.progressInterval = null;
        }
    }

    saveAnnouncement() {
        this.submitted = true;

        if (this.announcement.title?.trim()) {
            this.isLoading = true;
            this.progressValue = 0;
            this.startTimer();

            this.announcement.tags = [];

            const finalizeCallback = () => {
                this.isLoading = false;
                this.stopTimer();
            };

            if (this.announcement.id) {
                // Güncelleme
                const { id, ...announcementData } = this.announcement;
                this.announcementService.update(id, announcementData).subscribe({
                    next: () => {
                        this.messageService.add({ severity: 'success', summary: 'Başarılı', detail: 'Duyuru güncellendi.', life: 3000 });
                        this.getAnnouncements();
                        this.announcementDialog = false;
                        this.announcement = {} as Announcement;
                    },
                    error: (error) => {
                        console.error('Güncelleme hatası:', error);
                        this.messageService.add({ severity: 'error', summary: 'Hata', detail: 'Duyuru güncellenirken hata oluştu.' });
                    },
                    complete: finalizeCallback
                });
            } else {
                // Yeni Kayıt
                this.announcementService.create(this.announcement).subscribe({
                    next: () => {
                        this.messageService.add({ severity: 'success', summary: 'Başarılı', detail: 'Duyuru oluşturuldu.', life: 3000 });
                        this.getAnnouncements();
                        this.announcementDialog = false;
                        this.announcement = {} as Announcement;
                    },
                    error: (error) => {
                        console.error('Oluşturma hatası:', error);
                        this.messageService.add({ severity: 'error', summary: 'Hata', detail: 'Duyuru oluşturulurken hata oluştu.' });
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
        // Check if date is valid
        if (!isNaN(date.getTime())) {
            // Use explicit formatting to ensure consistency
            return this.datePipe.transform(date, 'dd.MM.yyyy') || '';
        }
        return dateString;
    }

    getImageUrl(url: string): string {
        if (!url) return '';
        if (url.startsWith('http')) return url;
        return `${environment.imageBaseUrl}${url}`;
    }
}
