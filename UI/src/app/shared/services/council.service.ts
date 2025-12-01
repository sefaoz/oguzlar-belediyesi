import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { CouncilDocument } from '../models/council-document.model';
import { environment } from '../../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class CouncilService {
  constructor(private readonly http: HttpClient) { }

  getDocuments(): Observable<CouncilDocument[]> {
    return this.http.get<CouncilDocument[]>(environment.meclisApiUrl);
  }
}
