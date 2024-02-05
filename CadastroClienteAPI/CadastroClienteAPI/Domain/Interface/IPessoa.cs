using CadastroClienteAPI.Domain.Dto;
using CadastroClienteAPI.Domain.ModelView;

namespace CadastroClienteAPI.Domain.Interface
{
  public interface IPessoa
  {
    public Task<ModelViewResponsePessoa> CadastroPessoa(DtoPessoa pessoa);
    public Task<ModelViewResponsePessoa> CadastroPessoaRelacionamento(DtoPessoaRelacionamento pessoaR);
    public Task<List<DtoTipoTelefone>> ListarTiposTelefones();
    public Task<List<DtoPessoa>> LocalicarListaRelacionamentoPessoa(string cpf);
    public Task<List<DtoGrauRelacionamento>> ListarRelacionamento();
  }
}
