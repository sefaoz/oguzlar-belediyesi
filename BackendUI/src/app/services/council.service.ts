import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';
import { CouncilDocument } from '../models/council-document';

@Injectable({
    providedIn: 'root'
})
export class CouncilService {
    private apiUrl = `${environment.apiUrl}/meclis`;

    constructor(private http: HttpClient) { }

    getAll(): Observable<CouncilDocument[]> {
        return this.http.get<CouncilDocument[]>(this.apiUrl);
    }

    getById(id: string): Observable<CouncilDocument> {
        return this.http.get<CouncilDocument>(`${this.apiUrl}/${id}`);
    }

    create(document: CouncilDocument, file?: File): Observable<CouncilDocument> {
        const formData = new FormData();
        formData.append('title', document.title);
        formData.append('type', document.type);
        formData.append('date', document.date);
        if (document.description) formData.append('description', document.description);
        if (file) formData.append('file', file);

        return this.http.post<CouncilDocument>(this.apiUrl, formData);
    }

    update(id: string, document: CouncilDocument, file?: File): Observable<void> {
        const formData = new FormData();
        formData.append('title', document.title);
        formData.append('type', document.type);
        formData.append('date', document.date);
        if (document.description) formData.append('description', document.description);
        if (file) formData.append('file', file);

        return this.http.put<void>(`${this.apiUrl}/${id}`, formData);
    }

    delete(id: string): Observable<void> {
        return this.http.delete<void>(`${this.apiUrl}/${id}`);
    }
}
