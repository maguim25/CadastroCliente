export class cliente {
  codigo: number = 0;
  nome: string = "";
  sobrenome: string = "";
  cpf!: string  | null;
  email: string = "";
  ddd: string = "";
  telefone: string = "";
  tipoTelefone: number = 0;
  isRelacionamento: boolean = false;
  criarRelacionamento: boolean = false;
  tipoGrauParentesco: string = "";
  ativo: boolean = false;
  dtInsercao!: string;
}
