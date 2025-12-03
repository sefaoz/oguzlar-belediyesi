import { Injectable, signal } from '@angular/core';
import { environment } from '../../../environments/environment';

@Injectable({
    providedIn: 'root'
})
export class ApiConfigService {
    // Site Settings'den gelen dinamik API URL'i tutacak
    private _dynamicApiUrl = signal<string | null>(null);

    /**
     * Mevcut API URL'ini döndürür (dinamik varsa onu, yoksa environment'tan)
     */
    getApiUrl(): string {
        const dynamicUrl = this._dynamicApiUrl();
        return dynamicUrl || environment.apiUrl;
    }

    /**
     * Image Base URL'ini döndürür
     */
    getImageBaseUrl(): string {
        const dynamicUrl = this._dynamicApiUrl();
        if (dynamicUrl) {
            // Dinamik URL'den base URL'i çıkar (/api kısmını kaldır)
            return dynamicUrl.replace('/api', '');
        }
        return environment.imageBaseUrl;
    }

    /**
     * Site Settings'den gelen API URL'ini günceller
     */
    updateApiUrl(apiUrl: string | null): void {
        this._dynamicApiUrl.set(apiUrl);
    }

    /**
     * Dinamik API URL'ini temizler (environment değerini kullanır)
     */
    resetToDefault(): void {
        this._dynamicApiUrl.set(null);
    }

    /**
     * Tam endpoint URL'ini oluşturur
     */
    buildEndpoint(path: string): string {
        const baseUrl = this.getApiUrl();
        const cleanPath = path.startsWith('/') ? path.slice(1) : path;
        return `${baseUrl}/${cleanPath}`;
    }
}