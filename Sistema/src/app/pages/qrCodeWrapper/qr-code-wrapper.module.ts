// qr-code-wrapper.module.ts
import { NgModule } from '@angular/core';
import { QRCodeModule } from 'angularx-qrcode';

@NgModule({
  exports: [QRCodeModule]
})
export class QrCodeWrapperModule {}
