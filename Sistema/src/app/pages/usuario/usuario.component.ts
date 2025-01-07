import { Component, OnInit } from '@angular/core';
import { ImportsModule } from '../../imports';
import { MessageService } from 'primeng/api';
import { UsuarioLogadoModel } from '../../models/UsuarioLogado.Model';
import { HttpService } from '../../services/http-service.service';
import { Utils } from '../../services/utils';
import { Router } from '@angular/router';
import { Base64Service } from '../../services/base64.service';
import { CryptoService } from '../../services/crypto.service';
import { PerfilUsuarioListaModel } from '../../models/PerfilUsuarioLista.Model';
import { UsuarioLojaModel } from '../../models/UsuarioLoja.Model';
import { Table } from 'primeng/table';
import { PerfilModel } from '../../models/Perfil.Model';

@Component({
  selector: 'app-usuario',
  standalone: true,
  imports: [ImportsModule],
  templateUrl: './usuario.component.html',
  styleUrl: './usuario.component.css',
  providers: [MessageService],
})
export class UsuarioComponent implements OnInit {
  boolLoading = false;
  objUsuarioLogado: UsuarioLogadoModel = new UsuarioLogadoModel();
  objPerfilSelecionado: PerfilUsuarioListaModel = new PerfilUsuarioListaModel();

  boolManterRegistro: boolean = true;

  lstPerfil: PerfilModel[] = [];
  lstPerfilSelecionado: PerfilModel[] = [];
  objPerfil: PerfilModel = new PerfilModel();
  objUsuarioLoja: UsuarioLojaModel = new UsuarioLojaModel();
  lstUsuarioLoja: UsuarioLojaModel[] = [];
  lstUsuarioLojaGrid: UsuarioLojaModel[] = [];

  constructor(
    private http: HttpService,
    private messageService: MessageService,
    public utils: Utils,
    private router: Router,
    private base64Service: Base64Service,
    private cryptoService: CryptoService
  ) {
    this.objUsuarioLogado = JSON.parse(
      this.cryptoService.lerDoSessionStorage('usr')
    );
    // console.warn('Usuário Logado (Sessão): ', this.objUsuarioLogado);
    this.objPerfilSelecionado = JSON.parse(
      this.cryptoService.lerDoSessionStorage('prf')
    );
    // console.warn('Perfil Selecionado (Sessão): ', this.objPerfilSelecionado);
  }

  ngOnInit() {
    this.GetPerfilByPerCodi('2,4,5');
    this.GetUsuarioByLojCodi(this.objPerfilSelecionado.lojCodi);
  }

  GetPerfilByPerCodi(idsPerfil: string) {
    this.boolLoading = true;
    try {
      this.lstUsuarioLoja = [];
      this.http.GetPerfilByPerCodi(idsPerfil).subscribe({
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

  GetUsuarioByLojCodi(lojCodi: number) {
    this.boolLoading = true;
    try {
      this.lstUsuarioLoja = [];
      this.http.GetUsuarioByLojCodi(lojCodi).subscribe({
        next: (response) => {
          this.lstUsuarioLoja = response;
          response.forEach((item) => {
            let existe = this.lstUsuarioLojaGrid.find(
              (u) => u.UsuCodi === item.UsuCodi
            );
            if (existe) {
              existe.PerNome += ', ' + item.PerNome;
            } else {
              this.lstUsuarioLojaGrid.push(item);
            }
          });

          console.warn('Lista de Usuarios:', this.lstUsuarioLoja);
          console.warn('Lista de Usuarios da Grid:', this.lstUsuarioLojaGrid);
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

  NovoRegistro() {
    this.boolManterRegistro = true;
  }

  EditarRegistro(objUsuarioLoja: UsuarioLojaModel) {
    console.warn(objUsuarioLoja);
  }

  SalvarRegistro() {}

  CancelaRegitro() {
    this.boolManterRegistro = false;
  }

  AddPerfil(objPerfil: PerfilModel) {
    this.lstPerfilSelecionado.push(objPerfil);
  }

  DelPerfil(objPerfil: PerfilModel) {
    this.lstPerfilSelecionado = this.lstPerfilSelecionado.filter(
      (item) => item.PerCodi !== objPerfil.PerCodi
    );
  }

  onGlobalFilter(table: Table, event: Event) {
    table.filterGlobal((event.target as HTMLInputElement).value, 'contains');
  }
}
