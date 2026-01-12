using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace API_Visitatus.Models
{
    [Table("GestaoAdministrativa", Schema = "dbo")]
    public partial class GestaoAdministrativa
    {
        public GestaoAdministrativa()
        {
            GestaoCargos = new HashSet<GestaoCargo>();
        }

        [Key]
        [Column("gstAdmCodi")]
        public long GstAdmCodi { get; set; }
        [Column("gstAdmNome")]
        [StringLength(100)]
        [Unicode(false)]
        public string GstAdmNome { get; set; } = null!;
        [Column("gstAdmDtIn", TypeName = "date")]
        public DateTime GstAdmDtIn { get; set; }
        [Column("gstAdmDtFi", TypeName = "date")]
        public DateTime GstAdmDtFi { get; set; }
        [Column("gstAdmStat")]
        public bool GstAdmStat { get; set; }
        [Column("lojCodi")]
        public long LojCodi { get; set; }

        [ForeignKey(nameof(LojCodi))]
        [InverseProperty(nameof(Loja.GestaoAdministrativas))]
        public virtual Loja LojCodiNavigation { get; set; } = null!;
        [InverseProperty(nameof(GestaoCargo.GstAdmCodiNavigation))]
        public virtual ICollection<GestaoCargo> GestaoCargos { get; set; }
    }
}
