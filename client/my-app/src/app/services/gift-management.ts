import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { HttpClient } from '@angular/common/http';
import { AddGiftDto, DonorDto, GiftDto, SearchGiftDto } from '../models/gift';
import { Product } from '../models/product';


//service
@Injectable({
  providedIn: 'root',
})
export class GiftManagement {
  AUTH_URL = 'http://localhost:5000/api/Gift';

  BASE_URL = 'http://localhost:5000/api/User';
  constructor(private http: HttpClient) { }


  getAllGifts(): Observable<GiftDto[]> {
    const token = localStorage.getItem('token');
    return this.http.get<GiftDto[]>
      (`${this.AUTH_URL}/GetAllGifts`,
        { headers: { Authorization: `Bearer ${token}` } })
  }

  deleteGift(giftId: number): Observable<string> {
    const token = localStorage.getItem('token');
    return this.http.delete(
      `${this.AUTH_URL}/DeleteGift/${giftId}`,
      {
        headers: { Authorization: `Bearer ${token}` },
        responseType: 'text'
      }
    );
  }

  addGift(gift: AddGiftDto): Observable<string> {
    const token = localStorage.getItem('token');
    return this.http.post(
      `${this.AUTH_URL}/AddGift`,
      gift,
      {
        headers: { Authorization: `Bearer ${token}` },
        responseType: 'text'
      }
    );
  }

  updateGift(gift: AddGiftDto, id: number): Observable<string> {
    const token = localStorage.getItem('token');
    return this.http.put(
      `${this.AUTH_URL}/UpdateGift/${id}`,
      gift,
      {
        headers: { Authorization: `Bearer ${token}` },
        responseType: 'text'
      }
    );
  }

  getDonorDetails(id: number): Observable<DonorDto> {
    const token = localStorage.getItem('token');
    return this.http.get<DonorDto>
      (`${this.AUTH_URL}/GetDonorByGiftId/${id}`,
        { headers: { Authorization: `Bearer ${token}` } })
  }


  updateGiftPicture(id: number, file: File): Observable<string> {
    const token = localStorage.getItem('token') ?? '';
    const formData = new FormData();
    formData.append('picture', file);

    return this.http.put(`${this.AUTH_URL}/AddPictureToGift/Picture/${id}`, formData, {
      headers: { Authorization: `Bearer ${token}` },
      responseType: 'text'
    });
  }

  searchGift(param: SearchGiftDto): Observable<GiftDto[] | []> {
    const token = localStorage.getItem('token');

    return this.http.get<GiftDto[] | []>(
      `${this.AUTH_URL}/SearchGifts`,
      {
        params: param as any,
        headers: { Authorization: `Bearer ${token}` }
      }
    );
  }


  getAllProducts(): Observable<Product[]> {
    const token = localStorage.getItem('token');
    return this.http.get<Product[]>
      (`${this.BASE_URL}/GetAllGifts`,
        { headers: { Authorization: `Bearer ${token}` } })
  }



  sortingProducts(param: string): Observable<Product[]> {
    const token = localStorage.getItem('token');
    return this.http.get<Product[]>(
      `${this.BASE_URL}/SortingGifts`,
      {
        params: { sortParam: param },
        headers: { Authorization: `Bearer ${token}` }
      }
    );
  }











}

