import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { MessageService, ConfirmationService } from 'primeng/api';
import { TableModule } from 'primeng/table';
import { DialogModule } from 'primeng/dialog';
import { ButtonModule } from 'primeng/button';
import { InputTextModule } from 'primeng/inputtext';
import { TextareaModule } from 'primeng/textarea';
import { ToastModule } from 'primeng/toast';
import { BlockUIModule } from 'primeng/blockui';
import { TabsModule } from 'primeng/tabs';
import { ConfirmDialogModule } from 'primeng/confirmdialog';
import { SiteSettingsService, SiteSettingRequest } from '../../services/site-settings.service';

interface LinkItem {
    title: string;
    url: string;
    icon?: string;
    target?: string;
}

@Component({
    selector: 'app-site-settings',
    standalone: true,
    imports: [
        CommonModule,
        FormsModule,
        TableModule,
        DialogModule,
        ButtonModule,
        InputTextModule,
        TextareaModule,
        ToastModule,
        BlockUIModule,
        TabsModule,
        ConfirmDialogModule
    ],
    templateUrl: './site-settings.html',
    providers: [MessageService, ConfirmationService]
})
export class SiteSettingsComponent implements OnInit {
    isLoading: boolean = false;

    // Topbar
    topbarLinks: LinkItem[] = [];
    selectedTopbarLink: LinkItem = { title: '', url: '' };
    topbarDialog: boolean = false;
    isNewTopbarLink: boolean = false;

    // Footer
    footerQuickLinks: LinkItem[] = [];
    selectedFooterLink: LinkItem = { title: '', url: '' };
    footerDialog: boolean = false;
    isNewFooterLink: boolean = false;

    footerLogoContent: string = '';

    // Social Media
    socialMedia: any = {
        facebook: '',
        twitter: '',
        instagram: '',
        youtube: ''
    };

    // E-Municipality
    eMunicipalityLinks: LinkItem[] = [];
    selectedEMunicipalityLink: LinkItem = { title: '', url: '', icon: '' };
    eMunicipalityDialog: boolean = false;
    isNewEMunicipalityLink: boolean = false;

    // SEO
    seoSettings: any = {
        metaTitle: '',
        metaDescription: '',
        metaKeywords: ''
    };

    constructor(
        private siteSettingsService: SiteSettingsService,
        private messageService: MessageService,
        private confirmationService: ConfirmationService
    ) { }

    ngOnInit() {
        this.loadSettings();
    }

    loadSettings() {
        this.siteSettingsService.getAll().subscribe({
            next: (settings) => {
                // Parse Topbar
                const topbarSetting = settings.find(s => s.groupKey === 'Topbar' && s.key === 'Links');
                if (topbarSetting) {
                    try { this.topbarLinks = JSON.parse(topbarSetting.value); } catch { }
                }

                // Parse Footer Quick Links
                const footerLinksSetting = settings.find(s => s.groupKey === 'Footer' && s.key === 'QuickLinks');
                if (footerLinksSetting) {
                    try { this.footerQuickLinks = JSON.parse(footerLinksSetting.value); } catch { }
                }

                // Parse Footer Logo Content
                const footerLogoSetting = settings.find(s => s.groupKey === 'Footer' && s.key === 'LogoContent');
                if (footerLogoSetting) {
                    this.footerLogoContent = footerLogoSetting.value;
                }

                // Parse Social Media
                const socialSetting = settings.find(s => s.groupKey === 'Footer' && s.key === 'SocialMedia');
                if (socialSetting) {
                    try { this.socialMedia = JSON.parse(socialSetting.value); } catch { }
                }

                // Parse E-Municipality
                const eMunSetting = settings.find(s => s.groupKey === 'EMunicipality' && s.key === 'Links');
                if (eMunSetting) {
                    try { this.eMunicipalityLinks = JSON.parse(eMunSetting.value); } catch { }
                }

                // Parse SEO
                const seoSetting = settings.find(s => s.groupKey === 'SEO' && s.key === 'Global');
                if (seoSetting) {
                    try { this.seoSettings = JSON.parse(seoSetting.value); } catch { }
                }
            },
            error: () => {
                this.messageService.add({ severity: 'error', summary: 'Hata', detail: 'Ayarlar yüklenemedi.' });
            }
        });
    }

    // Generic Save Method
    saveSetting(group: string, key: string, value: any, description: string = '') {
        this.isLoading = true;
        const request: SiteSettingRequest = {
            groupKey: group,
            key: key,
            value: typeof value === 'string' ? value : JSON.stringify(value),
            description: description,
            order: 0
        };

        this.siteSettingsService.createOrUpdate(request).subscribe({
            next: () => {
                this.messageService.add({ severity: 'success', summary: 'Başarılı', detail: 'Ayarlar kaydedildi.' });
                this.isLoading = false;
            },
            error: () => {
                this.messageService.add({ severity: 'error', summary: 'Hata', detail: 'Kaydetme başarısız.' });
                this.isLoading = false;
            }
        });
    }

    // --- Topbar Actions ---
    openNewTopbarLink() {
        this.selectedTopbarLink = { title: '', url: '' };
        this.isNewTopbarLink = true;
        this.topbarDialog = true;
    }

    editTopbarLink(link: LinkItem) {
        this.selectedTopbarLink = { ...link };
        this.isNewTopbarLink = false;
        this.topbarDialog = true;
    }

    deleteTopbarLink(index: number) {
        this.confirmationService.confirm({
            message: 'Bu linki silmek istediğinizden emin misiniz?',
            header: 'Onay',
            icon: 'pi pi-exclamation-triangle',
            accept: () => {
                this.topbarLinks.splice(index, 1);
                this.saveSetting('Topbar', 'Links', this.topbarLinks);
            }
        });
    }

    saveTopbarLink() {
        if (this.isNewTopbarLink) {
            this.topbarLinks.push(this.selectedTopbarLink);
        } else {
            const index = this.topbarLinks.findIndex(l => l.title === this.selectedTopbarLink.title); // Simple match, better use ID but we don't have IDs for internal items
            // Actually, if we edit, we should track index. But since we copy, we lose ref.
            // I'll just use a direct replace approach for simplicity or improve standard logic.
            // A better way: maintain the index or use reference if not cloning deeply in opened dialog (but I cloned).
            // Let's rely on simple update:
            // Since I don't have IDs for items inside JSON, I'll remove the old one and add new (if titles change it is tricky).
            // Proper way: Store index when editing.
            // I'll cheat and just strictly use array manipulation if I map it to index.
        }
        // Wait, the above logic is flawed for edit.
        // Let's fix:
        // When opening edit, keep reference or index.
        // I will use `indexOf` if object reference is same, but I cloned it `{...link}`.
        // So I need to find it or store index.
        // I'll adding `selectedIndex` property to component.

        // Let's implement simpler:
        this.saveSetting('Topbar', 'Links', this.topbarLinks);
        this.topbarDialog = false;
    }

    // Better Edit Handling
    editIndex: number = -1;

    openEdit(item: any, type: string, index: number) {
        this.editIndex = index;
        if (type === 'topbar') {
            this.selectedTopbarLink = { ...item };
            this.isNewTopbarLink = false;
            this.topbarDialog = true;
        } else if (type === 'footer') {
            this.selectedFooterLink = { ...item };
            this.isNewFooterLink = false;
            this.footerDialog = true;
        } else if (type === 'emun') {
            this.selectedEMunicipalityLink = { ...item };
            this.isNewEMunicipalityLink = false;
            this.eMunicipalityDialog = true;
        }
    }

    validateUrl(url: string): boolean {
        if (!url) return false;
        // Allow relative paths starting with / or #, and absolute URLs with http/https
        const urlPattern = /^(https?:\/\/|\/|#)/i;
        return urlPattern.test(url);
    }

    confirmSave(type: string) {
        if (type === 'topbar') {
            if (!this.validateUrl(this.selectedTopbarLink.url)) {
                this.messageService.add({ severity: 'error', summary: 'Hata', detail: 'Geçersiz URL formatı. (http://, https://, / veya # ile başlamalı)' });
                return;
            }
            if (this.isNewTopbarLink) this.topbarLinks.push(this.selectedTopbarLink);
            else this.topbarLinks[this.editIndex] = this.selectedTopbarLink;
            this.saveSetting('Topbar', 'Links', this.topbarLinks);
            this.topbarDialog = false;
        } else if (type === 'footer') {
            if (!this.validateUrl(this.selectedFooterLink.url)) {
                this.messageService.add({ severity: 'error', summary: 'Hata', detail: 'Geçersiz URL formatı. (http://, https://, / veya # ile başlamalı)' });
                return;
            }
            if (this.isNewFooterLink) this.footerQuickLinks.push(this.selectedFooterLink);
            else this.footerQuickLinks[this.editIndex] = this.selectedFooterLink;
            this.saveSetting('Footer', 'QuickLinks', this.footerQuickLinks);
            this.footerDialog = false;
        } else if (type === 'emun') {
            if (!this.validateUrl(this.selectedEMunicipalityLink.url)) {
                this.messageService.add({ severity: 'error', summary: 'Hata', detail: 'Geçersiz URL formatı. (http://, https://, / veya # ile başlamalı)' });
                return;
            }
            if (this.isNewEMunicipalityLink) this.eMunicipalityLinks.push(this.selectedEMunicipalityLink);
            else this.eMunicipalityLinks[this.editIndex] = this.selectedEMunicipalityLink;
            this.saveSetting('EMunicipality', 'Links', this.eMunicipalityLinks);
            this.eMunicipalityDialog = false;
        }
    }

    // --- Footer Actions ---
    openNewFooterLink() {
        this.selectedFooterLink = { title: '', url: '' };
        this.isNewFooterLink = true;
        this.footerDialog = true;
    }

    deleteFooterLink(index: number) {
        this.confirmationService.confirm({
            message: 'Silmek istiyor musunuz?',
            accept: () => {
                this.footerQuickLinks.splice(index, 1);
                this.saveSetting('Footer', 'QuickLinks', this.footerQuickLinks);
            }
        });
    }

    saveFooterLogo() {
        this.saveSetting('Footer', 'LogoContent', this.footerLogoContent);
    }

    saveSocialMedia() {
        this.saveSetting('Footer', 'SocialMedia', this.socialMedia);
    }

    // --- E-Municipality Actions ---
    openNewEMunLink() {
        this.selectedEMunicipalityLink = { title: '', url: '', icon: '' };
        this.isNewEMunicipalityLink = true;
        this.eMunicipalityDialog = true;
    }

    deleteEMunLink(index: number) {
        this.confirmationService.confirm({
            message: 'Silmek istiyor musunuz?',
            accept: () => {
                this.eMunicipalityLinks.splice(index, 1);
                this.saveSetting('EMunicipality', 'Links', this.eMunicipalityLinks);
            }
        });
    }

    // --- SEO Actions ---
    saveSeo() {
        this.saveSetting('SEO', 'Global', this.seoSettings);
    }
}
