using System;
using System.Collections.Generic;

namespace API_Visitatus.Models
{
    public partial class TipoSessao
    {
        public TipoSessao()
        {
            Sessaos = new HashSet<Sessao>();
        }

        public int TiScodi { get; set; }
        public string TiSnome { get; set; } = null!;
        public bool TiSstat { get; set; }

        public virtual ICollection<Sessao> Sessaos { get; set; }
    }
}
