import { Component, signal } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { Register } from './components/Auth/register/register';
import { ToastModule } from 'primeng/toast';
import {Footer} from '././footer/footer'

@Component({
  selector: 'app-root',
  imports: [RouterOutlet,ToastModule,Footer],
  templateUrl: './app.html',
  styleUrl: './app.sass'
})
export class App {
  protected readonly title = signal('my-app');
}



