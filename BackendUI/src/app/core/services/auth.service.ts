import { Injectable, signal } from '@angular/core';
import { Router } from '@angular/router';
import { HttpClient } from '@angular/common/http';
import { firstValueFrom, Observable, tap } from 'rxjs';
import { ApiConfigService } from './api-config.service';

interface LoginResponse {
    token: string;
    refreshToken: string;
}

@Injectable({
    providedIn: 'root'
})
export class AuthService {
    private readonly authTokenKey = 'auth_token';
    private readonly userInfoKey = 'user_info';
    private readonly expiresAtKey = 'expires_at';
    private readonly refreshTokenKey = 'refresh_token';

    private readonly _isAuthenticated = signal<boolean>(false);

    constructor(
        private readonly http: HttpClient,
        private readonly router: Router,
        private readonly apiConfig: ApiConfigService
    ) {
        const storedToken = localStorage.getItem(this.authTokenKey);
        if (storedToken) {
            this._isAuthenticated.set(true);
        }
    }

    get isAuthenticated() {
        return this._isAuthenticated();
    }

    async login(email: string, password: string): Promise<boolean> {
        if (!email?.trim() || !password) {
            return false;
        }

        return new Promise<boolean>((resolve) => {
            const url = this.apiConfig.buildEndpoint('auth/login');

            this.http.post<LoginResponse>(url, {
                username: email.trim(),
                password
            }).subscribe({
                next: (response) => {
                    if (!response?.token) {
                        resolve(false);
                        return;
                    }

                    localStorage.setItem(this.authTokenKey, response.token);
                    localStorage.setItem(this.refreshTokenKey, response.refreshToken);
                    localStorage.setItem(this.userInfoKey, JSON.stringify({ username: email.trim() }));

                    const expiresAt = this.getTokenExpiration(response.token);
                    if (expiresAt) {
                        localStorage.setItem(this.expiresAtKey, expiresAt.toISOString());
                    }

                    this._isAuthenticated.set(true);
                    this.router.navigate(['/']);
                    resolve(true);
                },
                error: (error) => {
                    console.error('Login failed', error);
                    resolve(false);
                }
            });
        });
    }

    refreshToken(): Observable<LoginResponse> {
        const token = localStorage.getItem(this.authTokenKey);
        const refreshToken = localStorage.getItem(this.refreshTokenKey);
        const url = this.apiConfig.buildEndpoint('auth/refresh-token');

        return this.http.post<LoginResponse>(url, { token, refreshToken }).pipe(
            tap(response => {
                localStorage.setItem(this.authTokenKey, response.token);
                localStorage.setItem(this.refreshTokenKey, response.refreshToken);
            })
        );
    }

    logout(): void {
        localStorage.removeItem(this.authTokenKey);
        localStorage.removeItem(this.userInfoKey);
        localStorage.removeItem(this.expiresAtKey);
        localStorage.removeItem(this.refreshTokenKey);
        this._isAuthenticated.set(false);
        this.router.navigate(['/auth/login']);
    }

    private getTokenExpiration(token: string): Date | null {
        const parts = token.split('.');
        if (parts.length !== 3) {
            return null;
        }

        try {
            const payload = JSON.parse(atob(parts[1]));
            if (payload?.exp && !Number.isNaN(payload.exp)) {
                return new Date(payload.exp * 1000);
            }
        } catch {
            // ignore decoding errors
        }

        return null;
    }
}
