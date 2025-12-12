import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
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
import { BlockUIModule } from 'primeng/blockui';
import { ProgressBarModule } from 'primeng/progressbar';
import { MessageService, ConfirmationService } from 'primeng/api';
import { VehicleService } from '../../services/vehicle.service';
import { Vehicle } from '../../models/vehicle';
import { environment } from '../../../environments/environment';

@Component({
    selector: 'app-vehicles',
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
        BlockUIModule,
        ProgressBarModule
    ],
    providers: [],
    templateUrl: './vehicles.html',
    styleUrl: './vehicles.scss',
})
export class VehiclesComponent implements OnInit {
    vehicleDialog: boolean = false;
    vehicles: Vehicle[] = [];
    vehicle: Vehicle = {} as Vehicle;
    submitted: boolean = false;
    selectedImage: File | undefined;
    selectedImagePreview: string | undefined;
    originalImageUrl: string | undefined;
    isLoading: boolean = false;
    progressValue: number = 0;
    progressInterval: any;

    constructor(
        private vehicleService: VehicleService,
        private messageService: MessageService,
        private confirmationService: ConfirmationService
    ) { }

    ngOnInit() {
        this.getVehicles();
    }

    getVehicles() {
        this.vehicleService.getAll().subscribe({
            next: (data) => {
                this.vehicles = data;
            },
            error: (error) => {
                this.messageService.add({ severity: 'error', summary: 'Hata', detail: 'Araçlar yüklenemedi.' });
            }
        });
    }

    openNew() {
        this.vehicle = {} as Vehicle;
        this.submitted = false;
        this.vehicleDialog = true;
        this.selectedImage = undefined;
        this.selectedImagePreview = undefined;
        this.originalImageUrl = undefined;
    }

    editVehicle(vehicle: Vehicle) {
        this.vehicle = { ...vehicle };
        this.originalImageUrl = this.vehicle.imageUrl;
        this.selectedImagePreview = undefined;
        this.selectedImage = undefined;
        this.vehicleDialog = true;
    }

    deleteVehicle(vehicle: Vehicle) {
        this.confirmationService.confirm({
            message: '"' + vehicle.name + '" adlı aracı silmek istediğinize emin misiniz?',
            header: 'Onay',
            icon: 'pi pi-exclamation-triangle',
            acceptLabel: 'Evet',
            rejectLabel: 'Hayır',
            accept: () => {
                this.vehicleService.delete(vehicle.id).subscribe({
                    next: () => {
                        this.messageService.add({ severity: 'success', summary: 'Başarılı', detail: 'Araç silindi.', life: 3000 });
                        this.getVehicles();
                    },
                    error: (error) => {
                        this.messageService.add({ severity: 'error', summary: 'Hata', detail: 'Araç silinirken hata oluştu.' });
                    }
                });
            }
        });
    }

    hideDialog() {
        this.vehicleDialog = false;
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

    saveVehicle() {
        this.submitted = true;

        if (this.vehicle.name?.trim()) {
            this.isLoading = true;
            this.progressValue = 0;
            this.startTimer();

            const finalizeCallback = () => {
                this.isLoading = false;
                this.stopTimer();
            };

            if (this.vehicle.id) {
                const { id, ...vehicleData } = this.vehicle;
                this.vehicleService.update(id, vehicleData, this.selectedImage).subscribe({
                    next: () => {
                        this.messageService.add({ severity: 'success', summary: 'Başarılı', detail: 'Araç güncellendi.', life: 3000 });
                        this.getVehicles();
                        this.vehicleDialog = false;
                        this.vehicle = {} as Vehicle;
                    },
                    error: (error) => {
                        this.messageService.add({ severity: 'error', summary: 'Hata', detail: 'Araç güncellenirken hata oluştu.' });
                    },
                    complete: finalizeCallback
                });
            } else {
                this.vehicleService.create(this.vehicle, this.selectedImage).subscribe({
                    next: () => {
                        this.messageService.add({ severity: 'success', summary: 'Başarılı', detail: 'Araç oluşturuldu.', life: 3000 });
                        this.getVehicles();
                        this.vehicleDialog = false;
                        this.vehicle = {} as Vehicle;
                    },
                    error: (error) => {
                        this.messageService.add({ severity: 'error', summary: 'Hata', detail: 'Araç oluşturulurken hata oluştu.' });
                    },
                    complete: finalizeCallback
                });
            }
        }
    }

    onGlobalFilter(table: Table, event: Event) {
        table.filterGlobal((event.target as HTMLInputElement).value, 'contains');
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
