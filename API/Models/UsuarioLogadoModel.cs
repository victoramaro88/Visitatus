namespace API_Visitatus.Models
{
    public class UsuarioLogadoModel
    {
        public long usLCodi { get; set; }
        public long usuCodi { get; set; }
        public string? usuNome { get; set; }
        public long lojCodi { get; set; }
        public List<PerfilUsuarioListaModel>? lstPerfil { get; set; }
    }
}
