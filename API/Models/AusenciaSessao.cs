using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace API_Visitatus.Models
{
    [Table("AusenciaSessao", Schema = "dbo")]
    public partial class AusenciaSessao
    {
        [Key]
        [Column("ausSesCodi")]
        public long AusSesCodi { get; set; }
        [Column("ausSecMoti")]
        [StringLength(250)]
        [Unicode(false)]
        public string AusSecMoti { get; set; } = null!;
        [Column("ausSecDtHr", TypeName = "datetime")]
        public DateTime AusSecDtHr { get; set; }
        [Column("ausSecStat")]
        public bool AusSecStat { get; set; }
        [Column("usuCodi")]
        public long UsuCodi { get; set; }
        [Column("sesCodi")]
        public long SesCodi { get; set; }

        [ForeignKey(nameof(SesCodi))]
        [InverseProperty(nameof(Sessao.AusenciaSessaos))]
        public virtual Sessao SesCodiNavigation { get; set; } = null!;
        [ForeignKey(nameof(UsuCodi))]
        [InverseProperty(nameof(Usuario.AusenciaSessaos))]
        public virtual Usuario UsuCodiNavigation { get; set; } = null!;
    }
}
