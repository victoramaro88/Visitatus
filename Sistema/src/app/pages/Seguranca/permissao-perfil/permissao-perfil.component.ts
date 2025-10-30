import { Component, OnInit } from '@angular/core';
import { ImportsModule } from '../../../imports';
import { MessageService } from 'primeng/api';
import { UsuarioLogadoModel } from '../../../models/UsuarioLogado.Model';
import { PerfilUsuarioListaModel } from '../../../models/PerfilUsuarioLista.Model';
import { PerfilModel } from '../../../models/Perfil.Model';
import { HttpService } from '../../../services/http-service.service';
import { Utils } from '../../../services/utils';
import { Router } from '@angular/router';
import { Base64Service } from '../../../services/base64.service';
import { CryptoService } from '../../../services/crypto.service';
import { PermissaoPerfilListaModel } from '../../../models/PermissaoPerfilLista.Model ';

@Component({
  selector: 'app-permissao-perfil',
  standalone: true,
  imports: [ImportsModule],
  templateUrl: './permissao-perfil.component.html',
  styleUrl: './permissao-perfil.component.css',
  providers: [MessageService],
})
export class PermissaoPerfilComponent implements OnInit {
  boolLoading = false;
  objUsuarioLogado: UsuarioLogadoModel = new UsuarioLogadoModel();
  objPerfilSelecionado: PerfilUsuarioListaModel = new PerfilUsuarioListaModel();

  lstPerfil: PerfilModel[] = [];
  objPerfil: PerfilModel = new PerfilModel();
  lstPermissaoPerfil: PermissaoPerfilListaModel[] = [];

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
    this.GetPerfil(0);
  }

  GetPerfil(perCodi: number) {
    this.boolLoading = true;
    try {
      this.http.GetPerfil(perCodi).subscribe({
        next: (response) => {
          this.lstPerfil = response.filter((pp) => pp.PerStat === true);
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

  GetPermissaoPerfil(perCodi: number) {
    this.boolLoading = true;
    try {
      this.http.GetPermissaoPerfil(perCodi).subscribe({
        next: (response) => {
          this.lstPermissaoPerfil = response.filter(
            (pp) => pp.pemStat === true
          );
          // console.warn('Permissões do Perfil:', this.lstPermissaoPerfil);
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

  Salvar() {
    if (this.lstPermissaoPerfil.length > 0) {
      this.boolLoading = true;
      // console.warn(this.lstPermissaoPerfil);

      try {
        this.http.PutPermissaoPerfil(this.lstPermissaoPerfil).subscribe({
          next: (response) => {
            // console.warn('Retorno:', response);
            this.boolLoading = false;

            this.messageService.add({
              severity: 'success',
              summary: 'Sucesso!',
              detail: 'Registros alterados com sucesso!',
            });
          },
          error: (error) => {
            console.error('Erro ao carregar dados:', error);
            this.boolLoading = false;
            this.messageService.add({
              severity: 'error',
              summary: 'Erro: ',
              detail: 'Falha ao realizar a operação, contate o Administrador.',
            });
          },
        });
      } catch (error) {
        console.error('Erro ao carregar dados:', error);
        this.boolLoading = false;
        this.messageService.add({
          severity: 'error',
          summary: 'Erro: ',
          detail: 'Falha ao realizar a operação, contate o Administrador.',
        });
      }
    } else {
      this.messageService.add({
        severity: 'warn',
        summary: 'Atenção: ',
        detail: 'Selecione um perfil.',
      });
    }
  }
}
