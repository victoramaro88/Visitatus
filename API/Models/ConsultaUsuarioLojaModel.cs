namespace API_Visitatus.Models
{
    public class ConsultaUsuarioLojaModel
    {
        public long sesCodi { get; set; }
        public UsrLoja? objUsuarioLoja { get; set; }
        public LojaConsulta? objLojaConsulta { get; set; }

        public class UsrLoja
        {
            public long UsuCodi { get; set; }
            public string? UsuNome { get; set; }
            public string? UsuNCIM { get; set; }
            public DateTime UsuNasc { get; set; }
            public string? UsuEmai { get; set; }
            public string? UsuNCel { get; set; }
            public bool UsuStat { get; set; }
            public string? LojNome { get; set; }
            public string? LojNumL { get; set; }
            public int PotCodi { get; set; }
        }

        public class LojaConsulta
        {
            public long LojCodi { get; set; }
            public string? LojNome { get; set; }
            public string? LojNumL { get; set; }
            public string? LojLogo { get; set; }
            public string? LojLogr { get; set; }
            public string? LojNume { get; set; }
            public string? LojBair { get; set; }
            public bool LojStat { get; set; }
            public long? CidCodi { get; set; }
            public int PotCodi { get; set; }
            public int? RitCodi { get; set; }
        }
    }
}
