// task-list.component.ts
import { Component, inject, input, output, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { TaskService } from '../../services/TaskService';
import { CategoryService } from '../../services/CategoryService';
import { TaskResponse } from '../../models/task.model';
import { Pagination } from '../pagination/pagination';

@Component({
  selector: 'app-task-list',
  standalone: true,
  imports: [ReactiveFormsModule, CommonModule, Pagination],
  templateUrl: './task-list.component.html',
})
export class TaskListComponent {
  private categoryService = inject(CategoryService);
  private fb = inject(FormBuilder);
  private taskService = inject(TaskService);

  categories = this.categoryService.categories;

  tasks = input.required<TaskResponse[]>();
  isLoading = input(false);
  selectedTaskId = input<string | null>(null);

  pageNumber = input(1);
  totalPages = input(1);

  pageChanged = output<number>();
  tasksChanged = output<TaskResponse>();
  taskCreated = output<void>();
  taskSelected = output<TaskResponse>();

  showNewTaskInput = signal(false);
  errorMessage = signal<string | null>(null);

  form = this.fb.group({
    title: ['', Validators.required],
    description: [''],
    categoryId: [null as string | null],
  });

  selectTask(task: TaskResponse) {
    this.taskSelected.emit(task);
  }

  toggleCompleted(task: TaskResponse, event: Event) {
    const checked = (event.target as HTMLInputElement).checked;

    this.taskService
      .update(task.id, {
        title: task.title,
        description: task.description,
        categoryId: task.categoryId,
        isCompleted: checked,
      })
      .subscribe({
        next: (updatedTask) => {
          this.tasksChanged.emit(updatedTask);
        },
        error: (err) => {
          const errors = err.error?.errors;

          const validationError = errors ? Object.values(errors).flat()[0] : null;
          this.errorMessage.set(validationError ?? err.error?.title ?? 'Failed to update task.');

          (event.target as HTMLInputElement).checked = task.isCompleted;
        },
      });
  }

  createTask() {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

    this.errorMessage.set(null);
    const value = this.form.getRawValue();

    this.taskService
      .create({
        title: value.title!,
        description: value.description || null,
        categoryId: value.categoryId,
      })
      .subscribe({
        next: () => {
          this.form.reset();
          this.showNewTaskInput.set(false);
          this.taskCreated.emit();
        },
        error: (err) => {
          const errors = err.error?.errors;

          const validationError = errors ? Object.values(errors).flat()[0] : null;

          this.errorMessage.set(validationError ?? err.error?.title ?? 'Failed to create task.');
          },
      });
  }
}
