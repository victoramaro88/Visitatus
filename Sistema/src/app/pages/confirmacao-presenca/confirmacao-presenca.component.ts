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
import { LojaModel } from '../../models/Loja.Model';
import { ChangeDetectorRef } from '@angular/core';

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
  objUsuario: UsuarioModel = new UsuarioModel();

  objSessaoConvite: SessaoConviteModel | undefined;
  lstPotencia: PotenciaModel[] = [];
  objPotenciaIrmao: PotenciaModel = new PotenciaModel();
  objPotenciaLoja: PotenciaModel = new PotenciaModel();
  objLoja: LojaModel = {
    LojCodi: 0,
    LojNome: "",
    LojNumL: "",
    LojLogo: "",
    LojLogr: "",
    LojNume: "",
    LojBair: "",
    LojStat: false,
    CidCodi: 0,
    PotCodi: 0,
    RitCodi: 0
  };

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
        this.GetPotenciaRegularByLojCodi(this.objSessaoConvite.LojCodi);
      },
      error: (error) => {
        console.error('Erro ao carregar dados:', error);
        this.boolLoading = false;
      }
    });
  }

  GetPotenciaRegularByLojCodi(lojCodi: number) {
    this.http.GetPotenciaRegularByLojCodi(lojCodi).subscribe({
      next: (response) => {
        this.lstPotencia = [];
        this.lstPotencia = response.filter(p => p.PotStat === true);
        console.warn('Lista das Potências:', this.lstPotencia);
        this.boolLoading = false;
      },
      error: (error) => {
        console.error('Erro ao carregar dados:', error);
        this.boolLoading = false;
      }
    });
  }

  GetLojaByPotLojNume(PotCodi: number, LojNumL: string) {
    if (PotCodi > 0 && LojNumL.length > 0) {
      this.boolLoading = true;
      this.http.GetLojaByPotLojNume(PotCodi, LojNumL.toString()).subscribe({
        next: (response) => {
          this.objLoja = {
            LojCodi: 0,
            LojNome: "",
            LojNumL: LojNumL,
            LojLogo: "",
            LojLogr: "",
            LojNume: "",
            LojBair: "",
            LojStat: false,
            CidCodi: 0,
            PotCodi: PotCodi,
            RitCodi: 0
          };
          if (response) {
            this.objLoja = response;
          }
          // console.warn('Loja Pesquisada:', this.objLoja);
          this.boolLoading = false;
        },
        error: (error) => {
          console.error('Erro ao carregar dados:', error);
          this.boolLoading = false;
        }
      });
    }
  }

}
