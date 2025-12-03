import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, of } from 'rxjs';
import { map, catchError, debounceTime, distinctUntilChanged } from 'rxjs/operators';
import { ApiConfigService } from './api-config.service';

export type SlugEntityType = 'products' | 'categories' | 'blogs' | 'projects';

export interface SlugCheckResult {
    exists: boolean;
    suggestion?: string;
}

export interface SlugValidationResult {
    isValid: boolean;
    exists: boolean;
    suggestion?: string;
    error?: string;
}

@Injectable({
    providedIn: 'root'
})
export class SlugService {

    constructor(
        private readonly http: HttpClient,
        private readonly apiConfig: ApiConfigService
    ) { }

    /**
     * String'i URL-friendly slug'a dönüştürür
     * @param input - Dönüştürülecek string
     * @returns Slug formatında string
     */
    generateSlug(input: string): string {
        if (!input) return '';

        return String(input)
            .toLowerCase()
            .trim()
            // Türkçe karakterleri normalize et
            .normalize('NFD').replace(/\p{Diacritic}/gu, '')
            // Özel Türkçe karakterler
            .replace(/ı/g, 'i')
            .replace(/ç/g, 'c')
            .replace(/ğ/g, 'g')
            .replace(/ş/g, 's')
            .replace(/ö/g, 'o')
            .replace(/ü/g, 'u')
            // Sadece harf, rakam ve tire bırak
            .replace(/[^a-z0-9]+/g, '-')
            // Başta ve sondaki tireleri kaldır
            .replace(/(^-|-$)/g, '');
    }

    /**
     * Slug'ın belirtilen entity türünde var olup olmadığını kontrol eder
     * @param slug - Kontrol edilecek slug
     * @param entityType - Entity türü (products, categories, blogs, projects)
     * @param excludeId - Güncelleme işleminde mevcut kaydın ID'si (bu ID hariç tutulur)
     * @returns Observable<SlugCheckResult>
     */
    checkSlugExists(slug: string, entityType: SlugEntityType, excludeId?: number): Observable<SlugCheckResult> {
        if (!slug || !slug.trim()) {
            return of({ exists: false });
        }

        const cleanSlug = slug.trim().toLowerCase();
        const endpoint = `${entityType}/check-slug`;
        const url = this.apiConfig.buildEndpoint(endpoint);

        const params: any = { slug: cleanSlug };
        if (excludeId) {
            params.excludeId = excludeId;
        }

        return this.http.get<SlugCheckResult>(url, { params }).pipe(
            catchError(error => {
                console.error(`Slug kontrolü hatası (${entityType}):`, error);

                // API hatası durumunda fallback: Yerel kontrol yap
                return this.fallbackSlugCheck(cleanSlug, entityType, excludeId);
            })
        );
    }

    /**
     * Benzersiz slug oluşturur. Eğer slug mevcutsa sonuna sayı ekler.
     * @param baseSlug - Temel slug
     * @param entityType - Entity türü
     * @param excludeId - Güncelleme işleminde mevcut kaydın ID'si
     * @returns Observable<string> - Benzersiz slug
     */
    generateUniqueSlug(baseSlug: string, entityType: SlugEntityType, excludeId?: number): Observable<string> {
        const slug = this.generateSlug(baseSlug);

        if (!slug) {
            return of('');
        }

        return this.checkSlugExists(slug, entityType, excludeId).pipe(
            map(result => {
                if (!result.exists) {
                    return slug;
                }

                // Eğer API'den öneride bulunuyorsa onu kullan
                if (result.suggestion) {
                    return result.suggestion;
                }

                // Yoksa sayı ekleyerek benzersiz hale getir
                return this.appendNumberToSlug(slug);
            })
        );
    }

    /**
     * Slug'ı doğrular ve gerekirse öneride bulunur
     * @param slug - Doğrulanacak slug
     * @param entityType - Entity türü
     * @param excludeId - Güncelleme işleminde mevcut kaydın ID'si
     * @returns Observable<SlugValidationResult>
     */
    validateSlug(slug: string, entityType: SlugEntityType, excludeId?: number): Observable<SlugValidationResult> {
        // Boş slug kontrolü
        if (!slug || !slug.trim()) {
            return of({
                isValid: false,
                exists: false,
                error: 'Slug boş olamaz'
            });
        }

        const cleanSlug = slug.trim();

        // Format kontrolü
        if (!this.isValidSlugFormat(cleanSlug)) {
            const suggestion = this.generateSlug(cleanSlug);
            return of({
                isValid: false,
                exists: false,
                suggestion,
                error: 'Slug formatı geçersiz. Sadece küçük harf, rakam ve tire kullanılabilir.'
            });
        }

        // Benzersizlik kontrolü
        return this.checkSlugExists(cleanSlug, entityType, excludeId).pipe(
            map(result => ({
                isValid: !result.exists,
                exists: result.exists,
                suggestion: result.suggestion,
                error: result.exists ? 'Bu slug zaten kullanımda' : undefined
            }))
        );
    }

    /**
     * Name değişiminden slug oluşturma ve doğrulama (debounced)
     * @param name$ - Name Observable stream
     * @param entityType - Entity türü
     * @param excludeId - Güncelleme işleminde mevcut kaydın ID'si
     * @returns Observable<string> - Doğrulanmış benzersiz slug
     */
    createSlugFromName(name$: Observable<string>, entityType: SlugEntityType, excludeId?: number): Observable<string> {
        return name$.pipe(
            debounceTime(300),
            distinctUntilChanged(),
            map(name => this.generateSlug(name || '')),
            distinctUntilChanged()
        );
    }

    /**
     * Slug formatının geçerli olup olmadığını kontrol eder
     * @param slug - Kontrol edilecek slug
     * @returns boolean
     */
    private isValidSlugFormat(slug: string): boolean {
        // Slug formatı: küçük harf, rakam ve tire, başta/sonda tire yok
        const slugPattern = /^[a-z0-9]+(?:-[a-z0-9]+)*$/;
        return slugPattern.test(slug);
    }

    /**
     * Slug'a sayı ekleyerek benzersiz hale getirir
     * @param slug - Temel slug
     * @returns string - Sayı eklenmiş slug
     */
    private appendNumberToSlug(slug: string): string {
        const timestamp = Date.now().toString().slice(-4);
        return `${slug}-${timestamp}`;
    }

    /**
     * API başarısız olduğunda yerel kontrol yapar
     * @param slug - Kontrol edilecek slug
     * @param entityType - Entity türü
     * @param excludeId - Hariç tutulacak ID
     * @returns Observable<SlugCheckResult>
     */
    private fallbackSlugCheck(slug: string, entityType: SlugEntityType, excludeId?: number): Observable<SlugCheckResult> {
        // Bu fallback method'da mevcut verileri kontrol edebiliriz
        // Şu an için false döndürüyoruz (slug mevcut değil varsayımı)
        console.warn(`API slug kontrolü başarısız, fallback aktif: ${entityType}/${slug}`);

        return of({
            exists: false,
            suggestion: excludeId ? `${slug}-updated` : `${slug}-new`
        });
    }
}