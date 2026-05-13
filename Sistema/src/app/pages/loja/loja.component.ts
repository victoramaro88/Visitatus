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
import { PerfilUsuarioListaModel } from '../../models/PerfilUsuarioLista.Model';
import { Table } from 'primeng/table';
import { PotenciaModel } from '../../models/Potencia.Model';

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
  objPerfilUsuario: PerfilUsuarioListaModel = new PerfilUsuarioListaModel();
  objUsuario: UsuarioModel = new UsuarioModel();

  boolManterRegistro: boolean = false;
  boolEditarRegistro: boolean = false;

  objLoja: LojaModel = new LojaModel(
    0,
    '',
    '',
    '',
    '',
    '',
    '',
    true,
    0,
    0,
    0
);
  lstLoja: LojaModel[] = [];
  lstPotencia: PotenciaModel[] = [];
  objPotencia: PotenciaModel = new PotenciaModel();

  constructor(
      private http: HttpService,
      private messageService: MessageService,
      private utils: Utils,
      private router: Router,
      private route: ActivatedRoute,
      private base64Service: Base64Service,
      private cryptoService: CryptoService
    ) {
      this.objPerfilUsuario = JSON.parse(
        this.cryptoService.lerDoSessionStorage('prf')
      );

      this.objUsuarioLogado = JSON.parse(
        this.cryptoService.lerDoSessionStorage('usr')
      );

      // console.warn("Usuário Logado: ", this.objUsuarioLogado);
      // console.warn("Perfil (Menu): ", this.objPerfilUsuario);
    }

  ngOnInit() {
    try {
      this.GetLoja(this.objPerfilUsuario.lojCodi);
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
        this.objLoja = this.lstLoja[0];
        this.GetPotencia(this.objLoja.PotCodi);
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

  GetPotencia(potCodi: number) {
    this.boolLoading = true;
    this.http.GetPotencia(potCodi).subscribe({
      next: (response) => {
        this.lstPotencia = response;
        this.boolLoading = false;
        console.warn('LISTA DE POTÊNCIAS:', this.lstPotencia);
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

  NovoRegistro() {
    this.boolManterRegistro = true;
    this.boolEditarRegistro = true;
  }

  EditarRegistro(nCIM: string) {
    this.boolEditarRegistro = true;
    this.boolManterRegistro = true;
  }

  SalvarRegistro(){}

  CancelaRegitro(){}

  onGlobalFilter(table: Table, event: Event) {
    table.filterGlobal((event.target as HTMLInputElement).value, 'contains');
  }
}
