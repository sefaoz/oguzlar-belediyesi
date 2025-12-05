import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';
import { PageContent } from '../models/page-content';

@Injectable({
    providedIn: 'root'
})
export class PageContentService {
    private apiUrl = environment.pageContentUrl;

    constructor(private http: HttpClient) { }

    getByKey(key: string): Observable<PageContent> {
        return this.http.get<PageContent>(`${this.apiUrl}/${key}`);
    }
}
