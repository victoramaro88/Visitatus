
import { HttpClient } from "@angular/common/http";
import { Injectable } from '@angular/core';
import { Observable, of } from 'rxjs';
import { environment } from "../../environments/environment";
import { LoginModel } from "../models/Login.Model";
import { UsuarioLogadoModel } from "../models/UsuarioLogado.Model";
import { PermissaoPerfilListaModel } from "../models/PermissaoPerfilLista.Model ";

@Injectable({
    providedIn: 'root'
  })

export class HttpService {

  constructor(private http: HttpClient) { }

  // #region GET

  public GetPermissaoPerfil(perCodi: number): Observable<PermissaoPerfilListaModel[]> {
    return this.http.get<PermissaoPerfilListaModel[]>(`${environment.apiServicos}/Permissao/GetPermissaoPerfil/${perCodi}`);
  }

  // #endregion

  // #region POST

  public ValidarLogin(usuario: string, senha: string): Observable<UsuarioLogadoModel> {
    let objLogin: LoginModel = new LoginModel();
    objLogin.usuario = usuario;
    objLogin.senha = senha;
    return this.http.post<UsuarioLogadoModel>(`${environment.apiServicos}/Util/Login`, objLogin);
  }

  // public InserirPessoa(objPessoa: PessoaDTO): Observable<string> {
  //   return this.http.post<string>(`${environment.apiServicos}/Pessoa/InserirPessoa`, objPessoa);
  // }

  // #endregion

  // #region PUT

  // public PutConsultorio(conCodi: number, objConsultorio: ConsultorioModel): Observable<string> {
  //   return this.http.put<string>(`${environment.apiServicos}/Consultorio/${conCodi}`, objConsultorio);
  // }

  // #endregion

}
