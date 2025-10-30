using System;
using System.Collections.Generic;

namespace API_Visitatus.Models
{
    public partial class PermissaoPerfil
    {
        public int PerCodi { get; set; }
        public int PemCodi { get; set; }
        public bool PapAtvo { get; set; }
        public bool PepStat { get; set; }

        public virtual Permissao PemCodiNavigation { get; set; } = null!;
        public virtual Perfil PerCodiNavigation { get; set; } = null!;
    }
}
