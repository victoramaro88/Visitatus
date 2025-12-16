import { GrauModel } from './../models/Grau.Model';
import { TipoSessaoModel } from './../models/TipoSessao.Model';
import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, of } from 'rxjs';
import { environment } from '../../environments/environment';
import { LoginModel } from '../models/Login.Model';
import { UsuarioLogadoModel } from '../models/UsuarioLogado.Model';
import { PermissaoPerfilListaModel } from '../models/PermissaoPerfilLista.Model ';
import { SessaoModel } from '../models/Sessao.Model';
import { SessaoListaModel } from '../models/SessaoLista.Model';
import { LojaModel } from '../models/Loja.Model';
import { SessaoConviteModel } from '../models/SessaoConvite.Model';
import { TemplateLojaModel } from '../models/TemplateLoja.Model';
import { PotenciaModel } from '../models/Potencia.Model';
import { ConsultaUsuarioLojaModel } from '../models/ConsultaUsuarioLoja.Model';
import { CryptoService } from './crypto.service';
import { EncryptAPIModel } from '../models/EncryptAPI.Model';
import { ListaPresencaModel } from '../models/ListaPresenca.Model';
import { UsuarioLojaModel } from '../models/UsuarioLoja.Model';
import { PerfilModel } from '../models/Perfil.Model';
import { UsuarioPotenciaModel } from '../models/UsuarioPotencia.Model ';
import { PermissaoModel } from '../models/Permissao.Model';
import { ProximaSessaoModel } from '../models/ProximaSessao.Model';
import { GestaoAdministrativa } from '../models/GestaoAdministrativa.Model';
import { QuantitativoPresencaModel } from '../models/QuantitativoPresenca.Model';

@Injectable({
  providedIn: 'root',
})
export class HttpService {
  constructor(
    private http: HttpClient,
    private encryptionService: CryptoService
  ) {}

  // #region GET

  public GetPermissaoPerfil(
    perCodi: number
  ): Observable<PermissaoPerfilListaModel[]> {
    return this.http.get<PermissaoPerfilListaModel[]>(
      `${environment.apiServicos}/Permissao/GetPermissaoPerfil/${perCodi}`
    );
  }

  public GetSessao(perCodi: number): Observable<SessaoModel[]> {
    return this.http.get<SessaoModel[]>(
      `${environment.apiServicos}/Sessao/GetSessao/${perCodi}`
    );
  }

  public GetSessaoByLojCodi(lojCodi: number): Observable<SessaoListaModel[]> {
    return this.http.get<SessaoListaModel[]>(
      `${environment.apiServicos}/Sessao/GetSessaoByLojCodi/${lojCodi}`
    );
  }

  public GetLojaByIdUsuario(usuCodi: number): Observable<LojaModel[]> {
    return this.http.get<LojaModel[]>(
      `${environment.apiServicos}/Loja/GetLojaByIdUsuario/${usuCodi}`
    );
  }

  public GetTipoSessao(TiScodi: number): Observable<TipoSessaoModel[]> {
    return this.http.get<TipoSessaoModel[]>(
      `${environment.apiServicos}/TipoSessao/GetTipoSessao/${TiScodi}`
    );
  }

  public GetGrau(GraCodi: number): Observable<GrauModel[]> {
    return this.http.get<GrauModel[]>(
      `${environment.apiServicos}/Grau/GetGrau/${GraCodi}`
    );
  }

  public GetValidaNumeroSessao(
    sesNume: number,
    lojCodi: number
  ): Observable<SessaoModel> {
    return this.http.get<SessaoModel>(
      `${environment.apiServicos}/Sessao/GetValidaNumeroSessao/${sesNume}/${lojCodi}`
    );
  }

  public GetSessaoBySesCodi(sesCodi: number): Observable<SessaoConviteModel> {
    return this.http.get<SessaoConviteModel>(
      `${environment.apiServicos}/Sessao/GetSessaoBySesCodi/${sesCodi}`
    );
  }

  public GetTemplateLoja(lojCodi: number): Observable<TemplateLojaModel> {
    return this.http.get<TemplateLojaModel>(
      `${environment.apiServicos}/TemplateLoja/GetTemplateLoja/${lojCodi}`
    );
  }

  public GetPotenciaRegularByLojCodi(
    lojCodi: number
  ): Observable<PotenciaModel[]> {
    return this.http.get<PotenciaModel[]>(
      `${environment.apiServicos}/Potencia/GetPotenciaRegularByLojCodi/${lojCodi}`
    );
  }

  public GetLojaByPotLojNume(
    PotCodi: number,
    LojNumL: string
  ): Observable<LojaModel> {
    return this.http.get<LojaModel>(
      `${environment.apiServicos}/Loja/GetLojaByPotLojNume/${PotCodi}/${LojNumL}`
    );
  }

  public GetUsuarioByLoja(
    UsuNCIM: string,
    PotCodi: number,
    LojNumL: string
  ): Observable<ConsultaUsuarioLojaModel> {
    return this.http.get<ConsultaUsuarioLojaModel>(
      `${environment.apiServicos}/Usuario/GetUsuarioByLoja/${UsuNCIM}/${PotCodi}/${LojNumL}`
    );
  }

  public GetListaPresencaBySesCodi(
    SesCodi: number
  ): Observable<ListaPresencaModel[]> {
    return this.http.get<ListaPresencaModel[]>(
      `${environment.apiServicos}/Presenca/GetListaPresencaBySesCodi/${SesCodi}`
    );
  }

  public GetUsuarioByLojCodi(lojCodi: number): Observable<UsuarioLojaModel[]> {
    return this.http.get<UsuarioLojaModel[]>(
      `${environment.apiServicos}/Usuario/GetUsuarioByLojCodi/${lojCodi}`
    );
  }

  public GetPerfil(perCodi: number): Observable<PerfilModel[]> {
    return this.http.get<PerfilModel[]>(
      `${environment.apiServicos}/Perfil/GetPerfil/${perCodi}`
    );
  }

  public GetPerfilByPerCodi(idsPerfil: string): Observable<PerfilModel[]> {
    return this.http.get<PerfilModel[]>(
      `${environment.apiServicos}/Perfil/GetPerfilByPerCodi/${idsPerfil}`
    );
  }

  public GetUsuarioByPotCodi(
    potCodi: number,
    usuNCIM: string
  ): Observable<UsuarioPotenciaModel> {
    return this.http.get<UsuarioPotenciaModel>(
      `${environment.apiServicos}/Usuario/GetUsuarioByPotCodi/${potCodi}/${usuNCIM}`
    );
  }

  public GetCertificado(usuCodi: number, sesCodi: number): Observable<string> {
    return this.http.get<string>(
      `${environment.apiServicos}/Presenca/GetCertificado/${usuCodi}/${sesCodi}`
    );
  }

  public GetPermissao(pemCodi: number): Observable<PermissaoModel[]> {
    return this.http.get<PermissaoModel[]>(
      `${environment.apiServicos}/Permissao/GetPermissao/${pemCodi}`
    );
  }

  public GetProximaSessao(pemCodi: number): Observable<ProximaSessaoModel[]> {
    return this.http.get<ProximaSessaoModel[]>(
      `${environment.apiServicos}/Sessao/GetProximaSessao/${pemCodi}`
    );
  }

  public GetLiberaBloqueiaSessao(
    sesCodi: number,
    sesLibe: boolean
  ): Observable<string> {
    return this.http.get<string>(
      `${environment.apiServicos}/Sessao/GetLiberaBloqueiaSessao/${sesCodi}/${sesLibe}`
    );
  }

  public GetListaGestaoAdmByLojCodi(
    lojCodi: number
  ): Observable<GestaoAdministrativa[]> {
    return this.http.get<GestaoAdministrativa[]>(
      `${environment.apiServicos}/GestaoAdministrativa/GetListaGestaoAdmByLojCodi/${lojCodi}`
    );
  }

  public GetQtdPresencaBySesCodi(
    lojCodi: number
  ): Observable<QuantitativoPresencaModel> {
    return this.http.get<QuantitativoPresencaModel>(
      `${environment.apiServicos}/Presenca/GetQtdPresencaBySesCodi/${lojCodi}`
    );
  }

  // #endregion

  // #region POST

  public ValidarLogin(
    usuario: string,
    senha: string
  ): Observable<UsuarioLogadoModel> {
    let objLogin: LoginModel = new LoginModel();
    objLogin.usuario = usuario;
    objLogin.senha = senha;
    return this.http.post<UsuarioLogadoModel>(
      `${environment.apiServicos}/Util/Login`,
      objLogin
    );
  }

  public PostSessao(objSessao: SessaoListaModel): Observable<string> {
    return this.http.post<string>(
      `${environment.apiServicos}/Sessao/PostSessao`,
      objSessao
    );
  }

  public PostConfirmaPresenca(
    objPresenca: ConsultaUsuarioLojaModel
  ): Observable<string> {
    return this.http.post<string>(
      `${environment.apiServicos}/Presenca/PostConfirmaPresenca`,
      objPresenca
    );
  }

  public PostUsuarioCompleto(
    objPresenca: UsuarioPotenciaModel
  ): Observable<string> {
    return this.http.post<string>(
      `${environment.apiServicos}/Usuario/PostUsuarioCompleto`,
      objPresenca
    );
  }

  public encryptAPI(objMensagem: EncryptAPIModel): Observable<string> {
    // let encryptedText = this.encryptionService.encryptAPI(
    //   objMensagem.valorMensagem
    // );
    // console.log('Texto Encriptado:', encryptedText);

    // Opcional: Enviar para o backend
    return this.http.post<string>(
      `${environment.apiServicos}/Util/Encrypt`,
      objMensagem // Envia a string diretamente
    );
  }

  public decryptAPI(objMensagem: EncryptAPIModel): Observable<string> {
    // let decryptedText = this.encryptionService.decryptAPI(
    //   objMensagem.valorMensagem
    // );
    // console.log('Texto Decriptado:', decryptedText);

    // Opcional: Enviar para o backend
    return this.http.post<string>(
      `${environment.apiServicos}/Util/Decrypt`,
      objMensagem,
      { responseType: 'text' as 'json' }
    );
  }

  public PostPerfil(objPerfil: PerfilModel): Observable<string> {
    return this.http.post<string>(
      `${environment.apiServicos}/Perfil/PostPerfil`,
      objPerfil
    );
  }

  public PostLancamentoPresencaSessao(
    objPresenca: ListaPresencaModel[]
  ): Observable<string> {
    return this.http.post<string>(
      `${environment.apiServicos}/Presenca/PostLancamentoPresencaSessao`,
      objPresenca
    );
  }

  public PostPermissao(objPermissao: PermissaoModel): Observable<string> {
    return this.http.post<string>(
      `${environment.apiServicos}/Permissao/PostPermissao`,
      objPermissao
    );
  }

  // #endregion

  // #region PUT

  public PutSessao(
    sesCodi: number,
    objSessao: SessaoModel
  ): Observable<string> {
    return this.http.put<string>(
      `${environment.apiServicos}/Sessao/PutSessao/${sesCodi}`,
      objSessao
    );
  }

  public PutUsuarioCompleto(
    usuCodi: number,
    usuarioCompleto: UsuarioPotenciaModel
  ): Observable<string> {
    return this.http.put<string>(
      `${environment.apiServicos}/Usuario/PutUsuarioCompleto/${usuCodi}`,
      usuarioCompleto
    );
  }

  public PutPermissaoPerfil(
    lstPermissaoPerfil: PermissaoPerfilListaModel[]
  ): Observable<string> {
    return this.http.put<string>(
      `${environment.apiServicos}/Permissao/PutPermissaoPerfil`,
      lstPermissaoPerfil
    );
  }

  public PutPerfil(
    perCodi: number,
    objPerfil: PerfilModel
  ): Observable<string> {
    return this.http.put<string>(
      `${environment.apiServicos}/Perfil/PutPerfil/${perCodi}`,
      objPerfil
    );
  }

  public PutPermissao(
    pemCodi: number,
    objPermissao: PermissaoModel
  ): Observable<string> {
    return this.http.put<string>(
      `${environment.apiServicos}/Permissao/PutPermissao/${pemCodi}`,
      objPermissao
    );
  }

  // #endregion
}
