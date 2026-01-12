using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace API_Visitatus.Models
{
    [Table("Estado", Schema = "dbo")]
    public partial class Estado
    {
        public Estado()
        {
            Cidades = new HashSet<Cidade>();
        }

        [Key]
        [Column("estCodi")]
        public int EstCodi { get; set; }
        [Column("estNome")]
        [StringLength(250)]
        [Unicode(false)]
        public string EstNome { get; set; } = null!;
        [Column("estSigl")]
        [StringLength(2)]
        [Unicode(false)]
        public string EstSigl { get; set; } = null!;
        [Column("estStat")]
        public bool EstStat { get; set; }

        [InverseProperty(nameof(Cidade.EstCodiNavigation))]
        public virtual ICollection<Cidade> Cidades { get; set; }
    }
}
