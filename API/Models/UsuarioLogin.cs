using System;
using System.Collections.Generic;

namespace API_Visitatus.Models
{
    public partial class UsuarioLogin
    {
        public long UsLcodi { get; set; }
        public string UsLuser { get; set; } = null!;
        public string UsLpass { get; set; } = null!;
        public bool UsLstat { get; set; }
        public long UsuCodi { get; set; }

        public virtual Usuario UsuCodiNavigation { get; set; } = null!;
    }
}
