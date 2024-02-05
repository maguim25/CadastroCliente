namespace CadastroClienteAPI.Domain.Dto
{
  public class DtoPessoaRelacionamento
  {
    public int CodigoCliente { get; set; }
    public List<DtoPessoa>? PessoaRelacionada { get; set; }
  }
}
