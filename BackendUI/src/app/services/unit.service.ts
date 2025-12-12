import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';
import { MunicipalUnit } from '../models/municipal-unit';

@Injectable({
    providedIn: 'root'
})
export class UnitService {
    private apiUrl = `${environment.apiUrl}/units`;

    constructor(private http: HttpClient) { }

    getAll(): Observable<MunicipalUnit[]> {
        return this.http.get<MunicipalUnit[]>(this.apiUrl);
    }

    getById(id: string): Observable<MunicipalUnit> {
        return this.http.get<MunicipalUnit>(`${this.apiUrl}/${id}`);
    }

    create(unit: MunicipalUnit, staffFiles: { [key: number]: File }): Observable<MunicipalUnit> {
        const formData = this.toFormData(unit, staffFiles);
        return this.http.post<MunicipalUnit>(this.apiUrl, formData);
    }

    update(id: string, unit: MunicipalUnit, staffFiles: { [key: number]: File }): Observable<void> {
        const formData = this.toFormData(unit, staffFiles);
        return this.http.put<void>(`${this.apiUrl}/${id}`, formData);
    }

    delete(id: string): Observable<void> {
        return this.http.delete<void>(`${this.apiUrl}/${id}`);
    }

    private toFormData(unit: MunicipalUnit, staffFiles: { [key: number]: File }): FormData {
        const formData = new FormData();
        formData.append('title', unit.title || '');
        formData.append('content', unit.content || '');
        formData.append('icon', unit.icon || '');
        formData.append('slug', unit.slug || '');

        if (unit.staff) {
            formData.append('staffJson', JSON.stringify(unit.staff));
        }

        Object.keys(staffFiles).forEach(key => {
            const index = Number(key);
            formData.append(`staffImage_${index}`, staffFiles[index]);
        });

        return formData;
    }
}
