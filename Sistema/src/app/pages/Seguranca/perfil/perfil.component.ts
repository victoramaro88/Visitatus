import { Component, OnInit } from '@angular/core';
import { MessageService } from 'primeng/api';
import { ImportsModule } from '../../../imports';
import { HttpService } from '../../../services/http-service.service';
import { Utils } from '../../../services/utils';
import { Router } from '@angular/router';
import { Base64Service } from '../../../services/base64.service';
import { CryptoService } from '../../../services/crypto.service';
import { UsuarioLogadoModel } from '../../../models/UsuarioLogado.Model';
import { PerfilUsuarioListaModel } from '../../../models/PerfilUsuarioLista.Model';
import { PerfilModel } from '../../../models/Perfil.Model';
import { Table } from 'primeng/table';

@Component({
  selector: 'app-perfil',
  standalone: true,
  imports: [ImportsModule],
  templateUrl: './perfil.component.html',
  styleUrl: './perfil.component.css',
  providers: [MessageService],
})
export class PerfilComponent implements OnInit {
  boolLoading = true;
  objUsuarioLogado: UsuarioLogadoModel = new UsuarioLogadoModel();
  objPerfilSelecionado: PerfilUsuarioListaModel = new PerfilUsuarioListaModel();

  lstPerfil: PerfilModel[] = [];
  objPerfil: PerfilModel = new PerfilModel();
  boolManterRegistro: boolean = false;

  constructor(
    private http: HttpService,
    private messageService: MessageService,
    private utils: Utils,
    private router: Router,
    private base64Service: Base64Service,
    private cryptoService: CryptoService
  ) {
    this.objUsuarioLogado = JSON.parse(
      this.cryptoService.lerDoSessionStorage('usr')
    );
    // console.warn("Usuário Logado (Sessão): ", this.objUsuarioLogado);
    this.objPerfilSelecionado = JSON.parse(
      this.cryptoService.lerDoSessionStorage('prf')
    );
    // console.warn("Perfil Selecionado (Sessão): ", this.objPerfilSelecionado);
  }

  ngOnInit() {
    this.GetPerfil(0);
  }

  GetPerfil(perCodi: number) {
    this.boolLoading = true;
    try {
      this.http.GetPerfil(perCodi).subscribe({
        next: (response) => {
          this.lstPerfil = response;
          console.warn('Lista de Perfis:', this.lstPerfil);
          this.boolLoading = false;
        },
        error: (error) => {
          console.error('Erro ao carregar dados:', error);
          this.boolLoading = false;
        },
      });
    } catch (error) {
      console.error('Erro ao carregar dados:', error);
      this.boolLoading = false;
    }
  }

  NovoPerfil() {}

  AtivaInativa(statusAtual: boolean, objPerfil: PerfilModel) {
    console.warn(statusAtual);
    console.warn(objPerfil);
  }

  onGlobalFilter(table: Table, event: Event) {
    table.filterGlobal((event.target as HTMLInputElement).value, 'contains');
  }
}
