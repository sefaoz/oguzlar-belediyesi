import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';
import { Tender } from '../models/tender';

@Injectable({
    providedIn: 'root'
})
export class TenderService {
    private apiUrl = `${environment.apiUrl}/tenders`;

    constructor(private http: HttpClient) { }

    getAll(): Observable<Tender[]> {
        return this.http.get<Tender[]>(this.apiUrl);
    }

    getById(slug: string): Observable<Tender> {
        return this.http.get<Tender>(`${this.apiUrl}/${slug}`);
    }

    private toFormData(tender: Omit<Tender, 'id'>, documentFiles?: File[]): FormData {
        const formData = new FormData();
        formData.append('title', tender.title || '');
        formData.append('description', tender.description || '');
        formData.append('tenderDate', tender.tenderDate || '');
        formData.append('registrationNumber', tender.registrationNumber || '');
        formData.append('status', tender.status || '');
        formData.append('documentsJson', tender.documentsJson || '[]');

        if (documentFiles && documentFiles.length > 0) {
            documentFiles.forEach(f => {
                formData.append('documentFiles', f);
            });
        }

        return formData;
    }

    create(tender: Omit<Tender, 'id'>, documentFiles?: File[]): Observable<Tender> {
        const formData = this.toFormData(tender, documentFiles);
        return this.http.post<Tender>(this.apiUrl, formData);
    }

    update(id: string, tender: Omit<Tender, 'id'>, documentFiles?: File[]): Observable<void> {
        const formData = this.toFormData(tender, documentFiles);
        return this.http.put<void>(`${this.apiUrl}/${id}`, formData);
    }

    delete(id: string): Observable<void> {
        return this.http.delete<void>(`${this.apiUrl}/${id}`);
    }
}
