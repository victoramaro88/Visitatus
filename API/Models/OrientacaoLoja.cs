using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace API_Visitatus.Models
{
    [Table("OrientacaoLoja", Schema = "dbo")]
    public partial class OrientacaoLoja
    {
        [Key]
        [Column("orlCodi")]
        public long OrlCodi { get; set; }
        [Column("orlDesc")]
        [StringLength(2000)]
        [Unicode(false)]
        public string? OrlDesc { get; set; }
        [Column("orlDtHr", TypeName = "datetime")]
        public DateTime OrlDtHr { get; set; }
        [Column("orlStat")]
        public bool OrlStat { get; set; }
        [Column("lojCodi")]
        public long LojCodi { get; set; }
        [Column("usuCodi")]
        public long UsuCodi { get; set; }

        [ForeignKey(nameof(LojCodi))]
        [InverseProperty(nameof(Loja.OrientacaoLojas))]
        public virtual Loja LojCodiNavigation { get; set; } = null!;
        [ForeignKey(nameof(UsuCodi))]
        [InverseProperty(nameof(Usuario.OrientacaoLojas))]
        public virtual Usuario UsuCodiNavigation { get; set; } = null!;
    }
}
