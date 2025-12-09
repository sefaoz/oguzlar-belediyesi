import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, BehaviorSubject, tap } from 'rxjs';
import { environment } from '../../environments/environment';

export interface SiteSetting {
    key: string;
    value: string;
    groupKey: string;
}

@Injectable({
    providedIn: 'root'
})
export class SiteSettingsService {
    private apiUrl = environment.apiBaseUrl + '/site-settings';
    private settingsSubject = new BehaviorSubject<SiteSetting[]>([]);
    public settings$ = this.settingsSubject.asObservable();

    constructor(private http: HttpClient) {
        this.loadSettings();
    }

    private loadSettings() {
        this.http.get<SiteSetting[]>(this.apiUrl).subscribe(settings => {
            this.settingsSubject.next(settings);
        });
    }

    getSetting(groupKey: string, key: string): string | null {
        const settings = this.settingsSubject.value;
        const setting = settings.find(s => s.groupKey === groupKey && s.key === key);
        return setting ? setting.value : null;
    }

    // Helper to get parsed JSON
    getJsonSetting<T>(groupKey: string, key: string): T | null {
        const val = this.getSetting(groupKey, key);
        if (!val) return null;
        try {
            return JSON.parse(val) as T;
        } catch {
            return null;
        }
    }
}
