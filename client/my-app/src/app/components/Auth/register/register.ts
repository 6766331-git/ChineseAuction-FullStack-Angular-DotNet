import { Component, inject } from '@angular/core';
import { AuthService } from '../../../services/auth';
import { CommonModule } from '@angular/common'; 
import { FormControl, FormGroup, ReactiveFormsModule, Validators, FormsModule } from '@angular/forms';
import { MessageService } from 'primeng/api';
import { Router } from '@angular/router';

// PrimeNG Modules
import { InputMaskModule } from 'primeng/inputmask';
import { MessageModule } from 'primeng/message';
import { PasswordModule } from 'primeng/password';
import { ButtonModule } from 'primeng/button';
import { ToastModule } from 'primeng/toast';
import { InputTextModule } from 'primeng/inputtext';

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [    
    ReactiveFormsModule,
    InputMaskModule,
    FormsModule,
    CommonModule,
    MessageModule,
    PasswordModule,
    ButtonModule,
    ToastModule,
    InputTextModule
  ],
  providers: [MessageService],
  templateUrl: './register.html',
  styleUrl: './register.sass',
})
export class Register {
  private router = inject(Router);
  private authServ = inject(AuthService);
  private messageService = inject(MessageService);

  // הגדרה ישירה של הטופס בסגנון הנקי
  registerForm = new FormGroup({
    name: new FormControl('', Validators.required),
    email: new FormControl('', [Validators.required, Validators.email]),
    passwordHash: new FormControl('', [
      Validators.required,
      Validators.minLength(6),
      Validators.maxLength(15)
    ]),
    phone: new FormControl('', [
      Validators.required,
      Validators.pattern('^05[0-9]-[0-9]{7}$')
    ])
  });

  onSubmit(): void {
    if (this.registerForm.invalid) {
      this.registerForm.markAllAsTouched();
      return;
    }

    const dto = this.registerForm.getRawValue();
    // ניקוי המקף מהטלפון לפני השליחה לשרת
    const cleanDto = {
      ...dto,
      phone: (dto.phone ?? '').replace(/\D/g, '')
    };

    this.authServ.registerUser(cleanDto).subscribe({
      next: () => {
        this.messageService.add({
          severity: 'success',
          summary: 'Success',
          detail: 'User registered successfully!'
        });
        setTimeout(() => this.router.navigate(['/login']), 1500);
      },
      error: (err) => {
        const errorMessage = typeof err.error === 'string' ? err.error : (err.error?.message || 'Registration failed');
        this.messageService.add({
          severity: 'error',
          summary: 'Error',
          detail: errorMessage
        });
      }
    });
  }

  goToLogin(): void {
    this.router.navigate(['/login']);
  }
}