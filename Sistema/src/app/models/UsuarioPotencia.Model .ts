export class LojaUsuarioPotenciaModel {
  LojCodi: number;
  PerCodi: number;
  LojNome: string;
  LojNumL: string;
  PotCodi: number;
  PerNome: string;
  PerStat: boolean;

  constructor() {
    this.LojCodi = 0;
    this.PerCodi = 0;
    this.LojNome = '';
    this.LojNumL = '';
    this.PotCodi = 0;
    this.PerNome = '';
    this.PerStat = false;
  }
}

export class UsuarioPotenciaModel {
  UsuCodi: number;
  UsuNome: string;
  UsuNasc: Date;
  UsuEmai: string;
  UsuNCel: string;
  UsuStat: boolean;
  UsuNCIM: string;
  lstLjUsrPot: LojaUsuarioPotenciaModel[];

  constructor() {
    this.UsuCodi = 0;
    this.UsuNome = '';
    this.UsuNasc = new Date();
    this.UsuEmai = '';
    this.UsuNCel = '';
    this.UsuStat = false;
    this.UsuNCIM = '';
    this.lstLjUsrPot = [];
  }
}
