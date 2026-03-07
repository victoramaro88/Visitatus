import { Component, OnInit, ViewChild, ElementRef } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { DomSanitizer, SafeHtml, bootstrapApplication } from '@angular/platform-browser';
import { HttpService } from '../../services/http-service.service';
import { CryptoService } from '../../services/crypto.service';
import { Base64Service } from '../../services/base64.service';
import { SessaoConviteModel } from '../../models/SessaoConvite.Model';
import { TemplateLojaModel } from '../../models/TemplateLoja.Model';
import { ImportsModule } from '../../imports';
import { MessageService } from 'primeng/api';
import { Title, Meta } from '@angular/platform-browser';

interface Mensagem {
  titulo: string;
  corpoMensagem: string;
  icone: string;
  corIcone: string;
}

@Component({
  selector: 'app-convite',
  standalone: true,
  templateUrl: './convite.component.html',
  styleUrls: ['./convite.component.css'],
  imports: [ImportsModule],
  providers: [MessageService],
})
export class ConviteComponent implements OnInit {
  @ViewChild('dynamicContainer', { static: false })
  dynamicContainer!: ElementRef;

  boolLoading = true;
  parametroRota!: string | null;
  idSessaoCrypto: number = 0;
  htmlContent: SafeHtml | undefined;
  objSessaoConvite: SessaoConviteModel | undefined;
  objTemplate: TemplateLojaModel | undefined;
  boolDialogMensagem: boolean = false;
  boolDialogPropaganda: boolean = false;
  mensagem: Mensagem = {
    titulo: '',
    corpoMensagem: '',
    icone: '',
    corIcone: '',
  };

  constructor(
    private http: HttpService,
    private route: ActivatedRoute,
    private router: Router,
    private cryptoService: CryptoService,
    private base64Service: Base64Service,
    private messageService: MessageService,
    private sanitizer: DomSanitizer,
    private titleService: Title,
    private metaService: Meta
  ) {}

  ngOnInit(): void {
    try {
      //-> NOVA FORMA DE CAPTURA DE PARÂMETRO
      this.route.queryParams.subscribe((params) => {
        this.parametroRota = params['data'];
        // console.warn('PARÂMETRO RECEBIDO: ', this.parametroRota);

        let baseDecripto = this.cryptoService.decriptografar(
          this.parametroRota!
        );

        this.idSessaoCrypto =
          this.base64Service.decodeBase64ToNumber(baseDecripto);
      });

      this.GetSessaoBySesCodi(this.idSessaoCrypto);
    } catch (error) {
      this.boolLoading = false;
      console.warn('Falha ao receber os parâmetros.');
      console.error(error);
    }
  }

  renderDynamicHtml(rawHtml: string) {
    this.htmlContent = this.sanitizer.bypassSecurityTrustHtml(rawHtml);
  }

  GetSessaoBySesCodi(sesCodi: number) {
    this.boolLoading = true;
    this.http.GetSessaoBySesCodi(sesCodi).subscribe({
      next: (response) => {
        this.objSessaoConvite = response;
        // console.warn('Sessão Retorno:', this.objSessaoConvite);
        if (!this.objSessaoConvite.SesLibe || !this.objSessaoConvite.SesStat) {
          // alert('Sessão não encontrada. Contate o responsável da Loja.');
          this.mensagem.corIcone = 'yellow';
          this.mensagem.icone = 'pi-exclamation-triangle';
          this.mensagem.titulo = 'Convite não encontrado.';
          this.mensagem.corpoMensagem =
            'Sessão não encontrada. Contate o responsável da Loja.';

          this.boolDialogMensagem = true;
          this.boolLoading = false;
        } else {
          //-> VALIDANDO SE É BAALBEK, SE FOR, ABRE A MENSAGEM DO CONVITE DO IAA.
          if(this.objSessaoConvite.LojCodi === 1){
            this.boolDialogPropaganda = true;
          }

          this.GetTemplateLoja(response.LojCodi);
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

  GetTemplateLoja(lojCodi: number) {
    this.http.GetTemplateLoja(lojCodi).subscribe({
      next: (response) => {
        this.objTemplate = response;
        const objHtmlReplace = this.objTemplate.TmpCvtMode.replace(
          '[LojLogo]',
          this.objSessaoConvite?.LojLogo!
        )
          .replace('[PotLogo]', this.objSessaoConvite?.PotLogo!)
          .replace('[LojNome]', this.objSessaoConvite?.LojNome!)
          .replace('[LojNumL]', this.objSessaoConvite?.LojNumL!)
          .replace('[PotSigl]', this.objSessaoConvite?.PotSigl!)
          .replace('[PotNome]', this.objSessaoConvite?.PotNome!)
          .replace('[SesDesc]', this.objSessaoConvite?.SesDesc!)
          .replace('[RitNome]', this.objSessaoConvite?.RitNome!)
          .replace('[GraNome]', this.objSessaoConvite?.GraNome!)
          .replace('[TiSCodi]', this.objSessaoConvite?.TiSNome!)
          .replace(
            '[SesDtHr_DATA]',
            new Date(this.objSessaoConvite?.SesDtHr!).toLocaleDateString(
              'pt-BR'
            )
          )
          .replace(
            '[SesDtHr_HORA]',
            new Date(this.objSessaoConvite?.SesDtHr!).toLocaleTimeString(
              'pt-BR',
              { hour: '2-digit', minute: '2-digit' }
            )
          )
          .replace('[LojLogr]', this.objSessaoConvite?.LojLogr!)
          .replace('[LojNume]', this.objSessaoConvite?.LojNume!)
          .replace('[LojBair]', this.objSessaoConvite?.LojBair!)
          .replace('[CidNome]', this.objSessaoConvite?.CidNome!)
          .replace('[EstSigl]', this.objSessaoConvite?.EstSigl!)
          .replace(
            '[UsuNome]',
            this.objSessaoConvite?.lstGestaoAdmAtiva?.length! > 0
              ? this.objSessaoConvite?.lstGestaoAdmAtiva?.find(
                  (u) => u.CarCodi === 1
                )?.UsuNome!
              : ''
          )
          .replace(
            '[CarNome]',
            this.objSessaoConvite?.lstGestaoAdmAtiva?.length! > 0
              ? this.objSessaoConvite?.lstGestaoAdmAtiva?.find(
                  (u) => u.CarCodi === 1
                )?.CarNome!
              : ''
          );

        //-> AJUSTANDO O TÍTULO, IMAGEM E DESCRIÇÃO DO ATALHO DA PÁGINA:
        this.titleService.setTitle(
          'Convite - ' +
            this.objSessaoConvite?.LojNome +
            ' - ' +
            this.objSessaoConvite?.LojNumL
        );
        // this.metaService.updateTag({
        //   name: 'description',
        //   content:
        //     'Esta é a página inicial do nosso site. Compartilhe no WhatsApp!',
        // });

        // console.warn(this.objSessaoConvite?.lstGestaoAdmAtiva);
        this.renderDynamicHtml(objHtmlReplace);
        this.boolLoading = false;
      },
      error: (error) => {
        console.error('Erro ao carregar dados:', error);
        if (error.status === 404) {
          this.messageService.add({
            severity: 'error',
            summary: 'Erro: ',
            detail: 'Loja sem modelo de Convite, contate o suporte.',
          });
        } else {
          this.messageService.add({
            severity: 'error',
            summary: 'Erro: ',
            detail: 'Falha ao realizar a operação, contate o suporte.',
          });
        }
        this.boolLoading = false;
      },
    });
  }

  FechaDialog() {
    this.boolDialogMensagem = false;
  }

  onContainerClick(event: Event): void {
    const target = event.target as HTMLElement;
    if (target.id === 'confirmar-presenca-btn') {
      this.ConfirmarPresenca();
    }
  }

  ConfirmarPresenca() {
    const path = [
      '/confirmacao',
      this.cryptoService.criptografar(
        this.base64Service.convertNumberToBase64(
          this.objSessaoConvite?.SesCodi!
        )
      ),
    ];
    this.router.navigate(path);
  }

  abrirLink(tipo: string) {
    if(tipo === 'PIX'){
      window.open('https://wa.link/duxusp', '_blank');
    } else {
      window.open('https://uticket.com.br/event/01LT2RJ6G62VER', '_blank');
    }
  }
}
