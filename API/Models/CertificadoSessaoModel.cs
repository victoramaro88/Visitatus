namespace API_Visitatus.Models
{
    public class CertificadoSessaoModel
    {
        public CertificadoDadosLojaModel? objCertificadoDadosLojaModel { get; set; }
        public List<CertificadoDadosPresencaModel>? objCertificadoDadosPresencaModel { get; set; }
        public List<GestaoAdmAtivaModel>? lstCargosGestaoLoja { get; set; }
    }
}
