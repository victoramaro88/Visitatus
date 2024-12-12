import { Component } from '@angular/core';
import html2canvas from 'html2canvas';

@Component({
  selector: 'app-template-certificado',
  standalone: true,
  imports: [],
  templateUrl: './template-certificado.component.html',
  styleUrl: './template-certificado.component.css'
})
export class TemplateCertificadoComponent {
  
  exportAsImage(): void {
    const element = document.getElementById('capture-area'); // Seleciona o elemento a capturar
    if (element) {
      html2canvas(element).then(canvas => {
        // Converte o canvas para uma URL de imagem
        const image = canvas.toDataURL('image/png');
        // Cria um link para download
        const link = document.createElement('a');
        link.href = image;
        link.download = 'certificado.png';
        link.click(); // Simula o clique para baixar a imagem
      });
    }
  }
}
