namespace API_Visitatus.Models
{
    public class UsuarioPotenciaModel
    {
        public long UsuCodi { get; set; }
        public string? UsuNome { get; set; }
        public DateTime UsuNasc { get; set; }
        public string? UsuEmai { get; set; }
        public string? UsuNCel { get; set; }
        public bool UsuStat { get; set; }
        public string? UsuNCIM { get; set; }
        public string? UsLUser { get; set; }
        public string? UsLPass { get; set; }
        public bool UsLStat { get; set; }

        public List<LojaUsuarioPotenciaModel>? lstLjUsrPot { get; set; }

    }

    public class LojaUsuarioPotenciaModel
    {
        public long LojCodi { get; set; }
        public int PerCodi { get; set; }
        public string? LojNome { get; set; }
        public string? LojNumL { get; set; }
        public int PotCodi { get; set; }
        public string? PerNome { get; set; }
        public bool PerStat { get; set; }
    }
}
