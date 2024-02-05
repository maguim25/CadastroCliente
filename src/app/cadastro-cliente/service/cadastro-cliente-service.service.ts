import { Injectable } from '@angular/core';
import { HttpClient, HttpErrorResponse, HttpHeaders } from '@angular/common/http';
import { throwError } from 'rxjs';
import { map } from 'rxjs/operators';
import {catchError} from 'rxjs/operators'; 
import { cliente } from '../model/cliente.Model';
import { clienteRelacionamento } from '../model/clienteRelacionamento.Model';
import { environment } from '../../../environments/environment'

@Injectable({
  providedIn: 'root'
})
export class CadastroClienteServiceService {

  constructor(
    private _http:HttpClient
  ) { }

  url = environment.servidor;

  public postLocalizarRelacionamentoCadastro(cliente:cliente){
    const httpOptions = {
      headers: new HttpHeaders({'Content-Type': 'application/json'})
    }

    const body = JSON.stringify(cliente)
    console.log(body,'body postCadastroCliente')
    return this._http.post(this.url+'/Bank/leadClientPj/',body,httpOptions)
    .pipe(
      map(
        (response: any) => response
      )
      ,catchError(error => {
        return throwError (error)
      }));
  }

  public postCadastroCliente(cpf: cliente){
    const httpOptions = {
      headers: new HttpHeaders({'Content-Type': 'application/json'})
    }

    const body = JSON.stringify(cpf)

    return this._http.post(this.url+'/Cadastro/cliente',body,httpOptions)
    .pipe(
      map(
        (response: any) => response
      )
      ,catchError(error => {
        return throwError (error)
      }));
  }

  public postListarTiposTelefones(){
    const httpOptions = {
      headers: new HttpHeaders({'Content-Type': 'application/json'})
    }

    return this._http.post(this.url+'/Cadastro/listarTipoTelefone',null,httpOptions)
    .pipe(
      map(
        (response: any) => response
      )
      ,catchError(error => {
        return throwError (error)
      }));
  }

  public postListarGrauParentesco(){
    const httpOptions = {
      headers: new HttpHeaders({'Content-Type': 'application/json'})
    }

    return this._http.post(this.url+'/RelacionamentoCliente/listarGrauRelacionamento',null,httpOptions)
    .pipe(
      map(
        (response: any) => response
      )
      ,catchError(error => {
        return throwError (error)
      }));
  }

  public postListarRelacionamentoCliente(cliente:cliente){
    const httpOptions = {
      headers: new HttpHeaders({'Content-Type': 'application/json'})
    }
    
    const body = JSON.stringify(cliente)

    return this._http.post(this.url+'/RelacionamentoCliente/listarPessoas',body,httpOptions)
    .pipe(
      map(
        (response: any) => response
      )
      ,catchError(error => {
        return throwError (error)
      }));
  }

  public postCadastrarRelacionamentoCliente(cliente:clienteRelacionamento){
    const httpOptions = {
      headers: new HttpHeaders({'Content-Type': 'application/json'})
    }
    
    const body = JSON.stringify(cliente)
    console.log(body, 'postCadastrarRelacionamentoCliente')

    return this._http.post(this.url+'/RelacionamentoCliente/cadastrarRelacionamento',body,httpOptions)
    .pipe(
      map(
        (response: any) => response
      )
      ,catchError(error => {
        return throwError (error)
      }));
  }

  public handleError(error: HttpErrorResponse) {
    let errorMessage = '';
    if (error.error instanceof ErrorEvent) {
      // Erro ocorreu no lado do client
      errorMessage = error.error.message;
    } else {
      // Erro ocorreu no lado do servidor
      errorMessage = `Código do erro: ${error.status}, ` + `menssagem: ${error.message}`;
    }
    console.log(errorMessage);
    return throwError(errorMessage);
  };
}
