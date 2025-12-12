import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { ContactMessage } from '../models/contact-message';
import { environment } from '../../environments/environment';

@Injectable({
    providedIn: 'root'
})
export class ContactMessageService {
    private apiUrl = `${environment.apiUrl}/contact-messages`;

    constructor(private http: HttpClient) { }

    getAll(): Observable<ContactMessage[]> {
        return this.http.get<ContactMessage[]>(this.apiUrl);
    }

    getById(id: string): Observable<ContactMessage> {
        return this.http.get<ContactMessage>(`${this.apiUrl}/${id}`);
    }

    delete(id: string): Observable<void> {
        return this.http.delete<void>(`${this.apiUrl}/${id}`);
    }

    markAsRead(id: string): Observable<void> {
        return this.http.put<void>(`${this.apiUrl}/${id}/mark-read`, {});
    }

    getUnreadCount(): Observable<{ count: number }> {
        return this.http.get<{ count: number }>(`${this.apiUrl}/unread-count`);
    }
}
