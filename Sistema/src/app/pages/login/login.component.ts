import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MessageService } from 'primeng/api';
import { ImportsModule } from '../../imports';
import { HttpService } from '../../services/http-service.service';
import { CryptoService } from '../../services/crypto.service';
import { UsuarioLogadoModel } from '../../models/UsuarioLogado.Model';
import { PerfilUsuarioListaModel } from '../../models/PerfilUsuarioLista.Model';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [ImportsModule],
  templateUrl: './login.component.html',
  styleUrl: './login.component.css',
  providers: [MessageService]
})
export class LoginComponent implements OnInit {

  formulario: FormGroup;
  boolLoading = false;
  objUsuarioLogado: UsuarioLogadoModel = new UsuarioLogadoModel();
  boolDialogPerfil: boolean = false;
  objPerfilSelecionado: PerfilUsuarioListaModel = new PerfilUsuarioListaModel();

  constructor(
    private router: Router,
    private http: HttpService,
    private messageService: MessageService,
    private formBuilder: FormBuilder,
    private cryptoService: CryptoService
  ) {
    this.formulario = this.formBuilder.group({
      usr: ['', Validators.required],
      senha: ['', [Validators.required]]
    });
  }

  ngOnInit(): void {

  }

  ValidaFormulario(): void {
    if (this.formulario.valid) {
      this.Login();
    } else {
      console.log('Formulário inválido. Verifique os campos.');
    }
  }

  Login(): void {
    this.boolLoading = true;
    try {
      this.http.ValidarLogin(this.formulario.value.usr.replace('.', '').replace('.', '').replace('-', ''), this.formulario.value.senha).subscribe({
        next: (response) => {
          // console.warn('Lista de Perfis:', response);
          this.objUsuarioLogado = response;
          let perfisAtivos = response.lstPerfil.filter(p => p.peUStat === true);
          // console.warn("Perfis Ativos Pesquisa:", perfisAtivos);
          this.objUsuarioLogado.lstPerfil = perfisAtivos;
          // console.warn("Perfis Ativos Final:", perfisAtivos);
          this.cryptoService.salvarNoSessionStorage("usr", JSON.stringify(this.objUsuarioLogado));
          // console.warn(this.cryptoService.lerDoSessionStorage("usr"));
          this.boolLoading = false;

          //-> Verifica se possui mais de um perfil para selecionar, senão já manda o único perfil direto.
          if (this.objUsuarioLogado.lstPerfil.length > 1) {
            this.boolDialogPerfil = true;
          } else if (this.objUsuarioLogado.lstPerfil.length === 1){
            this.objPerfilSelecionado = this.objUsuarioLogado.lstPerfil[0];
            this.SelecionaPerfilUsuario();
          } else {
            this.messageService.add({severity:'error', summary:'Atenção: ', detail: 'Usuário sem perfil ativo para acesso.'});
          }
        },
        error: (error) => {
          this.boolLoading = false;
          if(error.error ==='Senha incorreta.') {
            this.messageService.add({severity:'error', summary:'Erro: ', detail: error.error});
          } else if(error.error ==='Usuário não encontrado.') {
            this.messageService.add({severity:'error', summary:'Erro: ', detail: error.error});
          } else if(error.error === 'Usuário sem vínculo com nenhuma Loja.') {
            this.messageService.add({severity:'error', summary:'Erro: ', detail: error.error});
          } else {
            console.error(error.error);
            this.messageService.add({severity:'error', summary:'Erro: ', detail: 'Falha ao realizar a operação, contate o suporte.'});
          }
        }
      });
    } catch (error) {
      console.error(error);
    }
  }

  SelecionaPerfilUsuario() {
    this.boolLoading = true;
    if (this.objPerfilSelecionado.perCodi > 0) {
      this.boolDialogPerfil = false;
      // console.warn("Perfil Selecionado", this.objPerfilSelecionado);
      this.cryptoService.salvarNoSessionStorage("prf", JSON.stringify(this.objPerfilSelecionado));
      this.router.navigate(['/home']);
      this.boolLoading = false;
    } else {
      this.boolLoading = false;
      this.messageService.add({severity:'warn', summary:'Atenção: ', detail: 'Selecione um perfil para continuar.'});
    }
  }

}
