import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { KvkkDocument } from '../models/kvkk-document.model';

@Injectable({
    providedIn: 'root'
})
export class KvkkService {
    constructor(private readonly http: HttpClient) { }

    getDocuments(): Observable<KvkkDocument[]> {
        return this.http.get<KvkkDocument[]>(environment.kvkkApiUrl);
    }
}
