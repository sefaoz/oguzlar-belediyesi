import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { ButtonModule } from 'primeng/button';
import { CheckboxModule } from 'primeng/checkbox';
import { InputTextModule } from 'primeng/inputtext';
import { PasswordModule } from 'primeng/password';
import { RippleModule } from 'primeng/ripple';
import { AuthService } from '../../shared/services/auth.service';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, InputTextModule, PasswordModule, ButtonModule, CheckboxModule, RippleModule, RouterModule],
  templateUrl: './login.html',
  styleUrls: ['./login.css']
})
export class LoginComponent implements OnInit {
  loginForm!: FormGroup;
  isLoading = false;
  feedback?: string;

  constructor(private readonly fb: FormBuilder, private readonly authService: AuthService) {}

  ngOnInit(): void {
    this.loginForm = this.fb.group({
      username: ['', [Validators.required]],
      password: ['', [Validators.required]],
      remember: [false]
    });
  }

  get rememberControl() {
    return this.loginForm.get('remember');
  }

  onSubmit(): void {
    if (this.loginForm.invalid) {
      this.loginForm.markAllAsTouched();
      return;
    }

    this.isLoading = true;
    this.feedback = undefined;
    this.authService.login(this.loginForm.value).subscribe({
      next: response => {
        localStorage.setItem('oguzlar-token', response.token);
        this.feedback = 'Giriş başarılı. Token kaydedildi.';
        this.isLoading = false;
      },
      error: () => {
        this.feedback = 'Kullanıcı adı veya şifre hatalı.';
        this.isLoading = false;
      }
    });
  }
}
