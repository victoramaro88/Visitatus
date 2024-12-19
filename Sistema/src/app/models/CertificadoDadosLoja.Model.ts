export class CertificadoDadosLojaModel {
  lojCodi: number;
  lojNome: string;
  lojNumL: string;
  potSigl: string;
  potNome: string;
  sesDtHr: Date;
  cidNome: string;
  estSigl: string;
  lojLogo: string;
  potLogo: string;

  constructor() {
    this.lojCodi = 0;
    this.lojNome = '';
    this.lojNumL = '';
    this.potSigl = '';
    this.potNome = '';
    this.sesDtHr = new Date();
    this.cidNome = '';
    this.estSigl = '';
    this.lojLogo = '';
    this.potLogo = '';
  }
}
