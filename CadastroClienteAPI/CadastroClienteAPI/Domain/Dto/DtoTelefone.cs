using CadastroClienteAPI.Domain.Enum;

namespace CadastroClienteAPI.Domain.Dto
{
  public class DtoTelefone
  {
    public int CodigoPessoa { get; set; }
    public int DDD { get; set; }
    public string NumeroTelefone { get; set; }
    public TipoTelefoneEnum TipoTelefoneEnum { get; set; }
  }
}
