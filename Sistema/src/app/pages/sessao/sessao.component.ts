import { Component, OnInit } from '@angular/core';
import { ImportsModule } from '../../imports';
import { MessageService } from 'primeng/api';
import { HttpService } from '../../services/http-service.service';
import { Utils } from '../../services/utils';
import { Router } from '@angular/router';
import { CryptoService } from '../../services/crypto.service';
import { SessaoModel } from '../../models/Sessao.Model';
import { UsuarioLogadoModel } from '../../models/UsuarioLogado.Model';
import { SessaoListaModel } from '../../models/SessaoLista.Model';
import { Table } from 'primeng/table';

@Component({
  selector: 'app-sessao',
  standalone: true,
  imports: [ImportsModule],
  templateUrl: './sessao.component.html',
  styleUrl: './sessao.component.css',
  providers: [MessageService]
})
export class SessaoComponent implements OnInit {

  boolLoading = true;

  lstSessao: SessaoListaModel[] = [];
  objSessao: SessaoListaModel | undefined;
  objUsuarioLogado: UsuarioLogadoModel = new UsuarioLogadoModel();
  boolManterRegistro: boolean = false;

  constructor(
    private http: HttpService,
    private messageService: MessageService,
    private utils: Utils,
    private router: Router,
    private cryptoService: CryptoService
  ) {
    // this.objPerfilUsuario = JSON.parse(this.cryptoService.lerDoSessionStorage("prf"));
    // console.warn(this.objPerfilUsuario);
    this.objUsuarioLogado = JSON.parse(this.cryptoService.lerDoSessionStorage("usr"));
    console.warn("Usuário Logado: ", this.objUsuarioLogado);
  }

  ngOnInit() {
    this.GetSessaoByLojCodi(this.objUsuarioLogado.lojCodi);
  }

  GetSessaoByLojCodi(lojCodi: number) {
    try {
      this.boolLoading = true;
      this.http.GetSessaoByLojCodi(lojCodi).subscribe({
        next: (response) => {
          this.lstSessao = response;
          console.warn("Lista de Sessões:", this.lstSessao);
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

  onGlobalFilter(table: Table, event: Event) {
    table.filterGlobal((event.target as HTMLInputElement).value, 'contains');
  }

  EditarRegistro(objSessao: SessaoListaModel) {
    this.boolManterRegistro = true;
    this.objSessao = objSessao;
    console.warn("Obj para Editar: ", this.objSessao);
  }

  CancelaRegitro() {
    this.objSessao = undefined;
    this.boolManterRegistro = false;
    this.GetSessaoByLojCodi(this.objUsuarioLogado.lojCodi);
  }

}
