export interface TaskResponse {
  id: string;
  title: string;
  description: string | null;
  categoryId: string | null;
  categoryName: string | null;
  createdAt: string;
  updatedAt: string | null;
  isCompleted: boolean;
}

export interface PagedResponse<T> {
  items: T[];
  pageNumber: number;
  pageSize: number;
  totalCount: number;
  totalPages: number;
}

export interface SearchTaskRequest {
  categoryName?: string | null;
  categoryId?: string | null;
  pageNumber?: number;
  pageSize?: number;
}

export interface UpdateTaskRequest {
  title: string;
  description: string | null;
  categoryId: string | null;
  isCompleted: boolean;
}

export interface CreateTaskRequest {
  title: string;
  description?: string | null;
  categoryId?: string | null;
}
