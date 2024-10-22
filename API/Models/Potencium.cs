using System;
using System.Collections.Generic;

namespace API_Visitatus.Models
{
    public partial class Potencium
    {
        public Potencium()
        {
            Lojas = new HashSet<Loja>();
        }

        public int PotCodi { get; set; }
        public string PotNome { get; set; } = null!;
        public string PotLogo { get; set; } = null!;
        public bool PotRegu { get; set; }
        public bool PotStat { get; set; }

        public virtual ICollection<Loja> Lojas { get; set; }
    }
}
