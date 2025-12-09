import { provideHttpClient, withInterceptors } from '@angular/common/http';
import { jwtInterceptor } from './app/core/interceptors/jwt.interceptor';
import { ApplicationConfig, LOCALE_ID } from '@angular/core';
import { provideAnimationsAsync } from '@angular/platform-browser/animations/async';
import { provideRouter, withEnabledBlockingInitialNavigation, withInMemoryScrolling } from '@angular/router';
import { registerLocaleData } from '@angular/common';
import localeTr from '@angular/common/locales/tr';
import Aura from '@primeuix/themes/aura';
import { providePrimeNG } from 'primeng/config';
import { appRoutes } from './app.routes';
import { definePreset } from '@primeuix/themes';
import { MessageService } from 'primeng/api';

registerLocaleData(localeTr);

export const appConfig: ApplicationConfig = {
    providers: [
        provideRouter(appRoutes, withInMemoryScrolling({ anchorScrolling: 'enabled', scrollPositionRestoration: 'enabled' }), withEnabledBlockingInitialNavigation()),
        provideHttpClient(withInterceptors([jwtInterceptor])),
        provideAnimationsAsync(),
        { provide: LOCALE_ID, useValue: 'tr' },
        providePrimeNG({
            theme: {
                preset: definePreset(Aura, {
                    semantic: {
                        primary: {
                            50: '{blue.50}',
                            100: '{blue.100}',
                            200: '{blue.200}',
                            300: '{blue.300}',
                            400: '{blue.400}',
                            500: '{blue.500}',
                            600: '{blue.600}',
                            700: '{blue.700}',
                            800: '{blue.800}',
                            900: '{blue.900}',
                            950: '{blue.950}'
                        },
                        colorScheme: {
                            light: {
                                surface: {
                                    0: '#ffffff',
                                    50: '{zinc.50}',
                                    100: '{zinc.100}',
                                    200: '{zinc.200}',
                                    300: '{zinc.300}',
                                    400: '{zinc.400}',
                                    500: '{zinc.500}',
                                    600: '{zinc.600}',
                                    700: '{zinc.700}',
                                    800: '{zinc.800}',
                                    900: '{zinc.900}',
                                    950: '{zinc.950}'
                                }
                            },
                            dark: {
                                surface: {
                                    0: '#ffffff',
                                    50: '{zinc.50}',
                                    100: '{zinc.100}',
                                    200: '{zinc.200}',
                                    300: '{zinc.300}',
                                    400: '{zinc.400}',
                                    500: '{zinc.500}',
                                    600: '{zinc.600}',
                                    700: '{zinc.700}',
                                    800: '{zinc.800}',
                                    900: '{zinc.900}',
                                    950: '{zinc.950}'
                                }
                            }
                        }
                    }
                }),
                options: {
                    darkModeSelector: '.app-dark'
                }
            },
            translation: {
                firstDayOfWeek: 1,
                dayNames: ['Pazar', 'Pazartesi', 'Salı', 'Çarşamba', 'Perşembe', 'Cuma', 'Cumartesi'],
                dayNamesShort: ['Paz', 'Pzt', 'Sal', 'Çar', 'Per', 'Cum', 'Cmt'],
                dayNamesMin: ['Pz', 'Pt', 'Sa', 'Ça', 'Pe', 'Cu', 'Ct'],
                monthNames: ['Ocak', 'Şubat', 'Mart', 'Nisan', 'Mayıs', 'Haziran', 'Temmuz', 'Ağustos', 'Eylül', 'Ekim', 'Kasım', 'Aralık'],
                monthNamesShort: ['Oca', 'Şub', 'Mar', 'Nis', 'May', 'Haz', 'Tem', 'Ağu', 'Eyl', 'Eki', 'Kas', 'Ara'],
                today: 'Bugün',
                clear: 'Temizle'
            }
        }),
        MessageService
    ]
};
