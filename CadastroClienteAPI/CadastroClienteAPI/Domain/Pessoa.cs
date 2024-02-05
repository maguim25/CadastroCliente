using CadastroClienteAPI.Data;
using CadastroClienteAPI.Data.Repositories;
using CadastroClienteAPI.Domain.Dto;
using CadastroClienteAPI.Domain.Enum;
using CadastroClienteAPI.Domain.Interface;
using CadastroClienteAPI.Domain.ModelView;
using Dapper;

namespace CadastroClienteAPI.Domain
{
  public class Pessoa: IPessoa
  {
    private readonly Context _context;
    private readonly IRepositoryPessoa _repositoryPessoa;
    public Pessoa(Context context, IRepositoryPessoa repositoryPessoa)
    {
      _context = context;
      _repositoryPessoa = repositoryPessoa;
    }

    #region ModelView

    public async Task<ModelViewResponsePessoa> CadastroPessoa(DtoPessoa pessoa)
    {
      var model = new ModelViewResponsePessoa();
      var validacaoCPF = new DtoCPF();
      var localizarPessoa = new DtoPessoa();

      if (!pessoa.isRelacionamento)
      {
        validacaoCPF = ValidadorDeCPF(pessoa.CPF).Result;

        if (validacaoCPF.Retorno == "CPF Invalido")
        {
          model.Retorno = validacaoCPF.Retorno;
          return model;
        }

        localizarPessoa = _repositoryPessoa.LocalicarPessoa(pessoa).Result;

        if (localizarPessoa == null)
        {
          localizarPessoa = new DtoPessoa();
        }
      }

      if (localizarPessoa.CPF == null)
      {
        await _repositoryPessoa.CadastrarPessoa(pessoa);

        var checarPessoaInserida = _repositoryPessoa.LocalicarPessoa(pessoa).Result;
        pessoa.Codigo = checarPessoaInserida.Codigo;

        await _repositoryPessoa.CadastrarTelefonePessoa(pessoa);
        model.Retorno = "Cadastro Realizado com Sucesso";
      }
      else
      {
        model.Retorno = "Cliente localizado, Dados incompletos para cadastro";
      }
      // verificar relacionamento e retorna para front caso exista dados incompletos

      var localizarPessoasRelacionadas = _repositoryPessoa.LocalicarListaRelacionamentoPessoa(pessoa.CPF).Result;
      var localizarTelefonePessoa = _repositoryPessoa.ListarTelefonePessoa(_repositoryPessoa.LocalicarPessoa(pessoa).Result.Codigo).Result;

      model.Pessoa = _repositoryPessoa.LocalicarPessoa(pessoa).Result;
      model.Pessoa.isRelacionamento = localizarPessoasRelacionadas.Any();
      model.Pessoa.DDD = (localizarTelefonePessoa.Count == 0) ? model.Pessoa.DDD : localizarTelefonePessoa.FirstOrDefault().DDD.ToString();
      model.Pessoa.Telefone = (localizarTelefonePessoa.Count == 0) ? model.Pessoa.Telefone : localizarTelefonePessoa.FirstOrDefault().NumeroTelefone.ToString();
      model.Pessoa.TipoTelefone = (localizarTelefonePessoa.Count == 0) ? model.Pessoa.TipoTelefone : (int)localizarTelefonePessoa.FirstOrDefault().TipoTelefoneEnum;

      return model;
    }

    public async Task<ModelViewResponsePessoa> CadastroPessoaRelacionamento(DtoPessoaRelacionamento pessoaR)
    {
      var model = new ModelViewResponsePessoa();

      var pessoa = _repositoryPessoa.LocalicarPessoaRelacionamento(pessoaR.PessoaRelacionada.FirstOrDefault());

      var localizarListaRelacionamento = LocalicarListaRelacionamentoPessoa(pessoaR.PessoaRelacionada.FirstOrDefault().CPF).Result;

      if (pessoa != null || localizarListaRelacionamento.Count > 0)
      {
        model.Retorno = "Cliente ja existente ou ja Relacionado";
      }
      else // se não localizar adiciona novo cliente na tabela Pessoa e cria um relacionamento com codigoCliente informado
      {
        var newPessoa = pessoaR.PessoaRelacionada.FirstOrDefault();
        newPessoa.isRelacionamento = true;
        var cadastrarPessoa = CadastroPessoa(newPessoa).Result;

        if (cadastrarPessoa.Retorno == "Cadastro Realizado com Sucesso")
        {
          using (var con = _context.Connection().Result)
          {
            con.Execute("insert into [RELACIONAMENTO_PESSOA](CD_PESSOA,CD_PESSOA_RELACIONAMENTO, NR_TIPO_RELACIONAMENTO_PESSOA) values(@CDPESSOA, @CDPESSOARELACIONAMENTO, @NRTIPORELACIONAMENTOPESSOA)", new
            {
              CDPESSOA = pessoaR.CodigoCliente,
              CDPESSOARELACIONAMENTO = cadastrarPessoa.Pessoa.Codigo,
              NRTIPORELACIONAMENTOPESSOA = 1
            });

            model.Retorno = "Relacionamento criaodo com Sucesso";
          }
        }

      }

      return model;
    }
    #endregion ModelView

    #region Listas
    public async Task<List<DtoTipoTelefone>> ListarTiposTelefones()
    {
      try
      {
        return _repositoryPessoa.ListarTiposTelefones().Result;
      }
      catch (Exception ex)
      {

        throw ex;
      }
    }

    public async Task<List<DtoPessoa>> LocalicarListaRelacionamentoPessoa(string cpf)
    {
      try
      {
        return _repositoryPessoa.LocalicarListaRelacionamentoPessoa(cpf).Result;
      }
      catch (Exception ex)
      {

        throw ex;
      }

    }

    public async Task<List<DtoGrauRelacionamento>> ListarRelacionamento()
    {
      try
      {
        return _repositoryPessoa.ListarRelacionamento().Result;
      }
      catch (Exception ex)
      {

        throw ex;
      }
    }
    #endregion Listas


    #region Dto
    private async Task<DtoCPF> ValidadorDeCPF(string cpf)
    {
      DtoCPF retorno = new DtoCPF();

      int[] multiplicador1 = new int[9] { 10, 9, 8, 7, 6, 5, 4, 3, 2 };

      int[] multiplicador2 = new int[10] { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };

      string tempCpf;

      string digito;

      int soma;

      int resto;

      cpf = cpf.Trim();

      cpf = cpf.Replace(".", "").Replace("-", "");

      if (cpf.Length != 11)
      {
        retorno.Retorno = "CPF Invalido";
        return retorno;
      }

      tempCpf = cpf.Substring(0, 9);

      soma = 0;

      for (int i = 0; i < 9; i++)
      {
        soma += int.Parse(tempCpf[i].ToString()) * multiplicador1[i];
      }

      resto = soma % 11;

      if (resto < 2)
      {
        resto = 0;
      }
      else
      {
        resto = 11 - resto;
      }

      digito = resto.ToString();

      tempCpf = tempCpf + digito;

      soma = 0;

      for (int i = 0; i < 10; i++)
      {
        soma += int.Parse(tempCpf[i].ToString()) * multiplicador2[i];
      }

      resto = soma % 11;

      if (resto < 2)
      {
        resto = 0;
      }
      else
      {
        resto = 11 - resto;
      }

      digito = digito + resto.ToString();

      var cpfValido = cpf.EndsWith(digito);

      if (cpfValido)
      {
        retorno.Retorno = "CPF Valido";
      }
      else
      {
        retorno.Retorno = "CPF Invalido";
      }

      return retorno;

    }


    #endregion Dto

  }
}
