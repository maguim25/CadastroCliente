using CadastroClienteAPI.Domain.Dto;

namespace CadastroClienteAPI.Domain.ModelView
{
  public class ModelViewResponsePessoa
  {
    public string? Retorno { get; set; }
    public DtoPessoa? Pessoa { get; set; }
  }
}
