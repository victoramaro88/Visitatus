using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace API_Visitatus.Models
{
    [Table("GestaoCargos", Schema = "dbo")]
    public partial class GestaoCargo
    {
        [Key]
        [Column("gstAdmCodi")]
        public long GstAdmCodi { get; set; }
        [Key]
        [Column("carCodi")]
        public long CarCodi { get; set; }
        [Key]
        [Column("usuCodi")]
        public long UsuCodi { get; set; }
        [Column("gstCarStat")]
        public bool GstCarStat { get; set; }

        [ForeignKey(nameof(CarCodi))]
        [InverseProperty(nameof(Cargo.GestaoCargos))]
        public virtual Cargo CarCodiNavigation { get; set; } = null!;
        [ForeignKey(nameof(GstAdmCodi))]
        [InverseProperty(nameof(GestaoAdministrativa.GestaoCargos))]
        public virtual GestaoAdministrativa GstAdmCodiNavigation { get; set; } = null!;
        [ForeignKey(nameof(UsuCodi))]
        [InverseProperty(nameof(Usuario.GestaoCargos))]
        public virtual Usuario UsuCodiNavigation { get; set; } = null!;
    }
}
