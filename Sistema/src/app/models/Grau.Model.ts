export class GrauModel {
  GraCodi: number;
  GraNome: string;
  GraStat: boolean;

  constructor(
    GraCodi: number,
    GraNome: string,
    GraStat: boolean
  ) {
    this.GraCodi = GraCodi;
    this.GraNome = GraNome;
    this.GraStat = GraStat;
  }
}
