import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

/* Services & Models */
import { CategoryService } from '../../../services/category';
import { CategoryDto } from '../../../models/category';

/* PrimeNG Modules */
import { ToastModule } from 'primeng/toast';
import { ConfirmDialogModule } from 'primeng/confirmdialog';
import { ProgressSpinnerModule } from 'primeng/progressspinner';
import { ButtonModule } from 'primeng/button';
import { InputTextModule } from 'primeng/inputtext';
import { MessageService, ConfirmationService } from 'primeng/api';
import { Observable, of } from 'rxjs';
import { map, tap } from 'rxjs/operators';

@Component({
  selector: 'app-categories',
  standalone: true,
  imports: [
    CommonModule, 
    FormsModule, 
    ToastModule, 
    ConfirmDialogModule, 
    ProgressSpinnerModule, 
    ButtonModule, 
    InputTextModule
  ],
  providers: [MessageService, ConfirmationService],
  templateUrl: './categories.html',
  styleUrl: './categories.sass',
})
export class Categories implements OnInit {
  categories: CategoryDto[] = [];
  categories$: Observable<CategoryDto[]> = of([]); // הוספת המשתנה שהיה חסר
  newCategoryName: string = '';
  loading: boolean = false; // הוספת המשתנה שהיה חסר

  // משתנים עבור עריכה בשורה (Inline Edit)
  editingCategoryId: number | undefined | null = null;
  editingCategoryName: string = '';

  constructor(
    private categoryServ: CategoryService,
    private msg: MessageService,
    private confirmationService: ConfirmationService,
    private cdr: ChangeDetectorRef
  ) {}

  ngOnInit(): void {
    this.loadCategories();
  }

  loadCategories() {
    this.loading = true;
    // אנחנו מעדכנים גם את categories$ כדי שה-AsyncPipe ב-HTML יעבוד
    this.categories$ = this.categoryServ.getAllCategories().pipe(
      tap(data => {
        this.categories = data;
        this.loading = false;
        this.cdr.markForCheck();
      }),
      // במקרה של שגיאה
      tap({
        error: () => {
          this.loading = false;
          this.showError('Failed to load categories');
        }
      })
    );
  }

  addNewCategory() {
    if (!this.newCategoryName.trim()) {
      this.showError('Category name is required');
      return;
    }

    const newCat: CategoryDto = { name: this.newCategoryName };

    this.categoryServ.addCategory(newCat).subscribe({
      next: (createdCategory) => {
        this.newCategoryName = '';
        this.loadCategories(); // רענון הרשימה
        this.showSuccess('Category added successfully');
      },
      error: (err) => {
        const message = typeof err.error === 'string' ? err.error : 'Server error occurred';
        this.showError(message);
      }
    });
  }

  private showSuccess(detail: string) {
    this.msg.add({ severity: 'success', summary: 'Success', detail });
  }

  private showError(detail: string) {
    this.msg.add({ severity: 'error', summary: 'Error', detail });
  }

  onDeleteCategory(category: CategoryDto) {
    this.confirmationService.confirm({
      message: `Deleting this category will also delete all related gifts. Are you sure?`,
      header: 'Confirm Deletion',
      icon: 'pi pi-exclamation-triangle',
      accept: () => {
        this.categoryServ.deleteCategory(category.id!).subscribe({
          next: () => {
            this.msg.add({ severity: 'success', summary: 'Deleted', detail: 'Category deleted successfully' });
            this.loadCategories();
          },
          error: (err) => {
            const serverMsg = err?.error?.message ?? 'cannot delete category';
            this.msg.add({ severity: 'error', summary: 'Error', detail: serverMsg });
          }
        });
      }
    });
  }

  startEdit(category: CategoryDto) {
    this.editingCategoryId = category.id;
    this.editingCategoryName = category.name;
  }

  cancelEdit() {
    this.editingCategoryId = null;
    this.editingCategoryName = '';
  }

  saveEdit(category: CategoryDto) {
    const trimmedName = this.editingCategoryName.trim();
    if (!trimmedName) return;
    
    const updatedDto: CategoryDto = { id: category.id, name: trimmedName };
    this.categoryServ.updateCategory(updatedDto).subscribe({
      next: () => {
        this.msg.add({ severity: 'success', summary: 'Updated', detail: 'Category updated successfully' });
        this.cancelEdit();
        this.loadCategories();
      },
      error: (err) => {
        const serverMsg = err?.error?.message ?? 'cannot update category';
        this.msg.add({ severity: 'error', summary: 'Error', detail: serverMsg });
      }
    });
  }
}