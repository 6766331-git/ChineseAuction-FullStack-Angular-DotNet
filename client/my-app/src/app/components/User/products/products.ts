import { Component, OnInit, OnDestroy, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ProductService } from '../../../services/product';
import { CartSidebar } from '../cart-sidebar/cart-sidebar';
import { Subject, takeUntil, finalize } from 'rxjs';
import { MessageService } from 'primeng/api';

@Component({
  selector: 'app-products',
  standalone: true,
  imports: [CommonModule, CartSidebar],
  templateUrl: './products.html',
  styleUrls: ['./products.sass']
})
export class Products implements OnInit, OnDestroy {
  gifts: any[] = [];
  loading = false;      // For initial page load
  isSorting = false;    // For server-side sorting (prevents flickering)
  isCartOpen = false;
  cartItemCount = 0;

  private destroy$ = new Subject<void>();
  private giftService = inject(ProductService);
  private messageService = inject(MessageService);

  ngOnInit(): void {
    this.loadGifts();
    this.updateCartCount();
    
    // Listen for cart updates
    this.giftService.cartUpdated$
      .pipe(takeUntil(this.destroy$))
      .subscribe(() => {
        this.updateCartCount();
      });
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }

  trackByGiftId(index: number, gift: any): number {
    return gift.id || index;
  }

  // Initial load - shows full loader
  loadGifts(): void {
    this.loading = true;
    this.giftService.getAllGiftsUser()
      .pipe(
        takeUntil(this.destroy$),
        finalize(() => this.loading = false)
      )
      .subscribe({
        next: (res) => {
          this.gifts = res;
        },
        error: (err) => {
          console.error('Error loading gifts:', err);
          if (!this.gifts)
          this.messageService.add({severity:'error', summary:'Error', detail:'Failed to load products'});
        }
      });
  }

  // Sort by price via server
  sortByPrice(): void {
    this.isSorting = true; 
    this.giftService.getSortedGifts('price')
      .pipe(
        takeUntil(this.destroy$),
        finalize(() => this.isSorting = false)
      )
      .subscribe({
        next: (res: any[]) => {
          this.gifts = res;
          this.messageService.add({severity:'success', summary:'Sorting', detail:'Products sorted by price', life: 1500});
        },
        error: (err) => {
          console.error('Error sorting by price:', err);
          this.messageService.add({severity:'error', summary:'Error', detail:'Sorting failed'});
        }
      });
  }

  // Sort by category via server
  sortByCategory(): void {
    this.isSorting = true;
    this.giftService.getSortedGifts('category')
      .pipe(
        takeUntil(this.destroy$),
        finalize(() => this.isSorting = false)
      )
      .subscribe({
        next: (res: any[]) => {
          this.gifts = res;
          this.messageService.add({severity:'success', summary:'Sorting', detail:'Products sorted by category', life: 1500});
        },
        error: (err) => {
          console.error('Error sorting by category:', err);
          this.messageService.add({severity:'error', summary:'Error', detail:'Sorting failed'});
        }
      });
  }

  updateCartCount(): void {
    this.giftService.getCartItems()
      .pipe(takeUntil(this.destroy$))
      .subscribe({
        next: (items) => {
          this.cartItemCount = items.reduce((sum, item) => sum + (item.quantity || 1), 0);
        },
        error: () => {
          this.cartItemCount = 0;
        }
      });
  }

  addToCart(giftId: number): void {
    if (!giftId) return;

    this.giftService.addGiftToCart(giftId)
      .pipe(takeUntil(this.destroy$))
      .subscribe({
        next: () => {
          this.messageService.add({severity:'success', summary:'Success', detail:'Product added to cart', life: 2000});
        },
        error: (err) => {
          this.messageService.add({severity:'error', summary:'Error', detail:'Failed to add product to cart'});
        }
      });
  }

  toggleCart(): void {
    this.isCartOpen = !this.isCartOpen;
  }

  closeCart(): void {
    this.isCartOpen = false;
  }
}