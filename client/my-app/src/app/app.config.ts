import { ApplicationConfig, provideZonelessChangeDetection } from '@angular/core';
import { provideRouter } from '@angular/router';
import { routes } from './app.routes';
import { provideClientHydration, withEventReplay } from '@angular/platform-browser';
import { provideHttpClient, withFetch, withInterceptorsFromDi, HTTP_INTERCEPTORS } from '@angular/common/http'; // הוספנו HTTP_INTERCEPTORS
import { providePrimeNG } from 'primeng/config';
import { MessageService } from 'primeng/api';
import Aura from '@primeuix/themes/aura';
import { definePreset } from '@primeuix/themes';
import { provideAnimationsAsync } from '@angular/platform-browser/animations/async';
import { AuthInterceptor } from './services/auth.interceptor';

const MyPreset = definePreset(Aura, {
  semantic: {
    primary: {
      50: '{yellow.50}', 100: '{yellow.100}', 200: '{yellow.200}', 300: '{yellow.300}',
      400: '{yellow.400}', 500: '{yellow.500}', 600: '{yellow.600}', 700: '{yellow.700}',
      800: '{yellow.800}', 900: '{yellow.900}', 950: '{yellow.950}'
    }
  }
});

export const appConfig: ApplicationConfig = {
  providers: [
    provideHttpClient(
      withFetch(),
      withInterceptorsFromDi() // מאפשר שימוש ב-Interceptors מבוססי Class
    ),
    
    // רישום ה-Interceptor עם ה-Token המתאים
    {
      provide: HTTP_INTERCEPTORS,
      useClass: AuthInterceptor,
      multi: true // חשוב מאוד! מאפשר ריבוי אינטרספטורים
    },

    provideAnimationsAsync(),
    providePrimeNG({ theme: { preset: MyPreset } }),
    MessageService,
    provideRouter(routes),
    provideClientHydration(withEventReplay()),
    provideZonelessChangeDetection()
  ]
};