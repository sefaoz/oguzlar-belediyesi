import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable, tap } from 'rxjs';
import { ApiConfigService } from '../core/services/api-config.service';
import { Slider } from '../models/slider';

@Injectable({
    providedIn: 'root'
})
export class SliderService {
    private slidersSubject = new BehaviorSubject<Slider[]>([]);
    private readonly baseUrl: string;

    constructor(private readonly http: HttpClient, private readonly apiConfig: ApiConfigService) {
        this.baseUrl = this.apiConfig.buildEndpoint('sliders');
        this.loadSliders();
    }

    getSliders(): Observable<Slider[]> {
        return this.slidersSubject.asObservable();
    }

    private toFormData(slider: Slider, file?: File): FormData {
        const formData = new FormData();
        formData.append('title', slider.title || '');
        formData.append('description', slider.description || '');
        formData.append('link', slider.link || '');
        formData.append('order', String(slider.order || 0));
        formData.append('isActive', String(slider.isActive ?? true));

        if (file) {
            formData.append('file', file);
        }

        if (slider.imageUrl) {
            formData.append('imageUrl', slider.imageUrl);
        }

        return formData;
    }

    create(slider: Slider, file?: File): Observable<Slider> {
        const formData = this.toFormData(slider, file);
        return this.http.post<Slider>(this.baseUrl, formData).pipe(tap(() => this.loadSliders()));
    }

    update(id: string, slider: Slider, file?: File): Observable<void> {
        const formData = this.toFormData(slider, file);
        return this.http.put<void>(`${this.baseUrl}/${id}`, formData).pipe(tap(() => this.loadSliders()));
    }

    deleteSlider(id: string): Observable<void> {
        return this.http.delete<void>(`${this.baseUrl}/${id}`).pipe(tap(() => this.loadSliders()));
    }

    private loadSliders(): void {
        this.http.get<Slider[]>(this.baseUrl).subscribe((sliders) => this.slidersSubject.next(sliders));
    }
}
