export class SessaoConviteModel {
  SesCodi: number;
  SesNume: number;
  SesNome?: string;
  SesDesc?: string;
  SesDtHr: Date;
  SesLibe: boolean;
  SesStat: boolean;
  TiSNome?: string;
  GraNome?: string;
  LojCodi: number;
  LojNome?: string;
  LojStat: boolean;
  LojNume?: string;
  LojLogr?: string;
  LojNumL?: string;
  LojBair?: string;
  PotNome?: string;
  PotSigl?: string;
  PotRegu: boolean;
  RitNome?: string;
  CidNome?: string;
  EstSigl?: string;
  LojLogo?: string;
  PotLogo?: string;

  constructor(
    SesCodi: number,
    SesNume: number,
    SesDtHr: Date,
    SesLibe: boolean,
    SesStat: boolean,
    LojCodi: number,
    LojStat: boolean,
    PotRegu: boolean,
    SesNome?: string,
    SesDesc?: string,
    TiSNome?: string,
    GraNome?: string,
    LojNome?: string,
    LojNume?: string,
    LojLogr?: string,
    LojNumL?: string,
    LojBair?: string,
    PotNome?: string,
    PotSigl?: string,
    RitNome?: string,
    CidNome?: string,
    EstSigl?: string,
    LojLogo?: string,
    PotLogo?: string
  ) {
    this.SesCodi = SesCodi;
    this.SesNume = SesNume;
    this.SesNome = SesNome;
    this.SesDesc = SesDesc;
    this.SesDtHr = SesDtHr;
    this.SesLibe = SesLibe;
    this.SesStat = SesStat;
    this.TiSNome = TiSNome;
    this.GraNome = GraNome;
    this.LojCodi = LojCodi;
    this.LojNome = LojNome;
    this.LojStat = LojStat;
    this.LojNume = LojNume;
    this.LojLogr = LojLogr;
    this.LojNumL = LojNumL;
    this.LojBair = LojBair;
    this.PotNome = PotNome;
    this.PotSigl = PotSigl;
    this.PotRegu = PotRegu;
    this.RitNome = RitNome;
    this.CidNome = CidNome;
    this.EstSigl = EstSigl;
    this.LojLogo = LojLogo;
    this.PotLogo = PotLogo;
  }
}
