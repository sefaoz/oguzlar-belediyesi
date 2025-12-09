import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';
import { KvkkDocument } from '../models/kvkk-document';

@Injectable({
    providedIn: 'root'
})
export class KvkkService {
    private apiUrl = `${environment.apiUrl}/kvkk`;

    constructor(private http: HttpClient) { }

    getAll(): Observable<KvkkDocument[]> {
        return this.http.get<KvkkDocument[]>(this.apiUrl);
    }

    create(kvkk: KvkkDocument, file?: File): Observable<KvkkDocument> {
        const formData = new FormData();
        formData.append('title', kvkk.title || '');
        formData.append('type', kvkk.type || '');
        if (file) {
            formData.append('file', file);
        }
        return this.http.post<KvkkDocument>(this.apiUrl, formData);
    }

    update(id: string, kvkk: KvkkDocument, file?: File): Observable<KvkkDocument> {
        const formData = new FormData();
        formData.append('id', id);
        formData.append('title', kvkk.title || '');
        formData.append('type', kvkk.type || '');
        if (file) {
            formData.append('file', file);
        }
        return this.http.put<KvkkDocument>(`${this.apiUrl}/${id}`, formData);
    }

    delete(id: string): Observable<void> {
        return this.http.delete<void>(`${this.apiUrl}/${id}`);
    }
}
