export class UsuarioModel{
  UsuCodi: number;
  UsuNome: string;
  UsuNcim: string;
  UsuNasc: Date;
  UsuEmai: string;
  UsuNcel: string;
  UsuStat: boolean;

  constructor() {
    this.UsuCodi = 0;
    this.UsuNome = "";
    this.UsuNcim = "";
    this.UsuNasc = new Date();
    this.UsuEmai = "";
    this.UsuNcel = "";
    this.UsuStat = false;
  }
}
