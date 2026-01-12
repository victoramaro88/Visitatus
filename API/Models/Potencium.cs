using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace API_Visitatus.Models
{
    [Table("Potencia", Schema = "dbo")]
    public partial class Potencium
    {
        public Potencium()
        {
            Lojas = new HashSet<Loja>();
        }

        [Key]
        [Column("potCodi")]
        public int PotCodi { get; set; }
        [Column("potNome")]
        [StringLength(250)]
        [Unicode(false)]
        public string PotNome { get; set; } = null!;
        [Column("potLogo")]
        [Unicode(false)]
        public string PotLogo { get; set; } = null!;
        [Column("potRegu")]
        public bool PotRegu { get; set; }
        [Column("potStat")]
        public bool PotStat { get; set; }
        [Column("potSigl")]
        [StringLength(10)]
        [Unicode(false)]
        public string? PotSigl { get; set; }

        [InverseProperty(nameof(Loja.PotCodiNavigation))]
        public virtual ICollection<Loja> Lojas { get; set; }
    }
}
