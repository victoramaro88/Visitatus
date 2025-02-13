namespace API_Visitatus.Models
{
    public class ProximaSessaoModel
    {
        public long SesCodi { get; set; }
        public long SesNume { get; set; }
        public DateTime SesDtHr { get; set; }
        public bool SesLibe { get; set; }
        public bool SesStat { get; set; }
        public string? SesNome { get; set; }
        public int TotalPresenca { get; set; }
    }
}
