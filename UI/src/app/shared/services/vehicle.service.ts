import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { Vehicle } from '../models/vehicle.model';

@Injectable({
    providedIn: 'root'
})
export class VehicleService {
    constructor(private readonly http: HttpClient) { }

    getVehicles(): Observable<Vehicle[]> {
        return this.http.get<Vehicle[]>(environment.vehicleApiUrl);
    }
}
