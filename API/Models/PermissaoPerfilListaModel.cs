namespace API_Visitatus.Models
{
    public class PermissaoPerfilListaModel
    {
        public int perCodi { get; set; }
        public string? perNome { get; set; }
        public bool perStat { get; set; }
        public int pemCodi { get; set; }
        public string? pemNome { get; set; }
        public bool pemStat { get; set; }
        public bool papAtvo { get; set; }
        public bool pepStat { get; set; }
    }
}
