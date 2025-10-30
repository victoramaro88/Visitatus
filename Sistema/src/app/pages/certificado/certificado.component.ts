import { PresencaModel } from './../../models/Presenca.Model';
import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { ImportsModule } from '../../imports';
import { MessageService } from 'primeng/api';
import { HttpService } from '../../services/http-service.service';
import { ActivatedRoute, Router } from '@angular/router';
import { CryptoService } from '../../services/crypto.service';
import { Base64Service } from '../../services/base64.service';
import { DomSanitizer, SafeHtml } from '@angular/platform-browser';
import { EncryptAPIModel } from '../../models/EncryptAPI.Model';

import { toPng } from 'html-to-image';
import html2canvas from 'html2canvas';

@Component({
  selector: 'app-certificado',
  standalone: true,
  imports: [ImportsModule],
  templateUrl: './certificado.component.html',
  styleUrl: './certificado.component.css',
  providers: [MessageService],
})
export class CertificadoComponent implements OnInit {
  @ViewChild('contentDiv', { static: false }) contentDiv!: ElementRef;

  boolLoading = true;
  idSessaoCrypto: number = 0;
  htmlContent: SafeHtml | undefined;
  parâmetroURL: string = '';
  objCryptAPI: EncryptAPIModel = new EncryptAPIModel();

  parametroRota!: string | null;
  boolButtonSaveImg: boolean = false;

  constructor(
    private http: HttpService,
    private route: ActivatedRoute,
    private router: Router,
    private cryptoService: CryptoService,
    private base64Service: Base64Service,
    private messageService: MessageService,
    private sanitizer: DomSanitizer
  ) {}

  ngOnInit(): void {
    try {
      this.boolButtonSaveImg = false;
      //-> URL TESTE DE RECEBIMENTO:
      // http://localhost:4200/certificado?data=VTJGc2RHVmtYMTk1Z3pIRXhlM2FUZWZxUjVLcmhTVVRSTENvRGJpR2RWOD0=
      // http://localhost:4200/certificado?data=ODJObWdWNFA3UExISDVUNnY4VFNzdz09

      // console.warn('========================================');
      // let paramBase64Convert: string = this.base64Service.decodeBase64ToString(
      //   parametroRecebidoTESTE
      // );
      // console.warn(
      //   'PARAMETRO RECEBIDO DESCONVERTIDO DO BASE64',
      //   paramBase64Convert
      // );
      // let paramDecripto: string =
      //   this.cryptoService.decriptografar(paramBase64Convert);
      // console.warn('PARAMETRO RECEBIDO FINAL', paramDecripto);
      // let usuCodiDecripto: number = +paramDecripto.split('|')[0];
      // console.warn('USUCODI DECRIPTO', usuCodiDecripto);
      // let sesCodiDecripto: number = +paramDecripto.split('|')[1];
      // console.warn('SESCODI DECRIPTO', sesCodiDecripto);

      //
      this.route.queryParams.subscribe((params) => {
        this.parametroRota = params['data'];
        // console.warn('PARÂMETROS RECEBIDOS:', this.parametroRota);
        let paramBase64Convert: string =
          this.base64Service.decodeBase64ToString(this.parametroRota!);
        let paramDecripto: string =
          this.cryptoService.decryptAPI(paramBase64Convert);

        let usuCodiDecripto: number = +paramDecripto.split('|')[0];
        let sesCodiDecripto: number = +paramDecripto.split('|')[1];

        this.GetCertificado(usuCodiDecripto, sesCodiDecripto);
      });
    } catch (error) {
      this.boolLoading = false;
      console.warn('Falha ao receber os parâmetros.');
    }
  }

  GetCertificado(usuCodi: number, sesCodi: number) {
    this.boolLoading = true;
    this.http.GetCertificado(usuCodi, sesCodi).subscribe({
      next: (response) => {
        // console.warn('HTML Retorno:', response);
        this.htmlContent = this.sanitizer.bypassSecurityTrustHtml(response);
        this.boolLoading = false;
        this.boolButtonSaveImg = true;
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

  // saveAsImage(): void {
  //   const node = this.contentDiv.nativeElement;

  //   toPng(node, { cacheBust: true })
  //     .then((dataUrl) => {
  //       const link = document.createElement('a');
  //       link.href = dataUrl;
  //       link.download = 'Certificado.png';
  //       link.click();
  //     })
  //     .catch((error) => {
  //       console.error('Erro ao salvar como imagem:', error);
  //     });
  // }

  saveAsImage(): void {
    if (this.isMobile()) {
      //-> Se for mobile, salva desta forma
      const node = this.contentDiv.nativeElement;

      html2canvas(node, {
        scale: 3, // Aumenta a resolução da captura
        width: node.scrollWidth, // Garante que toda a largura do conteúdo seja capturada
        height: node.scrollHeight, // Garante que toda a altura do conteúdo seja capturada
        useCORS: true,
        allowTaint: true,
      })
        .then((canvas) => {
          const dataUrl = canvas.toDataURL('image/png');
          const link = document.createElement('a');
          link.href = dataUrl;
          link.download = 'Certificado.png';
          link.click();
        })
        .catch((error) => {
          console.error('Erro ao salvar como imagem:', error);
        });
    } else {
      //-> Se não for mobile, salva desta forma
      const node = this.contentDiv.nativeElement;

      toPng(node, { cacheBust: true })
        .then((dataUrl) => {
          const link = document.createElement('a');
          link.href = dataUrl;
          link.download = 'Certificado.png';
          link.click();
        })
        .catch((error) => {
          console.error('Erro ao salvar como imagem:', error);
        });
    }
  }

  isMobile(): boolean {
    return window.innerWidth <= 768; // Considerando telas menores que 768px como mobile
  }

  Encriptar(textoEncriptar: EncryptAPIModel) {
    this.boolLoading = true;
    this.http.encryptAPI(textoEncriptar).subscribe({
      next: (response) => {
        // console.warn('Retorno encriptado:', response);
        let objDecrypt: EncryptAPIModel = { valorMensagem: response };
        this.Decriptar(objDecrypt);
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

  Decriptar(textoDecriptar: EncryptAPIModel) {
    this.boolLoading = true;
    this.http.decryptAPI(textoDecriptar).subscribe({
      next: (response) => {
        // console.warn('Retorno Decriptado:', response);
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
