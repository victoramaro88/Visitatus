using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace API_Visitatus.Models
{
    [Table("TemplateConvite", Schema = "dbo")]
    public partial class TemplateConvite
    {
        public TemplateConvite()
        {
            TemplateLojas = new HashSet<TemplateLoja>();
        }

        [Key]
        [Column("tmpCvtCodi")]
        public int TmpCvtCodi { get; set; }
        [Column("tmpCvtNome")]
        [StringLength(250)]
        [Unicode(false)]
        public string TmpCvtNome { get; set; } = null!;
        [Column("tmpCvtMode")]
        [Unicode(false)]
        public string TmpCvtMode { get; set; } = null!;
        [Column("tmpCvtStat")]
        public bool TmpCvtStat { get; set; }

        [InverseProperty(nameof(TemplateLoja.TmpCvtCodiNavigation))]
        public virtual ICollection<TemplateLoja> TemplateLojas { get; set; }
    }
}
