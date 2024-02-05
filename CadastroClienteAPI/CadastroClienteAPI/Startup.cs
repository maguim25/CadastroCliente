using CadastroClienteAPI.Data;
using CadastroClienteAPI.Data.Repositories;
using CadastroClienteAPI.Domain;
using CadastroClienteAPI.Domain.Interface;
using CadastroClienteAPI.Domain.Manager;

namespace CadastroClienteAPI
{
  public class Startup
  {

    public Startup(IConfiguration configuration)
    {
      Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    public void ConfigureServices(IServiceCollection services)
    {
      // Add services to the container.

      services.AddControllers();
      services.AddEndpointsApiExplorer();
      services.AddSwaggerGen();


      services.Configure<Manager>(options => Configuration.GetSection("Manager").Bind(options));

      services.AddScoped<Context>();
      services.AddTransient<IPessoa, Pessoa>();

      services.AddTransient<IRepositoryPessoa, PessoaInfrastruture>();

    }

    public void Configure(WebApplication app, IWebHostEnvironment env)
    {

      if (app.Environment.IsDevelopment())
      {
        app.UseSwagger();
        app.UseSwaggerUI();
      }

      app.UseCors(builder =>
      {
        builder.AllowAnyOrigin();
        builder.AllowAnyMethod();
        builder.AllowAnyHeader();
      });

      app.UseHttpsRedirection();

      app.UseAuthorization();

      app.MapControllers();

    }

  }
}
