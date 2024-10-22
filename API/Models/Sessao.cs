using System;
using System.Collections.Generic;

namespace API_Visitatus.Models
{
    public partial class Sessao
    {
        public Sessao()
        {
            Presencas = new HashSet<Presenca>();
        }

        public long SesCodi { get; set; }
        public string SesDesc { get; set; } = null!;
        public DateTime SesDtHr { get; set; }
        public bool SesLibe { get; set; }
        public bool SesStat { get; set; }
        public long LojCodi { get; set; }
        public int GraCodi { get; set; }
        public int TiScodi { get; set; }

        public virtual Grau GraCodiNavigation { get; set; } = null!;
        public virtual Loja LojCodiNavigation { get; set; } = null!;
        public virtual TipoSessao TiScodiNavigation { get; set; } = null!;
        public virtual ICollection<Presenca> Presencas { get; set; }
    }
}
