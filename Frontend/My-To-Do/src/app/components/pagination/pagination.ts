import { Component, input, output } from '@angular/core';

@Component({
  selector: 'app-pagination',
  standalone: true,
  templateUrl: './pagination.html',
})
export class Pagination {
  pageNumber = input(1);
  totalPages = input(1);

  pageChanged = output<number>();

  previousPage() {
    if (this.pageNumber() > 1) {
      this.pageChanged.emit(this.pageNumber() - 1);
    }
  }

  nextPage() {
    if (this.pageNumber() < this.totalPages()) {
      this.pageChanged.emit(this.pageNumber() + 1);
    }
  }
}
