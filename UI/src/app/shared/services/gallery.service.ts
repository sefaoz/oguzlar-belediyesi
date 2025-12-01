import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { catchError, Observable, of } from 'rxjs';
import { environment } from '../../../environments/environment';
import { GalleryFolder, GalleryImage } from '../models/gallery.model';

@Injectable({
    providedIn: 'root'
})
export class GalleryService {
    private readonly foldersUrl = `${environment.galleryApiUrl}/folders`;

    constructor(private readonly http: HttpClient) { }

    getFolders(): Observable<GalleryFolder[]> {
        return this.http.get<GalleryFolder[]>(this.foldersUrl);
    }

    getImages(folderId: string): Observable<GalleryImage[]> {
        return this.http.get<GalleryImage[]>(`${this.foldersUrl}/${folderId}/images`);
    }

    getFolder(folderId: string): Observable<GalleryFolder | undefined> {
        return this.http.get<GalleryFolder>(`${this.foldersUrl}/${folderId}`).pipe(
            catchError(() => of(undefined))
        );
    }

    getFolderBySlug(slug: string): Observable<GalleryFolder | undefined> {
        return this.http.get<GalleryFolder>(`${this.foldersUrl}/slug/${slug}`).pipe(
            catchError(() => of(undefined))
        );
    }
}
