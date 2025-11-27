import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { PageContentModel } from '../models/page-content.model';

@Injectable({
  providedIn: 'root'
})
export class PageContentService {
  constructor(private readonly http: HttpClient) {}

  getPageContent(key: string): Observable<PageContentModel> {
    return this.http.get<PageContentModel>(`${environment.pageContentUrl}/${key}`);
  }
}
