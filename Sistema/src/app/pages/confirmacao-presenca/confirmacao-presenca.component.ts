import { ConsultaUsuarioLojaModel } from './../../models/ConsultaUsuarioLoja.Model';
import { Component, OnInit } from '@angular/core';
import { ImportsModule } from '../../imports';
import { MessageService } from 'primeng/api';
import { HttpService } from '../../services/http-service.service';
import { Utils } from '../../services/utils';
import { Base64Service } from '../../services/base64.service';
import { CryptoService } from '../../services/crypto.service';
import { ActivatedRoute, Router } from '@angular/router';
import { SessaoListaModel } from '../../models/SessaoLista.Model';
import { UsuarioLogadoModel } from '../../models/UsuarioLogado.Model';
import { SessaoConviteModel } from '../../models/SessaoConvite.Model';
import { UsuarioModel } from '../../models/Usuario.Model';
import { PotenciaModel } from '../../models/Potencia.Model';
import { ChangeDetectorRef } from '@angular/core';
import { QuantitativoPresencaModel } from '../../models/QuantitativoPresenca.Model';

interface Mensagem {
  titulo: string;
  corpoMensagem: string;
  icone: string;
  corIcone: string;
}

@Component({
  selector: 'app-confirmacao-presenca',
  standalone: true,
  imports: [ImportsModule],
  templateUrl: './confirmacao-presenca.component.html',
  styleUrl: './confirmacao-presenca.component.css',
  providers: [MessageService],
})
export class ConfirmacaoPresencaComponent implements OnInit {
  boolLoading = false;
  parametroRota!: string | null;
  idSessaoCrypto: number = 0;
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
    '',
    false,
    0
  );
  objUsuarioLogado: UsuarioLogadoModel = new UsuarioLogadoModel();
  objUsuario: UsuarioModel = new UsuarioModel();

  objSessaoConvite: SessaoConviteModel | undefined;
  lstPotencia: PotenciaModel[] = [];
  objPotenciaIrmao: PotenciaModel = new PotenciaModel();
  objConsultaUsrLj: ConsultaUsuarioLojaModel = new ConsultaUsuarioLojaModel();
  boolBlockIntputsLoja: boolean = true;
  boolBlockIntputsUsuario: boolean = true;
  boolDialogMensagem: boolean = false;
  mensagem: Mensagem = {
    titulo: '',
    corpoMensagem: '',
    icone: '',
    corIcone: '',
  };
  confirmacaoEmail: string = '';
  objConfirmacoes: QuantitativoPresencaModel = new QuantitativoPresencaModel();
  boolDialogPropaganda: boolean = false;

  constructor(
    private http: HttpService,
    private messageService: MessageService,
    private utils: Utils,
    private router: Router,
    private route: ActivatedRoute,
    private base64Service: Base64Service,
    private cryptoService: CryptoService,
    private cd: ChangeDetectorRef
  ) {
    this.objUsuarioLogado = JSON.parse(
      this.cryptoService.lerDoSessionStorage('usr')
    );
    // console.warn("Usuário Logado: ", this.objUsuarioLogado);
  }

  ngOnInit() {
    try {
      this.parametroRota = this.route.snapshot.paramMap.get('data');
      let baseDecripto = this.cryptoService.decriptografar(this.parametroRota!);
      this.idSessaoCrypto =
        this.base64Service.decodeBase64ToNumber(baseDecripto);

      this.GetSessaoBySesCodi(this.idSessaoCrypto);
      this.GetQtdPresencaBySesCodi(this.idSessaoCrypto);
    } catch (error) {
      this.boolLoading = false;
      console.warn('Falha ao receber os parâmetros.');
    }
  }

  GetQtdPresencaBySesCodi(sesCodi: number) {
    this.boolLoading = true;
    this.http.GetQtdPresencaBySesCodi(sesCodi).subscribe({
      next: (response) => {
        this.objConfirmacoes = response;
        this.boolLoading = false;
        // console.warn('CONFIRMAÇÕES:', this.objConfirmacoes);
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

  GetSessaoBySesCodi(sesCodi: number) {
    this.boolLoading = true;
    this.http.GetSessaoBySesCodi(sesCodi).subscribe({
      next: (response) => {
        this.objSessaoConvite = response;
        // console.warn('Sessão:', this.objSessaoConvite);
        this.GetPotenciaRegularByLojCodi(this.objSessaoConvite.LojCodi);
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

  GetPotenciaRegularByLojCodi(lojCodi: number) {
    this.http.GetPotenciaRegularByLojCodi(lojCodi).subscribe({
      next: (response) => {
        this.lstPotencia = [];
        response.forEach((itemPotencia) => {
          if (itemPotencia.PotStat === true) {
            let objPot: PotenciaModel = {
              PotCodi: itemPotencia.PotCodi,
              PotNome: itemPotencia.PotNome,
              PotLogo: itemPotencia.PotLogo,
              PotRegu: itemPotencia.PotRegu,
              PotStat: itemPotencia.PotStat,
              PotSigl: itemPotencia.PotSigl + ' - ' + itemPotencia.PotNome,
            };
            this.lstPotencia.push(objPot);
          }
        });

        this.boolLoading = false;
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

  GetUsuarioByLoja(UsuNCIM: string, PotCodi: number, LojNumL: string) {
    if (UsuNCIM.length > 0 && PotCodi > 0 && LojNumL.length > 0) {
      this.boolLoading = true;
      this.boolBlockIntputsLoja = true;
      this.boolBlockIntputsUsuario = true;
      this.objConsultaUsrLj = new ConsultaUsuarioLojaModel();
      this.objConsultaUsrLj.objLojaConsulta.PotCodi = PotCodi;
      this.objConsultaUsrLj.objLojaConsulta.LojNumL = LojNumL;
      this.objConsultaUsrLj.objUsuarioLoja.UsuNCIM = UsuNCIM;
      this.http
        .GetUsuarioByLoja(UsuNCIM, PotCodi, LojNumL.toString())
        .subscribe({
          next: (response) => {
            // console.warn('Usuário pesquisado: ', response);
            if (response.objUsuarioLoja) {
              this.objConsultaUsrLj.objUsuarioLoja = response.objUsuarioLoja;
              this.objConsultaUsrLj.objUsuarioLoja.UsuNasc = new Date(
                response.objUsuarioLoja.UsuNasc.toString()
              );
              this.confirmacaoEmail =
                this.objConsultaUsrLj.objUsuarioLoja.UsuEmai;
            } else {
              this.boolBlockIntputsUsuario = false;
            }
            if (response.objLojaConsulta) {
              this.objConsultaUsrLj.objLojaConsulta = response.objLojaConsulta;
            } else {
              this.boolBlockIntputsLoja = false;
            }

            //this.boolLoading = false;
            //-> VERIFICANDO SE O USUÁRIO EXISTE NA BASE, PELO CIM E POTÊNCIA
            this.http.GetUsuarioByPotCodi(PotCodi, UsuNCIM).subscribe({
              next: (response) => {
                // console.warn('Usuário Por potência:', response);
                if (response.UsuCodi > 0) {
                  this.objConsultaUsrLj.objUsuarioLoja.UsuCodi =
                    response.UsuCodi;
                  this.objConsultaUsrLj.objUsuarioLoja.UsuNome =
                    response.UsuNome;
                  this.objConsultaUsrLj.objUsuarioLoja.UsuNCel =
                    response.UsuNCel;
                  this.objConsultaUsrLj.objUsuarioLoja.UsuEmai =
                    response.UsuEmai;
                  this.objConsultaUsrLj.objUsuarioLoja.UsuNasc = new Date(
                    response.UsuNasc.toString()
                  );
                  this.confirmacaoEmail =
                    this.objConsultaUsrLj.objUsuarioLoja.UsuEmai;
                  this.boolBlockIntputsUsuario = true;
                }

                this.boolLoading = false;
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
  }

  ConfirmarPresenca() {
    this.objConsultaUsrLj.objUsuarioLoja.UsuNCel =
      this.utils.RemoveMascaraTelefone(
        this.objConsultaUsrLj.objUsuarioLoja.UsuNCel
      );
    this.objConsultaUsrLj.objLojaConsulta.PotCodi =
      this.objPotenciaIrmao.PotCodi;
    this.objConsultaUsrLj.sesCodi = this.idSessaoCrypto;

    if (this.ValidaInformacoes()) {
      this.boolLoading = true;
      // console.warn(this.objConsultaUsrLj);
      this.http.PostConfirmaPresenca(this.objConsultaUsrLj).subscribe({
        next: (response) => {
          // console.warn(response);
          this.boolLoading = false;
          if (response === 'Presença confirmada com sucesso.') {
            this.GetQtdPresencaBySesCodi(this.idSessaoCrypto);
            this.mensagem.titulo = 'Presença Confirmada!';
            this.mensagem.corpoMensagem = 'Presença confirmada com sucesso!';
            this.mensagem.icone = 'pi-check';
            this.mensagem.corIcone = 'green';
            this.boolDialogMensagem = true;
          } else if (response === 'Presença já confirmada.') {
            this.mensagem.titulo = 'Presença já Confirmada!';
            this.mensagem.corpoMensagem =
              'Sua presença já foi confirmada para esta sessão.';
            this.mensagem.icone = 'pi-exclamation-triangle';
            this.mensagem.corIcone = 'yellow';
            this.boolDialogMensagem = true;
          } else {
            console.error('Erro ao confirmar a presença:', response);
            this.messageService.add({
              severity: 'error',
              summary: 'Erro: ',
              detail: 'Falha ao realizar a operação, contate o suporte.',
            });
          }
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
  }

  ValidaInformacoes() {
    if (this.objConsultaUsrLj.objLojaConsulta.PotCodi === 0) {
      this.messageService.add({
        severity: 'warn',
        summary: 'Atenção: ',
        detail: 'Selecione uma potência.',
      });
      return false;
    }
    if (this.objConsultaUsrLj.objLojaConsulta.LojNumL.length === 0) {
      this.messageService.add({
        severity: 'warn',
        summary: 'Atenção: ',
        detail: 'Insira o número da Loja.',
      });
      return false;
    }
    if (this.objConsultaUsrLj.objUsuarioLoja.UsuNCIM.length === 0) {
      this.messageService.add({
        severity: 'warn',
        summary: 'Atenção: ',
        detail: 'Insira o seu CIM.',
      });
      return false;
    }
    if (this.objConsultaUsrLj.objUsuarioLoja.UsuNome.length === 0) {
      this.messageService.add({
        severity: 'warn',
        summary: 'Atenção: ',
        detail: 'Insira o seu nome.',
      });
      return false;
    }
    if (this.objConsultaUsrLj.objLojaConsulta.LojNome.length === 0) {
      this.messageService.add({
        severity: 'warn',
        summary: 'Atenção: ',
        detail: 'Insira o nome de sua Loja.',
      });
      return false;
    }
    if (this.objConsultaUsrLj.objUsuarioLoja.UsuNCel.length === 0) {
      this.messageService.add({
        severity: 'warn',
        summary: 'Atenção: ',
        detail: 'Insira o seu número do celular.',
      });
      return false;
    }
    if (this.objConsultaUsrLj.objUsuarioLoja.UsuEmai.length === 0) {
      this.messageService.add({
        severity: 'warn',
        summary: 'Atenção: ',
        detail: 'Insira o seu e-mail.',
      });
      return false;
    }
    if (
      !this.utils.ValidarEmail(this.objConsultaUsrLj.objUsuarioLoja.UsuEmai)
    ) {
      this.messageService.add({
        severity: 'warn',
        summary: 'Atenção: ',
        detail: 'E-mail inválido, verifique.',
      });
      return false;
    }
    const email = this.objConsultaUsrLj.objUsuarioLoja.UsuEmai.replace(
      /\s+/g,
      ''
    );
    const confirm = this.confirmacaoEmail.replace(/\s+/g, '');

    if (email !== confirm) {
      this.messageService.add({
        severity: 'warn',
        summary: 'Atenção: ',
        detail: 'As informações de e-mail não batem, verifique.',
      });
      return false;
    }

    return true;
  }

  FechaDialog() {
    this.boolDialogMensagem = false;
    this.objConsultaUsrLj = new ConsultaUsuarioLojaModel();
    this.objPotenciaIrmao = new PotenciaModel();
    this.confirmacaoEmail = '';

    //-> VERIFICANDO SE A LOJA É BAALBEK, SE SIM, ABRE A PROPAGANDA
    // if(this.objSessaoConvite?.LojCodi === 1){ //-> COMENTADO PARA NÃO EXIBIR MAIS O CONVITE.
    //   this.boolDialogPropaganda = true;
    // }
  }

  abrirLink(tipo: string) {
    if(tipo === 'INFO'){
      window.open('https://wa.link/duxusp', '_blank');
    } else {
      // window.open('https://uticket.com.br/event/01LT2RJ6G62VER', '_blank');
      window.open('https://pag.ae/81CfhAguq/button', '_blank');
    }
  }
}
