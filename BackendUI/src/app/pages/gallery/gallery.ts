import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { TableModule } from 'primeng/table';
import { DialogModule } from 'primeng/dialog';
import { ButtonModule } from 'primeng/button';
import { RippleModule } from 'primeng/ripple';
import { ToastModule } from 'primeng/toast';
import { ToolbarModule } from 'primeng/toolbar';
import { ConfirmDialogModule } from 'primeng/confirmdialog';
import { InputTextModule } from 'primeng/inputtext';
import { FileUploadModule } from 'primeng/fileupload';
import { ImageModule } from 'primeng/image';
import { ToggleSwitchModule } from 'primeng/toggleswitch';
import { MessageService, ConfirmationService } from 'primeng/api';
import { GalleryService } from '../../services/gallery.service';
import { GalleryFolder, GalleryImage } from '../../models/gallery';
import { environment } from '../../../environments/environment';

@Component({
    selector: 'app-gallery',
    standalone: true,
    imports: [
        CommonModule, FormsModule, TableModule, DialogModule, ButtonModule, RippleModule,
        ToastModule, ToolbarModule, ConfirmDialogModule, InputTextModule,
        FileUploadModule, ImageModule, ToggleSwitchModule
    ],
    providers: [MessageService, ConfirmationService],
    templateUrl: './gallery.html'
})
export class GalleryComponent implements OnInit {
    folders: GalleryFolder[] = [];
    folderDialog: boolean = false;
    folder: GalleryFolder = {} as GalleryFolder;
    submitted: boolean = false;

    // Image Management
    imagesDialog: boolean = false;
    currentFolderId: string | null = null;
    folderImages: GalleryImage[] = [];

    constructor(
        private galleryService: GalleryService,
        private messageService: MessageService,
        private confirmationService: ConfirmationService
    ) { }

    ngOnInit() {
        this.getFolders();
    }

    getFolders() {
        this.galleryService.getFolders().subscribe(data => this.folders = data);
    }

    openNewFolder() {
        this.folder = { isFeatured: false, isActive: true } as GalleryFolder;
        this.submitted = false;
        this.folderDialog = true;
    }

    editFolder(folder: GalleryFolder) {
        this.folder = { ...folder };
        this.folderDialog = true;
    }

    deleteFolder(folder: GalleryFolder) {
        this.confirmationService.confirm({
            message: 'Klasörü silmek istediğinize emin misiniz?',
            header: 'Onay',
            icon: 'pi pi-exclamation-triangle',
            accept: () => {
                this.galleryService.deleteFolder(folder.id).subscribe(() => {
                    this.messageService.add({ severity: 'success', summary: 'Başarılı', detail: 'Klasör silindi' });
                    this.getFolders();
                });
            }
        });
    }

    saveFolder() {
        this.submitted = true;

        // Client-side validation for IsFeatured limit
        if (this.folder.isFeatured) {
            const existingFeatured = this.folders.filter(f => f.isFeatured && f.id !== this.folder.id).length;
            if (existingFeatured >= 2) {
                this.messageService.add({ severity: 'warn', summary: 'Uyarı', detail: 'En fazla 2 galeri ana sayfada gösterilebilir. Lütfen önce başka bir galeriyi kaldırın.' });
                return;
            }
        }

        if (this.folder.title?.trim()) {
            if (this.folder.id) {
                this.galleryService.updateFolder(this.folder.id, this.folder).subscribe({
                    next: () => {
                        this.messageService.add({ severity: 'success', summary: 'Başarılı', detail: 'Klasör güncellendi' });
                        this.folderDialog = false;
                        this.getFolders();
                    },
                    error: (err) => {
                        this.messageService.add({ severity: 'error', summary: 'Hata', detail: err.error || 'Klasör güncellenemedi' });
                    }
                });
            } else {
                this.galleryService.createFolder(this.folder).subscribe({
                    next: () => {
                        this.messageService.add({ severity: 'success', summary: 'Başarılı', detail: 'Klasör oluşturuldu' });
                        this.folderDialog = false;
                        this.getFolders();
                    },
                    error: (err) => {
                        this.messageService.add({ severity: 'error', summary: 'Hata', detail: err.error || 'Klasör oluşturulamadı' });
                    }
                });
            }
        }
    }

    openImages(folder: GalleryFolder) {
        this.currentFolderId = folder.id;
        this.imagesDialog = true;
        this.loadImages();
    }

    loadImages() {
        if (this.currentFolderId) {
            this.galleryService.getImages(this.currentFolderId).subscribe(data => this.folderImages = data);
        }
    }

    uploadHandler(event: any) {
        if (!this.currentFolderId) return;
        let files = event.files;
        let completed = 0;
        for (let file of files) {
            this.galleryService.addImage(this.currentFolderId, file, file.name).subscribe({
                next: () => {
                    completed++;
                    if (completed === files.length) {
                        this.loadImages();
                        this.messageService.add({ severity: 'success', summary: 'Başarılı', detail: 'Tüm resimler yüklendi' });
                        event.options.clear(); // Clear uploaded files from list
                    }
                },
                error: () => {
                    this.messageService.add({ severity: 'error', summary: 'Hata', detail: `Yükleme hatası: ${file.name}` });
                }
            });
        }
    }

    deleteImage(image: GalleryImage) {
        this.confirmationService.confirm({
            message: 'Resmi silmek istediğinize emin misiniz?',
            accept: () => {
                this.galleryService.deleteImage(image.id).subscribe(() => {
                    this.messageService.add({ severity: 'success', summary: 'Başarılı', detail: 'Resim silindi' });
                    this.loadImages();
                });
            }
        });
    }

    getImageUrl(url: string): string {
        if (!url) return '';
        if (url.startsWith('http')) return url;
        return `${environment.imageBaseUrl}${url}`;
    }
}
