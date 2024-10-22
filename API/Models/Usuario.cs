using System;
using System.Collections.Generic;

namespace API_Visitatus.Models
{
    public partial class Usuario
    {
        public Usuario()
        {
            PerfilUsuarios = new HashSet<PerfilUsuario>();
            Presencas = new HashSet<Presenca>();
            UsuarioLogins = new HashSet<UsuarioLogin>();
            UsuarioLojas = new HashSet<UsuarioLoja>();
        }

        public long UsuCodi { get; set; }
        public string UsuNome { get; set; } = null!;
        public string UsuNcim { get; set; } = null!;
        public DateTime UsuNasc { get; set; }
        public string UsuEmai { get; set; } = null!;
        public string UsuNcel { get; set; } = null!;
        public bool UsuStat { get; set; }

        public virtual ICollection<PerfilUsuario> PerfilUsuarios { get; set; }
        public virtual ICollection<Presenca> Presencas { get; set; }
        public virtual ICollection<UsuarioLogin> UsuarioLogins { get; set; }
        public virtual ICollection<UsuarioLoja> UsuarioLojas { get; set; }
    }
}
