import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../environments/environment';
import { GalleryFolder, GalleryImage } from '../models/gallery';
import { Observable } from 'rxjs';

@Injectable({
    providedIn: 'root'
})
export class GalleryService {
    private apiUrl = `${environment.apiUrl}/gallery`;

    constructor(private http: HttpClient) { }

    getFolders(): Observable<GalleryFolder[]> {
        return this.http.get<GalleryFolder[]>(`${this.apiUrl}/folders`);
    }

    getFolderById(id: string): Observable<GalleryFolder> {
        return this.http.get<GalleryFolder>(`${this.apiUrl}/folders/${id}`);
    }

    createFolder(data: { title: string, date: string, isFeatured: boolean, isActive: boolean }): Observable<GalleryFolder> {
        return this.http.post<GalleryFolder>(`${this.apiUrl}/folders`, data);
    }

    updateFolder(id: string, data: { title: string, date: string, isFeatured: boolean, isActive: boolean }): Observable<void> {
        return this.http.put<void>(`${this.apiUrl}/folders/${id}`, data);
    }

    deleteFolder(id: string): Observable<void> {
        return this.http.delete<void>(`${this.apiUrl}/folders/${id}`);
    }

    getImages(folderId: string): Observable<GalleryImage[]> {
        return this.http.get<GalleryImage[]>(`${this.apiUrl}/folders/${folderId}/images`);
    }

    addImage(folderId: string, file: File, title?: string): Observable<GalleryImage> {
        const formData = new FormData();
        formData.append('folderId', folderId);
        formData.append('file', file);
        if (title) formData.append('title', title);
        return this.http.post<GalleryImage>(`${this.apiUrl}/images`, formData);
    }

    deleteImage(id: string): Observable<void> {
        return this.http.delete<void>(`${this.apiUrl}/images/${id}`);
    }
}
