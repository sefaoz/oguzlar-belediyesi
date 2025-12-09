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
import { BlockUIModule } from 'primeng/blockui';
import { MessageService, ConfirmationService } from 'primeng/api';
import { UserService } from '../../services/user.service';
import { User } from '../../models/user';
import { PasswordModule } from 'primeng/password';
import { SelectModule } from 'primeng/select';

@Component({
    selector: 'app-users',
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
        BlockUIModule,
        PasswordModule,
        SelectModule
    ],
    providers: [DatePipe],
    templateUrl: './users.html',
    styleUrl: './users.scss',
})
export class UsersComponent implements OnInit {
    userDialog: boolean = false;
    users: User[] = [];
    user: User = {} as User;
    submitted: boolean = false;
    isLoading: boolean = false;
    roles: any[] = [
        { label: 'Yönetici', value: 'Admin' },
        { label: 'Editör', value: 'Editor' }
    ];

    constructor(
        private userService: UserService,
        private messageService: MessageService,
        private confirmationService: ConfirmationService,
        private datePipe: DatePipe
    ) { }

    ngOnInit() {
        this.getUsers();
    }

    getUsers() {
        this.userService.getAll().subscribe({
            next: (data) => {
                this.users = data;
            },
            error: (error) => {
                console.error('Kullanıcılar yüklenirken hata oluştu:', error);
                this.messageService.add({ severity: 'error', summary: 'Hata', detail: 'Kullanıcılar yüklenemedi.' });
            }
        });
    }

    openNew() {
        this.user = {} as User;
        this.user.role = 'Admin';
        this.submitted = false;
        this.userDialog = true;
    }

    editUser(user: User) {
        this.user = { ...user };
        this.user.password = ''; // Reset password field for security and logic
        this.userDialog = true;
    }

    deleteUser(user: User) {
        this.confirmationService.confirm({
            message: '"' + user.username + '" adlı kullanıcıyı silmek istediğinize emin misiniz?',
            header: 'Onay',
            icon: 'pi pi-exclamation-triangle',
            acceptLabel: 'Evet',
            rejectLabel: 'Hayır',
            accept: () => {
                this.userService.delete(user.id).subscribe({
                    next: () => {
                        this.messageService.add({ severity: 'success', summary: 'Başarılı', detail: 'Kullanıcı silindi.', life: 3000 });
                        this.getUsers();
                    },
                    error: (error) => {
                        console.error('Silme hatası:', error);
                        this.messageService.add({ severity: 'error', summary: 'Hata', detail: 'Kullanıcı silinirken hata oluştu.' });
                    }
                });
            }
        });
    }

    hideDialog() {
        this.userDialog = false;
        this.submitted = false;
    }

    saveUser() {
        this.submitted = true;

        if (this.user.username?.trim() && this.user.role) {
            if (!this.user.id && !this.user.password) {
                return;
            }

            this.isLoading = true;

            const finalizeCallback = () => {
                this.isLoading = false;
            };

            if (this.user.id) {
                this.userService.update(this.user.id, this.user).subscribe({
                    next: () => {
                        this.messageService.add({ severity: 'success', summary: 'Başarılı', detail: 'Kullanıcı güncellendi.', life: 3000 });
                        this.getUsers();
                        this.userDialog = false;
                        this.user = {} as User;
                    },
                    error: (error) => {
                        console.error('Güncelleme hatası:', error);
                        this.messageService.add({ severity: 'error', summary: 'Hata', detail: 'Kullanıcı güncellenirken hata oluştu.' });
                    },
                    complete: finalizeCallback
                });
            } else {
                this.userService.create(this.user).subscribe({
                    next: () => {
                        this.messageService.add({ severity: 'success', summary: 'Başarılı', detail: 'Kullanıcı oluşturuldu.', life: 3000 });
                        this.getUsers();
                        this.userDialog = false;
                        this.user = {} as User;
                    },
                    error: (error) => {
                        console.error('Oluşturma hatası:', error);
                        this.messageService.add({ severity: 'error', summary: 'Hata', detail: 'Kullanıcı oluşturulurken hata oluştu.' });
                    },
                    complete: finalizeCallback
                });
            }
        }
    }

    onGlobalFilter(table: Table, event: Event) {
        table.filterGlobal((event.target as HTMLInputElement).value, 'contains');
    }
}
