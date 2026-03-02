import { Injectable, Inject, PLATFORM_ID } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { tap } from 'rxjs/operators';
import { isPlatformBrowser } from '@angular/common';
import { BuyerDetailDto, LoginDto, LoginResponseDto, UserStatus } from '../models/user';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  AUTH_URL = 'http://localhost:5000/api/Auth';

  constructor(
    private http: HttpClient,
    @Inject(PLATFORM_ID) private platformId: Object
  ) {}

  registerUser(dto: any): Observable<BuyerDetailDto> {
    const body = {
      UserName: dto.email,
      Name: dto.name,
      Password: dto.passwordHash,
      Phone: dto.phone,
    };
    return this.http.post<BuyerDetailDto>(`${this.AUTH_URL}/Register`, body);
  }

  loginUser(dto: LoginDto): Observable<LoginResponseDto | { message: string }> {
    return this.http.post<LoginResponseDto | { message: string }>(`${this.AUTH_URL}/Login`, dto)
      .pipe(
        tap(res => {
          if (isPlatformBrowser(this.platformId) && res && 'token' in res && res.token) {
            localStorage.setItem('token', res.token);
            localStorage.setItem('userRole', res.role?.toString() ?? '0');
            localStorage.setItem('userName', res.user.userName);
          }
        })
      );
  }

  logout(): void {
    if (isPlatformBrowser(this.platformId)) {
      localStorage.clear();
    }
  }

  getUserRole(): number | null {
    if (isPlatformBrowser(this.platformId)) {
      const role = localStorage.getItem('userRole');
      return role ? +role : null;
    }
    return null;
  }

  isAdmin(): boolean {
    return this.getUserRole() === UserStatus.admin;
  }

  isLoggedIn(): boolean {
    if (isPlatformBrowser(this.platformId)) {
      return !!localStorage.getItem('token');
    }
    return false;
  }

  getAuthHeaders(): HttpHeaders {
    let headers = new HttpHeaders();
    if (isPlatformBrowser(this.platformId)) {
      const token = localStorage.getItem('token');
      if (token) {
        headers = headers.set('Authorization', `Bearer ${token}`);
      }
    }
    return headers;
  }
}
