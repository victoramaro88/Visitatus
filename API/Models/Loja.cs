using System;
using System.Collections.Generic;

namespace API_Visitatus.Models
{
    public partial class Loja
    {
        public Loja()
        {
            Sessaos = new HashSet<Sessao>();
            UsuarioLojas = new HashSet<UsuarioLoja>();
        }

        public long LojCodi { get; set; }
        public string LojNome { get; set; } = null!;
        public string LojNumL { get; set; } = null!;
        public string LojLogo { get; set; } = null!;
        public string LojLogr { get; set; } = null!;
        public string LojNume { get; set; } = null!;
        public string LojBair { get; set; } = null!;
        public bool LojStat { get; set; }
        public long CidCodi { get; set; }
        public int PotCodi { get; set; }
        public int RitCodi { get; set; }

        public virtual Cidade CidCodiNavigation { get; set; } = null!;
        public virtual Potencium PotCodiNavigation { get; set; } = null!;
        public virtual Rito RitCodiNavigation { get; set; } = null!;
        public virtual ICollection<Sessao> Sessaos { get; set; }
        public virtual ICollection<UsuarioLoja> UsuarioLojas { get; set; }
    }
}
