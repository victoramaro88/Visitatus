using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace API_Visitatus.Models
{
    [Table("TemplateCertificadoPresenca", Schema = "dbo")]
    public partial class TemplateCertificadoPresenca
    {
        public TemplateCertificadoPresenca()
        {
            TemplateCertificadoLojas = new HashSet<TemplateCertificadoLoja>();
        }

        [Key]
        [Column("tmpCrtPreCodi")]
        public int TmpCrtPreCodi { get; set; }
        [Column("tmpCrtPreNome")]
        [StringLength(250)]
        [Unicode(false)]
        public string TmpCrtPreNome { get; set; } = null!;
        [Column("tmpCrtPreMode")]
        [Unicode(false)]
        public string TmpCrtPreMode { get; set; } = null!;
        [Column("tmpCrtPreStat")]
        public bool TmpCrtPreStat { get; set; }

        [InverseProperty(nameof(TemplateCertificadoLoja.TmpCrtPreCodiNavigation))]
        public virtual ICollection<TemplateCertificadoLoja> TemplateCertificadoLojas { get; set; }
    }
}
