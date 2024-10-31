export class SessaoListaModel {
  SesCodi: number;
  SesDesc: string;
  SesDtHr: Date;
  SesLibe: boolean;
  SesStat: boolean;
  LojCodi: number;
  GraCodi: number;
  GraNome: string;
  TiSCodi: number;
  TiSNome: string;
  SesNume: number;

  constructor(
    SesCodi: number,
    SesDesc: string,
    SesDtHr: Date,
    SesLibe: boolean,
    SesStat: boolean,
    LojCodi: number,
    GraCodi: number,
    GraNome: string,
    TiSCodi: number,
    TiSNome: string,
    SesNume: number
  ) {
    this.SesCodi = SesCodi;
    this.SesDesc = SesDesc;
    this.SesDtHr = SesDtHr;
    this.SesLibe = SesLibe;
    this.SesStat = SesStat;
    this.LojCodi = LojCodi;
    this.GraCodi = GraCodi;
    this.GraNome = GraNome;
    this.TiSCodi = TiSCodi;
    this.TiSNome = TiSNome;
    this.SesNume = SesNume;
  }
}
