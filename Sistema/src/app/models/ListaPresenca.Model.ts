export class ListaPresencaModel {
  SesCodi: number;
  UsuCodi: number;
  UsuNome: string;
  UsuNCIM: string;
  LojNome: string;
  LojNumL: string;
  PreAtiv: boolean;

  constructor() {
    this.SesCodi = 0;
    this.UsuCodi = 0;
    this.UsuNome = '';
    this.UsuNCIM = '';
    this.LojNome = '';
    this.LojNumL = '';
    this.PreAtiv = false;
  }
}
