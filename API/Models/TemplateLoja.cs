using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace API_Visitatus.Models
{
    [Table("TemplateLoja", Schema = "dbo")]
    public partial class TemplateLoja
    {
        [Key]
        [Column("tmpCvtCodi")]
        public int TmpCvtCodi { get; set; }
        [Key]
        [Column("lojCodi")]
        public long LojCodi { get; set; }
        [Column("tmpLjStat")]
        public bool TmpLjStat { get; set; }

        [ForeignKey(nameof(LojCodi))]
        [InverseProperty(nameof(Loja.TemplateLojas))]
        public virtual Loja LojCodiNavigation { get; set; } = null!;
        [ForeignKey(nameof(TmpCvtCodi))]
        [InverseProperty(nameof(TemplateConvite.TemplateLojas))]
        public virtual TemplateConvite TmpCvtCodiNavigation { get; set; } = null!;
    }
}
