import { Component, OnInit } from '@angular/core';
import { ImportsModule } from '../../imports';
import { environment } from '../../../environments/environment';

@Component({
  selector: 'app-rodape',
  standalone: true,
  imports: [ImportsModule],
  templateUrl: './rodape.component.html',
  styleUrl: './rodape.component.css',
})
export class RodapeComponent implements OnInit {
  versao: string = '';
  anoAtual: string = '';
  ambiente: string = '';

  ngOnInit(): void {
    this.versao = environment.version.split(' ')[0];
    this.ambiente = environment.version.split(' ')[4];
    this.anoAtual = new Date().getFullYear().toString();
  }
}
