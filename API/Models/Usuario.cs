using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace API_Visitatus.Models
{
    [Table("Usuario", Schema = "dbo")]
    public partial class Usuario
    {
        public Usuario()
        {
            GestaoCargos = new HashSet<GestaoCargo>();
            PerfilUsuarios = new HashSet<PerfilUsuario>();
            Presencas = new HashSet<Presenca>();
            UsuarioLogins = new HashSet<UsuarioLogin>();
        }

        [Key]
        [Column("usuCodi")]
        public long UsuCodi { get; set; }
        [Column("usuNome")]
        [StringLength(250)]
        [Unicode(false)]
        public string UsuNome { get; set; } = null!;
        [Column("usuNCIM")]
        [StringLength(10)]
        [Unicode(false)]
        public string UsuNcim { get; set; } = null!;
        [Column("usuNasc", TypeName = "datetime")]
        public DateTime UsuNasc { get; set; }
        [Column("usuEmai")]
        [StringLength(200)]
        [Unicode(false)]
        public string UsuEmai { get; set; } = null!;
        [Column("usuNCel")]
        [StringLength(11)]
        [Unicode(false)]
        public string UsuNcel { get; set; } = null!;
        [Column("usuStat")]
        public bool UsuStat { get; set; }

        [InverseProperty(nameof(GestaoCargo.UsuCodiNavigation))]
        public virtual ICollection<GestaoCargo> GestaoCargos { get; set; }
        [InverseProperty(nameof(PerfilUsuario.UsuCodiNavigation))]
        public virtual ICollection<PerfilUsuario> PerfilUsuarios { get; set; }
        [InverseProperty(nameof(Presenca.UsuCodiNavigation))]
        public virtual ICollection<Presenca> Presencas { get; set; }
        [InverseProperty(nameof(UsuarioLogin.UsuCodiNavigation))]
        public virtual ICollection<UsuarioLogin> UsuarioLogins { get; set; }
    }
}
