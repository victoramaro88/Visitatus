import { GrauModel } from './../models/Grau.Model';
import { TipoSessaoModel } from './../models/TipoSessao.Model';

import { HttpClient } from "@angular/common/http";
import { Injectable } from '@angular/core';
import { Observable, of } from 'rxjs';
import { environment } from "../../environments/environment";
import { LoginModel } from "../models/Login.Model";
import { UsuarioLogadoModel } from "../models/UsuarioLogado.Model";
import { PermissaoPerfilListaModel } from "../models/PermissaoPerfilLista.Model ";
import { SessaoModel } from "../models/Sessao.Model";
import { SessaoListaModel } from "../models/SessaoLista.Model";
import { LojaModel } from "../models/Loja.Model";

@Injectable({
    providedIn: 'root'
  })

export class HttpService {

  constructor(private http: HttpClient) { }

  // #region GET

  public GetPermissaoPerfil(perCodi: number): Observable<PermissaoPerfilListaModel[]> {
    return this.http.get<PermissaoPerfilListaModel[]>(`${environment.apiServicos}/Permissao/GetPermissaoPerfil/${perCodi}`);
  }

  public GetSessao(perCodi: number): Observable<SessaoModel[]> {
    return this.http.get<SessaoModel[]>(`${environment.apiServicos}/Sessao/GetSessao/${perCodi}`);
  }

  public GetSessaoByLojCodi(lojCodi: number): Observable<SessaoListaModel[]> {
    return this.http.get<SessaoListaModel[]>(`${environment.apiServicos}/Sessao/GetSessaoByLojCodi/${lojCodi}`);
  }

  public GetLojaByIdUsuario(usuCodi: number): Observable<LojaModel[]> {
    return this.http.get<LojaModel[]>(`${environment.apiServicos}/Loja/GetLojaByIdUsuario/${usuCodi}`);
  }

  public GetTipoSessao(TiScodi: number): Observable<TipoSessaoModel[]> {
    return this.http.get<TipoSessaoModel[]>(`${environment.apiServicos}/TipoSessao/GetTipoSessao/${TiScodi}`);
  }

  public GetGrau(GraCodi: number): Observable<GrauModel[]> {
    return this.http.get<GrauModel[]>(`${environment.apiServicos}/Grau/GetGrau/${GraCodi}`);
  }

  public GetValidaNumeroSessao(sesNume: number, lojCodi: number): Observable<SessaoModel> {
    return this.http.get<SessaoModel>(`${environment.apiServicos}/Sessao/GetValidaNumeroSessao/${sesNume}/${lojCodi}`);
  }

  // #endregion

  // #region POST

  public ValidarLogin(usuario: string, senha: string): Observable<UsuarioLogadoModel> {
    let objLogin: LoginModel = new LoginModel();
    objLogin.usuario = usuario;
    objLogin.senha = senha;
    return this.http.post<UsuarioLogadoModel>(`${environment.apiServicos}/Util/Login`, objLogin);
  }

  public PostSessao(objSessao: SessaoListaModel): Observable<string> {
    return this.http.post<string>(`${environment.apiServicos}/Sessao/PostSessao`, objSessao);
  }

  // #endregion

  // #region PUT

  public PutSessao(sesCodi: number, objSessao: SessaoModel): Observable<string> {
    return this.http.put<string>(`${environment.apiServicos}/Sessao/PutSessao/${sesCodi}`, objSessao);
  }

  // #endregion

}
