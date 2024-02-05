using CadastroClienteAPI.Domain.Manager;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace CadastroClienteAPI.Data
{
  public class Context
  {
    private readonly Manager _manager;
    private SqlConnection con = null;
    public Context(IOptions<Manager> manager)
    {
       _manager = manager.Value;
    }

    public async Task<SqlConnection> Connection()
    {
      con = new SqlConnection(_manager.ConnectionString);

      try
      {
        con.Open();
      }
      catch (Exception ex)
      {
        Dispose();
      }
      return con;
    }

    public async void Dispose()
    {
      con.Close();
      con.Dispose();
    }



  }
}
