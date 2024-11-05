namespace API_Visitatus.Models
{
    public class SessaoListaModel
    {
        public long SesCodi { get; set; }
        public string? SesDesc { get; set; }
        public DateTime SesDtHr { get; set; }
        public bool SesLibe { get; set; }
        public bool SesStat { get; set; }
        public long LojCodi { get; set; }
        public int GraCodi { get; set; }
        public string? GraNome { get; set; }
        public int TiSCodi { get; set; }
        public string? TiSNome { get; set; }
        public long SesNume { get; set; }
        public string? SesNome { get; set; }
    }
}
