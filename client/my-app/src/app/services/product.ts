import { Injectable, inject, PLATFORM_ID } from '@angular/core';
import { HttpClient, HttpParams, HttpHeaders, HttpErrorResponse } from '@angular/common/http';
import { Observable, Subject, throwError, EMPTY } from 'rxjs';
import { tap, catchError } from 'rxjs/operators';
import { Product } from '../models/product';
import { isPlatformBrowser } from '@angular/common';
import { of } from 'rxjs';

@Injectable({ providedIn: 'root' })
export class ProductService {
  private http = inject(HttpClient);
  private platformId = inject(PLATFORM_ID);

  private readonly adminUrl = 'http://localhost:5000/api/Gift';
  private readonly userUrl = 'http://localhost:5000/api/User';

  private _cartUpdated$ = new Subject<void>();
  cartUpdated$ = this._cartUpdated$.asObservable();

  private getHeaders(): HttpHeaders {
    let token = '';
    if (isPlatformBrowser(this.platformId)) {
      token = localStorage.getItem('token') || '';
    }
    return new HttpHeaders({ 
      'Authorization': `Bearer ${token}`,
      'Content-Type': 'application/json'
    });
  }

  private handleError(error: HttpErrorResponse): Observable<never> {
    let errorMessage = 'An error occurred';
    
    if (error.error instanceof ErrorEvent) {
      // Client-side error
      errorMessage = `Error: ${error.error.message}`;
    } else {
      // Server-side error
      errorMessage = error.error || `Error Code: ${error.status}\nMessage: ${error.message}`;
    }
    
    console.error(errorMessage);
    return throwError(() => error);
  }

  // --- ניהול מתנות (Admin/General) ---

  // getAllGifts(): Observable<any[]> {
  //   return this.http.get<any[]>(`${this.adminUrl}/GetAllGifts`, { 
  //     headers: this.getHeaders() 
  //   }).pipe(
  //     catchError(this.handleError)
  //   );
  // }

  // deleteGift(id: string | number): Observable<any> {
  //   return this.http.delete(`${this.adminUrl}/DeleteGift/${id}`, { 
  //     headers: this.getHeaders() 
  //   }).pipe(
  //     catchError(this.handleError)
  //   );
  // }

  searchGifts(params: any): Observable<any[]> {
    return this.http.get<any[]>(`${this.adminUrl}/Search`, { 
      params, 
      headers: this.getHeaders() 
    }).pipe(
      catchError(this.handleError)
    );
  }

  // --- פעולות משתמש (User) ---

  getAllGiftsUser(): Observable<Product[]> {
    return this.http.get<Product[]>(`${this.userUrl}/GetAllGifts`).pipe(
      catchError(this.handleError)
    );
  }

  getSortedGifts(sortParam: string): Observable<Product[]> {
    const params = new HttpParams().set('sortParam', sortParam);
    return this.http.get<Product[]>(`${this.userUrl}/SortingGifts`, { params }).pipe(
      catchError(this.handleError)
    );
  }

  // --- עגלת קניות ---

  addGiftToCart(giftId: number): Observable<string> {
    if (!giftId) {
      console.error('Invalid giftId');
      return EMPTY;
    }

    const params = new HttpParams().set('giftId', giftId.toString());
    return this.http.post(`${this.userUrl}/AddGiftToCart`, null, { 
      params, 
      headers: this.getHeaders(), 
      responseType: 'text' 
    }).pipe(
      tap(() => this._cartUpdated$.next()),
      catchError(this.handleError)
    );
  }

  incrementQuantity(giftId: number): Observable<string> {
    return this.addGiftToCart(giftId);
  }

  decreaseQuantity(giftId: number): Observable<string> {
    if (!giftId) {
      console.error('Invalid giftId');
      return EMPTY;
    }

    const params = new HttpParams().set('giftId', giftId.toString());
    return this.http.put(`${this.userUrl}/UpdateAmountItemInCartAsync`, null, { 
      params, 
      headers: this.getHeaders(), 
      responseType: 'text' 
    }).pipe(
      tap(() => this._cartUpdated$.next()),
      catchError(this.handleError)
    );
  }

  deleteFromCart(giftId: number | string): Observable<string> {
    if (!giftId) {
      console.error('deleteFromCart was called with undefined ID');
      return EMPTY;
    }
    
    const params = new HttpParams().set('giftId', giftId.toString());
    return this.http.delete(`${this.userUrl}/RemoveGiftFromCart`, { 
      params, 
      headers: this.getHeaders(), 
      responseType: 'text' 
    }).pipe(
      tap(() => this._cartUpdated$.next()),
      catchError(this.handleError)
    );
  }

getCartItems(): Observable<any[]> {
  return this.http.get<any[]>(`${this.userUrl}/GetCartItems`, { 
    headers: this.getHeaders() 
  }).pipe(
    catchError((error: HttpErrorResponse) => {
      if (error.status === 400 || error.status === 404) {
        console.warn('Cart empty or not found, returning empty array');
        return of([]); 
      }
      return this.handleError(error);
    })
  );
}


  checkout(): Observable<string> {
    return this.http.post(`${this.userUrl}/Checkout`, null, { 
      headers: this.getHeaders(), 
      responseType: 'text' 
    }).pipe(
      tap(() => this._cartUpdated$.next()),
      catchError(this.handleError)
    );
  }
}