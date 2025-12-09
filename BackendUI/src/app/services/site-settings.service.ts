import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';

export interface SiteSetting {
    id?: string;
    key: string;
    value: string;
    groupKey: string;
    description?: string;
    order: number;
    createdAt?: string;
    updatedAt?: string;
}

export interface SiteSettingRequest {
    key: string;
    value: string;
    groupKey: string;
    description?: string;
    order: number;
}

@Injectable({
    providedIn: 'root'
})
export class SiteSettingsService {
    private apiUrl = environment.apiUrl + '/site-settings';

    constructor(private http: HttpClient) { }

    getAll(): Observable<SiteSetting[]> {
        return this.http.get<SiteSetting[]>(this.apiUrl);
    }

    getByGroup(groupKey: string): Observable<SiteSetting[]> {
        return this.http.get<SiteSetting[]>(`${this.apiUrl}/group/${groupKey}`);
    }

    getByKey(groupKey: string, key: string): Observable<SiteSetting> {
        return this.http.get<SiteSetting>(`${this.apiUrl}/key/${groupKey}/${key}`);
    }

    createOrUpdate(setting: SiteSettingRequest): Observable<SiteSetting> {
        return this.http.post<SiteSetting>(this.apiUrl, setting);
    }

    deleteByKey(groupKey: string, key: string): Observable<void> {
        return this.http.delete<void>(`${this.apiUrl}/key/${groupKey}/${key}`);
    }
}
