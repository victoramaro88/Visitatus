import { CertificadoComponent } from './pages/certificado/certificado.component';
import { TemplateCertificadoComponent } from './pages/template-certificado/template-certificado.component';
import { ConfirmacaoPresencaComponent } from './pages/confirmacao-presenca/confirmacao-presenca.component';
import { ConviteComponent } from './pages/convite/convite.component';
import { SessaoComponent } from './pages/sessao/sessao.component';
import { ContatoComponent } from './pages/contato/contato.component';
import { RouterModule, Routes } from '@angular/router';
import { LoginComponent } from './pages/login/login.component';
import { HomeComponent } from './pages/home/home.component';
import { NgModule } from '@angular/core';
import { TemplateConviteComponent } from './pages/template-convite/template-convite.component';
import { PresencaSessaoComponent } from './pages/presenca-sessao/presenca-sessao.component';
import { UsuarioComponent } from './pages/usuario/usuario.component';

export const routes: Routes = [
  { path: 'login', component: LoginComponent },
  { path: 'home', component: HomeComponent },
  { path: 'contato', component: ContatoComponent },
  { path: 'sessao', component: SessaoComponent },
  { path: 'template', component: TemplateConviteComponent },
  { path: 'template-certificado', component: TemplateCertificadoComponent },
  { path: 'convite/:data', component: ConviteComponent },
  { path: 'confirmacao/:data', component: ConfirmacaoPresencaComponent },
  { path: 'certificado/:data', component: CertificadoComponent },
  { path: 'presenca-sessao/:data', component: PresencaSessaoComponent },
  { path: 'usuario', component: UsuarioComponent },
  // { path: 'confirmacao', component: ConfirmacaoPresencaComponent }, //-> Temporário
  { path: '**', redirectTo: 'login' },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
})
export class AppRoutingModule {}
