export class ConsultaUsuarioLojaModel {
  objUsuarioLoja: UsrLoja;
  objLojaConsulta: LojaConsulta;

  constructor() {
    this.objUsuarioLoja = new UsrLoja();
    this.objLojaConsulta = new LojaConsulta();
  }
}

export class UsrLoja {
  UsuCodi: number;
  UsuNome: string;
  UsuNCIM: string;
  UsuNasc: Date;
  UsuEmai: string;
  UsuNCel: string;
  UsuStat: boolean;
  LojNome: string;
  LojNumL: string;
  PotCodi: number;

  constructor() {
    this.UsuCodi = 0;
    this.UsuNome = '';
    this.UsuNCIM = '';
    this.UsuNasc = new Date();
    this.UsuEmai = '';
    this.UsuNCel = '';
    this.UsuStat = false;
    this.LojNome = '';
    this.LojNumL = '';
    this.PotCodi = 0;
  }
}

export class LojaConsulta {
  LojCodi: number;
  LojNome: string;
  LojNumL: string;
  LojLogo: string;
  LojLogr: string;
  LojNume: string;
  LojBair: string;
  LojStat: boolean;
  CidCodi: number;
  PotCodi: number;
  RitCodi: number;

  constructor() {
    this.LojCodi = 0;
    this.LojNome = '';
    this.LojNumL = '';
    this.LojLogo = '';
    this.LojLogr = '';
    this.LojNume = '';
    this.LojBair = '';
    this.LojStat = false;
    this.CidCodi = 0;
    this.PotCodi = 0;
    this.RitCodi = 0;
  }
}
