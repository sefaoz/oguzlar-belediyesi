import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { MunicipalUnit } from '../models/unit.model';

@Injectable({
    providedIn: 'root'
})
export class UnitService {
    constructor(private readonly http: HttpClient) { }

    getUnits(): Observable<MunicipalUnit[]> {
        return this.http.get<MunicipalUnit[]>(environment.unitApiUrl);
    }
}
