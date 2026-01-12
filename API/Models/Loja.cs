using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace API_Visitatus.Models
{
    [Table("Loja", Schema = "dbo")]
    public partial class Loja
    {
        public Loja()
        {
            GestaoAdministrativas = new HashSet<GestaoAdministrativa>();
            PerfilUsuarios = new HashSet<PerfilUsuario>();
            Presencas = new HashSet<Presenca>();
            Sessaos = new HashSet<Sessao>();
            TemplateCertificadoLojas = new HashSet<TemplateCertificadoLoja>();
            TemplateLojas = new HashSet<TemplateLoja>();
        }

        [Key]
        [Column("lojCodi")]
        public long LojCodi { get; set; }
        [Column("lojNome")]
        [StringLength(1000)]
        [Unicode(false)]
        public string LojNome { get; set; } = null!;
        [Column("lojNumL")]
        [StringLength(10)]
        [Unicode(false)]
        public string LojNumL { get; set; } = null!;
        [Column("lojLogo")]
        [Unicode(false)]
        public string? LojLogo { get; set; }
        [Column("lojLogr")]
        [StringLength(500)]
        [Unicode(false)]
        public string? LojLogr { get; set; }
        [Column("lojNume")]
        [StringLength(10)]
        [Unicode(false)]
        public string? LojNume { get; set; }
        [Column("lojBair")]
        [StringLength(500)]
        [Unicode(false)]
        public string? LojBair { get; set; }
        [Column("lojStat")]
        public bool LojStat { get; set; }
        [Column("cidCodi")]
        public long? CidCodi { get; set; }
        [Column("potCodi")]
        public int PotCodi { get; set; }
        [Column("ritCodi")]
        public int? RitCodi { get; set; }

        [ForeignKey(nameof(CidCodi))]
        [InverseProperty(nameof(Cidade.Lojas))]
        public virtual Cidade? CidCodiNavigation { get; set; }
        [ForeignKey(nameof(PotCodi))]
        [InverseProperty(nameof(Potencium.Lojas))]
        public virtual Potencium PotCodiNavigation { get; set; } = null!;
        [ForeignKey(nameof(RitCodi))]
        [InverseProperty(nameof(Rito.Lojas))]
        public virtual Rito? RitCodiNavigation { get; set; }
        [InverseProperty(nameof(GestaoAdministrativa.LojCodiNavigation))]
        public virtual ICollection<GestaoAdministrativa> GestaoAdministrativas { get; set; }
        [InverseProperty(nameof(PerfilUsuario.LojCodiNavigation))]
        public virtual ICollection<PerfilUsuario> PerfilUsuarios { get; set; }
        [InverseProperty(nameof(Presenca.LojCodiNavigation))]
        public virtual ICollection<Presenca> Presencas { get; set; }
        [InverseProperty(nameof(Sessao.LojCodiNavigation))]
        public virtual ICollection<Sessao> Sessaos { get; set; }
        [InverseProperty(nameof(TemplateCertificadoLoja.LojCodiNavigation))]
        public virtual ICollection<TemplateCertificadoLoja> TemplateCertificadoLojas { get; set; }
        [InverseProperty(nameof(TemplateLoja.LojCodiNavigation))]
        public virtual ICollection<TemplateLoja> TemplateLojas { get; set; }
    }
}
