import { Component, OnInit, ViewChild, ChangeDetectorRef } from '@angular/core';
import { UsuarioModel } from '../../models/Usuario.Model';
import { UsuarioLogadoModel } from '../../models/UsuarioLogado.Model';
import { HttpService } from '../../services/http-service.service';
import { ConfirmationService, MessageService } from 'primeng/api';
import { Utils } from '../../services/utils';
import { ActivatedRoute, Router } from '@angular/router';
import { Base64Service } from '../../services/base64.service';
import { CryptoService } from '../../services/crypto.service';
import { ImportsModule } from '../../imports';
import { LojaModel } from '../../models/Loja.Model';
import { PerfilUsuarioListaModel } from '../../models/PerfilUsuarioLista.Model';
import { Table } from 'primeng/table';
import { PotenciaModel } from '../../models/Potencia.Model';
import { RitoModel } from '../../models/Rito.Model';
import { EstadoModel } from '../../models/Estado.Model';
import { CidadeModel } from '../../models/Cidade.Model';
import { OrientacaoLojaModel } from '../../models/OrientacaoLoja.Model';


@Component({
  selector: 'app-loja',
  standalone: true,
  imports: [ImportsModule],
  templateUrl: './loja.component.html',
  styleUrl: './loja.component.css',
  providers: [MessageService, ConfirmationService],
})
export class LojaComponent implements OnInit {
  @ViewChild('editor') editor: any;

  // orientacaoLoja: string = '';
  boolLoading = false;
  objUsuarioLogado: UsuarioLogadoModel = new UsuarioLogadoModel();
  objPerfilUsuario: PerfilUsuarioListaModel = new PerfilUsuarioListaModel();
  boolManterRegistro: boolean = false;
  boolNovoRegistro: boolean = false;
  contCaracterOrientacao: number = 2000;

  objLoja: LojaModel = new LojaModel(0,'','','','','','',true,0,0,0);
  imagemPadrao: string = 'assets/img/noImage.png';
  imagemBase64: string | null = null;
  lstLoja: LojaModel[] = [];
  lstPotencia: PotenciaModel[] = [];
  objPotencia: PotenciaModel = new PotenciaModel();
  lstRito: RitoModel[] = [];
  objRito: RitoModel = new RitoModel();
  lstEstado: EstadoModel[] = [];
  objEstado: EstadoModel = new EstadoModel();
  lstCidade: CidadeModel[] = [];
  objCidade: CidadeModel = new CidadeModel();
  lstOrientacaoLoja: OrientacaoLojaModel[] = [];
  objOrientacaoLoja: OrientacaoLojaModel = new OrientacaoLojaModel();

  constructor(
      private http: HttpService,
      private messageService: MessageService,
      private utils: Utils,
      private router: Router,
      private route: ActivatedRoute,
      private base64Service: Base64Service,
      private cryptoService: CryptoService,
      private cdr: ChangeDetectorRef,
      private confirmationService: ConfirmationService
    ) {
      this.objPerfilUsuario = JSON.parse(
        this.cryptoService.lerDoSessionStorage('prf')
      );

      this.objUsuarioLogado = JSON.parse(
        this.cryptoService.lerDoSessionStorage('usr')
      );

      // console.warn("Usuário Logado: ", this.objUsuarioLogado);
      // console.warn("Perfil (Menu): ", this.objPerfilUsuario);
    }

  ngOnInit() {
    try {
      this.GetPotencia(0);
    } catch (error) {
      this.boolLoading = false;
      console.warn('Falha ao realizar a operação inicial.', error);
    }
  }

  GetLoja(lojCodi: number) {
    this.boolLoading = true;
    this.http.GetLoja(lojCodi).subscribe({
      next: (response) => {
        this.lstLoja = response;
        this.boolLoading = false;
        this.objLoja = this.lstLoja[0];
        if(this.objPerfilUsuario.perCodi !== 1){
          this.LocalizaPotenciaLoja(this.objLoja.PotCodi);
          this.LocalizaRitoLoja(this.objLoja.RitCodi);
          this.LocalizaCidadeLoja(this.objLoja.CidCodi);
          this.LocalizaEstadoLoja(this.objCidade.EstCodi);
        }
        this.imagemBase64 = this.objLoja.LojLogo ? 'data:image/png;base64,' + this.objLoja.LojLogo : null;
        this.GetOrientacaoLojaByLojCodi(lojCodi);
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

  GetOrientacaoLojaByLojCodi(lojCodi: number) {
    this.boolLoading = true;
    try {
      this.http.GetOrientacaoLojaByLojCodi(lojCodi).subscribe({
        next: (response) => {
          console.warn(response);
          if(response){
            this.lstOrientacaoLoja = response;
            this.objOrientacaoLoja = this.lstOrientacaoLoja ? this.lstOrientacaoLoja[0] : new OrientacaoLojaModel();
            setTimeout(() => {
              this.AtualizarContador();
              this.boolLoading = false;
            }, 500);
            console.warn('ORIENTAÇÃO DA LOJA:', this.objOrientacaoLoja);
            this.boolLoading = false;
          }
        },
        error: (error) => {
          if(error.status != 404){
            console.error('Erro ao carregar dados:', error);
            this.messageService.add({
              severity: 'error',
              summary: 'Erro: ',
              detail: 'Falha ao realizar a operação, contate o suporte.',
            });
          }
          this.boolLoading = false;
        },
      });
    } catch (error) {

    }
  }

  GetPotencia(potCodi: number) {
    this.boolLoading = true;
    this.http.GetPotencia(potCodi).subscribe({
      next: (response) => {
        // this.lstPotencia = response;
        this.lstPotencia = response
        .filter((p: any) => p.potStat === true && p.potRegu === true)
        .map((p: any) => ({
          PotCodi: p.potCodi,
          PotNome: p.potNome,
          PotLogo: p.potLogo,
          PotRegu: p.potRegu,
          PotStat: p.potStat,
          PotSigl: p.potSigl
        }));
        this.GetRito(0);
        // this.boolLoading = false;
        // console.warn('LISTA DE POTÊNCIAS:', this.lstPotencia);
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

  GetRito(ritCodi: number) {
    this.boolLoading = true;
    this.http.GetRito(ritCodi).subscribe({
      next: (response) => {
        this.lstRito = response.filter(r => r.RitStat === true);
        this.GetEstado(0);
        // this.boolLoading = false;
        // console.warn('LISTA DE RITOS:', this.lstRito);
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

  GetEstado(estCodi: number) {
    this.boolLoading = true;
    this.http.GetEstados(estCodi).subscribe({
      next: (response) => {
        this.lstEstado = response;
        this.GetCidades(0);
        // this.boolLoading = false;
        // console.warn('LISTA DE ESTADOS:', this.lstEstado);
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

  GetCidades(estCodi: number) {
    this.boolLoading = true;
    this.http.GetCidades(estCodi).subscribe({
      next: (response) => {
        this.lstCidade = response;
        this.GetLoja(this.objPerfilUsuario.lojCodi);
        // this.boolLoading = false;
        // console.warn('LISTA DE CIDADES:', this.lstCidade);
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

  GetCidadesPorEstado(estCodi: number){
    this.boolLoading = true;
    this.http.GetCidadesPorEstado(estCodi).subscribe({
      next: (response) => {
        this.lstCidade = response;
        this.objCidade = new CidadeModel();
        this.boolLoading = false;
        // console.warn('LISTA DE CIDADES:', this.lstCidade);
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

  NovoRegistro() {
    this.objLoja = new LojaModel(0,'','','','','','',true,0,0,0);
    this.imagemBase64 = this.objLoja.LojLogo ? 'data:image/png;base64,' + this.objLoja.LojLogo : null;
    this.objOrientacaoLoja = new OrientacaoLojaModel();
    this.boolManterRegistro = true;
  }

  EditarRegistro(loja: LojaModel) {
    console.warn(loja);
    this.objLoja = loja;
    this.LocalizaPotenciaLoja(loja.PotCodi);
    this.LocalizaRitoLoja(loja.RitCodi);
    this.LocalizaCidadeLoja(loja.CidCodi);
    // this.objPotencia = this.lstPotencia.find(p => p.PotCodi === loja.PotCodi)!;
    // this.objRito = this.lstRito.find(r => r.RitCodi === loja.RitCodi)!;
    // this.objCidade = this.lstCidade.find(c => c.CidCodi === loja.CidCodi)!;
    this.imagemBase64 = loja.LojLogo ? 'data:image/png;base64,' + loja.LojLogo : null;
    this.GetOrientacaoLojaByLojCodi(loja.LojCodi);

    if(this.objCidade){
      this.LocalizaEstadoLoja(this.objCidade.EstCodi);
    }

    this.boolManterRegistro = true;
    this.boolLoading = false;
  }

  LocalizaPotenciaLoja(potCodi: number){
    this.objPotencia = this.lstPotencia.find(p => Number(p.PotCodi) === Number(potCodi))!;
  }

  LocalizaRitoLoja(ritCodi: number){
    this.objRito = this.lstRito.find(p => p.RitCodi === ritCodi)!;
  }

  LocalizaCidadeLoja(cidCodi: number){
    this.objCidade = this.lstCidade.find(p => p.CidCodi === cidCodi)!;
  }

  LocalizaEstadoLoja(estCodi: number){
    this.objEstado = this.lstEstado.find(p => p.EstCodi === estCodi)!;
  }

  SalvarRegistro(){
    this.objLoja.PotCodi = this.objPotencia ? this.objPotencia.PotCodi : 0;
    this.objLoja.RitCodi = this.objRito ? this.objRito.RitCodi : 0;
    this.objLoja.CidCodi = this.objCidade ? this.objCidade.CidCodi : 0;

    if(this.ValidaInformacoes()){
      // console.warn('OBJ LOJA: ', this.objLoja);
      // console.warn('ORIENTAÇÃO DA LOJA: ', this.objOrientacaoLoja);

      if(this.objLoja.LojCodi > 0){
        this.PutLoja(this.objLoja.LojCodi, this.objLoja);
      } else{
        this.PostLoja(this.objLoja);
      }
    }
  }

  ValidaInformacoes(){
    if(this.objLoja.LojNome.length === 0){
      this.messageService.add({severity: 'warn', summary: 'Atenção! ', detail: 'Insira o nome da Loja.'});
      return false;
    }
    if(this.objLoja.LojNumL.length === 0){
      this.messageService.add({severity: 'warn', summary: 'Atenção! ', detail: 'Insira o número da Loja.'});
      return false;
    }
    if(this.objLoja.PotCodi === 0){
      this.messageService.add({severity: 'warn', summary: 'Atenção! ', detail: 'Insira a potência da Loja.'});
      return false;
    }
    if(this.objLoja.RitCodi === 0){
      this.messageService.add({severity: 'warn', summary: 'Atenção! ', detail: 'Insira o rito da Loja.'});
      return false;
    }
    if(this.contCaracterOrientacao > 2000){
      this.messageService.add({severity: 'warn', summary: 'Atenção! ', detail: 'O limite máximo de caracteres da orientação é 2.000.'});
      return false;
    }
    if(this.objCidade && this.objCidade.CidCodi === 0){
      this.messageService.add({severity: 'warn', summary: 'Atenção! ', detail: 'Insira a cidade da Loja.'});
      return false;
    }

    return true;
  }

  PostLoja(loja: LojaModel){
    this.boolLoading = true;
    if (this.ValidaInformacoes()) {
      this.boolLoading = true;
      // console.warn(this.objConsultaUsrLj);
      this.http.PostLoja(loja).subscribe({
        next: (response) => {
          // console.warn(response);
          this.boolLoading = false;
          if (response) {
            if(this.objOrientacaoLoja.OrlDesc.length > 0){
              this.boolNovoRegistro = true; //-> FEITO PARA ALTERAR A MENSAGEM FINAL DE ALTERADO PARA INSERIDO NA ORIENTAÇÃO DA LOJA.
              this.objOrientacaoLoja.LojCodi = response.LojCodi;
              this.objOrientacaoLoja.UsuCodi = this.objUsuarioLogado.usuCodi;
              this.objOrientacaoLoja.OrlStat = true;
              this.PostOrientacaoLoja(this.objOrientacaoLoja);
            } else {
              this.messageService.add({
                  severity: 'success',
                  summary: 'Sucesso!',
                  detail: 'Registro inserido com sucesso!'
              });
              setTimeout(() => {
                this.boolLoading = false;
                this.router.navigate(['/home']);
              }, 1000);
            }
          } else {
            console.error('Erro ao confirmar a presença:', response);
            this.messageService.add({
              severity: 'error',
              summary: 'Erro: ',
              detail: 'Falha ao realizar a operação, contate o suporte.',
            });
          }
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

  PutLoja(lojCodi: number, loja: LojaModel){
    try {
      this.boolLoading = true;
      this.http.PutLoja(lojCodi, loja).subscribe({
        next: (response) => {
          // console.warn("RETORNO DA ALTERAÇÃO:", response);
          if (response === 'Alterado com sucesso!') {
            //-> INSERINDO OU ALTERANDO A ORIENTAÇÃO DA LOJA
            if(this.objOrientacaoLoja.OrlCodi > 0){
              this.PutOrientacaoLoja(this.objOrientacaoLoja.OrlCodi, this.objOrientacaoLoja);
            }else{
              if(this.objOrientacaoLoja.OrlDesc.length > 0){
                this.objOrientacaoLoja.LojCodi = this.objLoja.LojCodi;
                this.objOrientacaoLoja.UsuCodi = this.objUsuarioLogado.usuCodi;
                this.objOrientacaoLoja.OrlStat = true;
                this.PostOrientacaoLoja(this.objOrientacaoLoja);
              } else{
                this.messageService.add({
                  severity: 'success',
                  summary: 'Sucesso!',
                  detail: 'Registro alterado com sucesso!'
                });
                this.boolLoading = false;
                this.router.navigate(['/home']);
              }
            }
          } else {
            console.error('Erro ao salvar o registro: ', response);
            this.messageService.add({
              severity: 'error',
              summary: 'Erro:',
              detail: 'Falha ao alterar o registro.',
            });
            this.boolLoading = false;
          }
        },
        error: (error) => {
          console.error('Erro ao carregar dados:', error);
          this.boolLoading = false;
        },
      });
    } catch (error) {
      console.error('Erro ao carregar dados:', error);
      this.boolLoading = false;
    }
  }

  PostOrientacaoLoja(orientacaoLoja: OrientacaoLojaModel){
    try {
          this.http.PostOrientacaoLoja(orientacaoLoja).subscribe({
            next: (response) => {
              if (response) {
                if(this.boolNovoRegistro){
                  this.messageService.add({severity: 'success', summary: 'Sucesso!', detail: 'Registro inserido com sucesso!' });
                  setTimeout(() => {
                    this.boolLoading = false;
                    this.router.navigate(['/home']);
                  }, 1000);
                } else{
                  this.messageService.add({severity: 'success', summary: 'Sucesso!', detail: 'Registro alterado com sucesso!' });
                  setTimeout(() => {
                    this.boolLoading = false;
                    this.router.navigate(['/home']);
                  }, 1000);
                }
                this.boolLoading = false;
              } else{
                this.boolLoading = false;
                this.messageService.add({
                  severity: 'error',
                  summary: 'Erro: ',
                  detail: 'Falha ao realizar a operação, contate o suporte.',
                });
              }
            },
            error: (error) => {
              console.error('Erro ao carregar dados:', error);
              this.boolLoading = false;
              this.messageService.add({
                severity: 'error',
                summary: 'Erro: ',
                detail: 'Falha ao realizar a operação, contate o suporte.',
              });
            },
          });
        } catch (error) {
          console.error('Erro ao carregar dados:', error);
          this.boolLoading = false;
          this.messageService.add({
            severity: 'error',
            summary: 'Erro: ',
            detail: 'Falha ao realizar a operação, contate o suporte.',
          });
        }
  }

  PutOrientacaoLoja(orlCodi: number, orientacaoLoja: OrientacaoLojaModel){
    try {
      this.boolLoading = true;
      this.http.PutOrientacaoLoja(orlCodi, orientacaoLoja).subscribe({
        next: (response) => {
          if (response === 'Alterado com sucesso!') {
            this.messageService.add({
              severity: 'success',
              summary: 'Sucesso!',
              detail: 'Registro alterado com sucesso!',
            });            
            setTimeout(() => {
              this.boolLoading = false;
              this.router.navigate(['/home']);
            }, 1000);
          } else {
            console.error('Erro ao salvar o registro: ', response);
            this.messageService.add({
              severity: 'error',
              summary: 'Erro:',
              detail: 'Falha ao alterar o registro.',
            });
            this.boolLoading = false;
          }
        },
        error: (error) => {
          console.error('Erro ao carregar dados:', error);
          this.boolLoading = false;
          this.messageService.add({severity: 'error', summary: 'Erro:', detail: 'Falha ao alterar o registro.' });
        },
      });
    } catch (error) {
      console.error('Erro ao carregar dados:', error);
      this.boolLoading = false;
    }
  }

  // carregar imagem vinda do banco
  carregarImagem(base64Banco: string | null): void {

    if (base64Banco) {

      // caso o banco salve apenas o base64 puro
      // this.imagemBase64 =
      //   'data:image/jpeg;base64,' + base64Banco;
      this.imagemBase64 = base64Banco;

    }

  }

  // selecionar imagem do computador
  onFileSelected(event: any): void {

    const file = event.target.files[0];

    if (!file) {
      return;
    }

    // valida tipo apenas png e jpeg
    const tiposPermitidos = [
      'image/png',
      'image/jpeg'
    ];

    if (!tiposPermitidos.includes(file.type)) {

      this.messageService.add({
        severity: 'warn',
        summary: 'Atenção! ',
        detail: 'Selecione apenas imagens do formato ".png" ou ".jpeg"',
      });

      return;

    }

    const reader = new FileReader();
    reader.onload = (e: any) => {
      const img = new Image();
      img.onload = () => {
        const canvas = document.createElement('canvas');
        const ctx = canvas.getContext('2d');
        const maxWidth = 128;

        // mantém proporção
        const scale = maxWidth / img.width;
        canvas.width = maxWidth;
        canvas.height = img.height * scale;
        ctx?.drawImage(
          img,
          0,
          0,
          canvas.width,
          canvas.height
        );

        // gera base64 final
        this.imagemBase64 =
          canvas.toDataURL('image/png', 0.9);

        this.objLoja.LojLogo = this.imagemBase64.replace('data:image/png;base64,', '');
        // console.warn('BASE 64 da imagem da variável: ', this.imagemBase64);
        // console.warn('BASE 64 da imagem do objeto: ', this.objLoja.LojLogo);

      };

      img.src = e.target.result;

    };

    reader.readAsDataURL(file);

  }

  LimparImagem(){
    this.imagemBase64 = '';
    this.objLoja.LojLogo = '';
  }

  AtualizarContador(): void {

    if (!this.editor?.quill) {
      return;
    }

    const quill = this.editor.quill;

    // pega texto visível
    let texto = quill.getText().trimEnd();

    // limite máximo
    const limite = 2000;

    // se ultrapassou
    if (texto.length > limite) {

      // corta excesso
      quill.deleteText(
        limite,
        texto.length
      );

      texto = quill.getText().trimEnd();

    }

    // contador restante
    this.contCaracterOrientacao =
      limite - texto.length;

    // evita negativo
    if (this.contCaracterOrientacao < 0) {
      this.contCaracterOrientacao = 0;
    }

    this.cdr.detectChanges();

  }

  ConfirmaCancelar() {
        this.confirmationService.confirm({
            header: 'Deseja realmente cancelar?',
            message: 'Todas as alterações serão perdidas!',
            accept: () => {
              if(this.objPerfilUsuario.perCodi === 1){
                this.objOrientacaoLoja = new OrientacaoLojaModel();
                this.GetLoja(this.objPerfilUsuario.lojCodi);
                this.boolManterRegistro = false;
              } else{
                this.router.navigate(['/home']);
              }
            },
            reject: () => {
                
            }
        });
  }

  onGlobalFilter(table: Table, event: Event) {
    table.filterGlobal((event.target as HTMLInputElement).value, 'contains');
  }
}
