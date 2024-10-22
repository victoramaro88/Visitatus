using System;
using System.Collections.Generic;

namespace API_Visitatus.Models
{
    public partial class Cidade
    {
        public Cidade()
        {
            Lojas = new HashSet<Loja>();
        }

        public long CidCodi { get; set; }
        public string CidNome { get; set; } = null!;
        public bool CidStat { get; set; }
        public int EstCodi { get; set; }

        public virtual Estado EstCodiNavigation { get; set; } = null!;
        public virtual ICollection<Loja> Lojas { get; set; }
    }
}
