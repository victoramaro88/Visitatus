import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { MenuComponent } from '../menu/menu.component';
import { ImportsModule } from '../../imports';

@Component({
  selector: 'app-template-convite',
  standalone: true,
  imports: [ImportsModule],
  templateUrl: './template-convite.component.html',
  styleUrl: './template-convite.component.css'
})
export class TemplateConviteComponent {

  constructor(
    private router: Router
  ) {}

  ConfirmarPresenca(){
    console.warn('Presença Confirmada!');
  }

}
