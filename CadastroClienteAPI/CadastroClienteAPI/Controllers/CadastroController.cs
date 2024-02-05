using CadastroClienteAPI.Domain;
using CadastroClienteAPI.Domain.Dto;
using CadastroClienteAPI.Domain.Interface;
using Microsoft.AspNetCore.Mvc;

namespace CadastroClienteAPI.Controllers
{
  [ApiController]
  [Route("[controller]")]
  public class CadastroController : Controller
  {
    private readonly IPessoa _pessoa;
    public CadastroController(IPessoa pessoa)
    {
      _pessoa = pessoa;
    }

    [HttpPost("cliente")]
    public async Task<IActionResult> Cliente([FromBody] DtoPessoa pessoa)
    {
      return Ok(_pessoa.CadastroPessoa(pessoa).Result);
    }

    [HttpPost("listarTipoTelefone")]
    public IActionResult TipoTelefone()
    {
      return Ok(_pessoa.ListarTiposTelefones().Result);
    }

  }
}
