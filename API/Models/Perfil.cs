using System;
using System.Collections.Generic;

namespace API_Visitatus.Models
{
    public partial class Perfil
    {
        public Perfil()
        {
            PerfilUsuarios = new HashSet<PerfilUsuario>();
            PermissaoPerfils = new HashSet<PermissaoPerfil>();
        }

        public int PerCodi { get; set; }
        public string PerNome { get; set; } = null!;
        public bool PerStat { get; set; }

        public virtual ICollection<PerfilUsuario> PerfilUsuarios { get; set; }
        public virtual ICollection<PermissaoPerfil> PermissaoPerfils { get; set; }
    }
}
