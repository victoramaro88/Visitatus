namespace API_Visitatus.Models
{
    public class CertificadoDadosLojaModel
    {
        public long LojCodi { get; set; }
        public string? LojNome { get; set; }
        public string? LojNumL { get; set; }
        public string? PotSigl { get; set; }
        public string? PotNome { get; set; }
        public DateTime SesDtHr { get; set; }
        public string? CidNome { get; set; }
        public string? EstSigl { get; set; }
        public string? LojLogo { get; set; }
        public string? PotLogo { get; set; }
    }
}
