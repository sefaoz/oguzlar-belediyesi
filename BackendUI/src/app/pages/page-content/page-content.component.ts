import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { TableModule } from 'primeng/table';
import { ButtonModule } from 'primeng/button';
import { DialogModule } from 'primeng/dialog';
import { InputTextModule } from 'primeng/inputtext';
import { TextareaModule } from 'primeng/textarea';
import { ToastModule } from 'primeng/toast';
import { ToolbarModule } from 'primeng/toolbar';
import { ConfirmDialogModule } from 'primeng/confirmdialog';
import { MessageService, ConfirmationService } from 'primeng/api';
import { RouterModule } from '@angular/router';
import { PageContentService } from '../../services/page-content.service';
import { PageContent, ContactDetail } from '../../models/page-content';
import { EditorModule } from 'primeng/editor';
import { ImageModule } from 'primeng/image';

@Component({
    selector: 'app-page-content',
    standalone: true,
    imports: [
        CommonModule,
        FormsModule,
        TableModule,
        ButtonModule,
        DialogModule,
        InputTextModule,
        TextareaModule,
        ToastModule,
        ToolbarModule,
        ConfirmDialogModule,
        RouterModule,
        ImageModule,
        EditorModule
    ],
    templateUrl: './page-content.component.html',
    providers: [MessageService, ConfirmationService]
})
export class PageContentComponent implements OnInit {
    pageContents: PageContent[] = [];
    pageContentDialog: boolean = false;
    pageContent: PageContent = this.createEmptyPageContent();
    submitted: boolean = false;
    paragraphsText: string = '';
    originalImageUrl: string | null = null;

    selectedImagePreview: string | null = null;
    selectedFile: File | null = null;

    constructor(
        private pageContentService: PageContentService,
        private messageService: MessageService,
        private confirmationService: ConfirmationService
    ) { }

    ngOnInit() {
        this.loadPageContents();
    }

    loadPageContents() {
        this.pageContentService.getAll().subscribe({
            next: (data) => (this.pageContents = data),
            error: (err) => this.messageService.add({ severity: 'error', summary: 'Hata', detail: 'Veriler yüklenemedi.' })
        });
    }

    onFileSelected(event: any) {
        const file = event.target.files[0];
        if (file) {
            this.selectedFile = file;
            const reader = new FileReader();
            reader.onload = (e: any) => {
                this.selectedImagePreview = e.target.result;
            };
            reader.readAsDataURL(file);
        }
    }

    openNew() {
        this.pageContent = this.createEmptyPageContent();
        this.paragraphsText = '';
        this.submitted = false;
        this.originalImageUrl = null;
        this.selectedImagePreview = null;
        this.selectedFile = null;
        this.pageContentDialog = true;
    }

    editPageContent(pageContent: PageContent) {
        this.pageContent = { ...pageContent };
        // Join paragraphs with empty string if it's HTML content, or newline if it was text. 
        // For now, join with nothing to keep HTML intact if it's split, or just take the first element if it's a single HTML block.
        this.paragraphsText = this.pageContent.paragraphs ? this.pageContent.paragraphs.join('') : '';
        this.originalImageUrl = pageContent.imageUrl || null;
        this.selectedImagePreview = null;
        this.selectedFile = null;
        // Ensure contactDetails is initialized
        if (!this.pageContent.contactDetails) {
            this.pageContent.contactDetails = [];
        }
        this.pageContentDialog = true;
    }

    deletePageContent(pageContent: PageContent) {
        this.confirmationService.confirm({
            message: '"' + pageContent.title + '" sayfasını silmek istediğinize emin misiniz?',
            header: 'Onay',
            icon: 'pi pi-exclamation-triangle',
            accept: () => {
                this.pageContentService.delete(pageContent.id).subscribe({
                    next: () => {
                        this.pageContents = this.pageContents.filter((val) => val.id !== pageContent.id);
                        this.messageService.add({ severity: 'success', summary: 'Başarılı', detail: 'Sayfa silindi', life: 3000 });
                    },
                    error: () => this.messageService.add({ severity: 'error', summary: 'Hata', detail: 'Silme işlemi başarısız.' })
                });
            }
        });
    }

    hideDialog() {
        this.pageContentDialog = false;
        this.submitted = false;
    }

    savePageContent() {
        this.submitted = true;

        if (this.pageContent.title?.trim() && this.pageContent.key?.trim()) {
            // Save as single element array containing the HTML from editor
            this.pageContent.paragraphs = [this.paragraphsText];

            if (this.pageContent.id) {
                this.pageContentService.update(this.pageContent.id, this.pageContent, this.selectedFile || undefined).subscribe({
                    next: () => {
                        this.messageService.add({ severity: 'success', summary: 'Başarılı', detail: 'Sayfa güncellendi', life: 3000 });
                        this.loadPageContents();
                        this.pageContentDialog = false;
                        this.pageContent = this.createEmptyPageContent();
                    },
                    error: () => this.messageService.add({ severity: 'error', summary: 'Hata', detail: 'Güncelleme başarısız.' })
                });
            } else {
                this.pageContentService.create(this.pageContent, this.selectedFile || undefined).subscribe({
                    next: (newItem) => {
                        this.pageContents.push(newItem);
                        this.messageService.add({ severity: 'success', summary: 'Başarılı', detail: 'Sayfa oluşturuldu', life: 3000 });
                        this.pageContentDialog = false;
                        this.pageContent = this.createEmptyPageContent();
                    },
                    error: () => this.messageService.add({ severity: 'error', summary: 'Hata', detail: 'Oluşturma başarısız.' })
                });
            }
        }
    }

    createEmptyPageContent(): PageContent {
        return {
            id: '',
            key: '',
            title: '',
            subtitle: '',
            paragraphs: [],
            imageUrl: '',
            mapEmbedUrl: '',
            contactDetails: []
        };
    }

    addContactDetail() {
        if (!this.pageContent.contactDetails) {
            this.pageContent.contactDetails = [];
        }
        this.pageContent.contactDetails.push({ label: '', value: '' });
    }

    removeContactDetail(index: number) {
        if (this.pageContent.contactDetails) {
            this.pageContent.contactDetails.splice(index, 1);
        }
    }
}
