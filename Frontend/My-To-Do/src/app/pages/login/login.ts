import { Component, signal } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { AuthService } from '../../services/AuthService';

@Component({
  imports: [ReactiveFormsModule, RouterLink],
  selector: 'app-login',
  templateUrl: './login.html',
  standalone: true,
})
export class Login {
  form!: ReturnType<FormBuilder['group']>;
  errorMessage = signal<string | null>(null);
  isLoading = signal(false);


  constructor(
    private fb: FormBuilder,
    private authService: AuthService,
    private router: Router,
  ) {
    this.form = this.fb.group({
      email: ['', [Validators.required, Validators.email]],
      password: ['', Validators.required],
    });
  }

  onSubmit(): void {
    if (this.form.invalid) return;

    this.isLoading.set(true);
    this.errorMessage.set(null);

    this.authService.login(this.form.getRawValue() as any).subscribe({
      next: () => this.router.navigate(['/tasks']),
      error: (err) => {
        this.errorMessage.set(err.error?.title ?? 'Login failed. Check your credentials.');
        this.isLoading.set(false);
      },
    });
  }
}
