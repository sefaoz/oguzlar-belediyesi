import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { EventItem } from '../models/event.model';
import { environment } from '../../../environments/environment';

@Injectable({
    providedIn: 'root'
})
export class EventService {
    constructor(private readonly http: HttpClient) { }

    getEvents(filter?: { upcomingOnly?: boolean; search?: string }): Observable<EventItem[]> {
        let params = new HttpParams();

        if (filter?.upcomingOnly) {
            params = params.set('upcomingOnly', 'true');
        }

        if (filter?.search) {
            params = params.set('search', filter.search);
        }

        return this.http.get<EventItem[]>(environment.eventApiUrl, { params });
    }

    getEventBySlug(slug: string): Observable<EventItem> {
        return this.http.get<EventItem>(`${environment.eventApiUrl}/${slug}`);
    }
}
