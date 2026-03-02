import { Component, inject } from '@angular/core';
import { FormControl, FormGroup, Validators, ReactiveFormsModule, FormsModule } from '@angular/forms';
import { AuthService } from '../../../services/auth';
import { MessageService } from 'primeng/api';
import { Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { InputTextModule } from 'primeng/inputtext';
import { ButtonModule } from 'primeng/button';
import { CardModule } from 'primeng/card';
import { ToastModule } from 'primeng/toast';
import { MessageModule } from 'primeng/message';
import { PasswordModule } from 'primeng/password';
import { LoginDto, LoginResponseDto } from '../../../models/user';

@Component({
  selector: 'app-login',
  templateUrl: './login.html',
  styleUrls: ['./login.sass'],
  standalone: true,
  imports: [
    ReactiveFormsModule,
    FormsModule,
    CommonModule,
    InputTextModule,
    ButtonModule,
    CardModule,
    ToastModule,
    MessageModule,
    PasswordModule
  ],
  providers: [MessageService]
})
export class Login {
  // שירותים מוזרקים
  private authServ = inject(AuthService);
  private router = inject(Router);
  private messageService = inject(MessageService);

  // הגדרת הטופס ואתחול מיידי - נקי וברור
  loginForm = new FormGroup({
    email: new FormControl('', [
      Validators.required, 
      Validators.email
    ]),
    passwordHash: new FormControl('', [
      Validators.required,
      Validators.minLength(6),
      Validators.maxLength(15)
    ])
  });

  onSubmit(): void {
    if (this.loginForm.invalid) {
      this.loginForm.markAllAsTouched();
      return;
    }

    const dto: LoginDto = {
      UserName: this.loginForm.value.email!,
      Password: this.loginForm.value.passwordHash!
    };

    this.authServ.loginUser(dto).subscribe({
      next: (res) => {
        if ((res as LoginResponseDto).token) {
          const loginRes = res as LoginResponseDto;

          this.messageService.add({
            severity: 'success',
            summary: 'Success',
            detail: 'User logged in successfully'
          });

          // ניתוב לפי הרשאה
          if (loginRes.role === 1) {
            this.router.navigateByUrl('/manager/gifts');
          } else {
            this.router.navigateByUrl('/user/products');
          }

          this.loginForm.reset();
        } else {
          this.messageService.add({
            severity: 'error',
            summary: 'Error',
            detail: (res as { message: string }).message
          });
        }
      },
      error: (err) => {
        this.messageService.add({
          severity: 'error',
          summary: 'Error',
          detail: 'Invalid username or password'
        });
        console.error(err);
      }
    });
  }

  goToRegister(): void {
    this.router.navigate(['/register']);
  }
}