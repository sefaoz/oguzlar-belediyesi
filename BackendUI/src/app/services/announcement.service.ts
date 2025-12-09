import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';
import { Announcement } from '../models/announcement';

@Injectable({
    providedIn: 'root'
})
export class AnnouncementService {
    private apiUrl = `${environment.apiUrl}/announcements`;

    constructor(private http: HttpClient) { }

    getAll(): Observable<Announcement[]> {
        return this.http.get<Announcement[]>(this.apiUrl);
    }

    getById(id: string): Observable<Announcement> {
        return this.http.get<Announcement>(`${this.apiUrl}/${id}`);
    }

    private toFormData(announcement: Omit<Announcement, 'id'>, file?: File, galleryFiles?: File[]): FormData {
        const formData = new FormData();
        formData.append('title', announcement.title);
        if (announcement.summary) {
            formData.append('summary', announcement.summary);
        }
        formData.append('content', announcement.content);
        formData.append('date', announcement.date);
        formData.append('slug', announcement.slug);

        if (file) {
            formData.append('file', file);
        }

        if (announcement.image) {
            formData.append('image', announcement.image);
        }

        if (announcement.tags && announcement.tags.length > 0) {
            announcement.tags.forEach(tag => {
                formData.append('tags', tag);
            });
        }

        if (announcement.category) {
            formData.append('category', announcement.category);
        }

        if (announcement.photos && announcement.photos.length > 0) {
            announcement.photos.forEach(photo => {
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

    create(announcement: Omit<Announcement, 'id'>, file?: File, galleryFiles?: File[]): Observable<Announcement> {
        const formData = this.toFormData(announcement, file, galleryFiles);
        return this.http.post<Announcement>(this.apiUrl, formData);
    }

    update(id: string, announcement: Omit<Announcement, 'id'>, file?: File, galleryFiles?: File[]): Observable<void> {
        const formData = this.toFormData(announcement, file, galleryFiles);
        return this.http.put<void>(`${this.apiUrl}/${id}`, formData);
    }

    delete(id: string): Observable<void> {
        return this.http.delete<void>(`${this.apiUrl}/${id}`);
    }
}
