using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace API_Visitatus.Models
{
    [Table("UsuarioLogin", Schema = "dbo")]
    public partial class UsuarioLogin
    {
        [Key]
        [Column("usLCodi")]
        public long UsLcodi { get; set; }
        [Column("usLUser")]
        [StringLength(50)]
        [Unicode(false)]
        public string UsLuser { get; set; } = null!;
        [Column("usLPass")]
        [StringLength(500)]
        [Unicode(false)]
        public string UsLpass { get; set; } = null!;
        [Column("usLStat")]
        public bool UsLstat { get; set; }
        [Column("usuCodi")]
        public long UsuCodi { get; set; }

        [ForeignKey(nameof(UsuCodi))]
        [InverseProperty(nameof(Usuario.UsuarioLogins))]
        public virtual Usuario UsuCodiNavigation { get; set; } = null!;
    }
}
