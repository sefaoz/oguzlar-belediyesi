import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { TreeTableModule } from 'primeng/treetable';
import { ButtonModule } from 'primeng/button';
import { DialogModule } from 'primeng/dialog';
import { InputTextModule } from 'primeng/inputtext';
import { InputNumberModule } from 'primeng/inputnumber';
import { CheckboxModule } from 'primeng/checkbox';
import { ToastModule } from 'primeng/toast';
import { ConfirmDialogModule } from 'primeng/confirmdialog';
import { ToolbarModule } from 'primeng/toolbar';
import { MessageService, ConfirmationService, TreeNode } from 'primeng/api';
import { MenuService } from '../../services/menu.service';
import { Menu } from '../../models/menu';
import { BlockUIModule } from 'primeng/blockui';

@Component({
    selector: 'app-menu-management',
    standalone: true,
    imports: [
        CommonModule,
        FormsModule,
        TreeTableModule,
        ButtonModule,
        DialogModule,
        InputTextModule,
        InputNumberModule,
        CheckboxModule,
        ToolbarModule,
        BlockUIModule
    ],
    templateUrl: './menu-management.component.html',
    providers: []
})
export class MenuManagementComponent implements OnInit {
    menus: TreeNode[] = [];
    rawMenus: Menu[] = [];
    menuDialog: boolean = false;
    menu: Menu = this.createEmptyMenu();
    submitted: boolean = false;
    parentOptions: any[] = [];
    isLoading: boolean = false;

    targetOptions = [
        { label: 'Aynı Sekme (_self)', value: '_self' },
        { label: 'Yeni Sekme (_blank)', value: '_blank' }
    ];

    constructor(
        private menuService: MenuService,
        private messageService: MessageService,
        private confirmationService: ConfirmationService
    ) { }

    ngOnInit() {
        this.loadMenus();
    }

    loadMenus() {
        this.menuService.getAll().subscribe({
            next: (data) => {
                this.rawMenus = data;
                this.menus = this.buildTree(data);
                this.updateParentOptions();
            },
            error: () => this.messageService.add({ severity: 'error', summary: 'Hata', detail: 'Menüler yüklenemedi.' })
        });
    }

    buildTree(menus: Menu[]): TreeNode[] {
        const map = new Map<string, TreeNode>();
        const roots: TreeNode[] = [];

        menus.forEach(menu => {
            map.set(menu.id, {
                data: menu,
                children: [],
                expanded: true
            });
        });

        menus.forEach(menu => {
            const node = map.get(menu.id);
            if (menu.parentId && map.has(menu.parentId)) {
                map.get(menu.parentId)!.children!.push(node!);
            } else {
                roots.push(node!);
            }
        });

        const sortNodes = (nodes: TreeNode[]) => {
            nodes.sort((a, b) => (a.data.order || 0) - (b.data.order || 0));
            nodes.forEach(node => {
                if (node.children && node.children.length > 0) {
                    sortNodes(node.children);
                }
            });
        };
        sortNodes(roots);

        return roots;
    }

    updateParentOptions() {
        this.parentOptions = [
            { label: 'Ana Menü (Üst Menü Yok)', value: null },
            ...this.rawMenus.map(m => ({ label: m.title, value: m.id }))
        ];
    }

    openNew() {
        this.menu = this.createEmptyMenu();
        this.submitted = false;
        this.menuDialog = true;
    }

    editMenu(menu: Menu) {
        this.menu = { ...menu };
        this.menuDialog = true;
    }

    deleteMenu(menu: Menu) {
        this.confirmationService.confirm({
            message: '"' + menu.title + '" menüsünü silmek istediğinize emin misiniz?',
            header: 'Onay',
            icon: 'pi pi-exclamation-triangle',
            accept: () => {
                this.menuService.delete(menu.id).subscribe({
                    next: () => {
                        this.messageService.add({ severity: 'success', summary: 'Başarılı', detail: 'Menü silindi', life: 3000 });
                        this.loadMenus();
                    },
                    error: () => this.messageService.add({ severity: 'error', summary: 'Hata', detail: 'Silme işlemi başarısız.' })
                });
            }
        });
    }

    saveMenu() {
        this.submitted = true;

        if (this.menu.title?.trim()) {
            this.isLoading = true;
            const finalize = () => {
                this.isLoading = false;
            };

            if (this.menu.id) {
                this.menuService.update(this.menu.id, this.menu).subscribe({
                    next: () => {
                        this.messageService.add({ severity: 'success', summary: 'Başarılı', detail: 'Menü güncellendi', life: 3000 });
                        this.loadMenus();
                        this.menuDialog = false;
                        this.menu = this.createEmptyMenu();
                    },
                    error: () => this.messageService.add({ severity: 'error', summary: 'Hata', detail: 'Güncelleme başarısız.' }),
                    complete: finalize
                });
            } else {
                this.menuService.create(this.menu).subscribe({
                    next: () => {
                        this.messageService.add({ severity: 'success', summary: 'Başarılı', detail: 'Menü oluşturuldu', life: 3000 });
                        this.loadMenus();
                        this.menuDialog = false;
                        this.menu = this.createEmptyMenu();
                    },
                    error: () => this.messageService.add({ severity: 'error', summary: 'Hata', detail: 'Oluşturma başarısız.' }),
                    complete: finalize
                });
            }
        }
    }

    hideDialog() {
        this.menuDialog = false;
        this.submitted = false;
    }

    createEmptyMenu(): Menu {
        return {
            id: '',
            title: '',
            url: '',
            order: 0,
            parentId: null,
            isVisible: true,
            target: '_self'
        };
    }
}
