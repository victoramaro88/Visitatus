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
  boolEditarRegistro: boolean = false;
  boolResponsavelLoja: boolean = false;

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
    // console.warn('Perfil Selecionado (Sessão): ', this.objPerfilSelecionado);
  }

  ngOnInit() {
    this.GetPerfilByPerCodi('2,4,5,6');

    //-> Se o usuário possuir perfil de Responsável ou Secretário, habilita a opção de cadastro de Login
    if (this.objPerfilSelecionado.perCodi === 2 || this.objPerfilSelecionado.perCodi === 6) {
      this.boolResponsavelLoja = true;
    }
  }

  GetPerfilByPerCodi(idsPerfil: string) {
    this.boolLoading = true;
    try {
      this.lstUsuarioLoja = [];
      this.http.GetPerfilByPerCodi(idsPerfil).subscribe({
        next: (response) => {
          this.lstPerfil = response;
          // console.warn('Lista de Perfis:', this.lstPerfil);
          this.GetUsuarioByLojCodi(this.objPerfilSelecionado.lojCodi);
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
      this.lstUsuarioLojaGrid = [];
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
    this.boolEditarRegistro = true;
  }

  EditarRegistro(nCIM: string) {
    this.objUsuarioRegistro.UsuNCIM = nCIM;
    this.boolEditarRegistro = true;
    this.ConsultaUsuarioExistente(nCIM);
    this.boolManterRegistro = true;
  }

  SalvarRegistro() {
    this.objUsuarioRegistro.lstLjUsrPot = [];
    //-> Preenchendo os perfis selecionados
    this.lstPerfilSelecionado.forEach((itemPerfilSelecionado) => {
      let objAdd: LojaUsuarioPotenciaModel = {
        LojCodi: this.objPerfilSelecionado.lojCodi,
        PerCodi: itemPerfilSelecionado.PerCodi,
        LojNome: this.objPerfilSelecionado.lojNome,
        LojNumL: this.objPerfilSelecionado.lojNumL,
        PotCodi: this.objPerfilSelecionado.potCodi,
        PerNome: itemPerfilSelecionado.PerNome,
        PerStat: true,
      };
      this.objUsuarioRegistro.lstLjUsrPot.push(objAdd);
    });

    //-> Agora preenche os perfis das outras Lojas que ele faz parte
    this.lstOutrasLojas.forEach((itemOutrasLojas) => {
      this.objUsuarioRegistro.lstLjUsrPot.push(itemOutrasLojas);
    });

    if (this.ValidaInformacoes()) {
      this.objUsuarioRegistro.UsuNCel = this.utils.RemoveMascaraTelefone(
        this.objUsuarioRegistro.UsuNCel
      );
      this.boolLoading = true;
      //-> Se for 0, insere, senão edita
      if (this.objUsuarioRegistro.UsuCodi === 0) {
        try {
          this.http.PostUsuarioCompleto(this.objUsuarioRegistro).subscribe({
            next: (response) => {
              // console.warn('RETORNO SALVAMENTO:', response);
              if (response) {
                this.messageService.add({
                  severity: 'success',
                  summary: 'Sucesso! ',
                  detail: 'Registro salvo com sucesso!',
                });
                this.CancelaRegitro();
              }
              this.boolLoading = false;
            },
            error: (error) => {
              console.error('Erro ao carregar dados:', error);
              this.boolLoading = false;
              this.messageService.add({
                severity: 'error',
                summary: 'Erro: ',
                detail: 'Falha ao realizar a operação, contate o suporte.',
              });
            },
          });
        } catch (error) {
          console.error('Erro ao carregar dados:', error);
          this.boolLoading = false;
          this.messageService.add({
            severity: 'error',
            summary: 'Erro: ',
            detail: 'Falha ao realizar a operação, contate o suporte.',
          });
        }
      } else {
        try {
          this.http
            .PutUsuarioCompleto(
              this.objUsuarioRegistro.UsuCodi,
              this.objUsuarioRegistro
            )
            .subscribe({
              next: (response) => {
                // console.warn('RETORNO EDIÇÃO:', response);
                if (response) {
                  this.messageService.add({
                    severity: 'success',
                    summary: 'Sucesso! ',
                    detail: 'Registro salvo com sucesso!',
                  });
                  this.CancelaRegitro();
                }
                this.boolLoading = false;
              },
              error: (error) => {
                console.error('Erro ao carregar dados:', error);
                this.boolLoading = false;
                this.messageService.add({
                  severity: 'error',
                  summary: 'Erro: ',
                  detail: 'Falha ao realizar a operação, contate o suporte.',
                });
              },
            });
        } catch (error) {
          console.error('Erro ao carregar dados:', error);
          this.boolLoading = false;
          this.messageService.add({
            severity: 'error',
            summary: 'Erro: ',
            detail: 'Falha ao realizar a operação, contate o suporte.',
          });
        }
      }
    }
  }

  CancelaRegitro() {
    this.lstPerfilSelecionado = [];
    this.objUsuarioRegistro = new UsuarioPotenciaModel();
    this.lstOutrasLojas = [];
    this.boolManterRegistro = false;
    this.boolEditarRegistro = false;
    this.GetUsuarioByLojCodi(this.objPerfilSelecionado.lojCodi);
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
    if (this.boolEditarRegistro) {
      if (this.objUsuarioRegistro.UsuNCIM.length > 0) {
        this.boolLoading = true;
        try {
          this.http
            .GetUsuarioByPotCodi(this.objPerfilSelecionado.potCodi, nCIM)
            .subscribe({
              next: (response) => {
                // console.warn('Usuário Selecionado:', response);
                if (response.UsuCodi > 0) {
                  this.boolEditarRegistro = false;
                  //-> SE EXISTIR USUÁRIO, PREENCHE OS DADOS DELE
                  this.lstPerfilSelecionado = [];
                  this.lstOutrasLojas = [];
                  this.objUsuarioRegistro = new UsuarioPotenciaModel();
                  this.objUsuarioRegistro = response;
                  this.objUsuarioRegistro.UsLPass = '';
                  this.objUsuarioRegistro.UsuNasc = new Date(
                    response.UsuNasc.toString()
                  );
                  // console.warn('Lista de Usuarios:', this.objUsuarioRegistro);

                  //-> PREENCHENDO OS PERFIS CADASTRADOS
                  this.objUsuarioRegistro.lstLjUsrPot.forEach((itemPerfil) => {
                    if (
                      itemPerfil.LojCodi === this.objPerfilSelecionado.lojCodi
                    ) {
                      let item = this.lstPerfil.find(
                        (p) => p.PerCodi === itemPerfil.PerCodi
                      );
                      this.lstPerfilSelecionado.push(item!);
                    } else {
                      this.lstOutrasLojas.push(itemPerfil);
                    }
                  });
                  // console.warn('Outras Lojas: ', this.lstOutrasLojas);
                } else {
                  this.objUsuarioRegistro = new UsuarioPotenciaModel();
                  this.lstPerfilSelecionado = [];
                  this.lstOutrasLojas = [];
                  this.objUsuarioRegistro.UsuNCIM = nCIM;
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
    }
  }

  ValidaInformacoes() {
    if (
      this.objUsuarioRegistro.UsuNCIM === undefined ||
      this.objUsuarioRegistro.UsuNCIM.length <= 3
    ) {
      this.messageService.add({
        severity: 'warn',
        summary: 'Atenção: ',
        detail: 'Insira um número de CIM válido.',
      });
      return false;
    }
    if (this.objUsuarioRegistro.UsuNome.length <= 3) {
      this.messageService.add({
        severity: 'warn',
        summary: 'Atenção: ',
        detail: 'Insira um nome válido.',
      });
      return false;
    }
    if (!this.utils.ValidarEmail(this.objUsuarioRegistro.UsuEmai)) {
      this.messageService.add({
        severity: 'warn',
        summary: 'Atenção: ',
        detail: 'Insira um e-mail válido.',
      });
      return false;
    }
    if (this.objUsuarioRegistro.UsuNCel.length < 11) {
      this.messageService.add({
        severity: 'warn',
        summary: 'Atenção: ',
        detail: 'Insira um número de celular válido.',
      });
      return false;
    }
    if (
      this.objUsuarioRegistro.UsuNasc.toDateString() ===
      new Date().toDateString()
    ) {
      this.messageService.add({
        severity: 'warn',
        summary: 'Atenção: ',
        detail: 'Insira uma data de nascimento válida.',
      });
      return false;
    }
    if (this.objUsuarioRegistro.lstLjUsrPot.length === 0) {
      this.messageService.add({
        severity: 'warn',
        summary: 'Atenção: ',
        detail: 'Insira pelo menos um perfil para o usuário.',
      });
      return false;
    }
    if (this.lstPerfilSelecionado.length === 0) {
      this.messageService.add({
        severity: 'warn',
        summary: 'Atenção: ',
        detail: 'Insira pelo menos um perfil para o usuário.',
      });
      return false;
    }

    return true;
  }

  onGlobalFilter(table: Table, event: Event) {
    table.filterGlobal((event.target as HTMLInputElement).value, 'contains');
  }
}
