using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Visitatus.Data.Models
{
    public class PotenciaModel
    {
        public int potCodi { get; set; }
        public string? potNome { get; set; }
        public string? potLogo { get; set; }
        public bool potRegu { get; set; }
        public bool potStat { get; set; }
        public string? potSigl { get; set; }
    }
}
