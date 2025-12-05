import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';
import { Menu } from '../models/menu';

@Injectable({
    providedIn: 'root'
})
export class MenuService {
    private apiUrl = `${environment.apiBaseUrl}/menus`;

    constructor(private http: HttpClient) { }

    getAll(): Observable<Menu[]> {
        return this.http.get<Menu[]>(this.apiUrl);
    }
}
