import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { CadastroClienteComponent } from './cadastro-cliente/cadastro-cliente.component';
import { RelacionamentoClienteComponent } from './cadastro-cliente/relacionamento-cliente/relacionamento-cliente.component';

const routes: Routes = [

  { path: 'cadastro', component: CadastroClienteComponent },
  { path: 'relacionamento', component: RelacionamentoClienteComponent },
  { path: '', redirectTo: 'cadastro', pathMatch: 'full' },
  { path: '**', redirectTo: 'cadastro', pathMatch: 'full' }, // TODO pagina 404, manter essa rota por ultimo
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
