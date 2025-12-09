import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';
import { Vehicle } from '../models/vehicle';

@Injectable({
    providedIn: 'root'
})
export class VehicleService {
    private apiUrl = `${environment.apiUrl}/vehicles`;

    constructor(private http: HttpClient) { }

    getAll(): Observable<Vehicle[]> {
        return this.http.get<Vehicle[]>(this.apiUrl);
    }

    getById(id: string): Observable<Vehicle> {
        return this.http.get<Vehicle>(`${this.apiUrl}/${id}`);
    }

    private toFormData(vehicle: Omit<Vehicle, 'id'>, file?: File): FormData {
        const formData = new FormData();
        formData.append('name', vehicle.name);
        formData.append('type', vehicle.type);
        formData.append('plate', vehicle.plate);
        formData.append('description', vehicle.description);

        if (file) {
            formData.append('file', file);
        }

        if (vehicle.imageUrl) {
            formData.append('imageUrl', vehicle.imageUrl);
        }

        return formData;
    }

    create(vehicle: Omit<Vehicle, 'id'>, file?: File): Observable<Vehicle> {
        const formData = this.toFormData(vehicle, file);
        return this.http.post<Vehicle>(this.apiUrl, formData);
    }

    update(id: string, vehicle: Omit<Vehicle, 'id'>, file?: File): Observable<void> {
        const formData = this.toFormData(vehicle, file);
        return this.http.put<void>(`${this.apiUrl}/${id}`, formData);
    }

    delete(id: string): Observable<void> {
        return this.http.delete<void>(`${this.apiUrl}/${id}`);
    }
}
