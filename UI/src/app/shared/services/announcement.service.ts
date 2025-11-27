import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Announcement } from '../components/announcement-card/announcement-card';
import { environment } from '../../../environments/environment';

@Injectable({
    providedIn: 'root'
})
export class AnnouncementService {
    constructor(private readonly http: HttpClient) { }

    getAnnouncements(filter?: { search?: string; from?: string; to?: string }): Observable<Announcement[]> {
        let params = new HttpParams();

        if (filter?.search) {
            params = params.set('search', filter.search);
        }

        if (filter?.from) {
            params = params.set('from', filter.from);
        }

        if (filter?.to) {
            params = params.set('to', filter.to);
        }

        return this.http.get<Announcement[]>(environment.announcementApiUrl, { params });
    }

    getAnnouncementBySlug(slug: string): Observable<Announcement> {
        return this.http.get<Announcement>(`${environment.announcementApiUrl}/${slug}`);
    }
}
