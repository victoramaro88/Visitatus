import { Component, OnInit } from '@angular/core';
import { ConfirmationService, MessageService } from 'primeng/api';
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
  providers: [ConfirmationService, MessageService],
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
    private messageService: MessageService,
    private confirmationService: ConfirmationService
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
        // console.warn('Retorno confirmação', response);
        this.lstPresenca = [];
        this.lstPresencaGrid = [];
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
    this.boolLoading = true;
    // console.warn(this.lstPresencaGrid);

    //-> Filtrando apenas as pessoas que receberam a presença
    // let listaPresentes: ListaPresencaModel[] = this.lstPresencaGrid.filter(
    //   (p) => p.PreAtiv === true
    // );

    if (this.lstPresencaGrid) {
      this.http.PostLancamentoPresencaSessao(this.lstPresencaGrid).subscribe({
        next: (response) => {
          // console.warn('Retorno Serviço', response);
          this.boolLoading = false;

          this.messageService.add({
            severity: 'success',
            summary: 'Sucesso!',
            detail: 'Certificados enviados com sucesso!',
          });

          this.GetListaPresencaBySesCodi(this.parâmetroURL);
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
  }

  CancelaRegitro() {
    this.router.navigate(['/sessao']);
  }

  ConfirmaEnvio(event: Event) {
    this.confirmationService.confirm({
      target: event.target as EventTarget,
      message:
        'Deseja realmente confirmar a seleção de presença e enviar e-mail dos certificados?',
      header: 'Atenção',
      icon: 'pi pi-exclamation-triangle',
      acceptIcon: 'none',
      rejectIcon: 'none',
      rejectButtonStyleClass: 'p-button-text',
      accept: () => {
        this.SalvarRegistro();
      },
      reject: () => {
        // this.messageService.add({
        //   severity: 'error',
        //   summary: 'Rejected',
        //   detail: 'You have rejected',
        //   life: 3000,
        // });
      },
    });
  }

  onGlobalFilter(table: Table, event: Event) {
    table.filterGlobal((event.target as HTMLInputElement).value, 'contains');
  }
}
