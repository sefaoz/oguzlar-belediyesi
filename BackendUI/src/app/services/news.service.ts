import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';
import { News } from '../models/news';

@Injectable({
    providedIn: 'root'
})
export class NewsService {
    private apiUrl = `${environment.apiUrl}/news`;

    constructor(private http: HttpClient) { }

    getAll(): Observable<News[]> {
        return this.http.get<News[]>(this.apiUrl);
    }

    getById(id: string): Observable<News> {
        return this.http.get<News>(`${this.apiUrl}/${id}`);
    }

    private toFormData(news: Omit<News, 'id'>, file?: File): FormData {
        const formData = new FormData();
        formData.append('title', news.title);
        formData.append('description', news.description);
        formData.append('date', news.date);
        formData.append('slug', news.slug);

        if (file) {
            formData.append('file', file);
        }

        if (news.image) {
            formData.append('image', news.image);
        }

        // Handle photos if necessary, for now skipping complex gallery upload
        // unless explicitly requested to handle multiple files in the same call

        return formData;
    }

    create(news: Omit<News, 'id'>, file?: File): Observable<News> {
        const formData = this.toFormData(news, file);
        return this.http.post<News>(this.apiUrl, formData);
    }

    update(id: string, news: Omit<News, 'id'>, file?: File): Observable<void> {
        const formData = this.toFormData(news, file);
        return this.http.put<void>(`${this.apiUrl}/${id}`, formData);
    }

    delete(id: string): Observable<void> {
        return this.http.delete<void>(`${this.apiUrl}/${id}`);
    }
}
