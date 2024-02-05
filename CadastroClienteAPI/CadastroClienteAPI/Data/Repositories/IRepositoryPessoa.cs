using CadastroClienteAPI.Domain.Dto;
using CadastroClienteAPI.Domain.ModelView;

namespace CadastroClienteAPI.Data.Repositories
{
  public interface IRepositoryPessoa
  {
    public Task<ModelViewLocalizaPessoa> LocalicarPessoaModelView(string cpf);
    public Task<DtoPessoa> LocalicarPessoa(DtoPessoa pessoa);
    public Task CadastrarTelefonePessoa(DtoPessoa pessoa);

    public Task CadastrarPessoa(DtoPessoa pessoa);
    public Task<DtoPessoa> LocalicarPessoaRelacionamento(DtoPessoa pessoa);

    public Task<List<DtoPessoa>> LocalicarListaRelacionamentoPessoa(string cpf);
    public Task<List<DtoGrauRelacionamento>> ListarRelacionamento();

    public Task<List<DtoTelefone>> AtualizarDadosPessoa(int cdPessoa);

    public Task<List<DtoTelefone>> ListarTelefonePessoa(int cdPessoa);

    public Task<List<DtoTipoTelefone>> ListarTiposTelefones();

  }
}
