import { Component, OnInit, inject, signal } from '@angular/core';
import { TaskService } from '../../services/TaskService';
import { CategoryService } from '../../services/CategoryService';
import { TaskResponse } from '../../models/task.model';
import { NavComponent } from '../../components/nav.component/nav.component';
import { AsideComponent } from '../../components/aside.component/aside.component';
import { TaskListComponent } from '../../components/task-list.component/task-list.component';
import { TaskInfoComponent } from '../../components/task-info.component/task-info.component';

@Component({
  selector: 'app-task',
  standalone: true,
  imports: [NavComponent, AsideComponent, TaskListComponent, TaskInfoComponent],
  templateUrl: './task.html',
})
export class Task implements OnInit {
  private taskService = inject(TaskService);
  private categoryService = inject(CategoryService);

  tasks = signal<TaskResponse[]>([]);
  selectedCategoryId = signal<string | null>(null);
  selectedTask = signal<TaskResponse | null>(null);
  isLoading = signal(false);
  pageNumber = signal(1);
  totalPages = signal(1);

  categoryPageNumber = signal(1);
  categoryTotalPages = signal(1);
  searchText = signal<string>('');

  ngOnInit() {
    this.loadCategories();
    this.loadTasks();
  }

  loadCategories(page = 1) {
    this.categoryService.loadAll(page).subscribe({
      next: (result) => {
        this.categoryPageNumber.set(result.pageNumber);
        this.categoryTotalPages.set(result.totalPages);
      },
    });
  }

  loadTasks(page: number = 1) {
    this.isLoading.set(true);
    this.pageNumber.set(page);

    this.taskService.getAll(page).subscribe({
      next: (result) => {
        this.tasks.set(result.items);
        this.totalPages.set(result.totalPages);
        this.isLoading.set(false);
      },
      error: () => this.isLoading.set(false),
    });
  }

  onPageChanged(page: number) {
    if (this.selectedCategoryId()) {
      this.isLoading.set(true);
      this.pageNumber.set(page);
      this.taskService
        .search({ categoryId: this.selectedCategoryId(), pageNumber: page })
        .subscribe({
          next: (result) => {
            this.tasks.set(result.items);
            this.totalPages.set(result.totalPages);
            this.isLoading.set(false);
          },
          error: () => this.isLoading.set(false),
        });
    } else {
      this.loadTasks(page);
    }
  }

  onCategoryPageChanged(page: number) {
    this.categoryPageNumber.set(page);

    this.categoryService.loadAll(page).subscribe({
      next: (result) => {
        this.categoryTotalPages.set(result.totalPages);
      },
    });
  }

  onCategorySelected(categoryId: string | null) {
    this.selectedCategoryId.set(categoryId);
    this.searchText.set('');

    this.isLoading.set(true);
    this.pageNumber.set(1);

    this.taskService.search({ categoryId, pageNumber: 1 }).subscribe({
      next: (result) => {
        this.tasks.set(result.items);
        this.totalPages.set(result.totalPages);
        this.isLoading.set(false);
      },
      error: () => this.isLoading.set(false),
    });
  }

  onSearch(searchText: string) {
    this.searchText.set(searchText);
    this.selectedCategoryId.set(null);
    this.isLoading.set(true);
    this.pageNumber.set(1);

    this.taskService.search({ categoryName: searchText || null, pageNumber: 1 }).subscribe({
      next: (result) => {
        this.tasks.set(result.items);
        this.totalPages.set(result.totalPages);
        this.isLoading.set(false);
      },
      error: () => this.isLoading.set(false),
    });
  }

  onTaskChanged(updatedTask: TaskResponse) {
    this.tasks.update((tasks) =>
      tasks.map((task) => (task.id === updatedTask.id ? updatedTask : task)),
    );

    if (this.selectedTask()?.id === updatedTask.id) {
      this.selectedTask.set(updatedTask);
    }
  }

  onTaskDeleted() {
    this.selectedTask.set(null);

    const currentPage = this.pageNumber();

    if (this.tasks().length === 1 && currentPage > 1) {
      this.onPageChanged(currentPage - 1);
    } else {
      this.onPageChanged(currentPage);
    }
  }

  onCategoryDeleted() {
    const currentPage = this.categoryPageNumber();

    this.categoryService.loadAll(currentPage).subscribe({
      next: (result) => {
        if (currentPage > result.totalPages && result.totalPages > 0) {
          this.onCategoryPageChanged(result.totalPages);
          return;
        }
          this.categoryPageNumber.set(result.pageNumber);
          this.categoryTotalPages.set(result.totalPages);
      },
    });
  }
}
