using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using eCommerceAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace eCommerceAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TipsController : ControllerBase
    {
        private IDbConnection _connection;
        public TipsController()
        {
            _connection = new SqlConnection("Data Source=PSEST328;Initial Catalog=eCommerce;Persist Security Info=True;User ID=sa;Password=19752803");
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            string sql = "SELECT * FROM Usuarios Where Id = @Id;" +
                         "SELECT * FROM Contatos WHERE UsuarioId = @Id;" +
                         "SELECT * FROM EnderecosEntrega WHERE UsuarioId = @Id;" +
                         "SELECT D.* FROM UsuariosDepartamentos UD INNER JOIN Departamentos D ON UD.DepartamentoId = D.Id WHERE UD.UsuarioId = @Id;";

            using (var multipleResultSets = _connection.QueryMultiple(sql, new { Id = id }))
            {
               var usuario = multipleResultSets.Read<Usuario>().SingleOrDefault();
               var contato = multipleResultSets.Read<Contato>().SingleOrDefault();
               var enderecos = multipleResultSets.Read<EnderecoEntrega>().ToList();
               var departamentos = multipleResultSets.Read<Departamento>().ToList();

               if (usuario != null)
               {
                   usuario.Contato = contato;
                   usuario.EnderecosEntrega = enderecos;
                   usuario.Departamentos = departamentos;

                   return Ok(usuario);
               }
            }

            return NotFound();
        }
        
        //stored procedures
        
        [HttpPost("stored/usuarios")]
        public IActionResult StoredInsertUser()
        {
            var usuarios = _connection.Query<Usuario>("CadastrarUsuario", commandType: CommandType.StoredProcedure);
            
        }
        [HttpGet("stored/usuarios")]
        public IActionResult StoredGetAllUsers()
        {
            var usuarios = _connection.Query<Usuario>("SelecionarUsuarios", commandType: CommandType.StoredProcedure);
            return Ok(usuarios);
        }
        
        [HttpGet("stored/usuarios/{id}")]
        public IActionResult StoredGetAUser(int id)
        {
            var usuarios = _connection.Query<Usuario>("SelecionarUsuario", new { Id = id }, commandType: CommandType.StoredProcedure);
            return Ok(usuarios);
        }
        
        [HttpDelete("stored/usuarios/{id}")]
        public IActionResult StoredDeleteAUser(int id)
        {
            var usuarios = _connection.Query<Usuario>("DeletarUsuario", new { Id = id}, commandType: CommandType.StoredProcedure);
            return Ok(usuarios);
        }
    }
}