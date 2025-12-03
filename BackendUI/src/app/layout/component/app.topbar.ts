import { Component, inject } from '@angular/core';
import { MenuItem } from 'primeng/api';
import { RouterModule } from '@angular/router';
import { CommonModule } from '@angular/common';
import { StyleClassModule } from 'primeng/styleclass';
import { LayoutService } from '../service/layout.service';
import { AuthService } from '../../core/services/auth.service';
import { Router } from '@angular/router';

@Component({
    selector: 'app-topbar',
    standalone: true,
    imports: [RouterModule, CommonModule, StyleClassModule],
    template: ` <div class="layout-topbar">
        <div class="layout-topbar-logo-container">
            <button class="layout-menu-button layout-topbar-action" (click)="layoutService.onMenuToggle()">
                <i class="pi pi-bars"></i>
            </button>
            <a class="layout-topbar-logo" routerLink="/">
                <img src="assets/layout/images/logo.png" alt="logo" style="height: 2.5rem;" />
            </a>
        </div>

        <div class="layout-topbar-actions">
            <div class="layout-config-menu">
                <button type="button" class="layout-topbar-action" (click)="toggleDarkMode()">
                    <i [ngClass]="{ 'pi ': true, 'pi-moon': layoutService.isDarkTheme(), 'pi-sun': !layoutService.isDarkTheme() }"></i>
                </button>
            </div>

            <button class="layout-topbar-menu-button layout-topbar-action" pStyleClass="@next" enterFromClass="hidden" enterActiveClass="animate-scalein" leaveToClass="hidden" leaveActiveClass="animate-fadeout" [hideOnOutsideClick]="true">
                <i class="pi pi-ellipsis-v"></i>
            </button>

            <div class="layout-topbar-menu">
                <div class="layout-topbar-menu-content">
                    <button type="button" class="layout-topbar-action" (click)="handleLogout()">
                        <i class="pi pi-sign-out"></i>
                        <span class="hidden lg:inline">Çıkış</span>
                    </button>
                </div>
            </div>
        </div>
    </div>`
})
export class AppTopbar {
    items!: MenuItem[];

    private readonly authService = inject(AuthService);
    private readonly router = inject(Router);

    constructor(public layoutService: LayoutService) { }

    toggleDarkMode() {
        this.layoutService.layoutConfig.update((state) => ({ ...state, darkTheme: !state.darkTheme }));
    }

    handleLogout(): void {
        const authKeys = ['auth_token', 'refresh_token', 'user_info', 'expires_at'];
        const hasStoredAuth = authKeys.some((key) => !!localStorage.getItem(key));

        if (hasStoredAuth) {
            this.authService.logout();
        } else {
            this.router.navigate(['/auth/login']);
        }
    }
}
