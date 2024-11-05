export class LojaModel {
  LojCodi: number;
  LojNome: string;
  LojNumL: string;
  LojLogo: string;
  LojLogr: string;
  LojNume: string;
  LojBair: string;
  LojStat: boolean;
  CidCodi: number;
  PotCodi: number;
  RitCodi: number;

  constructor(
    LojCodi: number,
    LojNome: string,
    LojNumL: string,
    LojLogo: string,
    LojLogr: string,
    LojNume: string,
    LojBair: string,
    LojStat: boolean,
    CidCodi: number,
    PotCodi: number,
    RitCodi: number
  ) {
    this.LojCodi = LojCodi;
    this.LojNome = LojNome;
    this.LojNumL = LojNumL;
    this.LojLogo = LojLogo;
    this.LojLogr = LojLogr;
    this.LojNume = LojNume;
    this.LojBair = LojBair;
    this.LojStat = LojStat;
    this.CidCodi = CidCodi;
    this.PotCodi = PotCodi;
    this.RitCodi = RitCodi;
  }
}
