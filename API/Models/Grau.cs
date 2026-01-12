using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace API_Visitatus.Models
{
    [Table("Grau", Schema = "dbo")]
    public partial class Grau
    {
        public Grau()
        {
            Sessaos = new HashSet<Sessao>();
        }

        [Key]
        [Column("graCodi")]
        public int GraCodi { get; set; }
        [Column("graNome")]
        [StringLength(250)]
        [Unicode(false)]
        public string GraNome { get; set; } = null!;
        [Column("graStat")]
        public bool GraStat { get; set; }

        [InverseProperty(nameof(Sessao.GraCodiNavigation))]
        public virtual ICollection<Sessao> Sessaos { get; set; }
    }
}
