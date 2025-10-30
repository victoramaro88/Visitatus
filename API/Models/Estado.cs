using System;
using System.Collections.Generic;

namespace API_Visitatus.Models
{
    public partial class Estado
    {
        public Estado()
        {
            Cidades = new HashSet<Cidade>();
        }

        public int EstCodi { get; set; }
        public string EstNome { get; set; } = null!;
        public string EstSigl { get; set; } = null!;
        public bool EstStat { get; set; }

        public virtual ICollection<Cidade> Cidades { get; set; }
    }
}
