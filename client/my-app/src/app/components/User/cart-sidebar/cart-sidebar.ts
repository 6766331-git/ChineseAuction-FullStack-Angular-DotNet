import { Component, Input, Output, EventEmitter, OnInit, OnDestroy, inject, ChangeDetectorRef, NgZone } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ProductService } from '../../../services/product';
import { Subject, takeUntil } from 'rxjs';
import { MessageService } from 'primeng/api';
import { finalize } from 'rxjs/operators';

@Component({
  selector: 'app-cart-sidebar',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './cart-sidebar.html',
  styleUrls: ['./cart-sidebar.sass']
})
export class CartSidebar implements OnInit, OnDestroy {
  @Input() isOpen = false;
  @Output() close = new EventEmitter<void>();

  items: any[] = [];
  total = 0;
  loading = false;

  private destroy$ = new Subject<void>();
  private giftService = inject(ProductService);
  private messageService = inject(MessageService);
  private cd = inject(ChangeDetectorRef);
  private ngZone = inject(NgZone);

  ngOnInit(): void {
    this.refreshCart(true);

    this.giftService.cartUpdated$
      .pipe(takeUntil(this.destroy$))
      .subscribe(() => {
        this.ngZone.run(() => this.refreshCart(false));
      });
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }

  get totalItems(): number {
    return this.items.reduce((sum, item) => sum + (item.quantity || 0), 0);
  }

  trackByGiftId(index: number, item: any): number {
    return item.giftId || index;
  }

  refreshCart(showLoading: boolean = true): void {
    if (showLoading) {
      this.loading = true;
    }

    this.giftService.getCartItems()
      .pipe(
        takeUntil(this.destroy$),
        finalize(() => {
          if (this.loading) {
            this.loading = false;
            this.cd.markForCheck();
          }
        })
      )
      .subscribe({
        next: (res: any[]) => {
          this.items = res || [];
          this.total = this.items.reduce((acc, item) => acc + (item.totalPrice || 0), 0);
          if (!showLoading) {
            this.cd.markForCheck();
          }
        },
        error: () => {
          this.items = [];
          this.total = 0;
        }
      });
  }

  updateQuantity(item: any, change: number): void {
    if (!item?.giftId) return;

    const successHandler = () => this.ngZone.run(() => this.refreshCart(false));
    const errorHandler = () => this.ngZone.run(() =>
      this.messageService.add({ severity: 'error', summary: 'Error', detail: 'Failed to update quantity' })
    );

    if (change === 1) {
      this.giftService.incrementQuantity(item.giftId)
        .pipe(takeUntil(this.destroy$))
        .subscribe({ next: successHandler, error: errorHandler });

    } else if (change === -1) {
      if (item.quantity > 1) {
        this.giftService.decreaseQuantity(item.giftId)
          .pipe(takeUntil(this.destroy$))
          .subscribe({ next: successHandler, error: errorHandler });
      } else {
        this.removeItem(item.giftId);
      }
    }
  }

  removeItem(giftId: number): void {
    this.giftService.deleteFromCart(giftId)
      .pipe(takeUntil(this.destroy$))
      .subscribe({
        next: () => this.ngZone.run(() => {
          this.messageService.add({ severity: 'info', summary: 'Removed', detail: 'Item removed from cart' });
          this.refreshCart(false);
        }),
        error: () => this.ngZone.run(() =>
          this.messageService.add({ severity: 'error', summary: 'Error', detail: 'Failed to remove item' })
        )
      });
  }

  onCheckout(): void {
    if (this.items.length === 0) {
      this.messageService.add({ severity: 'warn', summary: 'Empty Cart', detail: 'Add items before checking out' });
      return;
    }

    this.giftService.checkout()
      .pipe(takeUntil(this.destroy$))
      .subscribe({
        next: (msg: string) => this.ngZone.run(() => {
          this.messageService.add({ severity: 'success', summary: 'Success', detail: msg || 'Order placed successfully!' });
          this.refreshCart(true);
          this.close.emit();
        }),
        error: () => this.ngZone.run(() =>
          this.messageService.add({ severity: 'error', summary: 'Error', detail: 'Checkout failed' })
        )
      });
  }

  onClose(): void {
    this.close.emit();
  }
  isCartOpen = false;

  toggleCart() {
    this.isCartOpen = !this.isCartOpen;
  }
}