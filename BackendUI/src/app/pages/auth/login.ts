import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { ButtonModule } from 'primeng/button';
import { CheckboxModule } from 'primeng/checkbox';
import { InputTextModule } from 'primeng/inputtext';
import { PasswordModule } from 'primeng/password';
import { RippleModule } from 'primeng/ripple';
import { AppFloatingConfigurator } from '../../layout/component/app.floatingconfigurator';
import { AuthService } from '../../core/services/auth.service';

@Component({
    selector: 'app-login',
    standalone: true,
    imports: [
        ButtonModule,
        CheckboxModule,
        CommonModule,
        InputTextModule,
        PasswordModule,
        FormsModule,
        RouterModule,
        RippleModule,
        AppFloatingConfigurator
    ],
    template: `
        <app-floating-configurator />
        <div class="bg-surface-50 dark:bg-surface-950 flex items-center justify-center min-h-screen min-w-screen overflow-hidden">
            <div class="flex flex-col items-center justify-center">
                <div style="border-radius: 56px; padding: 0.3rem; background: linear-gradient(180deg, var(--primary-color) 10%, rgba(33, 150, 243, 0) 30%)">
                    <div class="w-full bg-surface-0 dark:bg-surface-900 py-20 px-8 sm:px-20" style="border-radius: 53px">
                        <div class="text-center mb-8">
                            <div class="mb-8">
                                <img src="/assets/layout/images/logo.png" alt="Oğuzlar Belediyesi Logo" class="mb-8 w-24 shrink-0 mx-auto" />
                            </div>
                            <div class="text-surface-900 dark:text-surface-0 text-3xl font-medium mb-4">Yönetim Paneli</div>
                            <!-- <span class="text-muted-color font-medium">Yönetim Paneli</span> -->
                        </div>

                        <div>
                            <label for="email1" class="block text-surface-900 dark:text-surface-0 text-xl font-medium mb-2">E-posta</label>
                            <input pInputText id="email1" type="text" placeholder="E-posta adresi" class="w-full md:w-120 mb-8" [(ngModel)]="email" />

                            <label for="password1" class="block text-surface-900 dark:text-surface-0 font-medium text-xl mb-2">Şifre</label>
                            <p-password id="password1" [(ngModel)]="password" placeholder="Şifre" [toggleMask]="true" styleClass="mb-4" [fluid]="true" [feedback]="false"></p-password>

                            <div class="flex items-center justify-between mt-2 mb-8 gap-8">
                                <div class="flex items-center">
                                    <p-checkbox [(ngModel)]="checked" id="rememberme1" binary class="mr-2"></p-checkbox>
                                    <label for="rememberme1">Beni Hatırla</label>
                                </div>
                                <span class="font-medium no-underline ml-2 text-right cursor-pointer text-primary">Şifremi Unuttum?</span>
                            </div>
                            <p *ngIf="loginError" class="text-red-600 text-sm font-medium mb-4">{{ loginError }}</p>
                            <p-button
                                label="Giri\u015f Yap"
                                styleClass="w-full"
                                [loading]="isLoading"
                                [disabled]="isLoading"
                                (onClick)="onLogin()"
                            ></p-button>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    `
})
export class Login {
    email: string = '';

    password: string = '';

    checked: boolean = false;

    isLoading = false;

    loginError: string | null = null;

    constructor(private authService: AuthService) { }

    async onLogin(): Promise<void> {
        if (this.isLoading) {
            return;
        }

        this.loginError = null;
        this.isLoading = true;

        const success = await this.authService.login(this.email, this.password);

        this.isLoading = false;

        if (!success) {
            this.loginError = 'Giris basarisiz oldu. Bilgileri kontrol edip tekrar deneyin.';
        }
    }
}
