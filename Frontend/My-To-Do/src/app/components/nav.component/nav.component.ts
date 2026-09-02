import { Component, inject, input, output } from '@angular/core';
import { RouterLink } from '@angular/router';
import { AuthService } from '../../services/AuthService';

@Component({
  selector: 'app-nav',
  standalone: true,
  imports: [RouterLink],
  templateUrl: './nav.component.html',
})
export class NavComponent {
  authService = inject(AuthService);
  searchText = input('');
  search = output<string>();

  onSearch(value: string) {
    this.search.emit(value);
  }

  logout() {
    this.authService.logout();
  }
}
