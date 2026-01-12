using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace API_Visitatus.Models
{
    [Table("Rito", Schema = "dbo")]
    public partial class Rito
    {
        public Rito()
        {
            CargosRitos = new HashSet<CargosRito>();
            Lojas = new HashSet<Loja>();
        }

        [Key]
        [Column("ritCodi")]
        public int RitCodi { get; set; }
        [Column("ritNome")]
        [StringLength(250)]
        [Unicode(false)]
        public string RitNome { get; set; } = null!;
        [Column("ritLogo")]
        [Unicode(false)]
        public string? RitLogo { get; set; }
        [Column("ritStat")]
        public bool RitStat { get; set; }

        [InverseProperty(nameof(CargosRito.RitCodiNavigation))]
        public virtual ICollection<CargosRito> CargosRitos { get; set; }
        [InverseProperty(nameof(Loja.RitCodiNavigation))]
        public virtual ICollection<Loja> Lojas { get; set; }
    }
}
