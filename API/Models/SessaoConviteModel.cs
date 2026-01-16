namespace API_Visitatus.Models
{
    public class SessaoConviteModel
    {
        public long SesCodi { get; set; }
        public long SesNume { get; set; }
        public string? SesNome { get; set; }
        public string? SesDesc { get; set; }
        public DateTime SesDtHr { get; set; }
        public bool SesLibe { get; set; }
        public bool SesStat { get; set; }
        public string? TiSNome { get; set; }
        public string? GraNome { get; set; }
        public long LojCodi { get; set; }
        public string? LojNome { get; set; }
        public bool LojStat { get; set; }
        public string? LojNume { get; set; }
        public string? LojLogr { get; set; }
        public string? LojNumL { get; set; }
        public string? LojBair { get; set; }
        public string? PotNome { get; set; }
        public string? PotSigl { get; set; }
        public bool PotRegu { get; set; }
        public string? RitNome { get; set; }
        public string? CidNome { get; set; }
        public string? EstSigl { get; set; }
        public List<GestaoAdmAtivaModel>? lstGestaoAdmAtiva{ get; set; }
        public string? LojLogo { get; set; }
        public string? PotLogo { get; set; }
        public bool SesAgap { get; set; }
        public decimal? SesVlAg { get; set; }
    }
}
