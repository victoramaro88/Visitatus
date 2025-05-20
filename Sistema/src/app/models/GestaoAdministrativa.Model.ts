export class GestaoAdministrativa {
  GstAdmCodi: number;
  GstAdmNome: string;
  GstAdmDtIn: Date;
  GstAdmDtFi: Date;
  GstAdmStat: boolean;
  LojCodi: number;

  constructor() {
    this.GstAdmCodi = 0;
    this.GstAdmNome = '';
    this.GstAdmDtIn = new Date();
    this.GstAdmDtFi = new Date();
    this.GstAdmStat = false;
    this.LojCodi = 0;
  }
}
