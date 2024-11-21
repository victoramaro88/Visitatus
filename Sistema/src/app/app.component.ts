import { Component } from '@angular/core';
import { Router, RouterOutlet } from '@angular/router';
import { PrimeNGConfig } from 'primeng/api';
import { environment } from '../environments/environment';
import { MenuComponent } from "./pages/menu/menu.component";
import { ImportsModule } from './imports';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, MenuComponent, ImportsModule],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent {
  versionApp: string | undefined;

  isMenuHidden: boolean = false;
  private hiddenRoutes = ['/login', '/template', '/convite']; // Rotas onde o menu será oculto

  constructor(
    private primengConfig: PrimeNGConfig,
    private router: Router
  ) { }

  ngOnInit(): void {
    this.primengConfig.ripple = true;
    this.versionApp = environment.version;

    this.router.events.subscribe(() => {
      const currentRoute = this.router.url;
      this.isMenuHidden = this.hiddenRoutes.some(route => currentRoute.startsWith(route));
    });
  }
}
