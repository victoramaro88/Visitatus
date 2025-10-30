import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'cpfMask',
  standalone: true
})
export class CpfMaskPipe implements PipeTransform {
  transform(value: string): string {
    if (!value) {
      return value;
    }

    // Remove todos os caracteres que não sejam dígitos
    const cleanValue = value.replace(/\D/g, '');

    // Formata a string como CPF
    if (cleanValue.length === 11) {
      return cleanValue.replace(/(\d{3})(\d{3})(\d{3})(\d{2})/, '$1.$2.$3-$4');
    }

    return value; // Retorna o valor original se não for um CPF válido
  }
}
