import { Injectable, signal } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { tap } from 'rxjs';
import { CategoryRequest, CategoryResponse } from '../models/category.model';
import {CategoryPagedResponse} from '../models/category.model';

@Injectable({ providedIn: 'root' })
export class CategoryService {
  constructor(private http: HttpClient) {}

  private readonly apiUrl = 'https://localhost:7150/category';

  private categoriesSignal = signal<CategoryResponse[]>([]);
  readonly categories = this.categoriesSignal.asReadonly();

  loadAll(pageNumber = 1, pageSize = 15) {
    const params = new HttpParams().set('pageNumber', pageNumber).set('pageSize', pageSize);
    return this.http
      .get<CategoryPagedResponse<CategoryResponse>>(`${this.apiUrl}/get`, { params })
      .pipe(tap((categories) => this.categoriesSignal.set(categories.items)));
  }

  create(request: CategoryRequest) {
    return this.http
      .post<CategoryResponse>(`${this.apiUrl}/create`, request)
      .pipe(tap((category) => this.categoriesSignal.update((list) => [...list, category])));
  }

  update(id: string, request: CategoryRequest) {
    return this.http
      .patch<CategoryResponse>(`${this.apiUrl}/update/${id}`, request)
      .pipe(
        tap((updated) =>
          this.categoriesSignal.update((list) => list.map((c) => (c.id === id ? updated : c))),
        ),
      );
  }

  delete(id: string) {
    return this.http
      .delete(`${this.apiUrl}/delete/${id}`)
      .pipe(tap(() => this.categoriesSignal.update((list) => list.filter((c) => c.id !== id))));
  }
}
