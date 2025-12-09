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

    private toFormData(news: Omit<News, 'id'>, file?: File, galleryFiles?: File[]): FormData {
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

        if (news.tags && news.tags.length > 0) {
            news.tags.forEach(tag => {
                formData.append('tags', tag);
            });
        }

        if (news.photos && news.photos.length > 0) {
            news.photos.forEach(photo => {
                formData.append('photos', photo);
            });
        }

        if (galleryFiles && galleryFiles.length > 0) {
            galleryFiles.forEach(f => {
                formData.append('galleryFiles', f);
            });
        }

        return formData;
    }

    create(news: Omit<News, 'id'>, file?: File, galleryFiles?: File[]): Observable<News> {
        const formData = this.toFormData(news, file, galleryFiles);
        return this.http.post<News>(this.apiUrl, formData);
    }

    update(id: string, news: Omit<News, 'id'>, file?: File, galleryFiles?: File[]): Observable<void> {
        const formData = this.toFormData(news, file, galleryFiles);
        return this.http.put<void>(`${this.apiUrl}/${id}`, formData);
    }

    delete(id: string): Observable<void> {
        return this.http.delete<void>(`${this.apiUrl}/${id}`);
    }
}
