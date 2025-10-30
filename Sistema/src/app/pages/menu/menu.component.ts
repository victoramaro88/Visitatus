import { Component, OnInit } from '@angular/core';
import { ImportsModule } from '../../imports';
import { MenuItem, MessageService } from 'primeng/api';
import { PerfilUsuarioListaModel } from '../../models/PerfilUsuarioLista.Model';
import { CryptoService } from '../../services/crypto.service';
import { UsuarioLogadoModel } from '../../models/UsuarioLogado.Model';
import { Router } from '@angular/router';
import { Base64Service } from '../../services/base64.service';
import { HttpService } from '../../services/http-service.service';
import { PermissaoPerfilListaModel } from '../../models/PermissaoPerfilLista.Model ';

@Component({
  selector: 'app-menu',
  standalone: true,
  imports: [ImportsModule],
  templateUrl: './menu.component.html',
  styleUrl: './menu.component.css',
  providers: [MessageService],
})
export class MenuComponent implements OnInit {
  boolLoading = true;

  items: MenuItem[] | undefined;
  objPerfilUsuario: PerfilUsuarioListaModel = new PerfilUsuarioListaModel();
  objUsuarioLogado: UsuarioLogadoModel = new UsuarioLogadoModel();
  lstPermissaoPerfil: PermissaoPerfilListaModel[] = [];

  constructor(
    private http: HttpService,
    private messageService: MessageService,
    private cryptoService: CryptoService,
    private router: Router,
    private base64Service: Base64Service
  ) {
    this.objUsuarioLogado = JSON.parse(
      this.cryptoService.lerDoSessionStorage('usr')
    );
    // console.warn("Usuário Logado: ", this.objPerfilUsuario);
    this.objPerfilUsuario = JSON.parse(
      this.cryptoService.lerDoSessionStorage('prf')
    );
    // console.warn("Perfil (Menu): ", this.objPerfilUsuario);
  }

  ngOnInit() {
    this.objPerfilUsuario = JSON.parse(
      this.cryptoService.lerDoSessionStorage('prf')
    );

    if (this.objPerfilUsuario && this.objPerfilUsuario.perCodi > 0) {
      this.GetPermissaoPerfil(this.objPerfilUsuario.perCodi);
    }
  }

  Logout() {
    sessionStorage.clear();
    localStorage.clear();
    this.router.navigate(['/login']);
  }

  GetPermissaoPerfil(perCodi: number) {
    try {
      this.http.GetPermissaoPerfil(perCodi).subscribe({
        next: (response) => {
          this.lstPermissaoPerfil = response;
          // console.warn(
          //   'Lista de Permissões do Perfil do Usuário:',
          //   this.lstPermissaoPerfil
          // );

          //-> ATRIBUINDO A PERMISSÃO DE ACESSO AOS MENUS PARA O PERFIL SELECIONADO
          this.items = [
            {
              label: 'Home',
              icon: 'pi pi-home',
              command: () => {
                this.router.navigate(['/home']);
              },
            },
            {
              label: 'Cadastros',
              icon: 'pi pi-book',
              visible: this.ValidaMenu(7),
              items: [
                {
                  label: 'Loja',
                  icon: 'pi pi-warehouse',
                  visible: this.ValidaMenu(1),
                  command: () => {
                    this.router.navigate(['/loja']);
                  },
                },
                {
                  label: 'Usuário',
                  icon: 'pi pi-users',
                  visible: this.ValidaMenu(6),
                  command: () => {
                    this.router.navigate(['/usuario']);
                  },
                },
                {
                  label: 'Sessão',
                  icon: 'pi pi-pencil',
                  visible: this.ValidaMenu(4),
                  command: () => {
                    this.router.navigate(['/sessao']);
                  },
                },
                {
                  label: 'Gestão Administrativa',
                  icon: 'pi pi-sitemap',
                  visible: this.ValidaMenu(8),
                  command: () => {
                    this.router.navigate(['/gestao-adm']);
                  },
                },
                // {
                //   separator: true,
                // },
                // {
                //   label: 'Templates',
                //   icon: 'pi pi-palette',
                //   items: [
                //     {
                //       label: 'Apollo',
                //       icon: 'pi pi-palette',
                //       badge: '2',
                //     },
                //     {
                //       label: 'Ultima',
                //       icon: 'pi pi-palette',
                //       badge: '3',
                //     },
                //   ],
                // },
              ],
            },
            {
              label: 'Configurações',
              visible: this.ValidaMenu(5),
              items: [
                {
                  label: 'Perfis',
                  command: () => {
                    this.router.navigate(['/perfil']);
                  },
                },
                {
                  label: 'Permissões',
                  command: () => {
                    this.router.navigate(['/permissao']);
                  },
                },
                {
                  label: 'Permissões por Perfil',
                  command: () => {
                    this.router.navigate(['/permissao-perfil']);
                  },
                },
              ],
            },
            // {
            //   label: 'Contato',
            //   icon: 'pi pi-envelope',
            //   command: () => {
            //     this.router.navigate(['/contato']);
            //   },
            // },
            // {
            //   label: 'Temporários',
            //   items: [
            //     {
            //       label: 'Templates',
            //       command: () => {
            //         this.router.navigate(['/template']);
            //       },
            //     },
            //     {
            //       label: 'Certificado',
            //       command: () => {
            //         this.router.navigate(['/certificado']);
            //       },
            //     },
            //     {
            //       label: 'Confirmação de Presença',
            //       command: () => {
            //         this.GetSessaoBySesCodi(4); // -> Código da sessão para testes: 4
            //       },
            //     },
            //   ],
            // },
          ];
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

  ValidaMenu(pemCodiMenu: number) {
    let itemPermissao = this.lstPermissaoPerfil.find(
      (p) => p.pemCodi === pemCodiMenu
    );
    return itemPermissao?.papAtvo;
  }
}
