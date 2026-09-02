import { Injectable, inject } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import {
  PagedResponse,
  SearchTaskRequest,
  TaskResponse,
  CreateTaskRequest,
  UpdateTaskRequest,
} from '../models/task.model';
import { Router } from '@angular/router';

@Injectable({ providedIn: 'root' })
export class TaskService {
  constructor(
    private http: HttpClient,
    private router: Router,
  ) {}

  private readonly apiUrl = 'https://localhost:7150/tasks';

  getAll(pageNumber = 1, pageSize = 7) {
    const params = new HttpParams().set('pageNumber', pageNumber).set('pageSize', pageSize);

    return this.http.get<PagedResponse<TaskResponse>>(`${this.apiUrl}/getTasks`, { params });
  }

  search(request: SearchTaskRequest) {
    return this.http.post<PagedResponse<TaskResponse>>(`${this.apiUrl}/search`, request);
  }

  create(request: CreateTaskRequest) {
    return this.http.post<TaskResponse>(`${this.apiUrl}/create`, request);
  }

  update(id: string, request: UpdateTaskRequest) {
    return this.http.patch<TaskResponse>(`${this.apiUrl}/update/${id}`, request);
  }
  delete(id: string) {
    return this.http.delete(`${this.apiUrl}/delete/${id}`);
  }
}
