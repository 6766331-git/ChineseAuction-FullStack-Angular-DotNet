import { Injectable } from '@angular/core';
import { HttpClient, HttpParams, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Donor } from '../models/donor';
import { Gift, GiftDto } from '../models/gift';

@Injectable({
  providedIn: 'root'
})
export class DonorService {

  private readonly apiUrl = 'http://localhost:5000/api/Donor';

  constructor(private http: HttpClient) {}

  private getAuthHeaders(): { headers: HttpHeaders } {
    const token = localStorage.getItem('token') || '';
    return { headers: new HttpHeaders({ Authorization: `Bearer ${token}` }) };
  }

  getAllDonors(): Observable<Donor[]> {
    return this.http.get<Donor[]>(`${this.apiUrl}/GetAllDonors`, this.getAuthHeaders());
  }

 addDonor(donor: Donor): Observable<string> {
  const token = localStorage.getItem('token') || '';
  const headers = { Authorization: `Bearer ${token}` };
  
  return this.http.post(`${this.apiUrl}/AddDonor`, donor, { 
    headers: headers,
    responseType: 'text' // <--- התיקון הקריטי
  });
}
deleteDonor(id: number): Observable<string> {
  console.log('Deleting donor ID:', id);
  const token = localStorage.getItem('token') || '';
  
  // יצירת האדרים
  const headers = { Authorization: `Bearer ${token}` };

  // הוספת responseType: 'text' כדי ש-Angular לא ינסה לפענח את התשובה כ-JSON
  return this.http.delete(`${this.apiUrl}/DeleteDonor/${id}`, { 
    headers: headers,
    responseType: 'text' 
  });
}

 updateDonor(id: number, donor: Donor): Observable<any> {
  return this.http.put(
    `${this.apiUrl}/UpdateDonor/${id}`,
    donor,
    {
      headers: this.getAuthHeaders().headers,
      responseType: 'text'  
    },
    
  );
}

  getDonorGifts(id: number): Observable<Gift[]> {
    return this.http.get<Gift[]>(`${this.apiUrl}/GetDonorGifts/${id}`, this.getAuthHeaders());
  }

  filterDonors(name?: string, email?: string, giftId?: number): Observable<Donor[]> {
  let params = new HttpParams();
  if (name) params = params.set('name', name);
  if (email) params = params.set('email', email);
  if (giftId) params = params.set('giftId', giftId);

  return this.http.get<Donor[]>(`${this.apiUrl}/FilterDonors`, { ...this.getAuthHeaders(), params });
}

  getAllGifts(): Observable<GiftDto[]> {  
    const token = localStorage.getItem('token');
    return this.http.get<GiftDto[]>
    (`${this.apiUrl}/GetAllGifts`,{
        headers:{ Authorization: `Bearer ${token}`}
      })


    }

}
