export class PermissaoModel {
  PemCodi: number;
  PemNome: string;
  PemStat: boolean;

  constructor() {
    this.PemCodi = 0;
    this.PemNome = "";
    this.PemStat = false;
  }
}
