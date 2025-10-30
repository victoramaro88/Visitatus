using System;
using System.Collections.Generic;

namespace API_Visitatus.Models
{
    public partial class TemplateConvite
    {
        public TemplateConvite()
        {
            TemplateLojas = new HashSet<TemplateLoja>();
        }

        public int TmpCvtCodi { get; set; }
        public string TmpCvtNome { get; set; } = null!;
        public string TmpCvtMode { get; set; } = null!;
        public bool TmpCvtStat { get; set; }

        public virtual ICollection<TemplateLoja> TemplateLojas { get; set; }
    }
}
