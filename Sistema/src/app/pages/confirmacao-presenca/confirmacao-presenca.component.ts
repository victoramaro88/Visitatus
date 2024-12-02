import { LojaConsulta, UsrLoja } from './../../models/ConsultaUsuarioLoja.Model';
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
import { ConsultaUsuarioLojaModel } from '../../models/ConsultaUsuarioLoja.Model';

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
  objConsultaUsrLj: ConsultaUsuarioLojaModel = new ConsultaUsuarioLojaModel();
  boolBlockIntputsLoja: boolean = true;
  boolBlockIntputsUsuario: boolean = true;

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
        response.forEach(itemPotencia => {
          if (itemPotencia.PotStat === true) {
            let objPot: PotenciaModel = {
              PotCodi: itemPotencia.PotCodi,
              PotNome: itemPotencia.PotNome,
              PotLogo: itemPotencia.PotLogo,
              PotRegu: itemPotencia.PotRegu,
              PotStat: itemPotencia.PotStat,
              PotSigl: itemPotencia.PotSigl + ' - ' + itemPotencia.PotNome
            };
            this.lstPotencia.push(objPot);
          }
        });

        console.warn('Lista das Potências:', this.lstPotencia);
        this.boolLoading = false;
      },
      error: (error) => {
        console.error('Erro ao carregar dados:', error);
        this.boolLoading = false;
      }
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
      this.http.GetUsuarioByLoja(UsuNCIM, PotCodi, LojNumL.toString()).subscribe({
        next: (response) => {
          if (response.objUsuarioLoja) {
            this.objConsultaUsrLj.objUsuarioLoja = response.objUsuarioLoja;
            this.objConsultaUsrLj.objUsuarioLoja.UsuNasc = new Date(response.objUsuarioLoja.UsuNasc.toString());
          } else {
            this.boolBlockIntputsUsuario = false;
          }
          if (response.objLojaConsulta) {
            this.objConsultaUsrLj.objLojaConsulta = response.objLojaConsulta;
          } else {
            this.boolBlockIntputsLoja = false;
          }

          console.warn('Retorno Consulta Loja / Usuário:', this.objConsultaUsrLj);
          this.boolLoading = false;
        },
        error: (error) => {
          console.error('Erro ao carregar dados:', error);
          this.boolLoading = false;
        }
      });
    }
  }

  ConfirmarPresenca() {
    this.objConsultaUsrLj.objLojaConsulta.PotCodi = this.objPotenciaIrmao.PotCodi;

    if (this.ValidaInformacoes()) {
      console.warn(this.objConsultaUsrLj);
    }
  }

  ValidaInformacoes() {
    if (this.objConsultaUsrLj.objLojaConsulta.PotCodi === 0) {
      this.messageService.add({ severity: 'warn', summary: 'Atenção: ', detail: 'Selecione uma potência.' });
      return false;
    }
    if (this.objConsultaUsrLj.objLojaConsulta.LojNumL.length === 0) {
      this.messageService.add({ severity: 'warn', summary: 'Atenção: ', detail: 'Insira o número da Loja.' });
      return false;
    }
    if (this.objConsultaUsrLj.objUsuarioLoja.UsuNCIM.length === 0) {
      this.messageService.add({ severity: 'warn', summary: 'Atenção: ', detail: 'Insira o seu CIM.' });
      return false;
    }
    if (this.objConsultaUsrLj.objUsuarioLoja.UsuNome.length === 0) {
      this.messageService.add({ severity: 'warn', summary: 'Atenção: ', detail: 'Insira o seu nome.' });
      return false;
    }
    if (this.objConsultaUsrLj.objLojaConsulta.LojNome.length === 0) {
      this.messageService.add({ severity: 'warn', summary: 'Atenção: ', detail: 'Insira o nome de sua Loja.' });
      return false;
    }
    if (this.objConsultaUsrLj.objUsuarioLoja.UsuNCel.length === 0) {
      this.messageService.add({ severity: 'warn', summary: 'Atenção: ', detail: 'Insira o seu número do celular.' });
      return false;
    }
    if (this.objConsultaUsrLj.objUsuarioLoja.UsuEmai.length === 0) {
      this.messageService.add({ severity: 'warn', summary: 'Atenção: ', detail: 'Insira o seu e-mail.' });
      return false;
    }
    if (!this.utils.ValidarEmail(this.objConsultaUsrLj.objUsuarioLoja.UsuEmai)) {
      this.messageService.add({ severity: 'warn', summary: 'Atenção: ', detail: 'E-mail inválido, verifique.' });
      return false;
    }

    return true;
  }
}
