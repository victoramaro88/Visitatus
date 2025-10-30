using System;
using System.Collections.Generic;

namespace API_Visitatus.Models
{
    public partial class GestaoAdministrativa
    {
        public GestaoAdministrativa()
        {
            GestaoCargos = new HashSet<GestaoCargo>();
        }

        public long GstAdmCodi { get; set; }
        public string GstAdmNome { get; set; } = null!;
        public DateTime GstAdmDtIn { get; set; }
        public DateTime GstAdmDtFi { get; set; }
        public bool GstAdmStat { get; set; }
        public long LojCodi { get; set; }

        public virtual Loja LojCodiNavigation { get; set; } = null!;
        public virtual ICollection<GestaoCargo> GestaoCargos { get; set; }
    }
}
