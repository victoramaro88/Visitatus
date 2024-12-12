using System;
using System.Collections.Generic;

namespace API_Visitatus.Models
{
    public partial class TemplateCertificadoLoja
    {
        public int TmpCrtPreCodi { get; set; }
        public long LojCodi { get; set; }
        public bool TmpCrtStat { get; set; }

        public virtual Loja LojCodiNavigation { get; set; } = null!;
        public virtual TemplateCertificadoPresenca TmpCrtPreCodiNavigation { get; set; } = null!;
    }
}
