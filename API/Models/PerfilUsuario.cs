using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace API_Visitatus.Models
{
    [Table("PerfilUsuario", Schema = "dbo")]
    public partial class PerfilUsuario
    {
        [Key]
        [Column("peUCodi")]
        public int PeUcodi { get; set; }
        [Column("peUStat")]
        public bool PeUstat { get; set; }
        [Column("perCodi")]
        public int PerCodi { get; set; }
        [Column("usuCodi")]
        public long UsuCodi { get; set; }
        [Column("lojCodi")]
        public long LojCodi { get; set; }

        [ForeignKey(nameof(LojCodi))]
        [InverseProperty(nameof(Loja.PerfilUsuarios))]
        public virtual Loja LojCodiNavigation { get; set; } = null!;
        [ForeignKey(nameof(PerCodi))]
        [InverseProperty(nameof(Perfil.PerfilUsuarios))]
        public virtual Perfil PerCodiNavigation { get; set; } = null!;
        [ForeignKey(nameof(UsuCodi))]
        [InverseProperty(nameof(Usuario.PerfilUsuarios))]
        public virtual Usuario UsuCodiNavigation { get; set; } = null!;
    }
}
