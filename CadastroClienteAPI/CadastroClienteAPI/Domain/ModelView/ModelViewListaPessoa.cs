using CadastroClienteAPI.Domain.Dto;

namespace CadastroClienteAPI.Domain.ModelView
{
  public class ModelViewListaPessoa
  {
    public string? Retorno { get; set; }
    public List<DtoPessoa>? Pessoas { get; set; }
  }
}
