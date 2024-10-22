using System;
using System.Collections.Generic;

namespace API_Visitatus.Models
{
    public partial class Permissao
    {
        public Permissao()
        {
            PermissaoPerfils = new HashSet<PermissaoPerfil>();
        }

        public int PemCodi { get; set; }
        public string PemNome { get; set; } = null!;
        public bool PemStat { get; set; }

        public virtual ICollection<PermissaoPerfil> PermissaoPerfils { get; set; }
    }
}
