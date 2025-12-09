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
import { FileUploadModule } from 'primeng/fileupload';
import { BlockUIModule } from 'primeng/blockui';
import { MessageService, ConfirmationService } from 'primeng/api';
import { KvkkService } from '../../services/kvkk.service';
import { KvkkDocument } from '../../models/kvkk-document';
import { environment } from '../../../environments/environment';

@Component({
    selector: 'app-kvkk-documents',
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
        FileUploadModule,
        BlockUIModule
    ],
    providers: [DatePipe],
    templateUrl: './kvkk-documents.component.html',
    styleUrl: './kvkk-documents.component.scss'
})
export class KvkkDocumentsComponent implements OnInit {
    kvkkDialog: boolean = false;
    kvkkList: KvkkDocument[] = [];
    kvkk: KvkkDocument = {} as KvkkDocument;
    submitted: boolean = false;
    selectedFile: File | undefined;
    isLoading: boolean = false;

    constructor(
        private kvkkService: KvkkService,
        private messageService: MessageService,
        private confirmationService: ConfirmationService,
        private datePipe: DatePipe
    ) { }

    ngOnInit() {
        this.getAll();
    }

    getAll() {
        this.kvkkService.getAll().subscribe({
            next: (data) => {
                this.kvkkList = data;
            },
            error: (error) => {
                console.error('KVKK Belgeleri yüklenirken hata oluştu:', error);
                this.messageService.add({ severity: 'error', summary: 'Hata', detail: 'KVKK Belgeleri yüklenemedi.' });
            }
        });
    }

    openNew() {
        this.kvkk = {} as KvkkDocument;
        this.submitted = false;
        this.kvkkDialog = true;
        this.selectedFile = undefined;
    }

    editKvkk(kvkkItem: KvkkDocument) {
        this.kvkk = { ...kvkkItem };
        this.selectedFile = undefined;
        this.kvkkDialog = true;
    }

    deleteKvkk(kvkkItem: KvkkDocument) {
        this.confirmationService.confirm({
            message: '"' + kvkkItem.title + '" başlıklı belgeyi silmek istediğinize emin misiniz?',
            header: 'Onay',
            icon: 'pi pi-exclamation-triangle',
            acceptLabel: 'Evet',
            rejectLabel: 'Hayır',
            accept: () => {
                this.kvkkService.delete(kvkkItem.id).subscribe({
                    next: () => {
                        this.messageService.add({ severity: 'success', summary: 'Başarılı', detail: 'Belge silindi.', life: 3000 });
                        this.getAll();
                    },
                    error: (error) => {
                        console.error('Silme hatası:', error);
                        this.messageService.add({ severity: 'error', summary: 'Hata', detail: 'Belge silinirken hata oluştu.' });
                    }
                });
            }
        });
    }

    hideDialog() {
        this.kvkkDialog = false;
        this.submitted = false;
    }

    onFileSelected(event: any) {
        const file = event.target.files[0];
        if (file) {
            this.selectedFile = file;
        }
    }

    saveKvkk() {
        this.submitted = true;

        if (this.kvkk.title?.trim() && this.kvkk.type?.trim()) {
            this.isLoading = true;

            const finalizeCallback = () => {
                this.isLoading = false;
            };

            if (this.kvkk.id) {
                // Update
                this.kvkkService.update(this.kvkk.id, this.kvkk, this.selectedFile).subscribe({
                    next: () => {
                        this.messageService.add({ severity: 'success', summary: 'Başarılı', detail: 'Belge güncellendi.', life: 3000 });
                        this.getAll();
                        this.hideDialog();
                    },
                    error: (error) => {
                        console.error('Güncelleme hatası:', error);
                        this.messageService.add({ severity: 'error', summary: 'Hata', detail: 'Belge güncellenirken hata oluştu.' });
                    },
                    complete: finalizeCallback
                });
            } else {
                // Create
                this.kvkkService.create(this.kvkk, this.selectedFile).subscribe({
                    next: () => {
                        this.messageService.add({ severity: 'success', summary: 'Başarılı', detail: 'Belge oluşturuldu.', life: 3000 });
                        this.getAll();
                        this.hideDialog();
                    },
                    error: (error) => {
                        console.error('Oluşturma hatası:', error);
                        this.messageService.add({ severity: 'error', summary: 'Hata', detail: 'Belge oluşturulurken hata oluştu.' });
                    },
                    complete: finalizeCallback
                });
            }
        }
    }

    onGlobalFilter(table: Table, event: Event) {
        table.filterGlobal((event.target as HTMLInputElement).value, 'contains');
    }

    getFileUrl(url: string): string {
        if (!url) return '';
        if (url.startsWith('http')) return url;
        return `${environment.imageBaseUrl}${url}`;
    }
}
