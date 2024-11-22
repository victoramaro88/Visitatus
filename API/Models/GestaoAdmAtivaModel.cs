namespace API_Visitatus.Models
{
    public class GestaoAdmAtivaModel
    {
        public long LojCodi { get; set; }        
        public string? LojNome { get; set; }
        public string? LojNumL { get; set;}
        public string? GstAdmNome { get; set;}
        public DateTime GstAdmDtIn { get; set;}
        public DateTime GstAdmDtFi { get; set;}
        public bool GstAdmStat { get; set;}
        public long CarCodi { get; set; }
        public string? CarNome { get; set;}
        public string? UsuNome { get; set; }
    }
}
