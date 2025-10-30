using System;
using System.Collections.Generic;

namespace API_Visitatus.Models
{
    public partial class TemplateCertificadoPresenca
    {
        public TemplateCertificadoPresenca()
        {
            TemplateCertificadoLojas = new HashSet<TemplateCertificadoLoja>();
        }

        public int TmpCrtPreCodi { get; set; }
        public string TmpCrtPreNome { get; set; } = null!;
        public string TmpCrtPreMode { get; set; } = null!;
        public bool TmpCrtPreStat { get; set; }

        public virtual ICollection<TemplateCertificadoLoja> TemplateCertificadoLojas { get; set; }
    }
}
