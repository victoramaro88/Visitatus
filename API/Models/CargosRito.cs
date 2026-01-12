using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace API_Visitatus.Models
{
    [Table("CargosRito", Schema = "dbo")]
    public partial class CargosRito
    {
        [Key]
        [Column("carCodi")]
        public long CarCodi { get; set; }
        [Key]
        [Column("ritCodi")]
        public int RitCodi { get; set; }
        [Column("cariOrdm")]
        public int CariOrdm { get; set; }
        [Column("cariStat")]
        public bool CariStat { get; set; }

        [ForeignKey(nameof(CarCodi))]
        [InverseProperty(nameof(Cargo.CargosRitos))]
        public virtual Cargo CarCodiNavigation { get; set; } = null!;
        [ForeignKey(nameof(RitCodi))]
        [InverseProperty(nameof(Rito.CargosRitos))]
        public virtual Rito RitCodiNavigation { get; set; } = null!;
    }
}
