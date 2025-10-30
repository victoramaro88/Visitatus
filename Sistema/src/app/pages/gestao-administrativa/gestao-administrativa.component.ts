import { Component, OnInit } from '@angular/core';
import { ImportsModule } from '../../imports';
import { MessageService } from 'primeng/api';
import { UsuarioLogadoModel } from '../../models/UsuarioLogado.Model';
import { PerfilUsuarioListaModel } from '../../models/PerfilUsuarioLista.Model';
import { HttpService } from '../../services/http-service.service';
import { Utils } from '../../services/utils';
import { Router } from '@angular/router';
import { Base64Service } from '../../services/base64.service';
import { CryptoService } from '../../services/crypto.service';
import { GestaoAdministrativa } from '../../models/GestaoAdministrativa.Model';
import { Table } from 'primeng/table';

@Component({
  selector: 'app-gestao-administrativa',
  standalone: true,
  imports: [ImportsModule],
  templateUrl: './gestao-administrativa.component.html',
  styleUrl: './gestao-administrativa.component.css',
  providers: [MessageService],
})
export class GestaoAdministrativaComponent implements OnInit {
  boolLoading = false;
  objUsuarioLogado: UsuarioLogadoModel = new UsuarioLogadoModel();
  objPerfilSelecionado: PerfilUsuarioListaModel = new PerfilUsuarioListaModel();

  boolManterRegistro: boolean = false;
  lstGestaoAdm: GestaoAdministrativa[] = [];
  objGestaoAdm: GestaoAdministrativa = new GestaoAdministrativa();

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
    this.GetListaGestaoAdmByLojCodi(this.objPerfilSelecionado.lojCodi);
  }

  GetListaGestaoAdmByLojCodi(lojCodi: number) {
    this.boolLoading = true;
    try {
      this.http.GetListaGestaoAdmByLojCodi(lojCodi).subscribe({
        next: (response) => {
          this.lstGestaoAdm = response;
          console.warn('Lista de Gestões:', this.lstGestaoAdm);
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

  NovaGestao() {
    this.boolManterRegistro = true;
  }

  SalvarRegistro() {
    if (this.ValidaCampos()) {
      console.warn('INSERE!');
      console.warn('OBJ ENVIO: ', this.objGestaoAdm);
    }
  }

  CancelaRegitro() {
    this.objGestaoAdm = new GestaoAdministrativa();
    this.boolManterRegistro = false;
    this.lstGestaoAdm = [];
    this.ngOnInit();
  }

  ValidaCampos() {
    if (this.objGestaoAdm.GstAdmNome.length <= 3) {
      this.messageService.add({
        severity: 'warn',
        summary: 'Atenção:',
        detail: 'Insira uma descrição para a gestão.',
      });
      return false;
    }
    if (this.objGestaoAdm.GstAdmDtIn === this.objGestaoAdm.GstAdmDtFi) {
      this.messageService.add({
        severity: 'warn',
        summary: 'Atenção:',
        detail: 'As datas não podem ser iguais.',
      });
      return false;
    }
    if (this.objGestaoAdm.GstAdmDtIn >= this.objGestaoAdm.GstAdmDtFi) {
      this.messageService.add({
        severity: 'warn',
        summary: 'Atenção:',
        detail: 'A data final não pode ser maior que a data inicial.',
      });
      return false;
    }

    return true;
  }

  onGlobalFilter(table: Table, event: Event) {
    table.filterGlobal((event.target as HTMLInputElement).value, 'contains');
  }
}
