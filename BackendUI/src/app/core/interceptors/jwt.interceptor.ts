import { HttpInterceptorFn } from '@angular/common/http';

/**
 * JWT Interceptor - Her HTTP isteğine otomatik olarak Authorization header'ı ekler
 */
export const jwtInterceptor: HttpInterceptorFn = (req, next) => {
    const token = localStorage.getItem('auth_token');

    // Token varsa ve istek login endpoint'ine değilse, Authorization header'ı ekle
    if (token && !req.url.includes('/auth/login')) {
        const clonedReq = req.clone({
            setHeaders: {
                Authorization: `Bearer ${token}`
            }
        });
        return next(clonedReq);
    }

    return next(req);
};
