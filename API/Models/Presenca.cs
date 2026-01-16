using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace API_Visitatus.Models
{
    [Table("Presenca", Schema = "dbo")]
    public partial class Presenca
    {
        [Key]
        [Column("usuCodi")]
        public long UsuCodi { get; set; }
        [Key]
        [Column("sesCodi")]
        public long SesCodi { get; set; }
        [Column("preAtiv")]
        public bool PreAtiv { get; set; }
        [Key]
        [Column("lojCodi")]
        public long LojCodi { get; set; }
        [Column("preEmai")]
        public bool PreEmai { get; set; }
        [Column("preAgap")]
        public bool PreAgap { get; set; }

        [ForeignKey(nameof(LojCodi))]
        [InverseProperty(nameof(Loja.Presencas))]
        public virtual Loja LojCodiNavigation { get; set; } = null!;
        [ForeignKey(nameof(SesCodi))]
        [InverseProperty(nameof(Sessao.Presencas))]
        public virtual Sessao SesCodiNavigation { get; set; } = null!;
        [ForeignKey(nameof(UsuCodi))]
        [InverseProperty(nameof(Usuario.Presencas))]
        public virtual Usuario UsuCodiNavigation { get; set; } = null!;
    }
}
