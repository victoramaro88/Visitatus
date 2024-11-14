import { LojaModel } from "./Loja.Model";
import { PerfilUsuarioListaModel } from "./PerfilUsuarioLista.Model";

export class UsuarioLogadoModel {
  usLCodi: number;
  usuCodi: number;
  usuNome: string;
  lojasUsuario: LojaModel[];
  lstPerfil: PerfilUsuarioListaModel[];

  constructor() {
    this.usLCodi = 0;
    this.usuCodi = 0;
    this.usuNome = "";
    this.lojasUsuario = [];
    this.lstPerfil = [];
  }
}
