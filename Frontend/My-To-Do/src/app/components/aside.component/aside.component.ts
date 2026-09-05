import { Component, inject, input, output, signal } from '@angular/core';
import { CategoryService } from '../../services/CategoryService';
import { CategoryResponse } from '../../models/category.model';
import { Pagination } from '../pagination/pagination';

@Component({
  selector: 'app-aside',
  standalone: true,
  templateUrl: './aside.component.html',
  imports: [Pagination],
})
export class AsideComponent {
  private categoryService = inject(CategoryService);

  categories = this.categoryService.categories;
  selectedCategoryId = input<string | null>(null);
  categorySelected = output<string | null>();
  searchText = input('');

  newCategoryName = signal('');
  showNewCategoryInput = signal(false);
  errorMessage = signal<string | null>(null);

  categoryPageNumber = input(1);
  categoryTotalPages = input(1);
  categoryPageChanged = output<number>();
  deleted = output<void>();

  openMenuId = signal<string | null>(null);
  editingId = signal<string | null>(null);
  editName = signal('');

  select(categoryId: string | null) {
    this.categorySelected.emit(categoryId);
  }

  toggleMenu(categoryId: string, event: Event) {
    event.stopPropagation(); // don't trigger select() on the parent li/button
    this.openMenuId.set(this.openMenuId() === categoryId ? null : categoryId);
  }

  startEdit(category: CategoryResponse, event: Event) {
    event.stopPropagation();
    this.editingId.set(category.id);
    this.editName.set(category.name);
    this.openMenuId.set(null);
  }

  cancelEdit() {
    this.editingId.set(null);
    this.editName.set('');
  }

  saveEdit(categoryId: string) {
    const name = this.editName().trim();
    if (!name) return;

    this.errorMessage.set(null);

    this.categoryService.update(categoryId, { name }).subscribe({
      next: () => {
        this.editingId.set(null);
        this.editName.set('');
      },
      error: (err) => {
        const errors = err.error?.errors;
        this.errorMessage.set(errors?.Name?.[0] ?? err.error?.title ?? 'Failed to update category.');
      },
    });
  }

  deleteCategory(categoryId: string, event: Event) {
    event.stopPropagation();
    this.openMenuId.set(null);

    if (!confirm('Delete this category? Tasks in it will become uncategorized.')) return;

    this.categoryService.delete(categoryId).subscribe({
      next: () => this.deleted.emit(),
      error: (err) => {
        this.errorMessage.set(err.error?.title ?? 'Failed to delete category.');
      },
    });
  }

  createCategory() {
    const name = this.newCategoryName().trim();
    this.errorMessage.set(null);
    if (!name) return;

    this.categoryService.create({ name }).subscribe({
      next: () => {
        this.newCategoryName.set('');
        this.showNewCategoryInput.set(false);
      },
      error: (err) => {
        const errors = err.error?.errors;
        this.errorMessage.set(
          errors?.Name?.[0] ??
          err.error?.title ??
          'Failed to create category.'
        );
      },
    });
  }
}
