import { animate, state, style, transition, trigger } from '@angular/animations';
import { Component, Input, OnInit, OnChanges, SimpleChanges, ViewChild, Inject } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { RelacionamentoClienteComponent } from './relacionamento-cliente/relacionamento-cliente.component';

//model
import { cliente } from './model/cliente.Model';

//modulos
import { ToastrService } from 'ngx-toastr';
import {MatDialog} from '@angular/material/dialog';

//serviços
import { CadastroClienteServiceService } from './service/cadastro-cliente-service.service';

//interfaces
import { TipoTelefone } from './interface/tipoTelefone.Interface';
import { RelacionamentoCliente } from './interface/relacionamentoCliente.Interface';
import { clienteRelacionamento } from './model/clienteRelacionamento.Model';
import { GrauRelacionamento } from './interface/GrauRelacionamento.Interface';

let ELEMENT_DATA: RelacionamentoCliente[] = [

];

@Component({
  selector: 'app-cadastro-cliente',
  templateUrl: './cadastro-cliente.component.html',
  styleUrls: ['./cadastro-cliente.component.css'],
  animations: [
    trigger('detailExpand', [
      state('collapsed', style({height: '0px', minHeight: '0'})),
      state('expanded', style({height: '*'})),
      transition('expanded <=> collapsed', animate('225ms cubic-bezier(0.4, 0.0, 0.2, 1)')),
    ]),
  ],
})

export class CadastroClienteComponent implements OnInit, OnChanges {
  @Input() checkIndicacaoRelacionamento!: string;
  
  constructor(
    private toastr: ToastrService,
    private cadastroClienteService: CadastroClienteServiceService,
    public dialog: MatDialog
  ) { }

  loading:boolean = false;
  indicacao:boolean = false;
  panelOpenState:boolean = false;
  buttonEdicao:boolean = false;
  buttonCadastro:boolean = true;
  buttonCadastroIndicacao:boolean = false;
  buttonNovoCadastro:boolean = false;
  codigoCliente:boolean = false;
  inputCPF:boolean = false;
  indicarRelacionamento:boolean = false;
  validarRelacionamentoCliente:string = "";

  dataSource = ELEMENT_DATA;
  columnsToDisplay = ['codigo', 'nome', 'sobrenome', 'cpf', 'email', 'telefone'];
  expandedElement!: RelacionamentoCliente | null;

  cadastro = new FormGroup({
    codigo: new FormControl(''),
    nome: new FormControl(''),
    sobrenome: new FormControl(''),
    cpf: new FormControl('', [Validators.required, Validators.maxLength(500)]),
    email: new FormControl(''),
    ddd: new FormControl(''),
    telefone: new FormControl(''),
    tipoTelefone: new FormControl('1'),
    tipoGrauParentesco: new FormControl('1'),
  });

  formaTelefone: TipoTelefone[] = [];
  grauParentesco: GrauRelacionamento[] = [];

  ngOnInit(): void {
    this.cadastroClienteService.postListarTiposTelefones().subscribe(
      value=>{
        console.log(value, "postListarTiposTelefones")
        this.formaTelefone = value;
      }
    );

    this.cadastroClienteService.postListarGrauParentesco().subscribe(
      value=>{
        console.log(value, "postListarGrauParentesco")
        this.grauParentesco = value;
      }
    );
  }

  ngOnChanges(changes: SimpleChanges): void {
    this.validarRelacionamentoCliente = changes['checkIndicacaoRelacionamento'].currentValue;
    this.dataSource = ELEMENT_DATA;
    
    if(this.validarRelacionamentoCliente == "Criar Relacionamento Cliente"){
      this.buttonCadastroIndicacao = true;
      this.buttonCadastro = false;
      this.buttonNovoCadastro = false;
    }else{
      this.buttonCadastro = true;
    }
  }

  cadastrarCliente(){
    this.loading = true;
    console.log(this.cadastro);
    
    const formsCadastro = new cliente();
    formsCadastro.cpf = this.cadastro.value.cpf;
    formsCadastro.email = this.cadastro.value.email;
    formsCadastro.nome = this.cadastro.value.nome;
    formsCadastro.sobrenome = this.cadastro.value.sobrenome;
    formsCadastro.ddd = this.cadastro.value.ddd;
    formsCadastro.telefone = this.cadastro.value.telefone;
    formsCadastro.tipoTelefone = this.cadastro.value.tipoTelefone;

    if(formsCadastro.cpf  == null){
      this.toastr.error('Favor Informar o CPF', '');
      this.loading = false;
      return;
    }

    if(formsCadastro.cpf != ""){

      this.cadastroClienteService.postCadastroCliente(formsCadastro).subscribe(
        value=>{
          console.log(value,'localizarRelacionamentoCadastro');

            if(value.retorno == "CPF Invalido"){
              this.toastr.warning('Este CPF é Invalido !', '');
              this.buttonEdicao = false;
              this.loading = false;
              return;
            }

            this.toastr.success('CPF LOCALIZADO')
            this.codigoCliente = true;
            this.buttonEdicao = true;
            this.buttonCadastro = false;
            this.buttonNovoCadastro = true;
            this.loading = false;
            this.inputCPF = true;
            
            this.cadastro.patchValue(value.pessoa);

            localStorage.setItem("codigoCliente", value.pessoa.codigo)
            localStorage.setItem("clienteCPF", value.pessoa.cpf)

            this.indicacao = true;
            this.cadastroClienteService.postListarRelacionamentoCliente(formsCadastro).subscribe(
              value=>{
                console.log(value, 'postListarRelacionamentoCliente');
                this.dataSource = value;
                this.loading = false;
                this.inputCPF = true;
              }
            ); 
        }
      );
    }else{
      this.toastr.error('Favor Informar o CPF', '');
      this.loading = false;
    }

  }

  cadastrarRelacionamento(){
    
    const modelClienteRelacionamento = new clienteRelacionamento();
    modelClienteRelacionamento.pessoaRelacionada = [];

    const formsCadastro = new cliente();
    formsCadastro.cpf = this.cadastro.value.cpf;
    formsCadastro.email = this.cadastro.value.email;
    formsCadastro.nome = this.cadastro.value.nome;
    formsCadastro.sobrenome = this.cadastro.value.sobrenome;
    formsCadastro.ddd = this.cadastro.value.ddd;
    formsCadastro.telefone = this.cadastro.value.telefone;
    formsCadastro.tipoTelefone = this.cadastro.value.tipoTelefone;
    formsCadastro.tipoGrauParentesco = this.cadastro.value.tipoGrauParentesco;
    formsCadastro.criarRelacionamento = true;

    console.log(formsCadastro)

    if(formsCadastro.telefone == "" || formsCadastro.nome == ""){
      this.toastr.warning('Necessita de Telefone e Nome !', '');
      return;
    }

    modelClienteRelacionamento.codigoCliente = localStorage.getItem('codigoCliente')
    modelClienteRelacionamento.pessoaRelacionada.push(formsCadastro);

    this.cadastroClienteService.postCadastrarRelacionamentoCliente(modelClienteRelacionamento).subscribe(
      value=>{
        console.log(value, 'postCadastrarRelacionamentoCliente')

        if(value.retorno == "Cliente ja existente ou ja Relacionado"){
          this.toastr.error('Cliente ja Relacionado. !', '');
          return;
        }  
        this.toastr.success('Cliente Relacionado com Sucesso. !', '');
      }
    );

  }

  novoCadastro(){
    this.buttonEdicao = false;
    this.buttonCadastro = true;
    this.buttonNovoCadastro = false;
    this.codigoCliente = false;
    this.indicacao = false;
    this.inputCPF = false;
    this.cadastro.reset();

    this.cadastro = new FormGroup({
      codigo: new FormControl(''),
      nome: new FormControl(''),
      sobrenome: new FormControl(''),
      cpf: new FormControl('', [Validators.required, Validators.maxLength(500)]),
      email: new FormControl(''),
      ddd: new FormControl(''),
      telefone: new FormControl(''),
      tipoTelefone: new FormControl('1'),
    });

  }

  openDialog(): void {
    const dialogRef = this.dialog.open(RelacionamentoClienteComponent, {
      width: '800px',
      height: '800px',
      
    });

    dialogRef.afterClosed().subscribe(result => {
      console.log('The dialog was closed');
      const clientePrincipal = new cliente();
      clientePrincipal.cpf = localStorage.getItem('clienteCPF')
      this.cadastroClienteService.postListarRelacionamentoCliente(clientePrincipal).subscribe(
        value=>{
          console.log(value, 'postListarRelacionamentoCliente');
          this.dataSource = value;
        }
      );
    });
  }

}


