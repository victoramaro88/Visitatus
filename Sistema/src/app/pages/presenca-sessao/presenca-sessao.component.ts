import { Component, OnInit } from '@angular/core';
import { MessageService } from 'primeng/api';
import { ImportsModule } from '../../imports';
import { HttpService } from '../../services/http-service.service';
import { ActivatedRoute, Router } from '@angular/router';
import { ListaPresencaModel } from '../../models/ListaPresenca.Model';
import { Table } from 'primeng/table';

@Component({
  selector: 'app-presenca-sessao',
  standalone: true,
  imports: [ImportsModule],
  templateUrl: './presenca-sessao.component.html',
  styleUrl: './presenca-sessao.component.css',
  providers: [MessageService],
})
export class PresencaSessaoComponent implements OnInit {
  boolLoading = true;
  parâmetroURL: number = 0;

  lstPresenca: ListaPresencaModel[] = [];
  lstPresencaGrid: ListaPresencaModel[] = [];

  constructor(
    private http: HttpService,
    private route: ActivatedRoute,
    private router: Router,
    private messageService: MessageService
  ) {}

  ngOnInit(): void {
    try {
      this.parâmetroURL = +this.route.snapshot.paramMap.get('data')!;
      // console.warn('Parâmetro recebido:', this.parâmetroURL);

      this.GetListaPresencaBySesCodi(this.parâmetroURL);
    } catch (error) {
      this.boolLoading = false;
      console.warn('Falha ao receber os parâmetros.');
    }
  }

  GetListaPresencaBySesCodi(SesCodi: number) {
    this.boolLoading = true;
    this.http.GetListaPresencaBySesCodi(SesCodi).subscribe({
      next: (response) => {
        this.lstPresenca = response;
        response.forEach((itemPresenca) => {
          let exiteUsuario = this.lstPresencaGrid.findIndex(
            (p) => p.UsuCodi === itemPresenca.UsuCodi
          );
          if (exiteUsuario === -1) {
            this.lstPresencaGrid.push(itemPresenca);
          }
        });
        this.boolLoading = false;
      },
      error: (error) => {
        console.error('Erro ao carregar dados:', error);
        this.messageService.add({
          severity: 'error',
          summary: 'Erro: ',
          detail: 'Falha ao realizar a operação, contate o suporte.',
        });
        this.boolLoading = false;
      },
    });
  }

  SalvarRegistro() {
    console.warn(this.lstPresencaGrid);
  }

  CancelaRegitro() {
    this.router.navigate(['/sessao']);
  }

  onGlobalFilter(table: Table, event: Event) {
    table.filterGlobal((event.target as HTMLInputElement).value, 'contains');
  }
}
