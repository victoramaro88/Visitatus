import { PermissaoModel } from './../../../models/Permissao.Model';
import { Component, OnInit } from '@angular/core';
import { ImportsModule } from '../../../imports';
import { MessageService } from 'primeng/api';
import { UsuarioLogadoModel } from '../../../models/UsuarioLogado.Model';
import { PerfilUsuarioListaModel } from '../../../models/PerfilUsuarioLista.Model';
import { HttpService } from '../../../services/http-service.service';
import { Utils } from '../../../services/utils';
import { Router } from '@angular/router';
import { Base64Service } from '../../../services/base64.service';
import { CryptoService } from '../../../services/crypto.service';
import { Table } from 'primeng/table';

@Component({
  selector: 'app-permissao',
  standalone: true,
  imports: [ImportsModule],
  templateUrl: './permissao.component.html',
  styleUrl: './permissao.component.css',
  providers: [MessageService]
})
export class PermissaoComponent implements OnInit {
  boolLoading = true;
  objUsuarioLogado: UsuarioLogadoModel = new UsuarioLogadoModel();
  objPerfilSelecionado: PerfilUsuarioListaModel = new PerfilUsuarioListaModel();

  boolManterRegistro: boolean = false;
  lstPermissao: PermissaoModel[] = [];
  objPermissao: PermissaoModel = new PermissaoModel();

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
    this.GetPermissao(0);
  }

  GetPermissao(pemCodi: number) {
    this.boolLoading = true;
    try {
      this.http.GetPermissao(pemCodi).subscribe({
        next: (response) => {
          this.lstPermissao = response;
          // console.warn('Lista de Permissões:', this.lstPermissao);
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

  NovaPermissao() {
    this.boolManterRegistro = true;
    this.objPermissao.PemStat = true;
  }

  AtivaInativa(pemCodi: number, objPermissao: PermissaoModel) {
    objPermissao.PemStat = !objPermissao.PemStat;
    this.PutPermissao(pemCodi, objPermissao);
  }

  EditarRegistro(objPermissao: PermissaoModel) {
    this.boolManterRegistro = true;
    this.objPermissao = objPermissao;
  }

  PutPermissao(pemCodi: number, objPermissao: PermissaoModel) {
    try {
      this.boolLoading = true;
      this.http.PutPermissao(pemCodi, objPermissao).subscribe({
        next: (response) => {
          // console.warn("Retorno Alteração:", response);
          if (response === 'Alterado com sucesso!') {
            this.messageService.add({
              severity: 'success',
              summary: 'Sucesso!',
              detail: 'Registro alterado com sucesso!',
            });
            this.boolLoading = false;
            // this.GetSessaoByLojCodi(this.objUsuarioLogado.lojCodi);
          } else {
            console.error('Erro ao salvar o registro: ', response);
            this.messageService.add({
              severity: 'error',
              summary: 'Erro:',
              detail: 'Falha ao alterar o registro.',
            });
            this.boolLoading = false;
            // this.GetSessaoByLojCodi(this.objUsuarioLogado.lojCodi);
          }
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

  SalvarRegistro() {
    if (this.ValidaCampos()) {
      if (this.objPermissao.PemCodi === 0) {
        //-> Modo de Inserção
        try {
          this.boolLoading = true;
          this.http.PostPermissao(this.objPermissao).subscribe({
            next: (response) => {
              this.boolLoading = false;
              if (response === 'OK') {
                this.messageService.add({
                  severity: 'success',
                  summary: 'Sucesso!',
                  detail: 'Registro salvo com sucesso!',
                });
                this.CancelaRegitro();
                this.GetPermissao(0);
              } else {
                this.messageService.add({
                  severity: 'error',
                  summary: 'Erro:',
                  detail: 'Falha ao realizar a operação.',
                });
              }
            },
            error: (error) => {
              console.error('Erro ao carregar dados:', error);
              this.boolLoading = false;
              this.messageService.add({
                severity: 'error',
                summary: 'Erro:',
                detail: 'Falha ao realizar a operação.',
              });
            },
          });
        } catch (error) {
          console.error('Erro ao carregar dados:', error);
          this.boolLoading = false;
        }
      } else {
        this.boolLoading = true;
        //-> Modo de Edição
        try {
          this.http
            .PutPermissao(this.objPermissao.PemCodi, this.objPermissao)
            .subscribe({
              next: (response) => {
                this.boolLoading = false;
                if (response === 'Alterado com sucesso!') {
                  this.messageService.add({
                    severity: 'success',
                    summary: 'Sucesso!',
                    detail: 'Registro alterado com sucesso!',
                  });
                  this.CancelaRegitro();
                  this.GetPermissao(0);
                } else {
                  this.messageService.add({
                    severity: 'error',
                    summary: 'Erro:',
                    detail: 'Falha ao realizar a operação.',
                  });
                }
              },
              error: (error) => {
                console.error('Erro ao carregar dados:', error);
                this.boolLoading = false;
              },
            });
        } catch (error) {
          console.error('Erro ao carregar dados:', error);
          this.messageService.add({
            severity: 'error',
            summary: 'Erro:',
            detail: 'Falha ao realizar a operação.',
          });
          this.boolLoading = false;
        }
      }
    }
  }

  CancelaRegitro() {
    this.objPermissao = new PermissaoModel();
    this.boolManterRegistro = false;
    this.lstPermissao = [];

    this.GetPermissao(0);
  }

  ValidaCampos() {
    if (this.objPermissao.PemNome.length === 0) {
      this.messageService.add({
        severity: 'warn',
        summary: 'Atenção:',
        detail: 'Insira um nome para a permissão.',
      });
      return false;
    }

    return true;
  }

  onGlobalFilter(table: Table, event: Event) {
      table.filterGlobal((event.target as HTMLInputElement).value, 'contains');
    }
}
