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
import {
  LojaUsuarioPotenciaModel,
  UsuarioPotenciaModel,
} from '../../models/UsuarioPotencia.Model ';

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

  boolManterRegistro: boolean = false;

  lstPerfil: PerfilModel[] = [];
  lstPerfilSelecionado: PerfilModel[] = [];
  objPerfil: PerfilModel = new PerfilModel();
  lstUsuarioLoja: UsuarioLojaModel[] = [];
  lstUsuarioLojaGrid: UsuarioLojaModel[] = [];
  objUsuarioRegistro: UsuarioPotenciaModel = new UsuarioPotenciaModel();
  lstOutrasLojas: LojaUsuarioPotenciaModel[] = [];

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
    console.warn('Perfil Selecionado (Sessão): ', this.objPerfilSelecionado);
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
          // console.warn('Lista de Perfis:', this.lstPerfil);
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
          // console.warn('Lista de Usuarios:', this.lstUsuarioLoja);
          // console.warn('Lista de Usuarios da Grid:', this.lstUsuarioLojaGrid);
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

  GetUsuarioByPotCodi(potCodi: number, usuNCIM: string) {
    this.boolLoading = true;
    try {
      this.http.GetUsuarioByPotCodi(potCodi, usuNCIM).subscribe({
        next: (response) => {
          console.warn('Lista de Usuarios:', response);
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

  EditarRegistro(nCIM: string) {
    this.ConsultaUsuarioExistente(nCIM);
    this.boolManterRegistro = true;
  }

  SalvarRegistro() {}

  CancelaRegitro() {
    this.lstPerfilSelecionado = [];
    this.objUsuarioRegistro = new UsuarioPotenciaModel();
    this.boolManterRegistro = false;
  }

  AddPerfil(objPerfil: PerfilModel) {
    if (objPerfil.PerCodi > 0) {
      let existe = this.lstPerfilSelecionado.find(
        (p) => p.PerCodi === objPerfil.PerCodi
      );
      if (!existe) {
        this.lstPerfilSelecionado.push(objPerfil);
      }
      this.objPerfil = new PerfilModel();
    }
  }

  DelPerfil(objPerfil: PerfilModel) {
    this.lstPerfilSelecionado = this.lstPerfilSelecionado.filter(
      (item) => item.PerCodi !== objPerfil.PerCodi
    );
  }

  // MAS SIM EM OUTRA LOJA DA MESMA POTÊNCIA, A CONSULTA DEVE SER SEM O NÚMERO DA LOJA.
  ConsultaUsuarioExistente(nCIM: string) {
    this.boolLoading = true;
    try {
      this.http
        .GetUsuarioByPotCodi(this.objPerfilSelecionado.potCodi, nCIM)
        .subscribe({
          next: (response) => {
            // console.warn('Lista de Usuarios:', response);
            if (response) {
              //-> SE EXISTIR USUÁRIO, PREENCHE OS DADOS DELE
              this.lstPerfilSelecionado = [];
              this.objUsuarioRegistro = new UsuarioPotenciaModel();
              this.objUsuarioRegistro = response;
              this.objUsuarioRegistro.UsuNasc = new Date(
                response.UsuNasc.toString()
              );
              console.warn('Lista de Usuarios:', this.objUsuarioRegistro);

              //-> PREENCHENDO OS PERFIS CADASTRADOS
              this.objUsuarioRegistro.lstLjUsrPot.forEach((itemPerfil) => {
                if (itemPerfil.LojCodi === this.objPerfilSelecionado.lojCodi) {
                  let item = this.lstPerfil.find(
                    (p) => p.PerCodi === itemPerfil.PerCodi
                  );
                  this.lstPerfilSelecionado.push(item!);
                } else {
                  this.lstOutrasLojas.push(itemPerfil);
                }
                console.warn('Outras Lojas: ', this.lstOutrasLojas);
              });
            }
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

  onGlobalFilter(table: Table, event: Event) {
    table.filterGlobal((event.target as HTMLInputElement).value, 'contains');
  }
}
