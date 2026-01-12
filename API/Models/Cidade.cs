using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace API_Visitatus.Models
{
    [Table("Cidade", Schema = "dbo")]
    public partial class Cidade
    {
        public Cidade()
        {
            Lojas = new HashSet<Loja>();
        }

        [Key]
        [Column("cidCodi")]
        public long CidCodi { get; set; }
        [Column("cidNome")]
        [StringLength(500)]
        [Unicode(false)]
        public string CidNome { get; set; } = null!;
        [Column("cidStat")]
        public bool CidStat { get; set; }
        [Column("estCodi")]
        public int EstCodi { get; set; }

        [ForeignKey(nameof(EstCodi))]
        [InverseProperty(nameof(Estado.Cidades))]
        public virtual Estado EstCodiNavigation { get; set; } = null!;
        [InverseProperty(nameof(Loja.CidCodiNavigation))]
        public virtual ICollection<Loja> Lojas { get; set; }
    }
}
