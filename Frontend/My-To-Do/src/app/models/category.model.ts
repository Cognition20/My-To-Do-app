export interface CategoryResponse {
  id: string;
  name: string;
}

export interface CategoryPagedResponse<T> {
  items: T[];
  pageNumber: number;
  pageSize: number;
  totalCount: number;
  totalPages: number;
}

export interface CategoryRequest {
  name: string;
}
