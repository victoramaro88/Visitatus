using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace API_Visitatus.Models
{
    [Table("Sessao", Schema = "dbo")]
    public partial class Sessao
    {
        public Sessao()
        {
            Presencas = new HashSet<Presenca>();
        }

        [Key]
        [Column("sesCodi")]
        public long SesCodi { get; set; }
        [Column("sesDesc")]
        [StringLength(500)]
        [Unicode(false)]
        public string SesDesc { get; set; } = null!;
        [Column("sesDtHr", TypeName = "datetime")]
        public DateTime SesDtHr { get; set; }
        [Column("sesLibe")]
        public bool SesLibe { get; set; }
        [Column("sesStat")]
        public bool SesStat { get; set; }
        [Column("lojCodi")]
        public long LojCodi { get; set; }
        [Column("graCodi")]
        public int GraCodi { get; set; }
        [Column("tiSCodi")]
        public int TiScodi { get; set; }
        [Column("sesNume")]
        public long? SesNume { get; set; }
        [Column("sesNome")]
        [StringLength(100)]
        [Unicode(false)]
        public string SesNome { get; set; } = null!;
        [Column("sesAgap")]
        public bool SesAgap { get; set; }
        [Column("sesVlAg", TypeName = "decimal(18, 2)")]
        public decimal? SesVlAg { get; set; }

        [ForeignKey(nameof(GraCodi))]
        [InverseProperty(nameof(Grau.Sessaos))]
        public virtual Grau GraCodiNavigation { get; set; } = null!;
        [ForeignKey(nameof(LojCodi))]
        [InverseProperty(nameof(Loja.Sessaos))]
        public virtual Loja LojCodiNavigation { get; set; } = null!;
        [ForeignKey(nameof(TiScodi))]
        [InverseProperty(nameof(TipoSessao.Sessaos))]
        public virtual TipoSessao TiScodiNavigation { get; set; } = null!;
        [InverseProperty(nameof(Presenca.SesCodiNavigation))]
        public virtual ICollection<Presenca> Presencas { get; set; }
    }
}
