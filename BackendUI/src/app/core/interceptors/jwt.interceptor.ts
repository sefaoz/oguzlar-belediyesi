import { HttpInterceptorFn, HttpErrorResponse } from '@angular/common/http';
import { inject } from '@angular/core';
import { Router } from '@angular/router';
import { catchError, throwError, switchMap } from 'rxjs';
import { AuthService } from '../services/auth.service';

/**
 * JWT Interceptor - Her HTTP isteğine otomatik olarak Authorization header'ı ekler
 */
export const jwtInterceptor: HttpInterceptorFn = (req, next) => {
    const authService = inject(AuthService);
    const router = inject(Router);
    const token = localStorage.getItem('auth_token');

    let request = req;

    // Token varsa ve istek login endpoint'ine değilse, Authorization header'ı ekle
    if (token && !req.url.includes('/auth/login')) {
        request = req.clone({
            setHeaders: {
                Authorization: `Bearer ${token}`
            }
        });
    }

    return next(request).pipe(
        catchError((error: HttpErrorResponse) => {
            if (error.status === 401 && !req.url.includes('/auth/login') && !req.url.includes('/auth/refresh-token')) {
                return authService.refreshToken().pipe(
                    switchMap((response) => {
                        const newToken = response.token;
                        const clonedReq = req.clone({
                            setHeaders: {
                                Authorization: `Bearer ${newToken}`
                            }
                        });
                        return next(clonedReq);
                    }),
                    catchError((err) => {
                        authService.logout();
                        return throwError(() => err);
                    })
                );
            }
            return throwError(() => error);
        })
    );
};
