using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Visitatus.Data.Models
{
    public class PresencaModel
    {
        public long usuCodi { get; set; }
        public string? usuNome { get; set; }
        public string? usuNCIM { get; set; }
        public int perCodi { get; set; }
        public string? lojNome { get; set; }
        public string? lojNumL { get; set; }
        public string? potSigl { get; set; }
    }
}
