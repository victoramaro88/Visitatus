import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class Utils {

  constructor() {}

  ValidarCpf(cpf: string): boolean {
    if (!cpf) {
      return false;
    }

    // Remove caracteres não numéricos
    const cleanedCpf = cpf.replace(/\D/g, '');

    // Verifica se tem 11 dígitos
    if (cleanedCpf.length !== 11) {
      return false;
    }

    // Verifica se todos os dígitos são iguais
    if (/^(\d)\1+$/.test(cleanedCpf)) {
      return false;
    }

    // Validação dos dígitos verificadores
    let sum;
    let remainder;

    // Validação do primeiro dígito verificador
    sum = 0;
    for (let i = 1; i <= 9; i++) {
      sum += parseInt(cleanedCpf.substring(i - 1, i)) * (11 - i);
    }
    remainder = (sum * 10) % 11;
    if (remainder === 10 || remainder === 11) {
      remainder = 0;
    }
    if (remainder !== parseInt(cleanedCpf.substring(9, 10))) {
      return false;
    }

    // Validação do segundo dígito verificador
    sum = 0;
    for (let i = 1; i <= 10; i++) {
      sum += parseInt(cleanedCpf.substring(i - 1, i)) * (12 - i);
    }
    remainder = (sum * 10) % 11;
    if (remainder === 10 || remainder === 11) {
      remainder = 0;
    }
    if (remainder !== parseInt(cleanedCpf.substring(10, 11))) {
      return false;
    }

    return true; // CPF é válido
  }

  ValidarEmail(email: string): boolean {
    if (!email) {
      return false;
    }

    // Expressão regular para validar o formato de e-mail
    const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;

    // Testa o e-mail contra a expressão regular
    return emailRegex.test(email);
  }

  formataTelefone(tel: string) {
    let telefone: string = '';

    let input = tel.replace(/\D/g, '');

    if (input.length <= 10) {
      // Máscara para telefone fixo (10 dígitos)
      telefone = input.replace(/(\d{2})(\d{4})(\d{4})/, '($1) $2-$3');
    } else {
      // Máscara para telefone celular (11 dígitos)
      telefone = input.replace(/(\d{2})(\d{1})(\d{4})(\d{4})/, '($1) $2 $3-$4');
    }

    return telefone;
  }
}
