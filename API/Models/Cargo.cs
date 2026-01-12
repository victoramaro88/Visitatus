using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace API_Visitatus.Models
{
    [Table("Cargos", Schema = "dbo")]
    public partial class Cargo
    {
        public Cargo()
        {
            CargosRitos = new HashSet<CargosRito>();
            GestaoCargos = new HashSet<GestaoCargo>();
        }

        [Key]
        [Column("carCodi")]
        public long CarCodi { get; set; }
        [Column("carNome")]
        [StringLength(100)]
        [Unicode(false)]
        public string CarNome { get; set; } = null!;
        [Column("carDesc")]
        [StringLength(300)]
        [Unicode(false)]
        public string? CarDesc { get; set; }
        [Column("carStat")]
        public bool CarStat { get; set; }

        [InverseProperty(nameof(CargosRito.CarCodiNavigation))]
        public virtual ICollection<CargosRito> CargosRitos { get; set; }
        [InverseProperty(nameof(GestaoCargo.CarCodiNavigation))]
        public virtual ICollection<GestaoCargo> GestaoCargos { get; set; }
    }
}
