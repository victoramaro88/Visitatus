export class ListaPresencaModel {
  SesCodi: number;
  UsuCodi: number;
  UsuNome: string;
  UsuNCIM: string;
  LojCodi: number;
  LojNome: string;
  LojNumL: string;
  PreAtiv: boolean;
  PreEmai: boolean;
  MembroLoja: boolean;

  constructor() {
    this.SesCodi = 0;
    this.UsuCodi = 0;
    this.UsuNome = '';
    this.UsuNCIM = '';
    this.LojNome = '';
    this.LojCodi = 0;
    this.LojNumL = '';
    this.PreAtiv = false;
    this.PreEmai = false;
    this.MembroLoja = false;
  }
}
