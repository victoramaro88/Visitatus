using System;
using System.Collections.Generic;

namespace API_Visitatus.Models
{
    public partial class Presenca
    {
        public long UsuCodi { get; set; }
        public long SesCodi { get; set; }
        public bool PreAtiv { get; set; }

        public virtual Sessao SesCodiNavigation { get; set; } = null!;
        public virtual Usuario UsuCodiNavigation { get; set; } = null!;
    }
}
