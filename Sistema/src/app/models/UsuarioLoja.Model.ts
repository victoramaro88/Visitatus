export class UsuarioLojaModel {
  UsuCodi: number;
  UsuNome: string;
  UsuNCIM: string;
  UsuNCel: string;
  UsuEmai: string;
  UsuNasc: Date | null;
  UsuStat: boolean;
  PerCodi: number;
  PerNome: string;
  PeUStat: boolean;

  constructor() {
    this.UsuCodi = 0;
    this.UsuNome = '';
    this.UsuNCIM = '';
    this.UsuNCel = '';
    this.UsuEmai = '';
    this.UsuNasc = null;
    this.UsuStat = false;
    this.PerCodi = 0;
    this.PerNome = '';
    this.PeUStat = false;
  }
}
