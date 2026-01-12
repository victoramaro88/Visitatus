using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace API_Visitatus.Models
{
    [Table("Permissao", Schema = "dbo")]
    public partial class Permissao
    {
        public Permissao()
        {
            PermissaoPerfils = new HashSet<PermissaoPerfil>();
        }

        [Key]
        [Column("pemCodi")]
        public int PemCodi { get; set; }
        [Column("pemNome")]
        [StringLength(100)]
        [Unicode(false)]
        public string PemNome { get; set; } = null!;
        [Column("pemStat")]
        public bool PemStat { get; set; }

        [InverseProperty(nameof(PermissaoPerfil.PemCodiNavigation))]
        public virtual ICollection<PermissaoPerfil> PermissaoPerfils { get; set; }
    }
}
