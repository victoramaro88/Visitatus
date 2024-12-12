import { Component, OnInit } from '@angular/core';
import { ImportsModule } from '../../imports';
import { MessageService } from 'primeng/api';
import { HttpService } from '../../services/http-service.service';
import { ActivatedRoute, Router } from '@angular/router';
import { CryptoService } from '../../services/crypto.service';
import { Base64Service } from '../../services/base64.service';
import { DomSanitizer, SafeHtml } from '@angular/platform-browser';

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
  parametroRota!: string | null;
  idSessaoCrypto: number = 0;
  htmlContent: SafeHtml | undefined;
  // objSessaoConvite: SessaoConviteModel | undefined;
  // objTemplate: TemplateLojaModel | undefined;

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
      this.parametroRota = this.route.snapshot.paramMap.get('data');
      console.warn('Parâmetro recebido:', this.parametroRota);
      // let baseDecripto = this.cryptoService.decriptografar(this.parametroRota!);
      // this.idSessaoCrypto =
      // this.base64Service.decodeBase64ToNumber(baseDecripto);

      // this.GetSessaoBySesCodi(this.idSessaoCrypto);
    } catch (error) {
      this.boolLoading = false;
      console.warn('Falha ao receber os parâmetros.');
    }
  }
}
