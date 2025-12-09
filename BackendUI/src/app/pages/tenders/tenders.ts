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
import { TagModule } from 'primeng/tag';
import { SelectModule } from 'primeng/select';
import { MessageService, ConfirmationService } from 'primeng/api';
import { TenderService } from '../../services/tender.service';
import { Tender, TenderDocument } from '../../models/tender';
import { environment } from '../../../environments/environment';

@Component({
    selector: 'app-tenders',
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
        ProgressBarModule,
        TagModule,
        SelectModule
    ],
    providers: [ConfirmationService, DatePipe],
    templateUrl: './tenders.html',
    styleUrl: './tenders.scss',
})
export class TendersComponent implements OnInit {
    tenderDialog: boolean = false;
    tenders: Tender[] = [];
    tender: Tender = {} as Tender;
    submitted: boolean = false;
    isLoading: boolean = false;
    progressValue: number = 0;
    progressInterval: any;

    // Documents
    selectedDocumentFiles: File[] = [];
    // Existing Parsed Documents (Name + Url)
    existingDocuments: TenderDocument[] = [];

    statusOptions = [
        { label: 'Açık', value: 'Open' },
        { label: 'Sonuçlandı', value: 'Concluded' },
        { label: 'İptal Edildi', value: 'Cancelled' },
        { label: 'Taslak', value: 'Draft' }
    ];

    // File size limits
    private readonly MAX_FILE_SIZE = 50 * 1024 * 1024; // 50MB per file
    private readonly MAX_TOTAL_SIZE = 100 * 1024 * 1024; // 100MB total

    constructor(
        private tenderService: TenderService,
        private messageService: MessageService,
        private confirmationService: ConfirmationService,
        private datePipe: DatePipe
    ) { }

    ngOnInit() {
        this.getTenders();
    }

    getTenders() {
        this.tenderService.getAll().subscribe({
            next: (data) => {
                this.tenders = data;
            },
            error: (error) => {
                console.error('İhaleler yüklenirken hata oluştu:', error);
                this.messageService.add({ severity: 'error', summary: 'Hata', detail: 'İhaleler yüklenemedi.' });
            }
        });
    }

    openNew() {
        this.tender = {} as Tender;
        const today = new Date();
        this.tender.tenderDate = this.datePipe.transform(today, 'yyyy-MM-dd') || '';

        this.submitted = false;
        this.tenderDialog = true;

        // Reset Docs
        this.selectedDocumentFiles = [];
        this.existingDocuments = [];
    }

    editTender(tenderItem: Tender) {
        this.tender = { ...tenderItem };
        if (this.tender.tenderDate) {
            this.tender.tenderDate = this.datePipe.transform(this.tender.tenderDate, 'yyyy-MM-dd') || '';
        }

        this.selectedDocumentFiles = [];
        this.existingDocuments = [];

        if (this.tender.documentsJson) {
            try {
                this.existingDocuments = JSON.parse(this.tender.documentsJson);
            } catch (e) {
                this.existingDocuments = [];
            }
        }

        this.tenderDialog = true;
    }

    deleteTender(tenderItem: Tender) {
        this.confirmationService.confirm({
            message: '"' + tenderItem.title + '" başlıklı ihaleyi silmek istediğinize emin misiniz?',
            header: 'Onay',
            icon: 'pi pi-exclamation-triangle',
            acceptLabel: 'Evet',
            rejectLabel: 'Hayır',
            accept: () => {
                this.tenderService.delete(tenderItem.id).subscribe({
                    next: () => {
                        this.messageService.add({ severity: 'success', summary: 'Başarılı', detail: 'İhale silindi.', life: 3000 });
                        this.getTenders();
                    },
                    error: (error) => {
                        console.error('Silme hatası:', error);
                        this.messageService.add({ severity: 'error', summary: 'Hata', detail: 'İhale silinirken hata oluştu.' });
                    }
                });
            }
        });
    }

    hideDialog() {
        this.tenderDialog = false;
        this.submitted = false;
    }

    onDocumentFilesSelected(event: any) {
        if (event.target.files && event.target.files.length > 0) {
            const files: FileList = event.target.files;
            const allowedExtensions = ['.pdf', '.doc', '.docx', '.xls', '.xlsx'];

            // Calculate current total size
            let currentTotalSize = this.selectedDocumentFiles.reduce((sum, f) => sum + f.size, 0);

            for (let i = 0; i < files.length; i++) {
                const file = files[i];
                const extension = '.' + file.name.split('.').pop()?.toLowerCase();

                // Check file extension
                if (!allowedExtensions.includes(extension)) {
                    this.messageService.add({
                        severity: 'warn',
                        summary: 'Geçersiz Dosya',
                        detail: `${file.name} eklenmedi. Sadece PDF, Word ve Excel dosyaları kabul edilir.`
                    });
                    continue;
                }

                // Check individual file size (100MB limit)
                if (file.size > this.MAX_FILE_SIZE) {
                    const fileSizeMB = (file.size / (1024 * 1024)).toFixed(2);
                    this.messageService.add({
                        severity: 'error',
                        summary: 'Dosya Boyutu Çok Büyük',
                        detail: `${file.name} (${fileSizeMB} MB) eklenemedi. Tek dosya maksimum 50 MB olabilir.`,
                        life: 5000
                    });
                    continue;
                }

                // Check total size limit (500MB)
                if (currentTotalSize + file.size > this.MAX_TOTAL_SIZE) {
                    const totalSizeMB = (this.MAX_TOTAL_SIZE / (1024 * 1024)).toFixed(0);
                    this.messageService.add({
                        severity: 'error',
                        summary: 'Toplam Boyut Aşıldı',
                        detail: `${file.name} eklenemedi. Toplam dosya boyutu ${totalSizeMB} MB'ı geçemez.`,
                        life: 5000
                    });
                    continue;
                }

                // File is valid, add it
                this.selectedDocumentFiles.push(file);
                currentTotalSize += file.size;
            }

            // Clear input so same file can be selected again if needed
            event.target.value = '';
        }
    }

    removeNewFile(index: number) {
        this.selectedDocumentFiles.splice(index, 1);
    }

    getTotalFileSize(): string {
        const totalBytes = this.selectedDocumentFiles.reduce((sum, f) => sum + f.size, 0);
        return (totalBytes / (1024 * 1024)).toFixed(2);
    }

    removeExistingDocument(index: number) {
        this.existingDocuments.splice(index, 1);
        // Update is committed on Save
    }

    saveTender() {
        this.submitted = true;

        if (this.tender.title?.trim()) {
            this.isLoading = true;
            this.progressValue = 0;
            this.startTimer();

            // Prepare JSON for existing docs
            this.tender.documentsJson = JSON.stringify(this.existingDocuments);

            const finalizeCallback = () => {
                this.isLoading = false;
                this.stopTimer();
            };

            if (this.tender.id) {
                // Update
                const { id, ...tenderData } = this.tender;
                // Backend doesn't need 'documentsList' which is UI helper
                // But TS interface has optional documentsList, TenderService only sends matching correct FormData
                this.tenderService.update(id, tenderData, this.selectedDocumentFiles).subscribe({
                    next: () => {
                        this.messageService.add({ severity: 'success', summary: 'Başarılı', detail: 'İhale güncellendi.', life: 3000 });
                        this.getTenders();
                        this.tenderDialog = false;
                        this.tender = {} as Tender;
                    },
                    error: (error) => {
                        console.error('Güncelleme hatası:', error);
                        this.messageService.add({ severity: 'error', summary: 'Hata', detail: 'İhale güncellenirken hata oluştu.' });
                    },
                    complete: finalizeCallback
                });
            } else {
                // Create
                this.tenderService.create(this.tender, this.selectedDocumentFiles).subscribe({
                    next: () => {
                        this.messageService.add({ severity: 'success', summary: 'Başarılı', detail: 'İhale oluşturuldu.', life: 3000 });
                        this.getTenders();
                        this.tenderDialog = false;
                        this.tender = {} as Tender;
                    },
                    error: (error) => {
                        console.error('Oluşturma hatası:', error);
                        this.messageService.add({ severity: 'error', summary: 'Hata', detail: 'İhale oluşturulurken hata oluştu.' });
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

    startTimer() {
        this.progressInterval = setInterval(() => {
            this.progressValue += 1;
            if (this.progressValue >= 90) { }
        }, 100);
    }

    stopTimer() {
        if (this.progressInterval) {
            clearInterval(this.progressInterval);
            this.progressInterval = null;
        }
    }

    getDocName(doc: TenderDocument): string {
        return doc.Title || 'Döküman';
    }

    getSeverity(status: string) {
        switch (status) {
            case 'Open':
            case 'active':
                return 'success';
            case 'Concluded':
            case 'completed':
                return 'info';
            case 'Cancelled':
            case 'passive':
                return 'danger';
            case 'Draft':
                return 'warn';
            default:
                return 'info';
        }
    }

    getStatusLabel(status: string) {
        const option = this.statusOptions.find(opt => opt.value === status);
        if (option) return option.label;

        // Fallback or mapping for legacy values if not updated in DB
        if (status === 'active') return 'Açık';
        if (status === 'completed') return 'Sonuçlandı';
        if (status === 'passive') return 'İptal Edildi';

        return status;
    }
}
