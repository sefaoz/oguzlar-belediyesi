import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
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
import { BlockUIModule } from 'primeng/blockui';
import { ProgressBarModule } from 'primeng/progressbar';
import { TagModule } from 'primeng/tag';
import { ToggleSwitchModule } from 'primeng/toggleswitch';
import { InputNumberModule } from 'primeng/inputnumber';
import { MessageService, ConfirmationService } from 'primeng/api';
import { SliderService } from '../../services/slider.service';
import { Slider } from '../../models/slider';
import { environment } from '../../../environments/environment';

@Component({
    selector: 'app-slider',
    standalone: true,
    imports: [
        CommonModule,
        FormsModule,
        RouterModule,
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
        BlockUIModule,
        ProgressBarModule,
        TagModule,
        ToggleSwitchModule,
        InputNumberModule
    ],
    providers: [],
    templateUrl: './slider.html',
    styleUrl: './slider.scss',
})
export class SliderComponent implements OnInit {
    sliderDialog: boolean = false;
    sliders: Slider[] = [];
    slider: Slider = {};
    submitted: boolean = false;
    selectedImage: File | undefined;
    selectedImagePreview: string | undefined;
    originalImageUrl: string | undefined;
    isLoading: boolean = false;

    constructor(
        private sliderService: SliderService,
        private messageService: MessageService,
        private confirmationService: ConfirmationService
    ) { }

    ngOnInit() {
        this.getSliders();
    }

    getSliders() {
        this.sliderService.getSliders().subscribe({
            next: (data) => {
                this.sliders = data;
            },
            error: (error) => {
                console.error('Sliderlar yüklenirken hata oluştu:', error);
                this.messageService.add({ severity: 'error', summary: 'Hata', detail: 'Sliderlar yüklenemedi.' });
            }
        });
    }

    openNew() {
        this.slider = {};
        this.slider.order = this.sliders.length + 1; // Auto increment order
        this.slider.isActive = true; // Default active
        this.submitted = false;
        this.sliderDialog = true;
        this.selectedImage = undefined;
        this.selectedImagePreview = undefined;
        this.originalImageUrl = undefined;
    }

    editSlider(sliderItem: Slider) {
        this.slider = { ...sliderItem };
        this.originalImageUrl = this.slider.imageUrl;
        this.selectedImagePreview = undefined;
        this.selectedImage = undefined;
        this.sliderDialog = true;
    }

    deleteSlider(sliderItem: Slider) {
        this.confirmationService.confirm({
            message: '"' + sliderItem.title + '" başlıklı slider\'ı silmek istediğinize emin misiniz?',
            header: 'Onay',
            icon: 'pi pi-exclamation-triangle',
            acceptLabel: 'Evet',
            rejectLabel: 'Hayır',
            accept: () => {
                if (sliderItem.id) {
                    this.sliderService.deleteSlider(sliderItem.id).subscribe({
                        next: () => {
                            this.messageService.add({ severity: 'success', summary: 'Başarılı', detail: 'Slider silindi.', life: 3000 });
                            this.getSliders();
                        },
                        error: (error) => {
                            console.error('Silme hatası:', error);
                            this.messageService.add({ severity: 'error', summary: 'Hata', detail: 'Slider silinirken hata oluştu.' });
                        }
                    });
                }
            }
        });
    }

    hideDialog() {
        this.sliderDialog = false;
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

    saveSlider() {
        this.submitted = true;

        // Yeni slider için görsel zorunlu
        const isNewSlider = !this.slider.id;

        if (isNewSlider && !this.selectedImage) {
            this.messageService.add({
                severity: 'error',
                summary: 'Hata',
                detail: 'Görsel seçmek zorunludur.',
                life: 5000
            });
            return;
        }

        this.isLoading = true;

        const finalizeCallback = () => {
            this.isLoading = false;
        };

        if (this.slider.id) {
            // Güncelleme
            this.sliderService.update(this.slider.id, this.slider, this.selectedImage).subscribe({
                next: () => {
                    this.messageService.add({ severity: 'success', summary: 'Başarılı', detail: 'Slider güncellendi.', life: 3000 });
                    this.getSliders();
                    this.sliderDialog = false;
                    this.slider = {};
                },
                error: (error) => {
                    console.error('Güncelleme hatası:', error);
                    this.messageService.add({ severity: 'error', summary: 'Hata', detail: 'Slider güncellenirken hata oluştu.' });
                },
                complete: finalizeCallback
            });
        } else {
            // Yeni Kayıt
            this.sliderService.create(this.slider, this.selectedImage).subscribe({
                next: () => {
                    this.messageService.add({ severity: 'success', summary: 'Başarılı', detail: 'Slider oluşturuldu.', life: 3000 });
                    this.getSliders();
                    this.sliderDialog = false;
                    this.slider = {};
                },
                error: (error) => {
                    console.error('Oluşturma hatası:', error);
                    this.messageService.add({ severity: 'error', summary: 'Hata', detail: 'Slider oluşturulurken hata oluştu.' });
                },
                complete: finalizeCallback
            });
        }
    }

    onGlobalFilter(table: Table, event: Event) {
        table.filterGlobal((event.target as HTMLInputElement).value, 'contains');
    }

    getImageUrl(url: string | undefined): string {
        if (!url) return '';
        if (url.startsWith('http')) return url;
        return `${environment.imageBaseUrl}${url}`;
    }
}

