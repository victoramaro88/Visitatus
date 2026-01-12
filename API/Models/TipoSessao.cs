using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace API_Visitatus.Models
{
    [Table("TipoSessao", Schema = "dbo")]
    public partial class TipoSessao
    {
        public TipoSessao()
        {
            Sessaos = new HashSet<Sessao>();
        }

        [Key]
        [Column("tiSCodi")]
        public int TiScodi { get; set; }
        [Column("tiSNome")]
        [StringLength(500)]
        [Unicode(false)]
        public string TiSnome { get; set; } = null!;
        [Column("tiSStat")]
        public bool TiSstat { get; set; }

        [InverseProperty(nameof(Sessao.TiScodiNavigation))]
        public virtual ICollection<Sessao> Sessaos { get; set; }
    }
}
