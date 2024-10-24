import { Injectable } from '@angular/core';
import CryptoJS from 'crypto-js';

@Injectable({
  providedIn: 'root'
})
export class CryptoService {
  private chaveSecreta: string = 'jsdfjkksfj4lk213oilksaKJHSdfwes2374alslkakAKdasdjsdlfj234823=as-321e32lllkcjkjl@alffjhsfsd,csdlkjf';  // Defina sua chave secreta aqui

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
}
