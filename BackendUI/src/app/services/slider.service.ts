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

    saveSlider(slider: Slider): Observable<Slider> {
        if (slider.id) {
            return this.http.put<Slider>(`${this.baseUrl}/${slider.id}`, slider).pipe(tap(() => this.loadSliders()));
        }

        return this.http.post<Slider>(this.baseUrl, slider).pipe(tap(() => this.loadSliders()));
    }

    deleteSlider(id: string): Observable<void> {
        return this.http.delete<void>(`${this.baseUrl}/${id}`).pipe(tap(() => this.loadSliders()));
    }

    private loadSliders(): void {
        this.http.get<Slider[]>(this.baseUrl).subscribe((sliders) => this.slidersSubject.next(sliders));
    }
}
