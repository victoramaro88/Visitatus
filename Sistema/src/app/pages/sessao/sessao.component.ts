import { GrauModel } from './../../models/Grau.Model';
import { LojaModel } from './../../models/Loja.Model';
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
import { TipoSessaoModel } from '../../models/TipoSessao.Model';

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
  objSessao: SessaoListaModel = new SessaoListaModel(0, '', new Date(), false, false, 0, 0, '', 0, '', 0, '');
  objUsuarioLogado: UsuarioLogadoModel = new UsuarioLogadoModel();
  lstLoja: LojaModel[] = [];
  objLojaSelecionada: LojaModel = {LojCodi: 0,LojNome: "",LojNumL: "",LojLogo: "",LojLogr: "",LojNume: "",LojBair: "",LojStat: false,CidCodi: 0,PotCodi: 0,RitCodi: 0};
  lstTipoSessao: TipoSessaoModel[] = [];
  objTipoSessaoSelecionado: TipoSessaoModel = { TiScodi: 0, TiSnome: "", TiSstat: false };
  lstGrau: GrauModel[] = [];
  objGrauSelecionado: GrauModel = {GraCodi: 0,GraNome: "",GraStat: false};
  boolManterRegistro: boolean = true; //false;

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
    // console.warn("Usuário Logado: ", this.objUsuarioLogado);
  }

  ngOnInit() {
    this.GetSessaoByLojCodi(this.objUsuarioLogado.lojCodi);
    this.GetLojaByIdUsuario(this.objUsuarioLogado.usuCodi);
    this.GetTipoSessao(0);
    this.GetGrau(0);
  }

  GetSessaoByLojCodi(lojCodi: number) {
    try {
      this.boolLoading = true;
      this.http.GetSessaoByLojCodi(lojCodi).subscribe({
        next: (response) => {
          this.lstSessao = response;
          // console.warn("Lista de Sessões:", this.lstSessao);
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

  ConcatenaLojaNumero(objLoja: LojaModel): string {
    return objLoja.LojNome + " - " + objLoja.LojNumL
  }

  GetLojaByIdUsuario(usuCodi: number) {
    try {
      this.boolLoading = true;
      this.http.GetLojaByIdUsuario(usuCodi).subscribe({
        next: (response) => {
          this.lstLoja = response.map(loja => ({
            ...loja,
            displayName: `${loja.LojNome} - ${loja.LojNumL}` // Cria o campo concatenado
          }));
          // console.warn("Lista de Lojas do Usuário:", this.lstLoja);
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

  GetTipoSessao(TiScodi: number) {
    try {
      this.boolLoading = true;
      this.http.GetTipoSessao(TiScodi).subscribe({
        next: (response) => {
          this.lstTipoSessao = response;
          // console.warn("Tipo de Sessão:", this.lstTipoSessao);
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

  GetGrau(GraCodi: number) {
    try {
      this.boolLoading = true;
      this.http.GetGrau(GraCodi).subscribe({
        next: (response) => {
          this.lstGrau = response;
          // console.warn("Grau:", this.lstGrau);
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

  SalvarRegistro() {
    this.objSessao.TiSCodi = this.objTipoSessaoSelecionado.TiScodi;
    this.objSessao.LojCodi = this.objLojaSelecionada.LojCodi;
    console.warn(this.objSessao);
  }

  CancelaRegitro() {
    this.objSessao = new SessaoListaModel(0, '', new Date(), false, false, 0, 0, '', 0, '', 0, '');
    this.boolManterRegistro = false;
    this.GetSessaoByLojCodi(this.objUsuarioLogado.lojCodi);
  }

  PutSessao(sesCodi: number, objSessao: SessaoModel) {
    try {
      this.boolLoading = true;
      this.http.PutSessao(sesCodi, objSessao).subscribe({
        next: (response) => {
          // console.warn("Retorno Alteração:", response);
          if (response === 'Alterado com sucesso!') {
            this.messageService.add({ severity: 'success', summary: 'Sucesso!', detail: 'Registro alterado com sucesso!' });
            this.boolLoading = false;
            this.GetSessaoByLojCodi(this.objUsuarioLogado.lojCodi);
          } else {
            console.error('Erro ao salvar o registro: ', response);
            this.messageService.add({ severity: 'error', summary: 'Erro:', detail: 'Falha ao alterar o registro.' });
            this.boolLoading = false;
            this.GetSessaoByLojCodi(this.objUsuarioLogado.lojCodi);
          }
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

  LiberaTravaSessao(sesCodi: number, objSessao: SessaoModel) {
    objSessao.SesLibe = !objSessao.SesLibe;
    this.PutSessao(sesCodi, objSessao);
  }

  AtivaInativaSessao(sesCodi: number, objSessao: SessaoModel) {
    objSessao.SesStat = !objSessao.SesStat;
    this.PutSessao(sesCodi, objSessao);
  }

}
