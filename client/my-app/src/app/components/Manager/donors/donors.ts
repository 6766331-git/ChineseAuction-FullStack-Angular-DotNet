import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { DonorService } from '../../../services/donor';
import { Donor } from '../../../models/donor';
import { Gift } from '../../../models/gift';
import { CommonModule } from '@angular/common';
import { HttpClientModule } from '@angular/common/http';
import { Observable, BehaviorSubject, switchMap, catchError, of } from 'rxjs';
import {GiftManagement} from '../../../services/gift-management'

// PrimeNG Imports
import { ToastModule } from 'primeng/toast';
import { ConfirmDialogModule } from 'primeng/confirmdialog';
import { MessageService, ConfirmationService } from 'primeng/api';

@Component({
  selector: 'app-donors',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    HttpClientModule,
    ToastModule,
    ConfirmDialogModule,
    
  ],
  providers: [MessageService, ConfirmationService],
  templateUrl: './donors.html',
  styleUrls: ['./donors.sass']
})
export class Donors implements OnInit {

  donors$!: Observable<Donor[]>;
  private refresh$ = new BehaviorSubject<void>(undefined);
  gifts$!: Observable<Gift[]>;
  donorForm!: FormGroup;
  filterForm!: FormGroup;
  giftsList: Gift[] = [];


  // UI States
  selectedDonorId?: number;
  showModal = false;
  isGiftsOpen = false;
  selectedDonorName = '';

  constructor(
    private donorService: DonorService,
    private fb: FormBuilder,
    private messageService: MessageService,
    private confirmationService: ConfirmationService,
    private cdr: ChangeDetectorRef,
      private giftManagementServer: GiftManagement,  

  ) {}

 ngOnInit(): void {
  this.initForm();
  this.initFilterForm();

  this.giftManagementServer.getAllGifts().subscribe(g => {
    this.giftsList = g;
  });


  this.donors$ = this.refresh$.pipe(
  switchMap(() => {
    const name = (this.filterForm.get('name')?.value || '').trim();
    const email = (this.filterForm.get('email')?.value || '').trim();
    const giftId = this.filterForm.get('giftId')?.value;

    const request = (name || email || giftId)
      ? this.donorService.filterDonors(name, email, giftId)
      : this.donorService.getAllDonors();

    return request.pipe(
      catchError(err => {
        if (err.status !== 404) {
          this.showToast('Failed to load donors', true);
        }
        return of([]);
      })
    );
  })
);
}

  private initForm(): void {
    this.donorForm = this.fb.group({
      name: ['', [Validators.required, Validators.minLength(2)]],

      // ✅ EMAIL VALIDATION (already correct)
      email: ['', [Validators.required, Validators.email]]
    });
  }

 private initFilterForm(): void {
  this.filterForm = this.fb.group({
    name: [''],
    email: [''],
    giftId: [null]  
  });
}

  private showToast(message: string, isError: boolean = false): void {
    this.messageService.clear();
    this.messageService.add({
      key: 'donorToast',
      severity: isError ? 'error' : 'success',
      summary: isError ? 'Error' : 'Success',
      detail: message,
      life: 3000
    });

    this.cdr.detectChanges();
  }

  filterDonors(): void {
    this.refresh$.next();
  }


// איפוס הסינון
resetFilter(): void {
  this.filterForm.reset({
    name: '',
    email: '',
    giftId: null // חשוב לאפס ל-null מפורש
  });
  this.refresh$.next();
}
 

  submit(): void {
    if (this.donorForm.invalid) {
      this.donorForm.markAllAsTouched();
      return;
    }

    const donor: Donor = this.donorForm.value;
    const isUpdate = !!this.selectedDonorId;

    const request = isUpdate
      ? this.donorService.updateDonor(this.selectedDonorId!, donor)
      : this.donorService.addDonor(donor);

    request.subscribe({
      next: () => {
        this.showToast(
          isUpdate
            ? 'Donor updated successfully!'
            : 'Donor added successfully!'
        );

        this.closeModal();
        this.refresh$.next();
      },
      error: (err) => {
        const msg =
          err.status === 409
            ? 'Email address already exists'
            : 'Operation failed';

        this.showToast(msg, true);
      }
    });
  }

  deleteDonor(id: number): void {
    this.confirmationService.confirm({
      message: 'Are you sure you want to delete this donor?',
      header: 'Delete Confirmation',
      icon: 'pi pi-exclamation-triangle',
      acceptLabel: 'Yes',
      rejectLabel: 'No',
      acceptButtonStyleClass: 'p-button-danger',

      accept: () => {
        this.donorService.deleteDonor(id).subscribe({
          next: () => {
            this.showToast('Donor deleted successfully');
            this.refresh$.next();
            this.cdr.detectChanges();
          },
          error: () => this.showToast('Delete operation failed', true)
        });
      }
    });
  }

  loadGifts(id: number, name: string): void {
    this.selectedDonorName = name;
    this.isGiftsOpen = true;
    this.gifts$ = this.donorService.getDonorGifts(id);
  }

  closeGifts(): void {
    this.isGiftsOpen = false;
  }

  openNewDonorModal(): void {
    this.selectedDonorId = undefined;
    this.donorForm.reset();
    this.showModal = true;
  }

  editDonor(donor: Donor): void {
    this.selectedDonorId = donor.id;

    this.donorForm.patchValue({
      name: donor.name,
      email: donor.email
    });

    this.showModal = true;
  }

  closeModal(): void {
    this.showModal = false;
    this.donorForm.reset();
    this.selectedDonorId = undefined;
    this.cdr.detectChanges();
  }
}
