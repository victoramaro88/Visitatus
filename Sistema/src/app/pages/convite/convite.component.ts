import { Component, OnInit, AfterViewInit, ViewChild, ElementRef } from '@angular/core';
import { HttpService } from '../../services/http-service.service';
import { ActivatedRoute } from '@angular/router';
import { CryptoService } from '../../services/crypto.service';
import { Base64Service } from '../../services/base64.service';
import { DomSanitizer, SafeHtml } from '@angular/platform-browser';
import { SessaoConviteModel } from '../../models/SessaoConvite.Model';
import { TemplateLojaModel } from '../../models/TemplateLoja.Model';
import { Renderer2 } from '@angular/core';

@Component({
  selector: 'app-convite',
  templateUrl: './convite.component.html',
  styleUrls: ['./convite.component.css']
})
export class ConviteComponent implements OnInit, AfterViewInit {
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
    private cryptoService: CryptoService,
    private base64Service: Base64Service,
    private sanitizer: DomSanitizer,
    private renderer: Renderer2
  ) { }

  ngOnInit(): void {
    try {
      this.parametroRota = this.route.snapshot.paramMap.get('data');
      let baseDecripto = this.cryptoService.decriptografar(this.parametroRota!);
      this.idSessaoCrypto = this.base64Service.decodeBase64ToNumber(baseDecripto);

      this.GetSessaoBySesCodi(this.idSessaoCrypto);
    } catch (error) {
      this.boolLoading = false;
    }
  }

  ngAfterViewInit(): void {
    // Após a renderização, tentamos acessar o botão
    this.addButtonClickListener();
  }

  renderDynamicHtml(rawHtml: string) {
    this.htmlContent = this.sanitizer.bypassSecurityTrustHtml(rawHtml);
    const dynamicHtml = this.htmlContent;

    // Injetar o HTML dinâmico no container
    this.dynamicContainer.nativeElement.innerHTML = dynamicHtml;

    // Esperar um ciclo de detecção de mudanças para garantir que o DOM foi atualizado
    setTimeout(() => this.addButtonClickListener(), 0);
  }

  addButtonClickListener() {
    const button = this.dynamicContainer.nativeElement.querySelector('#confirmar-presenca-btn');
    if (button) {
      // Vincular manualmente o evento click ao botão
      this.renderer.listen(button, 'click', () => {
        this.ConfirmarPresenca();
      });
    }
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

  ConfirmarPresenca() {
    console.warn('Presença Confirmada!', this.objSessaoConvite?.SesCodi);
  }
}
