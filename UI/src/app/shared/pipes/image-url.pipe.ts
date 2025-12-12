import { Pipe, PipeTransform } from '@angular/core';
import { environment } from '../../../environments/environment';

@Pipe({
    name: 'imageUrl',
    standalone: true
})
export class ImageUrlPipe implements PipeTransform {
    transform(url: string | null | undefined): string {
        if (!url) return '';
        if (url.startsWith('http')) return url;



        return `${environment.imageBaseUrl}${url}`;
    }
}
