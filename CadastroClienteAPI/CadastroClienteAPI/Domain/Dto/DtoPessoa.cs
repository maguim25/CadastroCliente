using CadastroClienteAPI.Domain.Enum;

namespace CadastroClienteAPI.Domain.Dto
{
  public class DtoPessoa
  {
    public int Codigo { get; set; }
    public string? Nome { get; set; }
    public string? Sobrenome { get; set; }
    public string? CPF { get; set; }
    public string? Email { get; set; }
    public string? DDD { get; set; }
    public string? Telefone { get; set; }
    public int TipoTelefone { get; set; }
    public bool isRelacionamento { get; set; }
    public bool Ativo { get; set; }
    public TipoRelacionamentoClienteEnum TipoRelacionamento { get; set; }
    public DateTime DtInsercao { get; set; }

  }
}
