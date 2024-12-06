using System;
using System.Collections.Generic;

namespace API_Visitatus.Models
{
    public partial class Loja
    {
        public Loja()
        {
            GestaoAdministrativas = new HashSet<GestaoAdministrativa>();
            PerfilUsuarios = new HashSet<PerfilUsuario>();
            Presencas = new HashSet<Presenca>();
            Sessaos = new HashSet<Sessao>();
            TemplateLojas = new HashSet<TemplateLoja>();
        }

        public long LojCodi { get; set; }
        public string LojNome { get; set; } = null!;
        public string LojNumL { get; set; } = null!;
        public string? LojLogo { get; set; }
        public string? LojLogr { get; set; }
        public string? LojNume { get; set; }
        public string? LojBair { get; set; }
        public bool LojStat { get; set; }
        public long? CidCodi { get; set; }
        public int PotCodi { get; set; }
        public int? RitCodi { get; set; }

        public virtual Cidade? CidCodiNavigation { get; set; }
        public virtual Potencium PotCodiNavigation { get; set; } = null!;
        public virtual Rito? RitCodiNavigation { get; set; }
        public virtual ICollection<GestaoAdministrativa> GestaoAdministrativas { get; set; }
        public virtual ICollection<PerfilUsuario> PerfilUsuarios { get; set; }
        public virtual ICollection<Presenca> Presencas { get; set; }
        public virtual ICollection<Sessao> Sessaos { get; set; }
        public virtual ICollection<TemplateLoja> TemplateLojas { get; set; }
    }
}
