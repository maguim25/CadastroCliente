using CadastroClienteAPI.Domain.Dto;
using CadastroClienteAPI.Domain.ModelView;
using Dapper;

namespace CadastroClienteAPI.Data.Repositories
{
  public class PessoaInfrastruture : IRepositoryPessoa
  {
    private readonly Context _context;
    public PessoaInfrastruture(Context context)
    {
      _context = context;

    }

    public async Task CadastrarTelefonePessoa(DtoPessoa pessoa)
    {
      using (var con = _context.Connection().Result)
      {
        con.Execute("INSERT INTO [TELEFONE_PESSOA]([CD_PESSOA],[DDD],[TELEFONE],[NR_TIPO_TELEFONE])VALUES(@CDPESSOAVALUE,@DDDVALUE,@TELEFONEVALUE,@NRTIPOTELEFONEENUMVALUE)", new
        {
          CDPESSOAVALUE = pessoa.Codigo,
          DDDVALUE = pessoa.DDD,
          TELEFONEVALUE = pessoa.Telefone,
          NRTIPOTELEFONEENUMVALUE = pessoa.TipoTelefone
        });

      }
    }

    public async Task<DtoPessoa> LocalicarPessoa(DtoPessoa pessoa)
    {
      var pessoaR = new DtoPessoa();
      using (var con = _context.Connection().Result)
      {
        pessoaR = con.QueryFirstOrDefault<DtoPessoa>(@"SELECT top 1
                                                        [CD_PESSOA] as Codigo
                                                          ,[NM_PESSOA] as Nome
                                                          ,[NM_SOBRENOME] as Sobrenome
                                                          ,[NR_CPF] as CPF
                                                          ,[EMAIL] as Email
                                                          ,[FL_ATIVO] as Ativo
                                                          ,[DT_INSERCAO] as DtInsercao
                                                FROM [PESSOA]
                                                where NR_CPF = @CPFValue", new { CPFValue = pessoa.CPF });

        if (pessoaR.CPF == "")
        {
          pessoaR = con.QueryFirstOrDefault<DtoPessoa>(@"SELECT top 1
                                                      [CD_PESSOA] as Codigo
                                                      ,[NM_PESSOA] as Nome
                                                      ,[NM_SOBRENOME] as Sobrenome
                                                      ,[NR_CPF] as CPF
                                                      ,[EMAIL] as Email
                                                      ,[FL_ATIVO] as Ativo
                                                      ,[DT_INSERCAO] as DtInsercao
                                                      FROM [PESSOA]
                                                      where [NM_PESSOA] = @NomeValue ", new { NomeValue = pessoa.Nome });
        }


      }
      return pessoaR;
    }

    public async Task<ModelViewLocalizaPessoa> LocalicarPessoaModelView(string cpf)
    {
      var localizandoPessoa = new ModelViewLocalizaPessoa();
      using (var con = _context.Connection().Result)
      {
        localizandoPessoa.Pessoa = con.QueryFirstOrDefault<DtoPessoa>(@"SELECT top 1
                                                        [CD_PESSOA] as Codigo
                                                          ,[NM_PESSOA] as Nome
                                                          ,[NM_SOBRENOME] as Sobrenome
                                                          ,[NR_CPF] as CPF
                                                          ,[EMAIL] as Email
                                                          ,[FL_ATIVO] as Ativo
                                                          ,[DT_INSERCAO] as DtInsercao
                                                FROM [PESSOA]
                                                where NR_CPF = @CPFValue", new { CPFValue = cpf });
        if (localizandoPessoa != null)
        {
          localizandoPessoa.Retorno = "Pessoa Localizada";
        }
        else
        {
          localizandoPessoa.Retorno = "Pessoa nao Localizada";
        }
      }
      return localizandoPessoa;
    }

    public async Task<DtoPessoa> LocalicarPessoaRelacionamento(DtoPessoa pessoa)
    {
      var pessoaR = new DtoPessoa();
      using (var con = _context.Connection().Result)
      {
        if (pessoa.CPF != "")
        {

          pessoaR = con.QueryFirstOrDefault<DtoPessoa>(@"SELECT top 1
                                                          [CD_PESSOA] as Codigo
                                                            ,[NM_PESSOA] as Nome
                                                            ,[NM_SOBRENOME] as Sobrenome
                                                            ,[NR_CPF] as CPF
                                                            ,[EMAIL] as Email
                                                            ,[FL_ATIVO] as Ativo
                                                            ,[DT_INSERCAO] as DtInsercao
                                                  FROM [PESSOA]
                                                  where NR_CPF = @CPFValue", new { CPFValue = pessoa.CPF });
        }

        pessoaR = con.QueryFirstOrDefault<DtoPessoa>(@"SELECT top 1
                                                      [CD_PESSOA] as Codigo
                                                      ,[NM_PESSOA] as Nome
                                                      ,[NM_SOBRENOME] as Sobrenome
                                                      ,[NR_CPF] as CPF
                                                      ,[EMAIL] as Email
                                                      ,[FL_ATIVO] as Ativo
                                                      ,[DT_INSERCAO] as DtInsercao
                                                      FROM [PESSOA]
                                                      where [NM_PESSOA] = @NomeValue ", new { NomeValue = pessoa.Nome });

        if (pessoaR != null)
        {
          var telefonePessoa = con.Query<DtoTelefone>(@"Select 
                                                        CD_PESSOA as CodigoPessoa
                                                        ,DDD as DDD
                                                        ,TELEFONE as NumeroTelefone
                                                        ,NR_TIPO_TELEFONE  as TipoTelefoneEnum
                                                        from 
	                                                     TELEFONE_PESSOA where CD_PESSOA = @CDPESSOAValue", new { CDPESSOAValue = pessoa.Codigo }).ToList();

          if (telefonePessoa.Count > 0)
          {
            pessoaR.DDD = telefonePessoa.FirstOrDefault().DDD.ToString();
            pessoaR.Telefone = telefonePessoa.FirstOrDefault().NumeroTelefone;
            var tt = con.QueryFirst<DtoGrauRelacionamento>(@"").NrTipoGrauRelacionamento;
          }
          //pessoaR.TipoRelacionamento = 
        }


      }
      return pessoaR;
    }

    public async Task CadastrarPessoa(DtoPessoa pessoa)
    {
      //se nao localizar pessoa sera um cadastro novo de cliente
      using (var con = _context.Connection().Result)
      {
        con.Execute("insert into PESSOA([NM_PESSOA],[NM_SOBRENOME],[NR_CPF],[EMAIL]) values (@nome, @sobrenome, @cpf, @email)", new
        {
          nome = pessoa.Nome,
          sobrenome = pessoa.Sobrenome,
          cpf = pessoa.CPF,
          email = pessoa.Email
        });
      }
    }

    public async Task<List<DtoPessoa>> LocalicarListaRelacionamentoPessoa(string cpf)
    {

      using (var con = _context.Connection().Result)
      {
        return con.Query<DtoPessoa>(@"SELECT 
                                          RP.CD_PESSOA_RELACIONAMENTO AS Codigo
	                                      ,PR.NM_PESSOA AS Nome
	                                      ,PR.NM_SOBRENOME AS Sobrenome
	                                      ,PR.NR_CPF AS CPF
	                                      ,TP.DDD AS DDD
	                                      ,TP.TELEFONE AS Telefone
	                                      ,TT.NR_TIPO_TELEFONE AS TipoTelefone
                                      FROM 
                                      RELACIONAMENTO_PESSOA	    RP
                                      JOIN PESSOA				P	ON RP.CD_PESSOA					= P.CD_PESSOA 
                                      JOIN PESSOA				PR	ON RP.CD_PESSOA_RELACIONAMENTO  = PR.CD_PESSOA
                                      JOIN TELEFONE_PESSOA		TP	ON TP.CD_PESSOA					= RP.CD_PESSOA_RELACIONAMENTO
                                      JOIN TIPO_TELEFONE		TT	ON TT.NR_TIPO_TELEFONE			= TP.NR_TIPO_TELEFONE
                                      WHERE
                                      P.NR_CPF = @CPFValue", new { CPFValue = cpf }).OrderBy(x => x.Codigo).ToList();

      }
    }

    public async Task<List<DtoGrauRelacionamento>> ListarRelacionamento()
    {
      using (var con = _context.Connection().Result)
      {
        return con.Query<DtoGrauRelacionamento>(@"SELECT
                                                    [NR_TIPO_RELACIONAMENTO_PESSOA] as NrTipoGrauRelacionamento
                                                    ,[NM_TIPO_RELACIONAMENTO_PESSOA] as GrauRelacionamento
                                                  FROM 
	                                                [TIPO_RELACIONAMENTO_PESSOA]").ToList();
      }
    }

    public async Task<List<DtoTelefone>> AtualizarDadosPessoa(int cdPessoa)
    {
      using (var con = _context.Connection().Result)
      {
        return con.Query<DtoTelefone>(@"Select 
                                        CD_PESSOA as CodigoPessoa
                                        ,DDD as DDD
                                        ,TELEFONE as NumeroTelefone
                                        ,NR_TIPO_TELEFONE  as TipoTelefoneEnum
                                        from 
	                                        TELEFONE_PESSOA where CD_PESSOA = @CDPESSOAValue", new { CDPESSOAValue = cdPessoa }).ToList();

      }
    }

    public async Task<List<DtoTelefone>> ListarTelefonePessoa(int cdPessoa)
    {
      using (var con = _context.Connection().Result)
      {
        return con.Query<DtoTelefone>(@"Select 
                                        CD_PESSOA as CodigoPessoa
                                        ,DDD as DDD
                                        ,TELEFONE as NumeroTelefone
                                        ,NR_TIPO_TELEFONE  as TipoTelefoneEnum
                                        from 
	                                        TELEFONE_PESSOA where CD_PESSOA = @CDPESSOAValue", new { CDPESSOAValue = cdPessoa }).ToList();

      }
    }

    public async Task<List<DtoTipoTelefone>> ListarTiposTelefones()
    {
      using (var con = _context.Connection().Result)
      {
        return con.Query<DtoTipoTelefone>(@"SELECT
                                              [NR_TIPO_TELEFONE] as NrTipoTelefone
                                              ,[NM_TIPO_TELEFONE] as TipoTelefone
                                           FROM[TIPO_TELEFONE]").ToList();

      }
    }
  }
}
