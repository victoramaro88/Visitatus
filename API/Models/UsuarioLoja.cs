using System;
using System.Collections.Generic;

namespace API_Visitatus.Models
{
    public partial class UsuarioLoja
    {
        public long UsuCodi { get; set; }
        public long LojCodi { get; set; }
        public bool UsLstat { get; set; }

        public virtual Loja LojCodiNavigation { get; set; } = null!;
        public virtual Usuario UsuCodiNavigation { get; set; } = null!;
    }
}
