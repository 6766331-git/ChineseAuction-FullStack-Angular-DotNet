import { Component, inject } from '@angular/core'; // הוספנו inject
import { CommonModule } from '@angular/common';
import { CartSidebar } from '../../User/cart-sidebar/cart-sidebar';
import { Router, RouterModule } from '@angular/router';
import { ProductService } from '../../../services/product'; // וודא שהנתיב נכון

@Component({
  selector: 'app-header',
  standalone: true,
  imports: [RouterModule, CommonModule, CartSidebar],
  templateUrl: './user-menue.html',
  styleUrls: ['./user-menue.sass'],
})
export class UserMenue {
  private router = inject(Router);
  private giftService = inject(ProductService); 

  isCartOpen = false; 

  menuItems = [
    { label: 'Gifts', path: '/user/gifts' },
        // { label: 'Register', path: '/register' },

  ];

  // פונקציה לפתיחה/סגירה של העגלה
  toggleCart() {
    this.isCartOpen = !this.isCartOpen;
  }

  navigate(path: string) {
    this.router.navigate([path]);
  }

  isActive(path: string): boolean {
    return this.router.url === path;
  }

  logout() {
    console.log('Logging out...');
    this.router.navigate(['/login']);
  }
}