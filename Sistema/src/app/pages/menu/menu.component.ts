import { Component, OnInit } from '@angular/core';
import { ImportsModule } from '../../imports';
import { MenuItem, MessageService } from 'primeng/api';
import { PerfilUsuarioListaModel } from '../../models/PerfilUsuarioLista.Model';
import { CryptoService } from '../../services/crypto.service';
import { UsuarioLogadoModel } from '../../models/UsuarioLogado.Model';
import { Router } from '@angular/router';

@Component({
  selector: 'app-menu',
  standalone: true,
  imports: [ImportsModule],
  templateUrl: './menu.component.html',
  styleUrl: './menu.component.css',
  providers: [MessageService]
})
export class MenuComponent implements OnInit {

  items: MenuItem[] | undefined;
  objPerfilUsuario: PerfilUsuarioListaModel = new PerfilUsuarioListaModel();
  objUsuarioLogado: UsuarioLogadoModel = new UsuarioLogadoModel();

  constructor(
    private messageService: MessageService,
    private cryptoService: CryptoService,
    private router: Router
  ) {
    this.objUsuarioLogado = JSON.parse(this.cryptoService.lerDoSessionStorage("usr"));
    // console.warn("Usuário Logado: ", this.objPerfilUsuario);
    this.objPerfilUsuario = JSON.parse(this.cryptoService.lerDoSessionStorage("prf"));
    // console.warn("Perfil: ", this.objPerfilUsuario);
  }

  ngOnInit() {
    this.objPerfilUsuario = JSON.parse(this.cryptoService.lerDoSessionStorage("prf"));
    this.items = [
        {
          label: 'Home',
          icon: 'pi pi-home',
          command: () => {
            this.router.navigate(['/home']);
          }
        },
        {
            label: 'Cadastros',
            icon: 'pi pi-book',
            items: [
                {
                    label: 'Loja',
                    icon: 'pi pi-warehouse'
                },
                {
                    label: 'Usuario',
                    icon: 'pi pi-users'
                },
                {
                    label: 'Sessão',
                    icon: 'pi pi-pencil',
                    command: () => {
                      this.router.navigate(['/sessao']);
                    }
                },
                {
                    separator: true
                },
                {
                    label: 'Templates',
                    icon: 'pi pi-palette',
                    items: [
                        {
                            label: 'Apollo',
                            icon: 'pi pi-palette',
                            badge: '2'
                        },
                        {
                            label: 'Ultima',
                            icon: 'pi pi-palette',
                            badge: '3'
                        }
                    ]
                }
            ]
        },
        // {
        //     label: 'Features',
        //     icon: 'pi pi-star'
        // },
        {
          label: 'Contato',
          icon: 'pi pi-envelope',
          command: () => {
            this.router.navigate(['/contato']);
          }
        },
        {
          label: 'Template (tmp)',
          // icon: 'pi pi-home',
          command: () => {
            this.router.navigate(['/template']);
          }
        },
    ];
  }

  Logout() {
    sessionStorage.clear();
    localStorage.clear();
    this.router.navigate(['/login']);
  }
}
