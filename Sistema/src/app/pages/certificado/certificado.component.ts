import { PresencaModel } from './../../models/Presenca.Model';
import { Component, OnInit } from '@angular/core';
import { ImportsModule } from '../../imports';
import { MessageService } from 'primeng/api';
import { HttpService } from '../../services/http-service.service';
import { ActivatedRoute, Router } from '@angular/router';
import { CryptoService } from '../../services/crypto.service';
import { Base64Service } from '../../services/base64.service';
import { DomSanitizer, SafeHtml } from '@angular/platform-browser';
import { EncryptAPIModel } from '../../models/EncryptAPI.Model';

@Component({
  selector: 'app-certificado',
  standalone: true,
  imports: [ImportsModule],
  templateUrl: './certificado.component.html',
  styleUrl: './certificado.component.css',
  providers: [MessageService],
})
export class CertificadoComponent implements OnInit {
  boolLoading = true;
  idSessaoCrypto: number = 0;
  htmlContent: SafeHtml | undefined;
  parâmetroURL: string = '';
  objCryptAPI: EncryptAPIModel = new EncryptAPIModel();

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
      this.parâmetroURL = this.route.snapshot.paramMap.get('data')!;
      // console.warn('Parâmetro recebido:', this.parâmetroURL);

      // this.GetSessaoBySesCodi(this.idSessaoCrypto);
    } catch (error) {
      this.boolLoading = false;
      console.warn('Falha ao receber os parâmetros.');
    }

    this.objCryptAPI.valorMensagem = 'Teste Amaro';
    this.Encriptar(this.objCryptAPI);
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
