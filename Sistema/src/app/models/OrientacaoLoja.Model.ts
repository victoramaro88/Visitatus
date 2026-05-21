export class OrientacaoLojaModel {
  OrlCodi: number;
  OrlDesc: string;
  OrlDtHr: Date;
  OrlStat: boolean;
  LojCodi: number;
  UsuCodi: number;

  constructor() {
    this.OrlCodi = 0;
    this.OrlDesc = "";
    this.OrlDtHr = new Date();
    this.OrlStat = false;
    this.LojCodi = 0;
    this.UsuCodi = 0;
  }
}