import { Component, signal } from '@angular/core';
import { HttpClientModule } from '@angular/common/http';
import { LoginComponent } from './pages/login/login';

@Component({
  selector: 'app-root',
  imports: [LoginComponent, HttpClientModule],
  templateUrl: './app.html',
  styleUrl: './app.css'
})
export class App {
  protected readonly title = signal('backend-ui');
}
