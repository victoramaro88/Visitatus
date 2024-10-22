using System;
using System.Collections.Generic;

namespace API_Visitatus.Models
{
    public partial class Rito
    {
        public Rito()
        {
            Lojas = new HashSet<Loja>();
        }

        public int RitCodi { get; set; }
        public string RitNome { get; set; } = null!;
        public string? RitLogo { get; set; }
        public bool RitStat { get; set; }

        public virtual ICollection<Loja> Lojas { get; set; }
    }
}
