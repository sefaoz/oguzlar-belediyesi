import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';
import { PageContent } from '../models/page-content';

@Injectable({
    providedIn: 'root'
})
export class PageContentService {
    private apiUrl = `${environment.apiUrl}/pages`;

    constructor(private http: HttpClient) { }

    getAll(): Observable<PageContent[]> {
        return this.http.get<PageContent[]>(this.apiUrl);
    }

    getByKey(key: string): Observable<PageContent> {
        return this.http.get<PageContent>(`${this.apiUrl}/${key}`);
    }

    private toFormData(pageContent: Omit<PageContent, 'id'>, file?: File): FormData {
        const formData = new FormData();
        formData.append('key', pageContent.key);
        formData.append('title', pageContent.title);
        formData.append('subtitle', pageContent.subtitle || '');
        if (pageContent.mapEmbedUrl) formData.append('mapEmbedUrl', pageContent.mapEmbedUrl);

        if (pageContent.paragraphs) {
            pageContent.paragraphs.forEach((p, index) => {
                formData.append(`paragraphs[${index}]`, p);
            });
        }

        if (pageContent.contactDetails) {
            pageContent.contactDetails.forEach((c, index) => {
                formData.append(`contactDetails[${index}].label`, c.label);
                formData.append(`contactDetails[${index}].value`, c.value);
            });
        }

        if (file) {
            formData.append('file', file);
        }

        if (pageContent.imageUrl) {
            formData.append('imageUrl', pageContent.imageUrl);
        }

        return formData;
    }

    create(pageContent: Omit<PageContent, 'id'>, file?: File): Observable<PageContent> {
        const formData = this.toFormData(pageContent, file);
        return this.http.post<PageContent>(this.apiUrl, formData);
    }

    update(id: string, pageContent: Omit<PageContent, 'id'>, file?: File): Observable<void> {
        const formData = this.toFormData(pageContent, file);
        return this.http.put<void>(`${this.apiUrl}/${id}`, formData);
    }

    delete(id: string): Observable<void> {
        return this.http.delete<void>(`${this.apiUrl}/${id}`);
    }
}
