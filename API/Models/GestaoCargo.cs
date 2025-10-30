using System;
using System.Collections.Generic;

namespace API_Visitatus.Models
{
    public partial class GestaoCargo
    {
        public long GstAdmCodi { get; set; }
        public long CarCodi { get; set; }
        public long UsuCodi { get; set; }
        public bool GstCarStat { get; set; }

        public virtual Cargo CarCodiNavigation { get; set; } = null!;
        public virtual GestaoAdministrativa GstAdmCodiNavigation { get; set; } = null!;
        public virtual Usuario UsuCodiNavigation { get; set; } = null!;
    }
}
