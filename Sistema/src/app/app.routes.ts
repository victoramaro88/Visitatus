import { ConviteComponent } from './pages/convite/convite.component';
import { SessaoComponent } from './pages/sessao/sessao.component';
import { ContatoComponent } from './pages/contato/contato.component';
import { RouterModule, Routes } from '@angular/router';
import { LoginComponent } from './pages/login/login.component';
import { HomeComponent } from './pages/home/home.component';
import { NgModule } from '@angular/core';
import { TemplateConviteComponent } from './pages/template-convite/template-convite.component';

export const routes: Routes = [
  { path: 'login', component: LoginComponent },
  { path: 'home', component: HomeComponent },
  { path: 'contato', component: ContatoComponent },
  { path: 'sessao', component: SessaoComponent },
  { path: 'template', component: TemplateConviteComponent },
  { path: 'convite/:data', component: ConviteComponent },
  { path: '**', redirectTo: 'login' }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
