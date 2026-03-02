import { Injectable, NgZone } from '@angular/core';
import { HttpEvent, HttpHandler, HttpInterceptor, HttpRequest, HttpErrorResponse } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { Router } from '@angular/router';
import { AuthService } from './auth';
import { MessageService } from 'primeng/api'; // ייבוא חסר

@Injectable()
export class AuthInterceptor implements HttpInterceptor {
  constructor(
    private authService: AuthService,
    private router: Router,
    private ngZone: NgZone,
    private messageService: MessageService // הזרקה חסרה
  ) {}

  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    const headers = this.authService.getAuthHeaders();
    
    // יצירת הבקשה עם ה-Headers החדשים במידה וקיימים
    const authReq = req.clone({
      headers: headers
    });

    return next.handle(authReq).pipe(
      catchError((err: HttpErrorResponse) => {
        // טיפול בשגיאות הרשאה (401 - לא מורשה, 403 - אסור)
        if (err.status === 401 || err.status === 403) {
          this.authService.logout();
          
          this.ngZone.run(() => {
            this.messageService.add({ 
              severity: 'error', 
              summary: 'שגיאת התחברות', 
              detail: 'פג תוקף החיבור, אנא התחבר מחדש' 
            });

            this.router.navigateByUrl('/login', { replaceUrl: true });
          });
        }
        return throwError(() => err);
      })
    );
  }
}