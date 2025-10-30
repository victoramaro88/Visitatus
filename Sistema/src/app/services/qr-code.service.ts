import { Injectable } from '@angular/core';
import QRCode from 'qrcode';

@Injectable({
  providedIn: 'root'
})
export class QRCodeService {
  generateQRCode(data: string): Promise<string> {
    return QRCode.toDataURL(data);
  }
}
