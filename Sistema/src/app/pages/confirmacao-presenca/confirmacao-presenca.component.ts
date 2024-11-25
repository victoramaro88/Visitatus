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

@Component({
  selector: 'app-confirmacao-presenca',
  standalone: true,
  imports: [ImportsModule],
  templateUrl: './confirmacao-presenca.component.html',
  styleUrl: './confirmacao-presenca.component.css',
  providers: [MessageService]
})
export class ConfirmacaoPresencaComponent implements OnInit {

  boolLoading = false;
  parametroRota!: string | null;
  idSessaoCrypto: number = 0;
  objSessao: SessaoListaModel = new SessaoListaModel(0, '', new Date(), false, true, 0, 0, '', 0, '', 0, '');
  objUsuarioLogado: UsuarioLogadoModel = new UsuarioLogadoModel();

  objSessaoConvite: SessaoConviteModel | undefined;

  constructor(
    private http: HttpService,
    private messageService: MessageService,
    private utils: Utils,
    private router: Router,
    private route: ActivatedRoute,
    private base64Service: Base64Service,
    private cryptoService: CryptoService
  ) {
    this.objUsuarioLogado = JSON.parse(this.cryptoService.lerDoSessionStorage("usr"));
    // console.warn("Usuário Logado: ", this.objUsuarioLogado);
  }

  ngOnInit() {
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

  GetSessaoBySesCodi(sesCodi: number) {
    this.boolLoading = true;
    this.http.GetSessaoBySesCodi(sesCodi).subscribe({
      next: (response) => {
        this.objSessaoConvite = response;
        console.warn('Sessão:', this.objSessaoConvite);
        this.boolLoading = false;
      },
      error: (error) => {
        console.error('Erro ao carregar dados:', error);
        this.boolLoading = false;
      }
    });
  }

}
