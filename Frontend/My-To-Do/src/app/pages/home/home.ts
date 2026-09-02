import { Component, inject } from '@angular/core';
import { Router, RouterLink } from '@angular/router';
import { AuthService } from '../../services/AuthService';

@Component({
  imports: [RouterLink],
  selector: 'app-home',
  templateUrl: './home.html',
})
export class Home {
  private authService = inject(AuthService);
  private router = inject(Router);

  constructor() {
    if (this.authService.isAuthenticated()) {
      this.router.navigate(['/tasks']);
    }
  }

}
