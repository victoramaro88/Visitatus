using System;
using System.Collections.Generic;

namespace API_Visitatus.Models
{
    public partial class Grau
    {
        public Grau()
        {
            Sessaos = new HashSet<Sessao>();
        }

        public int GraCodi { get; set; }
        public string GraNome { get; set; } = null!;
        public bool GraStat { get; set; }

        public virtual ICollection<Sessao> Sessaos { get; set; }
    }
}
