import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Tender } from '../models/tender.model';
import { environment } from '../../../environments/environment';

@Injectable({
    providedIn: 'root'
})
export class TenderService {
    constructor(private readonly http: HttpClient) { }

    getTenders(filter?: { status?: string; search?: string }): Observable<Tender[]> {
        let params = new HttpParams();

        if (filter?.status) {
            params = params.set('status', filter.status);
        }

        if (filter?.search) {
            params = params.set('search', filter.search);
        }

        return this.http.get<Tender[]>(environment.tenderApiUrl, { params });
    }

    getTenderBySlug(slug: string): Observable<Tender> {
        return this.http.get<Tender>(`${environment.tenderApiUrl}/${slug}`);
    }
}
