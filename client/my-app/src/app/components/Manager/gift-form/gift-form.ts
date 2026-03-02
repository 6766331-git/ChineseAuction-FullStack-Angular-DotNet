import { CommonModule } from '@angular/common';
import { Component, EventEmitter, Input, OnChanges, OnInit, Output, inject } from '@angular/core';
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { Observable } from 'rxjs';

// PrimeNG
import { SelectModule } from 'primeng/select';
import { InputTextModule } from 'primeng/inputtext';
import { ButtonModule } from 'primeng/button';
import { MessageService } from 'primeng/api';
import { AddGiftDto, DonorDto, GiftDto } from '../../../models/gift';
import { CategoryDto } from '../../../models/category';
import { GiftManagement } from '../../../services/gift-management';
import { DonorService } from '../../../services/donor';
import { CategoryService } from '../../../services/category';

@Component({
  selector: 'app-gift-form',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, SelectModule, InputTextModule, ButtonModule],
  templateUrl: './gift-form.html'
})
export class GiftFormComponent implements OnInit, OnChanges {
  @Input() gift: GiftDto | null = null;
  @Output() saved = new EventEmitter<void>();
  @Output() close = new EventEmitter<void>();

  private router = inject(Router);
  private msg = inject(MessageService);

  donors$!: Observable<DonorDto[]>;
  categories$!: Observable<CategoryDto[]>;

  form = new FormGroup({
    name: new FormControl('', [Validators.required]),
    ticketCost: new FormControl<number | null>(null, [Validators.required, Validators.min(1)]),
    description: new FormControl(''),
    donorId: new FormControl<number | null>(null, [Validators.required, Validators.min(1)]),
    categoryId: new FormControl<number | null>(null, [Validators.required, Validators.min(1)]),
    
  });

  constructor(
    private giftServ: GiftManagement,
    private donorServ: DonorService,
    private categoryServ: CategoryService,
    
  ) {}

  ngOnInit(): void {
    this.donors$ = this.donorServ.getAllDonors();
    this.categories$ = this.categoryServ.getAllCategories();

  }

  ngOnChanges(): void {
    if (this.gift) {
      this.form.patchValue({
        name: this.gift.name,
        ticketCost: this.gift.ticketCost,
        description: this.gift.description ?? '',
        donorId: this.gift.donor?.id ?? null,
        categoryId: this.gift.category?.id ?? null,
      });
    } else {
      this.form.reset({
        name: '',
        ticketCost: null,
        description: '',
        donorId: null,
        categoryId: null
      });
    }
  }

  onSubmit() {
    this.form.markAllAsTouched();
    if (this.form.invalid) return;
    
    const dto: AddGiftDto = this.form.getRawValue() as AddGiftDto;

    if (!this.gift) {
      this.giftServ.addGift(dto).subscribe({
        next: (msg) => {
          this.msg.add({ severity: 'success', summary: 'Success', detail: msg });

        this.form.reset({
        name: '',
        ticketCost: null,
        description: '',
        donorId: null,
        categoryId: null
      });
          this.saved.emit(); 
        },
        error: (err) => {
          const serverMsg =
            err?.error?.message ??
            (typeof err?.error === 'string' ? err.error : null) ??
            'cannot add gift';

          this.msg.add({ severity: 'error', summary: 'Error', detail: serverMsg });

          if (err.status === 401) this.router.navigateByUrl('/login');
        }
      });
    } else {
      this.giftServ.updateGift(dto, this.gift.id).subscribe({
        next: (msg) => {
          this.msg.add({ severity: 'success', summary: 'Success', detail: msg });
          this.saved.emit();
        },
        error: (err) => {
          const serverMsg =
            err?.error?.message ??
            (typeof err?.error === 'string' ? err.error : null) ??
            'cannot update gift';

          this.msg.add({ severity: 'error', summary: 'Error', detail: serverMsg });

          if (err.status === 401) this.router.navigateByUrl('/login');
        }
      });
    }
  }
}

