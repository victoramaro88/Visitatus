using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace API_Visitatus.Models
{
    [Table("PermissaoPerfil", Schema = "dbo")]
    public partial class PermissaoPerfil
    {
        [Key]
        [Column("perCodi")]
        public int PerCodi { get; set; }
        [Key]
        [Column("pemCodi")]
        public int PemCodi { get; set; }
        [Column("papAtvo")]
        public bool PapAtvo { get; set; }
        [Column("pepStat")]
        public bool PepStat { get; set; }

        [ForeignKey(nameof(PemCodi))]
        [InverseProperty(nameof(Permissao.PermissaoPerfils))]
        public virtual Permissao PemCodiNavigation { get; set; } = null!;
        [ForeignKey(nameof(PerCodi))]
        [InverseProperty(nameof(Perfil.PermissaoPerfils))]
        public virtual Perfil PerCodiNavigation { get; set; } = null!;
    }
}
