import { Component, OnInit, ViewChild, ElementRef } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { DomSanitizer, SafeHtml } from '@angular/platform-browser';
import { HttpService } from '../../services/http-service.service';
import { CryptoService } from '../../services/crypto.service';
import { Base64Service } from '../../services/base64.service';
import { SessaoConviteModel } from '../../models/SessaoConvite.Model';
import { TemplateLojaModel } from '../../models/TemplateLoja.Model';
import { ImportsModule } from '../../imports';
import { MessageService } from 'primeng/api';

@Component({
  selector: 'app-convite',
  standalone: true,
  templateUrl: './convite.component.html',
  styleUrls: ['./convite.component.css'],
  imports: [ImportsModule],
  providers: [MessageService]
})
export class ConviteComponent implements OnInit {
  @ViewChild('dynamicContainer', { static: false }) dynamicContainer!: ElementRef;

  boolLoading = true;
  parametroRota!: string | null;
  idSessaoCrypto: number = 0;
  htmlContent: SafeHtml | undefined;
  objSessaoConvite: SessaoConviteModel | undefined;
  objTemplate: TemplateLojaModel | undefined;

  constructor(
    private http: HttpService,
    private route: ActivatedRoute,
    private router: Router,
    private cryptoService: CryptoService,
    private base64Service: Base64Service,
    private sanitizer: DomSanitizer
  ) {}

  ngOnInit(): void {
    try {
      this.parametroRota = this.route.snapshot.paramMap.get('data');
      let baseDecripto = this.cryptoService.decriptografar(this.parametroRota!);
      this.idSessaoCrypto = this.base64Service.decodeBase64ToNumber(baseDecripto);

      this.GetSessaoBySesCodi(this.idSessaoCrypto);
    } catch (error) {
      this.boolLoading = false;
      console.warn('Falha ao receber os parâmetros.');
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
        this.GetTemplateLoja(response.LojCodi);
      },
      error: (error) => {
        console.error('Erro ao carregar dados:', error);
        this.boolLoading = false;
      }
    });
  }

  GetTemplateLoja(lojCodi: number) {
    this.http.GetTemplateLoja(lojCodi).subscribe({
      next: (response) => {
        this.objTemplate = response;
        const objHtmlReplace = this.objTemplate.TmpCvtMode
          .replace('[LojLogo]', this.objSessaoConvite?.LojLogo!)
          .replace('[PotLogo]', this.objSessaoConvite?.PotLogo!)
          .replace('[LojNome]', this.objSessaoConvite?.LojNome!)
          .replace('[PotSigl]', this.objSessaoConvite?.PotSigl!)
          .replace('[PotNome]', this.objSessaoConvite?.PotNome!)
          .replace('[SesDesc]', this.objSessaoConvite?.SesDesc!)
          .replace('[RitNome]', this.objSessaoConvite?.RitNome!)
          .replace('[GraNome]', this.objSessaoConvite?.GraNome!)
          .replace('[SesDtHr_DATA]', new Date(this.objSessaoConvite?.SesDtHr!).toLocaleDateString('pt-BR'))
          .replace('[SesDtHr_HORA]', new Date(this.objSessaoConvite?.SesDtHr!).toLocaleTimeString('pt-BR', { hour: '2-digit', minute: '2-digit' }))
          .replace('[LojLogr]', this.objSessaoConvite?.LojLogr!)
          .replace('[LojNume]', this.objSessaoConvite?.LojNume!)
          .replace('[LojBair]', this.objSessaoConvite?.LojBair!)
          .replace('[CidNome]', this.objSessaoConvite?.CidNome!)
          .replace('[EstSigl]', this.objSessaoConvite?.EstSigl!)
          .replace('[UsuNome]', this.objSessaoConvite?.lstGestaoAdmAtiva?.find(u => u.CarCodi === 1)?.UsuNome!)
          .replace('[CarNome]', this.objSessaoConvite?.lstGestaoAdmAtiva?.find(u => u.CarCodi === 1)?.CarNome!);

        this.renderDynamicHtml(objHtmlReplace);
        this.boolLoading = false;
      },
      error: (error) => {
        console.error('Erro ao carregar dados:', error);
        this.boolLoading = false;
      }
    });
  }

  onContainerClick(event: Event): void {
    const target = event.target as HTMLElement;
    if (target.id === 'confirmar-presenca-btn') {
      this.ConfirmarPresenca();
    }
  }

  ConfirmarPresenca() {
    // console.warn('Presença Confirmada!', this.objSessaoConvite?.SesCodi);

    // Criar a árvore da URL corretamente
    // const urlTree = this.router.createUrlTree(['/confirmacao', this.cryptoService.criptografar(this.base64Service.convertNumberToBase64(this.objSessaoConvite?.SesCodi!))]);
    // const url = this.router.serializeUrl(urlTree);
    // const baseHref = document.getElementsByTagName('base')[0]?.href || '';
    // const fullUrl = baseHref.replace(/\/$/, '') + url;
    // window.open(fullUrl, '_self');

    const path = ['/confirmacao', this.cryptoService.criptografar(this.base64Service.convertNumberToBase64(this.objSessaoConvite?.SesCodi!))];
    this.router.navigate(path);
  }

}
