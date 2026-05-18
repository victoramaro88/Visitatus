export class EstadoModel {
  EstCodi: number;
  EstNome: string;
  EstSigl: string;
  EstStat: boolean;

  constructor() {
    this.EstCodi = 0;
    this.EstNome = "";
    this.EstSigl = "";
    this.EstStat = false;
  }
}
