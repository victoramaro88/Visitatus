using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace API_Visitatus.Models
{
    [Table("TemplateCertificadoLoja", Schema = "dbo")]
    public partial class TemplateCertificadoLoja
    {
        [Key]
        [Column("tmpCrtPreCodi")]
        public int TmpCrtPreCodi { get; set; }
        [Key]
        [Column("lojCodi")]
        public long LojCodi { get; set; }
        [Column("tmpCrtStat")]
        public bool TmpCrtStat { get; set; }

        [ForeignKey(nameof(LojCodi))]
        [InverseProperty(nameof(Loja.TemplateCertificadoLojas))]
        public virtual Loja LojCodiNavigation { get; set; } = null!;
        [ForeignKey(nameof(TmpCrtPreCodi))]
        [InverseProperty(nameof(TemplateCertificadoPresenca.TemplateCertificadoLojas))]
        public virtual TemplateCertificadoPresenca TmpCrtPreCodiNavigation { get; set; } = null!;
    }
}
