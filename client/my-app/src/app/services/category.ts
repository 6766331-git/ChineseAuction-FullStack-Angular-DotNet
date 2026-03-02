import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { CategoryDto } from '../models/category';

@Injectable({
  providedIn: 'root',
})
export class CategoryService {
  private readonly AUTH_URL = 'http://localhost:5000/api/Category';

  constructor(private http: HttpClient) {}

  // פונקציית עזר לקבלת כותרות עם טוקן
  private getHeaders() {
    const token = localStorage.getItem('token');
    return new HttpHeaders({
      'Authorization': `Bearer ${token}`
    });
  }

         getAllCategories(): Observable<CategoryDto[]> {  
        const token = localStorage.getItem('token');
        return this.http.get<CategoryDto[]>
        (`${this.AUTH_URL}/GetAllCategories`,
          { headers:{ Authorization: `Bearer ${token}`}})
        }

  // category.service.ts
addCategory(category: CategoryDto): Observable<any> {
  const token = localStorage.getItem('token');
  return this.http.post(`${this.AUTH_URL}/AddCategory`, category, {
    headers: { Authorization: `Bearer ${token}` },
    responseType: 'text' // חשוב מאוד! אומר לאנגולר לא לצפות ל-JSON
  });
}
// DELETE category by id
deleteCategory(id: number): Observable<string> {
  const token = localStorage.getItem('token');
  return this.http.delete(`${this.AUTH_URL}/DeleteCategory/${id}`, {
    headers: { Authorization: `Bearer ${token}` },
    responseType: 'text'
  });
}

// UPDATE category
updateCategory(dto: CategoryDto): Observable<string> {
  const token = localStorage.getItem('token');
  return this.http.put(`${this.AUTH_URL}/UpdateCategory`, dto, {
    headers: { Authorization: `Bearer ${token}` },
    responseType: 'text'
  });
}

  }
