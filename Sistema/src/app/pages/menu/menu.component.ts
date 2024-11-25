import { Component, OnInit } from '@angular/core';
import { ImportsModule } from '../../imports';
import { MenuItem, MessageService } from 'primeng/api';
import { PerfilUsuarioListaModel } from '../../models/PerfilUsuarioLista.Model';
import { CryptoService } from '../../services/crypto.service';
import { UsuarioLogadoModel } from '../../models/UsuarioLogado.Model';
import { Router } from '@angular/router';
import { Base64Service } from '../../services/base64.service';

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
    private router: Router,
    private base64Service: Base64Service
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
            label: 'Temporários',
            // icon: 'pi pi-book',
            items: [
              {
                label: 'Templates',
                command: () => {
                  this.router.navigate(['/template']);
                }
              },
              {
                label: 'Confirmação de Presença',
                command: () => {
                  this.GetSessaoBySesCodi(4); // -> Código da sessão para testes: 4
                }
            },
          ]
        }
    ];
  }

  Logout() {
    sessionStorage.clear();
    localStorage.clear();
    this.router.navigate(['/login']);
  }

  GetSessaoBySesCodi(lojCodi: number) {
    // this.router.navigate(['/convite', this.cryptoService.criptografar(this.base64Service.convertNumberToBase64(lojCodi))]);

    // Criar a árvore da URL corretamente
    const urlTree = this.router.createUrlTree(['/confirmacao', this.cryptoService.criptografar(this.base64Service.convertNumberToBase64(lojCodi))]);

    // Serializar a URL com base na rota configurada
    const url = this.router.serializeUrl(urlTree);

    // Obter o baseHref configurado na aplicação (para contextos específicos)
    const baseHref = document.getElementsByTagName('base')[0]?.href || '';

    // Concatenar a URL final corretamente
    const fullUrl = baseHref.replace(/\/$/, '') + url;

    // Abrir a nova aba com a URL corrigida
    window.open(fullUrl, '_blank');
  }
}
