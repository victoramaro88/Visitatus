import { Component, OnInit } from '@angular/core';
import { UsuarioModel } from '../../models/Usuario.Model';
import { UsuarioLogadoModel } from '../../models/UsuarioLogado.Model';
import { HttpService } from '../../services/http-service.service';
import { MessageService } from 'primeng/api';
import { Utils } from '../../services/utils';
import { ActivatedRoute, Router } from '@angular/router';
import { Base64Service } from '../../services/base64.service';
import { CryptoService } from '../../services/crypto.service';
import { ImportsModule } from '../../imports';
import { LojaModel } from '../../models/Loja.Model';
import { PerfilUsuarioListaModel } from '../../models/PerfilUsuarioLista.Model';
import { Table } from 'primeng/table';
import { PotenciaModel } from '../../models/Potencia.Model';
import { RitoModel } from '../../models/Rito.Model';
import { EstadoModel } from '../../models/Estado.Model';
import { CidadeModel } from '../../models/Cidade.Model';

@Component({
  selector: 'app-loja',
  standalone: true,
  imports: [ImportsModule],
  templateUrl: './loja.component.html',
  styleUrl: './loja.component.css',
  providers: [MessageService],
})
export class LojaComponent implements OnInit {
  orientacaoLoja: string = '';
  boolLoading = false;
  objUsuarioLogado: UsuarioLogadoModel = new UsuarioLogadoModel();
  objPerfilUsuario: PerfilUsuarioListaModel = new PerfilUsuarioListaModel();
  objUsuario: UsuarioModel = new UsuarioModel();

  boolManterRegistro: boolean = false;
  boolEditarRegistro: boolean = false;

  objLoja: LojaModel = new LojaModel(
    0,
    '',
    '',
    '',
    '',
    '',
    '',
    true,
    0,
    0,
    0
  );
  imagemPadrao: string = 'assets/img/noImage.png';
  imagemBase64: string | null = null;
  lstLoja: LojaModel[] = [];
  lstPotencia: PotenciaModel[] = [];
  objPotencia: PotenciaModel = new PotenciaModel();
  lstRito: RitoModel[] = [];
  objRito: RitoModel = new RitoModel();
  lstEstado: EstadoModel[] = [];
  objEstado: EstadoModel = new EstadoModel();
  lstCidade: CidadeModel[] = [];
  objCidade: CidadeModel = new CidadeModel();

  constructor(
      private http: HttpService,
      private messageService: MessageService,
      private utils: Utils,
      private router: Router,
      private route: ActivatedRoute,
      private base64Service: Base64Service,
      private cryptoService: CryptoService
    ) {
      this.objPerfilUsuario = JSON.parse(
        this.cryptoService.lerDoSessionStorage('prf')
      );

      this.objUsuarioLogado = JSON.parse(
        this.cryptoService.lerDoSessionStorage('usr')
      );

      // console.warn("Usuário Logado: ", this.objUsuarioLogado);
      // console.warn("Perfil (Menu): ", this.objPerfilUsuario);
    }

  ngOnInit() {
    try {
      this.GetLoja(this.objPerfilUsuario.lojCodi);
    } catch (error) {
      this.boolLoading = false;
      console.warn('Falha ao realizar a operação inicial.', error);
    }
  }

  GetLoja(lojCodi: number) {
    this.boolLoading = true;
    this.http.GetLoja(lojCodi).subscribe({
      next: (response) => {
        this.lstLoja = response;
        this.boolLoading = false;
        // console.warn('LISTA DE LOJAS:', this.lstLoja);
        this.objLoja = this.lstLoja[0];
        this.imagemBase64 = this.objLoja.LojLogo ? 'data:image/png;base64,' + this.objLoja.LojLogo : null;
        this.GetPotencia(0);
        this.GetRito(0);
        this.GetCidades(0);
      },
      error: (error) => {
        console.error('Erro ao carregar dados:', error);
        this.messageService.add({
          severity: 'error',
          summary: 'Erro: ',
          detail: 'Falha ao realizar a operação, contate o suporte.',
        });
        this.boolLoading = false;
      },
    });
  }

  GetPotencia(potCodi: number) {
    this.boolLoading = true;
    this.http.GetPotencia(potCodi).subscribe({
      next: (response) => {
        // this.lstPotencia = response;
        this.lstPotencia = response
        .filter((p: any) => p.potStat === true && p.potRegu === true)
        .map((p: any) => ({
          PotCodi: p.potCodi,
          PotNome: p.potNome,
          PotLogo: p.potLogo,
          PotRegu: p.potRegu,
          PotStat: p.potStat,
          PotSigl: p.potSigl
        }));
        this.LocalizaPotenciaLoja(this.objLoja.PotCodi);
        this.boolLoading = false;
        // console.warn('LISTA DE POTÊNCIAS:', this.lstPotencia);
      },
      error: (error) => {
        console.error('Erro ao carregar dados:', error);
        this.messageService.add({
          severity: 'error',
          summary: 'Erro: ',
          detail: 'Falha ao realizar a operação, contate o suporte.',
        });
        this.boolLoading = false;
      },
    });
  }

  GetRito(ritCodi: number) {
    this.boolLoading = true;
    this.http.GetRito(ritCodi).subscribe({
      next: (response) => {
        this.lstRito = response.filter(r => r.RitStat === true);
        this.LocalizaRitoLoja(this.objLoja.RitCodi);
        this.boolLoading = false;
        // console.warn('LISTA DE RITOS:', this.lstRito);
      },
      error: (error) => {
        console.error('Erro ao carregar dados:', error);
        this.messageService.add({
          severity: 'error',
          summary: 'Erro: ',
          detail: 'Falha ao realizar a operação, contate o suporte.',
        });
        this.boolLoading = false;
      },
    });
  }

  GetCidades(estCodi: number) {
    this.boolLoading = true;
    this.http.GetCidades(estCodi).subscribe({
      next: (response) => {
        this.lstCidade = response;
        this.LocalizaCidadeLoja(this.objLoja.CidCodi);
        this.GetEstado(0);
        this.boolLoading = false;
        // console.warn('LISTA DE CIDADES:', this.lstCidade);
      },
      error: (error) => {
        console.error('Erro ao carregar dados:', error);
        this.messageService.add({
          severity: 'error',
          summary: 'Erro: ',
          detail: 'Falha ao realizar a operação, contate o suporte.',
        });
        this.boolLoading = false;
      },
    });
  }

  GetEstado(estCodi: number) {
    this.boolLoading = true;
    this.http.GetEstados(estCodi).subscribe({
      next: (response) => {
        this.lstEstado = response;
        this.LocalizaEstadoLoja(this.objCidade.EstCodi);
        this.boolLoading = false;
        // console.warn('LISTA DE ESTADOS:', this.lstEstado);
      },
      error: (error) => {
        console.error('Erro ao carregar dados:', error);
        this.messageService.add({
          severity: 'error',
          summary: 'Erro: ',
          detail: 'Falha ao realizar a operação, contate o suporte.',
        });
        this.boolLoading = false;
      },
    });
  }

  NovoRegistro() {
    this.boolManterRegistro = true;
    this.boolEditarRegistro = true;
  }

  EditarRegistro(registro: string) {
    this.boolEditarRegistro = true;
    this.boolManterRegistro = true;
  }

  LocalizaPotenciaLoja(potCodi: number){
    this.objPotencia = this.lstPotencia.find(p => Number(p.PotCodi) === Number(potCodi))!;
  }

  LocalizaRitoLoja(ritCodi: number){
    this.objRito = this.lstRito.find(p => p.RitCodi === ritCodi)!;
  }

  LocalizaCidadeLoja(cidCodi: number){
    this.objCidade = this.lstCidade.find(p => p.CidCodi === cidCodi)!;
  }

  LocalizaEstadoLoja(estCodi: number){
    this.objEstado = this.lstEstado.find(p => p.EstCodi === estCodi)!;
  }

  SalvarRegistro(){
    console.warn('EDITOR:', this.orientacaoLoja);
  }

  CancelaRegitro(){}

  // carregar imagem vinda do banco
  carregarImagem(base64Banco: string | null): void {

    if (base64Banco) {

      // caso o banco salve apenas o base64 puro
      this.imagemBase64 =
        'data:image/jpeg;base64,' + base64Banco;

    }

  }

  // selecionar imagem do computador
onFileSelected(event: any): void {

  const file = event.target.files[0];

  if (!file) {
    return;
  }

  // valida tipo apenas png
  if (file.type !== 'image/png') {
    this.messageService.add({
      severity: 'warn',
      summary: 'Atenção! ',
      detail: 'Selecione apenas imagens do formato ".PNG"',
    });
    return;
  }

  const reader = new FileReader();

  reader.onload = (e: any) => {

    const img = new Image();

    img.onload = () => {

      const canvas = document.createElement('canvas');
      const ctx = canvas.getContext('2d');

      const maxWidth = 128;

      // mantém proporção
      const scale = maxWidth / img.width;

      canvas.width = maxWidth;
      canvas.height = img.height * scale;

      ctx?.drawImage(
        img,
        0,
        0,
        canvas.width,
        canvas.height
      );

      // gera base64 final
      this.imagemBase64 =
        canvas.toDataURL('image/png', 0.9);

    };

    img.src = e.target.result;

  };

  reader.readAsDataURL(file);

}

  onGlobalFilter(table: Table, event: Event) {
    table.filterGlobal((event.target as HTMLInputElement).value, 'contains');
  }
}
