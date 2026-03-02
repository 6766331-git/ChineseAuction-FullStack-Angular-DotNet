import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { CommonModule, AsyncPipe, DatePipe } from '@angular/common';
import * as XLSX from 'xlsx'; 

import { PurchaseDto, PurchaseDetailDto } from '../../../models/purchase';
import { Purchase as PurchaseService } from '../../../services/purchase';

import { Observable } from 'rxjs';

import { ToastModule } from 'primeng/toast';
import { TableModule } from 'primeng/table';
import { ButtonModule } from 'primeng/button';
import { DialogModule } from 'primeng/dialog';
import { TagModule } from 'primeng/tag';
import { ProgressSpinnerModule } from 'primeng/progressspinner';

import { MessageService } from 'primeng/api';

@Component({
  selector: 'app-purchase',
  standalone: true,

  imports: [
    CommonModule,      // NgIf / NgFor
    AsyncPipe,         // async pipe
    DatePipe,          // date pipe

    ToastModule,
    TableModule,
    ButtonModule,
    DialogModule,
    TagModule,
    ProgressSpinnerModule
  ],

  templateUrl: './purchase.html',
  styleUrls: ['./purchase.sass'],  // ✅ שונה מ-styleUrl ל-styleUrls
  providers: [MessageService]
})
export class Purchase implements OnInit {

  purchases$!: Observable<PurchaseDto[]>;

  displayDialog = false;
  selectedBuyers: PurchaseDetailDto[] = [];

  loadingBuyers = false;
  loadingLottery = false;

  constructor(
    private purchaseService: PurchaseService,
    private messageService: MessageService,
    private cd: ChangeDetectorRef
  ) {}

  ngOnInit() {
    this.purchases$ = this.purchaseService.getAllPurchases();
  }

  onShowPurchaseDetails(p: PurchaseDto) {
    console.log(p);
    this.loadingBuyers = true;

    this.purchaseService.getBuyersByGiftId(p.id).subscribe({
      next: (details) => {
        this.selectedBuyers = details;
        console.log(details);

        // ✅ פתרון NG0100
        this.displayDialog = true;
        this.cd.detectChanges();

        this.loadingBuyers = false;
      },
      error: () => {
        this.loadingBuyers = false;

        this.messageService.add({
          severity: 'error',
          summary: 'Error',
          detail: 'Failed to load buyers'
        });
      }
    });
  }
generateRevenueReport() {
  this.purchases$.subscribe(purchases => {
    const revenueData = purchases.map(p => ({
      'Gift': p.giftName,
      'Tickets Sold': p.quantity,
      'Total Revenue': p.quantity * 10 // נניח מחיר קבוע או מחיר מהאובייקט
    }));

    const worksheet = XLSX.utils.json_to_sheet(revenueData);
    const workbook = XLSX.utils.book_new();
    XLSX.utils.book_append_sheet(workbook, worksheet, 'Revenue');
    XLSX.writeFile(workbook, 'Revenue_Report.xlsx');
  });
}
  // purchases.component.ts

generateWinnersReport() {
  this.purchaseService.getWinnersReport().subscribe({
    next: (data) => {
      // עיבוד הנתונים למבנה שטוח שמתאים לאקסל
      const reportData = data.map(item => ({
        'Gift Name': item.giftName,
        'Description': item.giftDescription,
        'Winner Name': item.user?.name || 'No winner',
        'Email': item.user?.userName || '-',
        'Phone': item.user?.phone || '-',
        'Purchase Date': item.purchaseDate ? new Date(item.purchaseDate).toLocaleDateString() : '-'
      }));

      // יצירת קובץ אקסל
      const worksheet = XLSX.utils.json_to_sheet(reportData);
      const workbook = XLSX.utils.book_new();
      XLSX.utils.book_append_sheet(workbook, worksheet, 'Winners');

      // הורדה למחשב
      XLSX.writeFile(workbook, 'Winners_Report.xlsx');
      
      this.messageService.add({severity:'success', summary: 'Success', detail: 'הדוח הופק בהצלחה'});
    },
    error: (err) => {
      this.messageService.add({severity:'error', summary: 'Error', detail: err.error || 'שגיאה בהפקת הדוח'});
    }
  });
}

  onDrawWinner(p: PurchaseDto) {
    this.loadingLottery = true;

    this.purchaseService.makeLottery(p.id).subscribe({
      next: (message) => {
        this.loadingLottery = false;

        this.messageService.add({
          severity: 'success',
          summary: 'Lottery Completed',
          detail: message
        });

        this.purchases$ = this.purchaseService.getAllPurchases();
      },
      error: (err) => {
        this.loadingLottery = false;

        this.messageService.add({
          severity: 'error',
          summary: 'Lottery Failed',
          detail: err.error
        });
      }
    });
  }
}
