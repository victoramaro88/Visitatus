import { Injectable } from '@angular/core';
// import CryptoJS from 'crypto-js';
import * as CryptoJS from 'crypto-js';

@Injectable({
  providedIn: 'root',
})
export class CryptoService {
  //-> Config de chava para criptografia interna do angular
  private chaveSecreta: string =
    'jsdfjkksfj4lk213oilksaKJHSdfwes2374alslkakAKdasdjsdlfj234823=as-321e32lllkcjkjl@alffjhsfsd,csdlkjf'; // Defina sua chave secreta aqui

  //-> Config de chava para criptografia interna da API com o Angular
  private key = CryptoJS.enc.Utf8.parse('s$23lksJ%nfIr09P'); // Chave AES
  private iv = CryptoJS.enc.Utf8.parse('d0*23hDt8$as7Rv1'); // IV

  constructor() {}

  // Método para criptografar um objeto
  criptografar(dado: any): string {
    const dadoString = JSON.stringify(dado);
    return CryptoJS.AES.encrypt(dadoString, this.chaveSecreta).toString();
  }

  // Método para decriptografar um objeto
  decriptografar(dadoCriptografado: string): any {
    const bytes = CryptoJS.AES.decrypt(dadoCriptografado, this.chaveSecreta);
    const dadoString = bytes.toString(CryptoJS.enc.Utf8);
    return JSON.parse(dadoString);
  }

  // Método para salvar dados criptografados no sessionStorage
  salvarNoSessionStorage(chave: string, dado: any): void {
    const dadoCriptografado = this.criptografar(dado);
    sessionStorage.setItem(chave, dadoCriptografado);
  }

  // Método para ler dados criptografados do sessionStorage
  lerDoSessionStorage(chave: string): any {
    const dadoCriptografado = sessionStorage.getItem(chave);
    if (dadoCriptografado) {
      return this.decriptografar(dadoCriptografado);
    }
    return null;
  }

  //#region Métodos de criptografia da API com o Angular
  encryptAPI(text: string): string {
    const encrypted = CryptoJS.AES.encrypt(text, this.key, {
      iv: this.iv,
      mode: CryptoJS.mode.CBC,
      padding: CryptoJS.pad.Pkcs7,
    });
    return encrypted.toString();
  }

  decryptAPI(cipherText: string): string {
    const decrypted = CryptoJS.AES.decrypt(cipherText, this.key, {
      iv: this.iv,
      mode: CryptoJS.mode.CBC,
      padding: CryptoJS.pad.Pkcs7,
    });
    return CryptoJS.enc.Utf8.stringify(decrypted);
  }
  //#endregion
}
