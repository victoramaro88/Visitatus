import { CertificadoDadosLojaModel } from './CertificadoDadosLoja.Model';
import { CertificadoDadosPresencaModel } from './CertificadoDadosPresenca.Model';
import { GestaoAdmAtivaModel } from './GestaoAdmAtiva.Model';

export class CertificadoSessaoModel {
  objCertificadoDadosLojaModel: CertificadoDadosLojaModel | null;
  objCertificadoDadosPresencaModel: CertificadoDadosPresencaModel[] | null;
  lstCargosGestaoLoja: GestaoAdmAtivaModel[] | null;

  constructor() {
    this.objCertificadoDadosLojaModel = null;
    this.objCertificadoDadosPresencaModel = null;
    this.lstCargosGestaoLoja = null;
  }
}
