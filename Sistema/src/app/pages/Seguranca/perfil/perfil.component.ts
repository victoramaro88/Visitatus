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

  NovoPerfil() {
    this.boolManterRegistro = true;
    this.objPerfil.PerStat = true;
  }

  AtivaInativa(perCodi: number, objPerfil: PerfilModel) {
    objPerfil.PerStat = !objPerfil.PerStat;
    this.PutPerfil(perCodi, objPerfil);
  }

  EditarRegistro(objPerfil: PerfilModel) {
    this.boolManterRegistro = true;
    this.objPerfil = objPerfil;
  }

  PutPerfil(perCodi: number, objPerfil: PerfilModel) {
    try {
      this.boolLoading = true;
      this.http.PutPerfil(perCodi, objPerfil).subscribe({
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
      //-> Validando se já possui uma sessão com este número, para esta Loja
      if (this.objPerfil.PerCodi === 0) {
        //-> Modo de Inserção
        try {
          this.boolLoading = true;
          this.http.PostPerfil(this.objPerfil).subscribe({
            next: (response) => {
              this.boolLoading = false;
              if (response === 'OK') {
                this.messageService.add({
                  severity: 'success',
                  summary: 'Sucesso!',
                  detail: 'Registro salvo com sucesso!',
                });
                this.CancelaRegitro();
                this.GetPerfil(0);
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
            .PutPerfil(this.objPerfil.PerCodi, this.objPerfil)
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
                  this.GetPerfil(0);
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
    this.objPerfil = new PerfilModel();
    this.boolManterRegistro = false;
    this.lstPerfil = [];

    this.GetPerfil(0);
  }

  ValidaCampos() {
    if (this.objPerfil.PerNome.length === 0) {
      this.messageService.add({
        severity: 'warn',
        summary: 'Atenção:',
        detail: 'Insira um nome para o perfil.',
      });
      return false;
    }

    return true;
  }

  onGlobalFilter(table: Table, event: Event) {
    table.filterGlobal((event.target as HTMLInputElement).value, 'contains');
  }
}
