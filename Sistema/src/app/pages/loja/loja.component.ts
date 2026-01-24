import { Component, OnInit } from '@angular/core';
import { UsuarioModel } from '../../models/Usuario.Model';
import { UsuarioLogadoModel } from '../../models/UsuarioLogado.Model';
import { HttpService } from '../../services/http-service.service';
import { MessageService } from 'primeng/api';
import { Utils } from '../../services/utils';
import { ActivatedRoute, Router } from '@angular/router';
import { Base64Service } from '../../services/base64.service';
import { CryptoService } from '../../services/crypto.service';
import { ImportsModule } from '../../imports';
import { LojaModel } from '../../models/Loja.Model';

@Component({
  selector: 'app-loja',
  standalone: true,
  imports: [ImportsModule],
  templateUrl: './loja.component.html',
  styleUrl: './loja.component.css',
  providers: [MessageService],
})
export class LojaComponent implements OnInit {
  boolLoading = false;
  objUsuarioLogado: UsuarioLogadoModel = new UsuarioLogadoModel();
  objUsuario: UsuarioModel = new UsuarioModel();

  lstLoja: LojaModel[] = [];

  constructor(
      private http: HttpService,
      private messageService: MessageService,
      private utils: Utils,
      private router: Router,
      private route: ActivatedRoute,
      private base64Service: Base64Service,
      private cryptoService: CryptoService
    ) {
      this.objUsuarioLogado = JSON.parse(
        this.cryptoService.lerDoSessionStorage('usr')
      );
      console.warn("Usuário Logado: ", this.objUsuarioLogado);
    }

  ngOnInit() {
    try {
      this.GetLoja(0);
    } catch (error) {
      this.boolLoading = false;
      console.warn('Falha ao realizar a operação inicial.', error);
    }
  }

  GetLoja(lojCodi: number) {
    this.boolLoading = true;
    this.http.GetLoja(lojCodi).subscribe({
      next: (response) => {
        this.lstLoja = response;
        this.boolLoading = false;
        console.warn('LISTA DE LOJAS:', this.lstLoja);
      },
      error: (error) => {
        console.error('Erro ao carregar dados:', error);
        this.messageService.add({
          severity: 'error',
          summary: 'Erro: ',
          detail: 'Falha ao realizar a operação, contate o suporte.',
        });
        this.boolLoading = false;
      },
    });
  }

}
