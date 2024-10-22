using System;
using System.Collections.Generic;

namespace API_Visitatus.Models
{
    public partial class PerfilUsuario
    {
        public int PeUcodi { get; set; }
        public bool PeUstat { get; set; }
        public int PerCodi { get; set; }
        public long UsuCodi { get; set; }

        public virtual Perfil PerCodiNavigation { get; set; } = null!;
        public virtual Usuario UsuCodiNavigation { get; set; } = null!;
    }
}
