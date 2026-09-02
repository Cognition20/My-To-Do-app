import { Component, inject, input, output, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { TaskService } from '../../services/TaskService';
import { CategoryService } from '../../services/CategoryService';
import { TaskResponse } from '../../models/task.model';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';

@Component({
  selector: 'app-task-info',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './task-info.component.html',
})
export class TaskInfoComponent {
  private taskService = inject(TaskService);
  private categoryService = inject(CategoryService);
  private fb = inject(FormBuilder);

  task = input.required<TaskResponse>();
  closed = output<void>();
  deleted = output<void>();
  tasksChanged = output<TaskResponse>();


  categories = this.categoryService.categories;
  errorMessage = signal<string | null>(null);
  editTask = signal(false);

  form = this.fb.group({
    title: ['', Validators.required],
    description: [''],
    categoryId: [null as string | null],
  });

  openEdit() {
    this.form.patchValue({
      title: this.task().title,
      description: this.task().description,
      categoryId: this.task().categoryId,
    });
    this.errorMessage.set(null);
    this.editTask.set(true);
  }

  saveEdit() {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

    const value = this.form.getRawValue();
    this.errorMessage.set(null);

    this.taskService
      .update(this.task().id, {
        title: value.title!,
        description: value.description || null,
        categoryId: value.categoryId,
        isCompleted: this.task().isCompleted, // preserve existing completed state — this form doesn't edit it
      })
      .subscribe({
        next: (updatedTask) => {
          this.editTask.set(false);
          this.tasksChanged.emit(updatedTask);
        },
        error: (err) => {
          this.errorMessage.set(err.error?.title ?? 'Failed to update task.');
        },
      });
  }

  close() {
    this.closed.emit();
  }

  delete() {
    if (!confirm('Delete this task?')) return;

    this.taskService.delete(this.task().id).subscribe({
      next: () => this.deleted.emit(),
      error: (err) => {
        this.errorMessage.set(err.error?.title ?? 'Failed to delete task.');
      },
    });
  }
}
