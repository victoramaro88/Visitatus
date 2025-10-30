export class ProximaSessaoModel {
  SesCodi: number;
  SesNume: number;
  SesDtHr: Date;
  SesLibe: boolean;
  SesStat: boolean;
  SesNome: string;
  TotalPresenca: number;

  constructor() {
    this.SesCodi = 0;
    this.SesNume = 0;
    this.SesDtHr = new Date();
    this.SesLibe = false;
    this.SesStat = false;
    this.SesNome = '';
    this.TotalPresenca = 0;
  }
}
