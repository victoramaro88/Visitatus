namespace API_Visitatus.Models
{
    public class PerfilUsuarioListaModel
    {
        public int peUCodi { get; set; }
        public bool peUStat { get; set; }
        public int perCodi { get; set; }
        public long usuCodi { get; set; }
        public long lojCodi { get; set; }
        public string? perNome { get; set; }
        public bool perStat { get; set; }
        public string? lojNome { get; set; }
        public string? lojNumL { get; set; }
        public bool lojStat { get; set; }
        public int potCodi { get; set; }
    }
}
