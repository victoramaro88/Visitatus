import { Component, OnInit } from '@angular/core';
import { PerfilUsuarioListaModel } from '../../models/PerfilUsuarioLista.Model';
import { HttpService } from '../../services/http-service.service';
import { MessageService } from 'primeng/api';
import { ImportsModule } from '../../imports';
import { CryptoService } from '../../services/crypto.service';
import { Router } from '@angular/router';
import { Utils } from '../../services/utils';
import { PermissaoPerfilListaModel } from '../../models/PermissaoPerfilLista.Model ';

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [ImportsModule],
  templateUrl: './home.component.html',
  styleUrl: './home.component.css',
  providers: [MessageService]
})
export class HomeComponent implements OnInit {

  boolLoading = true;

  objPerfilUsuario: PerfilUsuarioListaModel = new PerfilUsuarioListaModel();
  lstPermissaoPerfil: PermissaoPerfilListaModel[] = [];

  constructor(
    private http: HttpService,
    private messageService: MessageService,
    private utils: Utils,
    private router: Router,
    private cryptoService: CryptoService
  ) {
    this.objPerfilUsuario = JSON.parse(this.cryptoService.lerDoSessionStorage("prf"));
    // console.warn(this.objPerfilUsuario);
  }

  ngOnInit(): void {
    this.GetPermissaoPerfil(this.objPerfilUsuario.perCodi);
  }

  GetPermissaoPerfil(perCodi: number) {
    try {
      this.http.GetPermissaoPerfil(perCodi).subscribe({
        next: (response) => {
          this.lstPermissaoPerfil = response;
          console.warn("Lista de Permissões do Perfil do Usuário:", this.lstPermissaoPerfil);
          this.boolLoading = false;
        },
        error: (error) => {
          console.error('Erro ao carregar dados:', error);
          this.boolLoading = false;
        }
      });
    } catch (error) {
      console.error('Erro ao carregar dados:', error);
      this.boolLoading = false;
    }
  }

}
