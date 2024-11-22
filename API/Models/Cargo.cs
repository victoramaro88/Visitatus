using System;
using System.Collections.Generic;

namespace API_Visitatus.Models
{
    public partial class Cargo
    {
        public Cargo()
        {
            CargosRitos = new HashSet<CargosRito>();
            GestaoCargos = new HashSet<GestaoCargo>();
        }

        public long CarCodi { get; set; }
        public string CarNome { get; set; } = null!;
        public string? CarDesc { get; set; }
        public bool CarStat { get; set; }

        public virtual ICollection<CargosRito> CargosRitos { get; set; }
        public virtual ICollection<GestaoCargo> GestaoCargos { get; set; }
    }
}
