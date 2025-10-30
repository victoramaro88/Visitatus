import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'telefone',
  standalone: true
})
export class TelefonePipe implements PipeTransform {

  transform(value: string): string {
    if (!value) {
      return '';
    }

    // Remove todos os caracteres não numéricos
    const cleanedValue = value.replace(/\D/g, '');

    if (cleanedValue.length === 10) {
      // Máscara para telefone fixo: (11) 1111-1111
      return cleanedValue.replace(/(\d{2})(\d{4})(\d{4})/, '($1) $2-$3');
    } else if (cleanedValue.length === 11) {
      // Máscara para telefone celular: (11) 1 1111-1111
      return cleanedValue.replace(/(\d{2})(\d{1})(\d{4})(\d{4})/, '($1) $2 $3-$4');
    }

    // Se o número não tiver 10 ou 11 dígitos, retorna como está
    return value;
  }

}
