export class TipoSessaoModel {
  TiScodi: number;
  TiSnome: string;
  TiSstat: boolean;

  constructor(
    TiScodi: number,
    TiSnome: string,
    TiSstat: boolean
  ) {
    this.TiScodi = TiScodi;
    this.TiSnome = TiSnome;
    this.TiSstat = TiSstat;
  }
}
