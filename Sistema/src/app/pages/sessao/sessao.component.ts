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
import { Base64Service } from '../../services/base64.service';
import { PerfilUsuarioListaModel } from '../../models/PerfilUsuarioLista.Model';
import { QrCodeWrapperModule } from '../qrCodeWrapper/qr-code-wrapper.module';

@Component({
  selector: 'app-sessao',
  standalone: true,
  imports: [ImportsModule, QrCodeWrapperModule],
  templateUrl: './sessao.component.html',
  styleUrl: './sessao.component.css',
  providers: [MessageService],
})
export class SessaoComponent implements OnInit {
  boolLoading = true;

  lstSessao: SessaoListaModel[] = [];
  objSessao: SessaoListaModel = new SessaoListaModel(
    0,
    '',
    new Date(),
    false,
    true,
    0,
    0,
    '',
    0,
    '',
    0,
    ''
  );
  objUsuarioLogado: UsuarioLogadoModel = new UsuarioLogadoModel();
  objPerfilSelecionado: PerfilUsuarioListaModel = new PerfilUsuarioListaModel();
  lstLoja: LojaModel[] = [];
  lstTipoSessao: TipoSessaoModel[] = [];
  objTipoSessaoSelecionado: TipoSessaoModel = {
    TiScodi: 0,
    TiSnome: '',
    TiSstat: false,
  };
  lstGrau: GrauModel[] = [];
  objGrauSelecionado: GrauModel = { GraCodi: 0, GraNome: '', GraStat: false };
  boolManterRegistro: boolean = false;
  caracteresRestantesTexto: number = 500;
  caracteresRestantesNome: number = 100;
  opcoesSelect: any[] = [
    { label: 'Sim', value: true },
    { label: 'Não', value: false },
  ];
  boolDialogQRCode: boolean = false;
  valorQrCode: string = '';

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
    this.GetSessaoByLojCodi(this.objPerfilSelecionado.lojCodi);
    this.GetTipoSessao(0);
    this.GetGrau(0);
  }

  GetSessaoByLojCodi(lojCodi: number) {
    this.boolLoading = true;
    try {
      this.lstSessao = [];
      this.http.GetSessaoByLojCodi(lojCodi).subscribe({
        next: (response) => {
          this.lstSessao = response;
          // console.warn('Lista de Sessões:', this.lstSessao);
          this.boolLoading = false;
        },
        error: (error) => {
          console.error('Erro ao carregar dados:', error);
          this.boolLoading = false;
          this.messageService.add({
            severity: 'error',
            summary: 'Erro: ',
            detail: 'Falha ao realizar a operação, contate o suporte.',
          });
        },
      });
    } catch (error) {
      console.error('Erro ao carregar dados:', error);
      this.boolLoading = false;
      this.messageService.add({
        severity: 'error',
        summary: 'Erro: ',
        detail: 'Falha ao realizar a operação, contate o suporte.',
      });
    }
  }

  contadorCaracteres(limite: number, campo: string) {
    switch (campo) {
      case 'TEXTO':
        // Atualiza o contador de caracteres restantes
        this.caracteresRestantesTexto = limite - this.objSessao.SesDesc.length;
        // Limita o texto ao máximo permitido
        if (this.objSessao.SesDesc.length > limite) {
          this.objSessao.SesDesc = this.objSessao.SesDesc.substring(0, limite);
        }
        break;
      case 'NOME':
        this.caracteresRestantesNome = limite - this.objSessao.SesNome.length;
        if (this.objSessao.SesNome.length > limite) {
          this.objSessao.SesNome = this.objSessao.SesNome.substring(0, limite);
        }
        break;

      default:
        break;
    }
  }

  ConcatenaLojaNumero(objLoja: LojaModel): string {
    return objLoja.LojNome + ' - ' + objLoja.LojNumL;
  }

  GetTipoSessao(TiScodi: number) {
    try {
      this.boolLoading = true;
      this.http.GetTipoSessao(TiScodi).subscribe({
        next: (response) => {
          this.lstTipoSessao = response;
          // console.warn("Tipo de Sessão:", this.lstTipoSessao);
          // this.boolLoading = false;
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

  GetGrau(GraCodi: number) {
    try {
      this.boolLoading = true;
      this.http.GetGrau(GraCodi).subscribe({
        next: (response) => {
          this.lstGrau = response;
          // console.warn("Grau:", this.lstGrau);
          // this.boolLoading = false;
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

  NovaSessao() {
    this.boolManterRegistro = true;
    this.objSessao.SesNume = this.lstSessao[0]
      ? this.lstSessao[0].SesNume + 1
      : 1;
  }

  EditarRegistro(objSessao: SessaoListaModel) {
    this.boolManterRegistro = true;
    this.objSessao = objSessao;
    this.objTipoSessaoSelecionado = this.lstTipoSessao.find(
      (ts) => ts.TiScodi === objSessao.TiSCodi
    )!;
    this.objGrauSelecionado = this.lstGrau.find(
      (g) => g.GraCodi === objSessao.GraCodi
    )!;
    this.objSessao.SesDtHr = new Date(objSessao.SesDtHr);
  }

  SalvarRegistro() {
    this.objSessao.GraCodi = this.objGrauSelecionado.GraCodi;
    this.objSessao.TiSCodi = this.objTipoSessaoSelecionado.TiScodi;
    this.objSessao.LojCodi = this.objPerfilSelecionado.lojCodi;
    const dataSelecionada = this.objSessao.SesDtHr;
    const dataUtc = new Date(
      Date.UTC(
        dataSelecionada.getFullYear(),
        dataSelecionada.getMonth(),
        dataSelecionada.getDate(),
        dataSelecionada.getHours(),
        dataSelecionada.getMinutes(),
        dataSelecionada.getSeconds()
      )
    );
    this.objSessao.SesDtHr = dataUtc;

    if (this.ValidaCampos()) {
      //-> Validando se já possui uma sessão com este número, para esta Loja
      if (
        this.objSessao.SesNume > 0 &&
        this.objSessao.LojCodi > 0 &&
        this.objSessao.SesCodi === 0
      ) {
        //-> Modo de Inserção
        try {
          this.boolLoading = true;
          this.http
            .GetValidaNumeroSessao(
              this.objSessao.SesNume,
              this.objSessao.LojCodi
            )
            .subscribe({
              next: (response) => {
                if (response && response.SesCodi > 0) {
                  this.boolLoading = false;
                  this.messageService.add({
                    severity: 'warn',
                    summary: 'Atenção:',
                    detail:
                      'Já existe uma sessão com este número para esta Loja.',
                  });
                } else {
                  //-> Se validou, salva as informações.
                  this.http.PostSessao(this.objSessao).subscribe({
                    next: (response) => {
                      this.boolLoading = false;
                      if (response === 'OK') {
                        this.messageService.add({
                          severity: 'success',
                          summary: 'Sucesso!',
                          detail: 'Registro salvo com sucesso!',
                        });
                        this.CancelaRegitro();
                        this.GetSessaoByLojCodi(
                          this.objPerfilSelecionado.lojCodi
                        );
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
      } else {
        this.boolLoading = true;
        //-> Modo de Edição
        try {
          let objPutSessao: SessaoModel = {
            SesCodi: this.objSessao.SesCodi,
            SesDesc: this.objSessao.SesDesc,
            SesNome: this.objSessao.SesNome,
            SesDtHr: this.objSessao.SesDtHr,
            SesLibe: this.objSessao.SesLibe,
            SesStat: this.objSessao.SesStat,
            LojCodi: this.objSessao.LojCodi,
            GraCodi: this.objSessao.GraCodi,
            TiScodi: this.objSessao.TiSCodi,
            SesNume: this.objSessao.SesNume,
          };

          this.http.PutSessao(this.objSessao.SesCodi, objPutSessao).subscribe({
            next: (response) => {
              this.boolLoading = false;
              if (response === 'Alterado com sucesso!') {
                this.messageService.add({
                  severity: 'success',
                  summary: 'Sucesso!',
                  detail: 'Registro alterado com sucesso!',
                });
                this.CancelaRegitro();
                this.GetSessaoByLojCodi(this.objPerfilSelecionado.lojCodi);
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

  ValidaCampos() {
    if (
      !this.objSessao.SesNume ||
      this.objSessao.SesNume === 0 ||
      !/^[0-9]*$/.test(this.objSessao.SesNume.toString())
    ) {
      this.messageService.add({
        severity: 'warn',
        summary: 'Atenção:',
        detail: 'Insira um número de sessão válido.',
      });
      return false;
    }
    if (this.objSessao.SesNome.length === 0) {
      this.messageService.add({
        severity: 'warn',
        summary: 'Atenção:',
        detail: 'Insira um nome para a sessão.',
      });
      return false;
    }
    if (this.objSessao.SesDesc.length === 0) {
      this.messageService.add({
        severity: 'warn',
        summary: 'Atenção:',
        detail: 'Insira um texto para a sessão.',
      });
      return false;
    }
    if (this.objSessao.LojCodi === 0) {
      this.messageService.add({
        severity: 'warn',
        summary: 'Atenção:',
        detail: 'Selecione uma loja.',
      });
      return false;
    }
    if (this.objSessao.TiSCodi === 0) {
      this.messageService.add({
        severity: 'warn',
        summary: 'Atenção:',
        detail: 'Selecione um tipo de sessão.',
      });
      return false;
    }
    if (this.objSessao.GraCodi === 0) {
      this.messageService.add({
        severity: 'warn',
        summary: 'Atenção:',
        detail: 'Selecione um grau para a sessão.',
      });
      return false;
    }
    if (this.objSessao.SesLibe && !this.objSessao.SesStat) {
      this.messageService.add({
        severity: 'warn',
        summary: 'Atenção:',
        detail:
          'Não é possível liberar a sessão com ela inativa. Ative a sessão para liberá-la.',
      });
      return false;
    }

    return true;
  }

  CancelaRegitro() {
    this.objSessao = new SessaoListaModel(
      0,
      '',
      new Date(),
      false,
      false,
      0,
      0,
      '',
      0,
      '',
      0,
      ''
    );
    this.objGrauSelecionado = new GrauModel(0, '', false);
    this.objTipoSessaoSelecionado = new TipoSessaoModel(0, '', false);
    this.boolManterRegistro = false;
    this.lstSessao = [];

    this.GetSessaoByLojCodi(this.objPerfilSelecionado.lojCodi);
  }

  PutSessao(sesCodi: number, objSessao: SessaoModel) {
    try {
      this.boolLoading = true;
      this.http.PutSessao(sesCodi, objSessao).subscribe({
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

  LiberaTravaSessao(sesCodi: number, objSessao: SessaoModel) {
    objSessao.SesLibe = !objSessao.SesLibe;
    this.PutSessao(sesCodi, objSessao);
  }

  AtivaInativaSessao(sesCodi: number, objSessao: SessaoModel) {
    objSessao.SesStat = !objSessao.SesStat;
    this.PutSessao(sesCodi, objSessao);
  }

  GetSessaoBySesCodi(lojCodi: number) {
    this.boolLoading = true;
    const url =
      window.location.origin +
      '/convite?data=' +
      encodeURIComponent(
        this.cryptoService.criptografar(
          this.base64Service.convertNumberToBase64(lojCodi)
        )
      );
    const novaJanela = window.open(url, '_blank');
    if (!novaJanela) {
      alert(
        'Pop-up bloqueado pelo navegador. Por favor, permita pop-ups para abrir a nova janela.'
      );
    }
    this.boolLoading = false;
  }

  MarcarPresencas(SesCodi: number) {
    this.router.navigate(['/presenca-sessao', SesCodi]);
  }

  GerarQRCode(SesCodi: number){
    this.valorQrCode =
      window.location.origin +
      '/convite?data=' +
      encodeURIComponent(
        this.cryptoService.criptografar(
          this.base64Service.convertNumberToBase64(SesCodi)
        )
      );
    this.boolDialogQRCode = true;
  }

  onGlobalFilter(table: Table, event: Event) {
    table.filterGlobal((event.target as HTMLInputElement).value, 'contains');
  }
}
