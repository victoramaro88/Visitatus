export class SessaoModel {
  SesCodi: number;
  SesDesc: string;
  SesNome: string;
  SesDtHr: Date;
  SesLibe: boolean;
  SesStat: boolean;
  LojCodi: number;
  GraCodi: number;
  TiScodi: number;
  SesNume: number;
  SesAgap: boolean;
  SesVlAg: number;

  constructor() {
    this.SesCodi = 0;
    this.SesDesc = "";
    this.SesNome = "";
    this.SesDtHr = new Date();
    this.SesLibe = false;
    this.SesStat = false;
    this.LojCodi = 0;
    this.GraCodi = 0;
    this.TiScodi = 0;
    this.SesNume = 0;
    this.SesAgap = false;
    this.SesVlAg = 0;
  }
}
