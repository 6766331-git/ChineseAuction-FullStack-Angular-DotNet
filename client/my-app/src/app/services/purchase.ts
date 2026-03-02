import { Injectable } from '@angular/core';
import { HttpClient, HttpParams, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Donor } from '../models/donor';
import { PurchaseDto , PurchaseDetailDto, WinnersGiftsReportDto} from '../models/purchase';

@Injectable({
  providedIn: 'root',
})
export class Purchase {
  
private readonly apiUrl = 'http://localhost:5000/api/Purchase';

  constructor(private http: HttpClient) {}
   private getAuthHeaders(): { headers: HttpHeaders } {
    const token = localStorage.getItem('token') || '';
    return { headers: new HttpHeaders({ Authorization: `Bearer ${token}` }) };
  }


   getAllPurchases(): Observable<PurchaseDto[]> {
    return this.http.get<PurchaseDto[]>(`${this.apiUrl}/GetAllPurchases`, this.getAuthHeaders());
  }
  
          // הוספת הפונקציה לתוך ה-Service הקיים
getBuyersByGiftId(giftId: number): Observable<PurchaseDetailDto[]> {
  return this.http.get<PurchaseDetailDto[]>(
    `${this.apiUrl}/BuyersDetails/${giftId}`, 
    this.getAuthHeaders()
  );
}

makeLottery(giftId: number): Observable<string> {
  // אנחנו יוצרים אובייקט אפשרויות שמשלב גם את ה-Headers וגם את ה-responseType
  const options = {
    headers: this.getAuthHeaders().headers,
    responseType: 'text' as 'json' // טריק קטן של TypeScript כדי שיסכים לקבל טקסט כ-Observable
  };

  return this.http.post(`${this.apiUrl}/MakeLottery/${giftId}`, {}, options) as Observable<string>;
}

// purchase.service.ts

getWinnersReport(): Observable<WinnersGiftsReportDto[]> {
  return this.http.get<WinnersGiftsReportDto[]>(
    `${this.apiUrl}/WinnersGiftsReports`, 
    this.getAuthHeaders()
  ); // סגירת הסוגריים של ה-get
} // סגירת הפונקציה

}