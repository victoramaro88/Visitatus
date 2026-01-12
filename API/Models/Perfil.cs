using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace API_Visitatus.Models
{
    [Table("Perfil", Schema = "dbo")]
    public partial class Perfil
    {
        public Perfil()
        {
            PerfilUsuarios = new HashSet<PerfilUsuario>();
            PermissaoPerfils = new HashSet<PermissaoPerfil>();
        }

        [Key]
        [Column("perCodi")]
        public int PerCodi { get; set; }
        [Column("perNome")]
        [StringLength(100)]
        [Unicode(false)]
        public string PerNome { get; set; } = null!;
        [Column("perStat")]
        public bool PerStat { get; set; }

        [InverseProperty(nameof(PerfilUsuario.PerCodiNavigation))]
        public virtual ICollection<PerfilUsuario> PerfilUsuarios { get; set; }
        [InverseProperty(nameof(PermissaoPerfil.PerCodiNavigation))]
        public virtual ICollection<PermissaoPerfil> PermissaoPerfils { get; set; }
    }
}
