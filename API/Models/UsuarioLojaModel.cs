namespace API_Visitatus.Models
{
    public class UsuarioLojaModel
    {
        public long UsuCodi { get; set; }
        public string? UsuNome { get; set; }
        public string? UsuNCIM { get; set; }
        public string? UsuNCel { get; set; }
        public string? UsuEmai { get; set; }
        public DateTime UsuNasc { get; set; }
        public bool UsuStat { get; set; }
        public int PerCodi { get; set; }
        public string? PerNome { get; set; }
        public bool PeUStat { get; set; }
    }
}
