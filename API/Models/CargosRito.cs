using System;
using System.Collections.Generic;

namespace API_Visitatus.Models
{
    public partial class CargosRito
    {
        public long CarCodi { get; set; }
        public int RitCodi { get; set; }
        public int CariOrdm { get; set; }
        public bool CariStat { get; set; }

        public virtual Cargo CarCodiNavigation { get; set; } = null!;
        public virtual Rito RitCodiNavigation { get; set; } = null!;
    }
}
