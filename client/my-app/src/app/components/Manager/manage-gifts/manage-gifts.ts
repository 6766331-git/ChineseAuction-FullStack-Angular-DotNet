import { Component, ElementRef, inject, ViewChild } from '@angular/core';
import { GiftManagement } from '../../../services/gift-management'
import { DonorDto, GiftDto, SearchGiftDto } from '../../../models/gift';
import { MessageService } from 'primeng/api';
import { CommonModule } from '@angular/common';
import { CardModule } from 'primeng/card';
import { TagModule } from 'primeng/tag';
import { MessageModule } from 'primeng/message';
import { ToastModule } from 'primeng/toast';
import { InputTextModule } from 'primeng/inputtext';
import { ButtonModule } from 'primeng/button';
import { Router } from '@angular/router';
import { ConfirmDialogModule } from 'primeng/confirmdialog';
import { ConfirmationService, ConfirmEventType } from 'primeng/api';
import { DialogModule } from 'primeng/dialog';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { ReactiveFormsModule } from '@angular/forms';
import { GiftFormComponent } from '../gift-form/gift-form';
import { emptyMessage } from '@primeuix/themes/aura/autocomplete';
import { FormsModule } from '@angular/forms';
import { ChangeDetectorRef } from '@angular/core';
//component
@Component({
  selector: 'app-manage-gifts',
  imports: [
    CommonModule,
    CardModule,
    TagModule,
    MessageModule,
    ToastModule,
    InputTextModule,
    ButtonModule,
    ConfirmDialogModule,
    DialogModule,
    ReactiveFormsModule,
    GiftFormComponent,
    FormsModule],
  providers: [MessageService, ConfirmationService],
  templateUrl: './manage-gifts.html',
  styleUrl: './manage-gifts.sass',
})
export class ManageGifts {
  private giftServ = inject(GiftManagement)
  private router = inject(Router);
  private confirmationService = inject(ConfirmationService);
  selectedGift: GiftDto | null = null;
  gifts: GiftDto[] | null = null;
  donor: DonorDto | null = null
  private msg = inject(MessageService);
  showAddGiftForm: boolean = false;
  addGiftForm!: FormGroup;
  constructor(private cdr: ChangeDetectorRef) { }

  ngOnInit() {
    this.giftServ.getAllGifts().subscribe({
      
      next: (g) => {
console.log('All gifts from server:', g); // הדפסה של כל המערך
        this.gifts = g;

        if (this.gifts.length == 0)
        {
          this.isRealyEmty=true
          //  this.msg.add({ severity: 'success', summary: 'Success', detail: 'you dont have gift yet...' });
        }
        else
        {
          this.isRealyEmty=false
          // this.msg.add({ severity: 'success', summary: 'Success', detail: 'gifts loaded successfully!' });
        }
      },
      error: (err) => {
        const serverMsg =
          err?.error?.message ??
          (typeof err?.error === 'string' ? err.error : null) ??
          'cannot gettt gifts';

        console.log('STATUS:', err.status);
        console.log('ERROR BODY:', err.error);
        console.log('MESSAGE:', err.message);
        this.gifts = []
        if(!this.gifts)
        this.msg.add({ severity: 'error', summary: 'Error', detail: serverMsg });
        if (err.status == 401)
          this.router.navigateByUrl('/login');

      }
    });
  }

  onDeleteGift(id: number) {
    this.giftServ.deleteGift(id).subscribe({
      next: (msg) => {
        this.msg.add({ severity: 'success', summary: 'Success', detail: msg });
        this.giftServ.getAllGifts().subscribe(g => {
          this.gifts = g;
          if(this.gifts.length==0)
            this.isRealyEmty=true;
        });
      },
      error: (err) => {
        const serverMsg =
          err?.error?.message ??
          (typeof err?.error === 'string' ? err.error : null) ??
          'cannot delete gift';
        this.msg.add({ severity: 'error', summary: 'Error', detail: serverMsg });
        if (err.status == 401)
          this.router.navigateByUrl('/login');

      }
    });
  }

  onAddGift() {
    this.confirmationService.confirm({
      message: 'You must make sure that the gifts donor exists in the system',
      header: 'Before adding a gift:',
      icon: 'pi pi-info-circle',
      rejectButtonProps: {
        label: 'Adding a donor',
      },
      acceptButtonProps: {
        label: 'Ok',
      },
      accept: () => {
        this.selectedGift=null
        this.showAddGiftForm = true
      },
      reject: (type: any) => {
  if (type === ConfirmEventType.REJECT) {
    this.router.navigateByUrl('/manager/donors');
  }
}
      
    });

  }

  onEditGift(g: GiftDto) {
    this.selectedGift = g;//@input
    this.showAddGiftForm = true;
  }
  onSaved() {//@output-event
    this.showAddGiftForm = false;
    this.selectedGift==null;
    this.giftServ.getAllGifts().subscribe(g => this.gifts = g);
  }


  onGetDonorDetails(id: number) {
    this.giftServ.getDonorDetails(id).subscribe({
      next: (d) => {
        this.donor = d;
        this.msg.add({
          severity: 'info',
          summary: 'Donor',
          detail: `Name:${this.donor.name} \n
        Email:${this.donor.email}`,
          life: 5000
        });

      },
      error: (err) => {
        const serverMsg =
          err?.error?.message ??
          (typeof err?.error === 'string' ? err.error : null) ??
          'cannot get donor';
        this.donor = null
        this.msg.add({ severity: 'error', summary: 'Error', detail: serverMsg });
        if (err.status == 401)
          this.router.navigateByUrl('/login');

      }
    });
  }

  onUploadPictureClick(giftId: number, input: HTMLInputElement) {
    input.click();
  }

  onFileChosen(event: Event, giftId: number, input: HTMLInputElement) {
    const file = (event.target as HTMLInputElement).files?.[0];
    if (!file) return;

    this.giftServ.updateGiftPicture(giftId, file).subscribe({
      next: (msg) => {
        this.msg.add({ severity: 'success', summary: 'Success', detail: msg, life: 2500 });
        input.value = '';
        this.giftServ.getAllGifts().subscribe(g => this.gifts = g);
      },
      error: (err) => {
        const serverMsg =
          err?.error?.message ??
          (typeof err?.error === 'string' ? err.error : null) ??
          'Upload failed';

        this.msg.add({ severity: 'error', summary: 'Error', detail: serverMsg, life: 3500 });
      }
    });
  }


  search: SearchGiftDto = {
    name: '',
    donorName: '',
    numOfTickets: undefined
  };
  isRealyEmty:boolean=false;
  onSearch() {
    const name = this.search.name?.trim();
    const donorName = this.search.donorName?.trim();
    const num = this.search.numOfTickets;

    const dto: any = {};
    if (name) dto.name = name;
    if (donorName) dto.donorName = donorName;
    if (num !== null && num !== undefined && num !== ('' as any)) dto.numOfTickets = Number(num);

    if (Object.keys(dto).length === 0) {
      this.msg.add({ severity: 'info', summary: 'Info', detail: 'Fill at least one field' });
      return;
    }

    this.giftServ.searchGift(dto).subscribe({
      next: (g) => {
        this.gifts=g??[]
        if(g.length == 0)
        {
          this.msg.add({ severity: 'info', summary: 'info', detail: 'no gift found' });
          this.isRealyEmty=false;
        }
        this.cdr.detectChanges();
      },
      error: (err) => {
        this.gifts=[]
        const serverMsg =
           err?.error?.message ??
          (typeof err?.error === 'string' ? err.error : null) ??
          'cannot search gift';
          this.msg.add({ severity: 'error', summary: 'Error', detail: serverMsg });
        this.cdr.detectChanges();

        if (err?.status === 401) this.router.navigateByUrl('/login');


      }
    });
  }

  onClear() {
    this.search = { name: '', donorName: '', numOfTickets: undefined };
    this.giftServ.getAllGifts().subscribe(g => {
      this.gifts = g;
      this.cdr.detectChanges();
    });
  }




}
