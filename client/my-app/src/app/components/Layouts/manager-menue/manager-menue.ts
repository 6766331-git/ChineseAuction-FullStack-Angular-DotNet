// manager-menue.component.ts
import { Component } from '@angular/core';
import { Router, RouterModule } from '@angular/router';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-manager-menue',
  standalone: true,
  imports: [RouterModule, CommonModule],
  templateUrl: './manager-menue.html',
  styleUrls: ['./manager-menue.sass'],
})
export class ManagerMenue {

  menuItems = [
    { label: 'Gifts', path: '/manager/gifts' },
    { label: 'Donors', path: '/manager/donors' },
    { label: 'Purchases', path: '/manager/purchases' },
    { label: 'Categories', path: '/manager/categories' },
    // { label: 'Register', path: '/register' },


  ];

  constructor(private router: Router) {}

  navigate(path: string) {
    this.router.navigate([path]);
  }

  isActive(path: string): boolean {
    return this.router.url === path;
  }

  logout() {
    // כאן אפשר להוסיף לוגיקה אמיתית של Logout
    console.log('Logging out...');
    this.router.navigate(['/login']); 
  }

}
