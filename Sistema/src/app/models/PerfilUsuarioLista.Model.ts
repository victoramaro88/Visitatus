export class PerfilUsuarioListaModel {
  peUCodi: number;
  peUStat: boolean;
  perCodi: number;
  usuCodi: number;
  lojCodi: number;
  perNome: string;
  perStat: boolean;
  lojNome: string;
  lojNumL: string;
  lojStat: boolean;
  potCodi: number;

  constructor() {
    this.peUCodi = 0;
    this.peUStat = false;
    this.perCodi = 0;
    this.usuCodi = 0;
    this.lojCodi = 0;
    this.perNome = '';
    this.perStat = false;
    this.lojNome = '';
    this.lojNumL = '';
    this.lojStat = false;
    this.potCodi = 0;
  }
}
