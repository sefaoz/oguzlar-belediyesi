import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';
import { EventItem } from '../models/event';

@Injectable({
    providedIn: 'root'
})
export class EventService {
    private apiUrl = `${environment.apiUrl}/events`;

    constructor(private http: HttpClient) { }

    getAll(): Observable<EventItem[]> {
        return this.http.get<EventItem[]>(this.apiUrl);
    }

    getById(id: string): Observable<EventItem> {
        return this.http.get<EventItem>(`${this.apiUrl}/${id}`);
    }

    private toFormData(eventItem: Omit<EventItem, 'id'>, file?: File): FormData {
        const formData = new FormData();
        formData.append('title', eventItem.title);
        formData.append('description', eventItem.description);
        formData.append('location', eventItem.location);
        formData.append('eventDate', eventItem.eventDate);
        formData.append('eventTime', eventItem.eventTime);
        formData.append('slug', eventItem.slug);

        if (file) {
            formData.append('file', file);
        }

        if (eventItem.image) {
            formData.append('image', eventItem.image);
        }

        return formData;
    }

    create(eventItem: Omit<EventItem, 'id'>, file?: File): Observable<EventItem> {
        const formData = this.toFormData(eventItem, file);
        return this.http.post<EventItem>(this.apiUrl, formData);
    }

    update(id: string, eventItem: Omit<EventItem, 'id'>, file?: File): Observable<void> {
        const formData = this.toFormData(eventItem, file);
        return this.http.put<void>(`${this.apiUrl}/${id}`, formData);
    }

    delete(id: string): Observable<void> {
        return this.http.delete<void>(`${this.apiUrl}/${id}`);
    }
}
