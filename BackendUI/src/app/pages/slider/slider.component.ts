import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { TableModule } from 'primeng/table';
import { ButtonModule } from 'primeng/button';
import { RippleModule } from 'primeng/ripple';
import { ToastModule } from 'primeng/toast';
import { ToolbarModule } from 'primeng/toolbar';
import { RatingModule } from 'primeng/rating';
import { InputTextModule } from 'primeng/inputtext';
import { TextareaModule } from 'primeng/textarea';
import { RadioButtonModule } from 'primeng/radiobutton';
import { InputNumberModule } from 'primeng/inputnumber';
import { DialogModule } from 'primeng/dialog';
import { TagModule } from 'primeng/tag';
import { FileUploadModule } from 'primeng/fileupload';
import { MessageService, ConfirmationService } from 'primeng/api';
import { ConfirmDialogModule } from 'primeng/confirmdialog';
import { Slider } from '../../models/slider';
import { SliderService } from '../../services/slider.service';

@Component({
    selector: 'app-slider',
    standalone: true,
    imports: [
        CommonModule,
        TableModule,
        FileUploadModule,
        FormsModule,
        ButtonModule,
        RippleModule,
        ToastModule,
        ToolbarModule,
        RatingModule,
        InputTextModule,
        TextareaModule,
        RadioButtonModule,
        InputNumberModule,
        DialogModule,
        TagModule,
        ConfirmDialogModule
    ],
    providers: [MessageService, ConfirmationService],
    template: `
        <div class="card">
            <p-toast></p-toast>
            <p-toolbar styleClass="mb-4 gap-2">
                <ng-template pTemplate="left">
                    <button pButton pRipple label="Yeni Ekle" icon="pi pi-plus" class="p-button-success mr-2" (click)="openNew()"></button>
                    <button pButton pRipple label="Sil" icon="pi pi-trash" class="p-button-danger" (click)="deleteSelectedSliders()" [disabled]="!selectedSliders || !selectedSliders.length"></button>
                </ng-template>
            </p-toolbar>

            <p-table #dt [value]="sliders" [rows]="10" [paginator]="true" [globalFilterFields]="['title', 'description', 'status']" [tableStyle]="{'min-width': '75rem'}"
                [(selection)]="selectedSliders" [rowHover]="true" dataKey="id"
                currentPageReportTemplate="{totalRecords} kayıttan {first} ile {last} arası gösteriliyor" [showCurrentPageReport]="true">
                <ng-template pTemplate="caption">
                    <div class="flex items-center justify-between">
                        <h5 class="m-0">Slider Yönetimi</h5>
                        <span class="p-input-icon-left">
                            <i class="pi pi-search"></i>
                            <input pInputText type="text" (input)="dt.filterGlobal($any($event.target).value, 'contains')" placeholder="Ara..." />
                        </span>
                    </div>
                </ng-template>
                <ng-template pTemplate="header">
                    <tr>
                        <th style="width: 4rem">
                            <p-tableHeaderCheckbox></p-tableHeaderCheckbox>
                        </th>
                        <th pSortableColumn="title" style="min-width:15rem">Başlık <p-sortIcon field="title"></p-sortIcon></th>
                        <th>Görsel</th>
                        <th pSortableColumn="order">Sıra <p-sortIcon field="order"></p-sortIcon></th>
                        <th pSortableColumn="isActive">Durum <p-sortIcon field="isActive"></p-sortIcon></th>
                        <th>İşlemler</th>
                    </tr>
                </ng-template>
                <ng-template pTemplate="body" let-slider>
                    <tr>
                        <td>
                            <p-tableCheckbox [value]="slider"></p-tableCheckbox>
                        </td>
                        <td>{{slider.title}}</td>
                        <td><img [src]="slider.imageUrl" [alt]="slider.title" width="100" class="shadow-4" /></td>
                        <td>{{slider.order}}</td>
                        <td><p-tag [value]="slider.isActive ? 'Aktif' : 'Pasif'" [severity]="slider.isActive ? 'success' : 'danger'"></p-tag></td>
                        <td>
                            <button pButton pRipple icon="pi pi-pencil" class="p-button-rounded p-button-success mr-2" (click)="editSlider(slider)"></button>
                            <button pButton pRipple icon="pi pi-trash" class="p-button-rounded p-button-warning" (click)="deleteSlider(slider)"></button>
                        </td>
                    </tr>
                </ng-template>
            </p-table>
        </div>

        <p-dialog [(visible)]="sliderDialog" [style]="{width: '450px'}" header="Slider Detayı" [modal]="true" styleClass="p-fluid">
            <ng-template pTemplate="content">
                <div class="field">
                    <label for="title">Başlık</label>
                    <input type="text" pInputText id="title" [(ngModel)]="slider.title" required autofocus />
                    <small class="p-error" *ngIf="submitted && !slider.title">Başlık gereklidir.</small>
                </div>
                <div class="field">
                    <label for="description">Açıklama</label>
                    <p-textarea id="description" [(ngModel)]="slider.description" required rows="3" cols="20"></p-textarea>
                </div>
                <div class="field">
                    <label for="imageUrl">Görsel URL</label>
                    <input type="text" pInputText id="imageUrl" [(ngModel)]="slider.imageUrl" />
                </div>
                <div class="field">
                    <label for="link">Link</label>
                    <input type="text" pInputText id="link" [(ngModel)]="slider.link" />
                </div>
                <div class="formgrid grid">
                    <div class="field col">
                        <label for="order">Sıra</label>
                        <p-inputNumber id="order" [(ngModel)]="slider.order"></p-inputNumber>
                    </div>
                    <div class="field col flex align-items-center">
                        <label for="isActive" class="mr-2">Aktif</label>
                        <p-radioButton name="isActive" [value]="true" [(ngModel)]="slider.isActive" label="Evet" class="mr-2"></p-radioButton>
                        <p-radioButton name="isActive" [value]="false" [(ngModel)]="slider.isActive" label="Hayır"></p-radioButton>
                    </div>
                </div>
            </ng-template>

            <ng-template pTemplate="footer">
                <button pButton pRipple label="İptal" icon="pi pi-times" class="p-button-text" (click)="hideDialog()"></button>
                <button pButton pRipple label="Kaydet" icon="pi pi-check" class="p-button-text" (click)="saveSlider()"></button>
            </ng-template>
        </p-dialog>

        <p-confirmDialog [style]="{width: '450px'}"></p-confirmDialog>
    `
})
export class SliderComponent implements OnInit {
    sliderDialog: boolean = false;
    sliders: Slider[] = [];
    slider: Slider = {};
    selectedSliders: Slider[] = [];
    submitted: boolean = false;

    constructor(private sliderService: SliderService, private messageService: MessageService, private confirmationService: ConfirmationService) { }

    ngOnInit() {
        this.sliderService.getSliders().subscribe(data => this.sliders = data);
    }

    openNew() {
        this.slider = {};
        this.submitted = false;
        this.sliderDialog = true;
    }

    deleteSelectedSliders() {
        this.confirmationService.confirm({
            message: 'Seçili sliderları silmek istediğinize emin misiniz?',
            header: 'Onay',
            icon: 'pi pi-exclamation-triangle',
            accept: () => {
                this.selectedSliders.forEach(s => {
                    if (s.id) this.sliderService.deleteSlider(s.id);
                });
                this.selectedSliders = [];
                this.messageService.add({ severity: 'success', summary: 'Başarılı', detail: 'Sliderlar Silindi', life: 3000 });
            }
        });
    }

    editSlider(slider: Slider) {
        this.slider = { ...slider };
        this.sliderDialog = true;
    }

    deleteSlider(slider: Slider) {
        this.confirmationService.confirm({
            message: '"' + slider.title + '" başlıklı sliderı silmek istediğinize emin misiniz?',
            header: 'Onay',
            icon: 'pi pi-exclamation-triangle',
            accept: () => {
                if (slider.id) {
                    this.sliderService.deleteSlider(slider.id);
                    this.slider = {};
                    this.messageService.add({ severity: 'success', summary: 'Başarılı', detail: 'Slider Silindi', life: 3000 });
                }
            }
        });
    }

    hideDialog() {
        this.sliderDialog = false;
        this.submitted = false;
    }

    saveSlider() {
        this.submitted = true;

        if (this.slider.title?.trim()) {
            if (!this.slider.id) {
                this.slider.isActive = true; // Default active
            }

            this.sliderService.saveSlider(this.slider).subscribe(() => {
                this.messageService.add({ severity: 'success', summary: 'Başarılı', detail: 'Slider Kaydedildi', life: 3000 });
                this.sliders = [...this.sliders]; // Refresh list trigger
                this.sliderDialog = false;
                this.slider = {};
            });
        }
    }
}
