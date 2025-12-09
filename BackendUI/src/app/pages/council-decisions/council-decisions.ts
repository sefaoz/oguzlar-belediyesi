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
import { EditorModule } from 'primeng/editor';
import { BlockUIModule } from 'primeng/blockui';
import { MessageService, ConfirmationService } from 'primeng/api';
import { CouncilService } from '../../services/council.service';
import { CouncilDocument } from '../../models/council-document';
import { environment } from '../../../environments/environment';

@Component({
    selector: 'app-council-decisions',
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
        EditorModule,
        BlockUIModule
    ],
    providers: [DatePipe],
    templateUrl: './council-decisions.html',
    styleUrl: './council-decisions.scss',
})
export class CouncilDecisionsComponent implements OnInit {
    documentDialog: boolean = false;
    documents: CouncilDocument[] = [];
    document: CouncilDocument = {} as CouncilDocument;
    submitted: boolean = false;
    selectedFile: File | undefined;
    isLoading: boolean = false;

    constructor(
        private councilService: CouncilService,
        private messageService: MessageService,
        private confirmationService: ConfirmationService,
        private datePipe: DatePipe
    ) { }

    ngOnInit() {
        this.getAll();
    }

    getAll() {
        this.councilService.getAll().subscribe({
            next: (data) => {
                this.documents = data;
            },
            error: (error) => {
                console.error('Meclis kararları yüklenirken hata:', error);
                this.messageService.add({ severity: 'error', summary: 'Hata', detail: 'Veriler yüklenemedi.' });
            }
        });
    }

    openNew() {
        this.document = {} as CouncilDocument;
        const today = new Date();
        this.document.date = this.datePipe.transform(today, 'yyyy-MM-dd') || '';

        this.submitted = false;
        this.documentDialog = true;
        this.selectedFile = undefined;
    }

    editDocument(doc: CouncilDocument) {
        this.document = { ...doc };
        if (this.document.date) {
            this.document.date = this.datePipe.transform(this.document.date, 'yyyy-MM-dd') || '';
        }
        this.documentDialog = true;
        this.selectedFile = undefined;
    }

    deleteDocument(doc: CouncilDocument) {
        this.confirmationService.confirm({
            message: '"' + doc.title + '" başlıklı kararı silmek istediğinize emin misiniz?',
            header: 'Onay',
            icon: 'pi pi-exclamation-triangle',
            acceptLabel: 'Evet',
            rejectLabel: 'Hayır',
            accept: () => {
                this.councilService.delete(doc.id).subscribe({
                    next: () => {
                        this.messageService.add({ severity: 'success', summary: 'Başarılı', detail: 'Karar silindi.', life: 3000 });
                        this.getAll();
                    },
                    error: (error) => {
                        console.error('Silme hatası:', error);
                        this.messageService.add({ severity: 'error', summary: 'Hata', detail: 'Silme işlemi başarısız.' });
                    }
                });
            }
        });
    }

    hideDialog() {
        this.documentDialog = false;
        this.submitted = false;
    }

    onFileSelected(event: any) {
        const file = event.target.files[0];
        if (file) {
            this.selectedFile = file;
        }
    }

    saveDocument() {
        this.submitted = true;

        if (this.document.title?.trim() && this.document.type?.trim()) {
            this.isLoading = true;

            const finalizeCallback = () => {
                this.isLoading = false;
            };

            if (this.document.id) {
                this.councilService.update(this.document.id, this.document, this.selectedFile).subscribe({
                    next: () => {
                        this.messageService.add({ severity: 'success', summary: 'Başarılı', detail: 'Karar güncellendi.', life: 3000 });
                        this.getAll();
                        this.documentDialog = false;
                        this.document = {} as CouncilDocument;
                    },
                    error: (error) => {
                        console.error('Güncelleme hatası:', error);
                        this.messageService.add({ severity: 'error', summary: 'Hata', detail: 'Güncelleme hatası.' });
                    },
                    complete: finalizeCallback
                });
            } else {
                this.councilService.create(this.document, this.selectedFile).subscribe({
                    next: () => {
                        this.messageService.add({ severity: 'success', summary: 'Başarılı', detail: 'Karar oluşturuldu.', life: 3000 });
                        this.getAll();
                        this.documentDialog = false;
                        this.document = {} as CouncilDocument;
                    },
                    error: (error) => {
                        console.error('Oluşturma hatası:', error);
                        this.messageService.add({ severity: 'error', summary: 'Hata', detail: 'Oluşturma hatası.' });
                    },
                    complete: finalizeCallback
                });
            }
        }
    }

    formatDate(dateString: string): string {
        if (!dateString) return '';
        const date = new Date(dateString);
        if (!isNaN(date.getTime())) {
            return this.datePipe.transform(date, 'dd.MM.yyyy') || '';
        }
        return dateString;
    }

    getFileUrl(url: string | undefined): string {
        if (!url) return '';
        if (url.startsWith('http')) return url;
        return `${environment.imageBaseUrl}${url}`;
    }
}
