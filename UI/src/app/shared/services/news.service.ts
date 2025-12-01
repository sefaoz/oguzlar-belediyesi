import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { NewsItem } from '../models/news.model';
import { environment } from '../../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class NewsService {
  constructor(private readonly http: HttpClient) { }

  getNews(): Observable<NewsItem[]> {
    return this.http.get<NewsItem[]>(environment.newsApiUrl);
  }

  getNewsBySlug(slug: string): Observable<NewsItem | undefined> {
    return this.http.get<NewsItem>(`${environment.newsApiUrl}/${slug}`);
  }
}
