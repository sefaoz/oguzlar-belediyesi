import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';

export interface ContactMessageRequest {
    name: string;
    email: string;
    phone: string;
    message: string;
    kvkkAccepted: boolean;
}

export interface ApiResponse {
    success: boolean;
    message: string;
}

@Injectable({
    providedIn: 'root'
})
export class ContactMessageService {
    private readonly apiUrl = environment.apiBaseUrl + '/api/contact-messages';

    constructor(private http: HttpClient) { }

    /**
     * İletişim formu mesajı gönderir.
     */
    sendContactMessage(request: ContactMessageRequest): Observable<ApiResponse> {
        return this.http.post<ApiResponse>(`${this.apiUrl}/contact`, request);
    }

    /**
     * Başkana mesaj gönderir.
     */
    sendMayorMessage(request: ContactMessageRequest): Observable<ApiResponse> {
        return this.http.post<ApiResponse>(`${this.apiUrl}/mayor`, request);
    }
}
