import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { CadastroClienteComponent } from './cadastro-cliente/cadastro-cliente.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';


//angular material
import {MatInputModule} from '@angular/material/input';
import {MatFormFieldModule} from '@angular/material/form-field';
import {MatTableModule} from '@angular/material/table';
import {MatCardModule} from '@angular/material/card';
import {MatButtonModule} from '@angular/material/button';
import {MatIconModule} from '@angular/material/icon';
import {MatProgressBarModule} from '@angular/material/progress-bar';
import {MatExpansionModule} from '@angular/material/expansion';
import { ReactiveFormsModule } from '@angular/forms';
import {MatDialogModule} from '@angular/material/dialog';
import {MatSelectModule} from '@angular/material/select';

import { ToastrModule } from 'ngx-toastr';
import { CadastroClienteServiceService } from './cadastro-cliente/service/cadastro-cliente-service.service';
import { HttpClientModule } from '@angular/common/http';
import { RelacionamentoClienteComponent } from './cadastro-cliente/relacionamento-cliente/relacionamento-cliente.component';

@NgModule({
  declarations: [
    AppComponent,
    CadastroClienteComponent,
    RelacionamentoClienteComponent
  ],
  imports: [
    HttpClientModule,
    BrowserModule,
    AppRoutingModule,
    BrowserAnimationsModule,
    MatInputModule,
    MatFormFieldModule,
    MatTableModule,
    MatCardModule,
    MatButtonModule,
    MatIconModule,
    MatProgressBarModule,
    MatExpansionModule,
    ReactiveFormsModule,
    ToastrModule.forRoot(), // ToastrModule added
    MatDialogModule,
    MatSelectModule,
    

  ],
  providers: [
    CadastroClienteServiceService,
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
