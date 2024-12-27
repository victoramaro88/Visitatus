namespace API_Visitatus.Models
{
    public class ListaPresencaModel
    {
        public long SesCodi { get; set; }
        public long UsuCodi { get; set; }
        public string? UsuNome { get; set; }
        public string? UsuNCIM { get; set; }
        public long LojCodi { get; set; }
        public string? LojNome { get; set; }
        public string? LojNumL { get; set; }
        public bool PreAtiv { get; set; }
        public bool PreEmai { get; set; }
    }
}
