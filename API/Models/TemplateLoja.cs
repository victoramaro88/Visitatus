using System;
using System.Collections.Generic;

namespace API_Visitatus.Models
{
    public partial class TemplateLoja
    {
        public int TmpCvtCodi { get; set; }
        public long LojCodi { get; set; }
        public bool TmpLjStat { get; set; }

        public virtual Loja LojCodiNavigation { get; set; } = null!;
        public virtual TemplateConvite TmpCvtCodiNavigation { get; set; } = null!;
    }
}
