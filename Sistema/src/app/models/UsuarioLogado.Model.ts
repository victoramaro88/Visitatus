import { PerfilUsuarioListaModel } from "./PerfilUsuarioLista.Model";

export class UsuarioLogadoModel {
  usLCodi: number;
  usuCodi: number;
  usuNome: string;
  lojCodi: number;
  lstPerfil: PerfilUsuarioListaModel[];

  constructor() {
    this.usLCodi = 0;
    this.usuCodi = 0;
    this.usuNome = "";
    this.lojCodi = 0;
    this.lstPerfil = [];
  }
}
