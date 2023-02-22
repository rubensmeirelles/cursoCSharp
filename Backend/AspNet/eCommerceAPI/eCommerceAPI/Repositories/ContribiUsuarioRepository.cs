using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using eCommerceAPI.Models;
using Dapper.Contrib.Extensions;

namespace eCommerceAPI.Repositories
{
    public class ContribiUsuarioRepository : IUsuarioRepository
    {
        private IDbConnection _connection;
        public ContribiUsuarioRepository()
        {
            _connection = new SqlConnection("Data Source=PSEST328;Initial Catalog=eCommerce;Persist Security Info=True;User ID=sa;Password=19752803");
        }
        
        public List<Usuario> Get()
        {
            return _connection.GetAll<Usuario>().ToList();
        }

        public Usuario Get(int id)
        {
            return _connection.Get<Usuario>(id);
        }

        public void Insert(Usuario usuario)
        {
            usuario.Id = Convert.ToInt32(_connection.Insert(usuario));
        }

        public void Update(Usuario usuario)
        {
            _connection.Update(usuario);
        }

        public void Delete(int id)
        {
            _connection.Delete(Get(id));
        }
    }
}