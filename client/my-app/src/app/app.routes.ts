
import { Login } from './components/Auth/login/login';
import { Register } from './components/Auth/register/register';
import { Products } from './components/User/products/products';
// import { Gifts } from './components/Manager/gifts/gifts';
import { Donors } from './components/Manager/donors/donors';
import { authGuard } from './guards/auth.guard';
import { managerGuard } from './guards/manager.guards';
import {ManageGifts} from './components/Manager/manage-gifts/manage-gifts'
import { Routes } from '@angular/router';
import { Purchase } from './components/Manager/purchase/purchase';
import { Categories } from './components/Manager/categories/categories';
import { ManagerMenue } from './components/Layouts/manager-menue/manager-menue';
import { UserMenue } from './components/Layouts/user-menue/user-menue';

export const routes: Routes = [
  { path: 'login', component: Login },
  { path: 'register', component: Register },
    { path: '', redirectTo: 'register', pathMatch: 'full' }, // ברירת מחדל



 // כל המשתמש
  {
    path: 'user',
    canActivateChild: [authGuard],
        component: UserMenue, 
    children: [
      { path: '', redirectTo: 'products', pathMatch: 'full' },
      { path: 'products', component: Products },
    ]
  },

  // כל המנהל
  
  {
    path: 'manager',
    canActivateChild: [authGuard, managerGuard],
    component: ManagerMenue, // קומפוננטת ההידר
    children: [
      { path: '', redirectTo: 'donors', pathMatch: 'full' },
      { path: 'gifts', component: ManageGifts },
      { path: 'donors', component: Donors },
      { path: 'purchases', component: Purchase },
      { path: 'categories', component: Categories },
    ]
  

  },
];
