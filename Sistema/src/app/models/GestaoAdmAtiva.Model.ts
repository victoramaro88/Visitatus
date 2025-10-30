export class GestaoAdmAtivaModel {
  LojCodi: number;
  LojNome?: string;
  LojNumL?: string;
  GstAdmNome?: string;
  GstAdmDtIn: Date;
  GstAdmDtFi: Date;
  GstAdmStat: boolean;
  CarCodi?: number;
  CarNome?: string;
  UsuNome?: string;

  constructor(
    LojCodi: number,
    GstAdmDtIn: Date,
    GstAdmDtFi: Date,
    GstAdmStat: boolean,
    LojNome?: string,
    LojNumL?: string,
    GstAdmNome?: string,
    CarCodi?: number,
    CarNome?: string,
    UsuNome?: string
  ) {
    this.LojCodi = LojCodi;
    this.LojNome = LojNome;
    this.LojNumL = LojNumL;
    this.GstAdmNome = GstAdmNome;
    this.GstAdmDtIn = GstAdmDtIn;
    this.GstAdmDtFi = GstAdmDtFi;
    this.GstAdmStat = GstAdmStat;
    this.CarCodi = CarCodi;
    this.CarNome = CarNome;
    this.UsuNome = UsuNome;
  }
}
