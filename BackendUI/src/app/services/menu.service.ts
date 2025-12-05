import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';
import { Menu } from '../models/menu';

@Injectable({
    providedIn: 'root'
})
export class MenuService {
    private apiUrl = `${environment.apiUrl}/menus`;

    constructor(private http: HttpClient) { }

    getAll(): Observable<Menu[]> {
        return this.http.get<Menu[]>(this.apiUrl);
    }

    getById(id: string): Observable<Menu> {
        return this.http.get<Menu>(`${this.apiUrl}/${id}`);
    }

    create(menu: Omit<Menu, 'id'>): Observable<Menu> {
        return this.http.post<Menu>(this.apiUrl, menu);
    }

    update(id: string, menu: Menu): Observable<void> {
        return this.http.put<void>(`${this.apiUrl}/${id}`, menu);
    }

    delete(id: string): Observable<void> {
        return this.http.delete<void>(`${this.apiUrl}/${id}`);
    }

    updateOrder(items: Menu[]): Observable<void> {
        return this.http.put<void>(`${this.apiUrl}/order`, items);
    }
}
