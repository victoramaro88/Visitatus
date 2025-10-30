export class PermissaoPerfilListaModel {
  perCodi: number;
  perNome: string;
  perStat: boolean;
  pemCodi: number;
  pemNome: string;
  pemStat: boolean;
  papAtvo: boolean;
  pepStat: boolean;

  constructor(
    perCodi: number,
    perNome: string,
    perStat: boolean,
    pemCodi: number,
    pemNome: string,
    pemStat: boolean,
    papAtvo: boolean,
    pepStat: boolean
  ) {
    this.perCodi = perCodi;
    this.perNome = perNome;
    this.perStat = perStat;
    this.pemCodi = pemCodi;
    this.pemNome = pemNome;
    this.pemStat = pemStat;
    this.papAtvo = papAtvo;
    this.pepStat = pepStat;
  }
}
