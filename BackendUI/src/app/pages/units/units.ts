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
import { EditorModule } from 'primeng/editor';
import { BlockUIModule } from 'primeng/blockui';
import { ImageModule } from 'primeng/image';
import { FileUploadModule } from 'primeng/fileupload';
import { MessageService, ConfirmationService } from 'primeng/api';
import { UnitService } from '../../services/unit.service';
import { MunicipalUnit, UnitStaff } from '../../models/municipal-unit';
import { environment } from '../../../environments/environment';

@Component({
    selector: 'app-units',
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
        EditorModule,
        BlockUIModule,
        ImageModule,
        FileUploadModule
    ],
    providers: [],
    templateUrl: './units.html',
})
export class UnitsComponent implements OnInit {
    unitDialog: boolean = false;
    units: MunicipalUnit[] = [];
    unit: MunicipalUnit = {} as MunicipalUnit;
    submitted: boolean = false;
    isLoading: boolean = false;

    // Staff Image Management
    staffFiles: { [key: number]: File } = {};
    staffPreviews: { [key: number]: string } = {};

    constructor(
        private unitService: UnitService,
        private messageService: MessageService,
        private confirmationService: ConfirmationService
    ) { }

    ngOnInit() {
        this.getUnits();
    }

    getUnits() {
        this.unitService.getAll().subscribe({
            next: (data) => {
                this.units = data;
            },
            error: (error) => {
                console.error('Birimler yüklenirken hata:', error);
                this.messageService.add({ severity: 'error', summary: 'Hata', detail: 'Birimler yüklenemedi.' });
            }
        });
    }

    openNew() {
        this.unit = {} as MunicipalUnit;
        this.unit.staff = [];
        this.staffFiles = {};
        this.staffPreviews = {};
        this.submitted = false;
        this.unitDialog = true;
    }

    editUnit(unit: MunicipalUnit) {
        this.unit = { ...unit };
        this.staffFiles = {};
        this.staffPreviews = {};

        // Deep copy staff to avoid reference issues
        if (this.unit.staff) {
            this.unit.staff = this.unit.staff.map(s => ({ ...s }));
        } else {
            this.unit.staff = [];
        }
        this.unitDialog = true;
    }

    deleteUnit(unit: MunicipalUnit) {
        this.confirmationService.confirm({
            message: '"' + unit.title + '" birimini silmek istediğinize emin misiniz?',
            header: 'Onay',
            icon: 'pi pi-exclamation-triangle',
            acceptLabel: 'Evet',
            rejectLabel: 'Hayır',
            accept: () => {
                this.unitService.delete(unit.id).subscribe({
                    next: () => {
                        this.messageService.add({ severity: 'success', summary: 'Başarılı', detail: 'Birim silindi.', life: 3000 });
                        this.getUnits();
                    },
                    error: (error) => {
                        this.messageService.add({ severity: 'error', summary: 'Hata', detail: 'Silme işleminde hata oluştu.' });
                    }
                });
            }
        });
    }

    hideDialog() {
        this.unitDialog = false;
        this.submitted = false;
    }

    saveUnit() {
        this.submitted = true;

        if (this.unit.title?.trim()) {
            this.isLoading = true;

            const finalize = () => {
                this.isLoading = false;
            };

            if (this.unit.id) {
                this.unitService.update(this.unit.id, this.unit, this.staffFiles).subscribe({
                    next: () => {
                        this.messageService.add({ severity: 'success', summary: 'Başarılı', detail: 'Birim güncellendi.', life: 3000 });
                        this.getUnits();
                        this.hideDialog();
                    },
                    error: () => {
                        this.messageService.add({ severity: 'error', summary: 'Hata', detail: 'Güncelleme hatası.' });
                    },
                    complete: finalize
                });
            } else {
                this.unitService.create(this.unit, this.staffFiles).subscribe({
                    next: () => {
                        this.messageService.add({ severity: 'success', summary: 'Başarılı', detail: 'Birim oluşturuldu.', life: 3000 });
                        this.getUnits();
                        this.hideDialog();
                    },
                    error: (err) => {
                        console.error(err);
                        this.messageService.add({ severity: 'error', summary: 'Hata', detail: 'Oluşturma hatası.' });
                    },
                    complete: finalize
                });
            }
        }
    }

    addStaff() {
        if (!this.unit.staff) this.unit.staff = [];
        this.unit.staff.push({ name: '', title: '', imageUrl: '' });
    }

    removeStaff(index: number) {
        this.unit.staff?.splice(index, 1);

        // Remove file objects as well and shift indices
        delete this.staffFiles[index];
        delete this.staffPreviews[index];

        // Re-index remaining files/previews if necessary, 
        // but easier just to clear and let user re-select if standard behavior isn't critical
        // Or properly shift keys... Since key is index, shifting is needed.
        // Simplified approach: Rebuild maps or just accept index drift if backend handles array index order?
        // Actually, backend uses "staffImage_{i}", so index must match array index.
        // We must re-index.

        const newFiles: { [key: number]: File } = {};
        const newPreviews: { [key: number]: string } = {};

        // Iterate over current file/preview keys and shift those > index
        // But since we already removed from array, we must align with NEW array indices.
        // This is complexstate management.
        // Better: Reset file inputs if removed? 
        // Let's implement simple shift.

        // This implementation assumes linear add/remove. 
        // If complex, maybe we should just reset files for safety or handle it properly.
        // Let's rely on user not deleting form middle often, or just clear files if they do.
        // Just clearing specific index and leaving holes is BAD if backend iterates 0..count.

        // Let's just reset files/previews to keep it simple or implement proper shift.
        // Shift logic:
        // Everything > index needs to be moved to k-1.

        // However, since we don't have deep state tracking of which file belonged to which ORIGINAL item easily without ID,
        // (UnitStaff doesn't have ID), it's tricky.
        // But wait, we are editing IN MEMORY array. So linear shift is correct.

        // Re-construct files map
        const shiftedFiles: { [key: number]: File } = {};
        const shiftedPreviews: { [key: number]: string } = {};

        // We need to look at OLD keys. 
        // But we already deleted key 'index'.
        // We need to look at keys > index.

        // Actually simpler: We need to traverse the old map.
        // But map keys are strings/numbers.

        // Let's do a reconstruction.
        // It is hard to know which file belonged to which index without keeping a diff.
        // But we know we just removed 'index'. 
        // So any key k > index in staffFiles should become k-1.

        const fileKeys = Object.keys(this.staffFiles).map(Number).sort((a, b) => a - b);
        fileKeys.forEach(k => {
            if (k > index) {
                shiftedFiles[k - 1] = this.staffFiles[k];
            } else if (k < index) {
                shiftedFiles[k] = this.staffFiles[k];
            }
        });
        this.staffFiles = shiftedFiles;

        const previewKeys = Object.keys(this.staffPreviews).map(Number).sort((a, b) => a - b);
        previewKeys.forEach(k => {
            if (k > index) {
                shiftedPreviews[k - 1] = this.staffPreviews[k];
            } else if (k < index) {
                shiftedPreviews[k] = this.staffPreviews[k];
            }
        });
        this.staffPreviews = shiftedPreviews;
    }

    onStaffImageSelected(event: any, index: number) {
        const file = event.target.files[0];
        if (file) {
            this.staffFiles[index] = file;
            const reader = new FileReader();
            reader.onload = (e) => {
                this.staffPreviews[index] = e.target?.result as string;
            };
            reader.readAsDataURL(file);
        }
    }

    getImageUrl(url: string | undefined | null): string {
        if (!url) return '';
        if (url.startsWith('http') || url.startsWith('data:')) return url;
        return `${environment.imageBaseUrl}${url}`;
    }
}
