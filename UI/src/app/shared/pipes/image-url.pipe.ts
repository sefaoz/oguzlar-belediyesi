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

        // Remove leading slash if present in url to avoid double slashes with base url
        // assuming imageBaseUrl doesn't end with slash, or url starts with one.
        // However, looking at environment.ts: const imageBaseUrl = 'http://localhost:5002';
        // It doesn't have a trailing slash.
        // If url comes as "/images/foo.jpg", we want "http://localhost:5002/images/foo.jpg".

        return `${environment.imageBaseUrl}${url}`;
    }
}
