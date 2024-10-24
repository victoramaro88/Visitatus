import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class Base64Service {

  constructor() {}

  convertNumberToBase64(num: number): string {
    const numString = num.toString();
    return btoa(numString);
  }

  decodeBase64ToNumber(base64: string): number {
    const decodedString = atob(base64);
    return parseFloat(decodedString);
  }

  convertStringToBase64(str: string): string {
    const strRet = str.toString();
    return btoa(strRet);
  }

  decodeBase64ToString(base64: string): string {
    const decodedString = atob(base64);
    return decodedString;
  }
}
