export const environment = {
  production: false,

  // version: '1.0.0 | 19/02/2025-08:06 | Localhost',
  version: '1.2.0 | 26/11/2025-13:11 | Localhost', //-> QR-Code e Mensagem Whatsapp

  apiServicos: 'https://localhost:7237/api',
  // apiServicos: 'https://dev.visitatus.com.br/_API/api',
  //apiServicos: 'https://visitatus.com.br/_API/api',
};

//-> PÁGINAS QUE NECESSITAM ATENÇÃO NA URL DAS IMAGENS:
/*
Páginas:
  - Convites(banco de dados);
  - Certificados(banco de dados);
*/

/* DEPENDÊNCIAS INSTALADAS */
// https://www.npmjs.com/package/angularx-qrcode#demo-app

// ng build --base-href="./" --configuration=dev
//