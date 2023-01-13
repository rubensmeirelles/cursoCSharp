using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using eCommerceAPI.Models;
using Dapper;

namespace eCommerceAPI.Repositories
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private IDbConnection _connection;
        public UsuarioRepository()
        {
            _connection = new SqlConnection("Data Source=PSEST328;Initial Catalog=eCommerce;Persist Security Info=True;User ID=sa;Password=19752803");
        }
        
        public List<Usuario> Get()
        {
            //return _connection.Query<Usuario>("SELECT * FROM Usuarios").ToList();
            List<Usuario> usuarios = new List<Usuario>();
            
           string sql = "SELECT U.*, C.*, EE.*, D.* FROM Usuarios as U " +
                          "LEFT JOIN Contatos C on C.UsuarioId = U.Id " +
                          "LEFT JOIN EnderecosEntrega as EE on EE.UsuarioId = U.Id " +
                          "LEFT JOIN UsuariosDepartamentos UD ON UD.UsuarioId = U.Id " +
                          "LEFT JOIN Departamentos D ON D.Id = UD.DepartamentoId " ;
           
            _connection.Query<Usuario, Contato, EnderecoEntrega, Departamento, Usuario>(sql,
                (usuario, contato, enderecoEntrega, departamento) =>
                {

                    if (usuarios.SingleOrDefault(a => a.Id == usuario.Id) == null)
                    {
                        usuario.Departamentos = new List<Departamento>();
                        usuario.EnderecosEntrega = new List<EnderecoEntrega>();
                        usuario.Contato = contato;
                        usuarios.Add(usuario);
                    }
                    else
                    {
                        usuario = usuarios.SingleOrDefault(a => a.Id == usuario.Id);
                    }
                    
                    //verificação do endereço de entrega
                    if (usuario.EnderecosEntrega.SingleOrDefault(a => a.Id == enderecoEntrega.Id) == null)
                    {
                        usuario.EnderecosEntrega.Add(enderecoEntrega);
                    }
                    
                    //verificação do departamento
                    if (usuario.Departamentos.SingleOrDefault(a => a.Id == departamento.Id) == null)
                    {
                        usuario.Departamentos.Add(departamento);
                    }

                    return usuario;
                });

            return usuarios;
        }

        public Usuario Get(int id)
        {
            List<Usuario> usuarios = new List<Usuario>();
            
            string sql = "SELECT U.*, C.*, EE.*, D.* FROM Usuarios as U " +
                         "LEFT JOIN Contatos C on C.UsuarioId = U.Id " +
                         "LEFT JOIN EnderecosEntrega as EE on EE.UsuarioId = U.Id " +
                         "LEFT JOIN UsuariosDepartamentos UD ON UD.UsuarioId = U.Id " +
                         "LEFT JOIN Departamentos D ON D.Id = UD.DepartamentoId " +
                         "WHERE U.Id = @Id";

            _connection.Query<Usuario, Contato, EnderecoEntrega, Departamento, Usuario>(sql,
                (usuario, contato, enderecoEntrega, departamento) =>
                {

                    if (usuarios.SingleOrDefault(a => a.Id == usuario.Id) == null)
                    {
                        usuario.Departamentos = new List<Departamento>();
                        usuario.EnderecosEntrega = new List<EnderecoEntrega>();
                        usuario.Contato = contato;
                        usuarios.Add(usuario);
                    }
                    else
                    {
                        usuario = usuarios.SingleOrDefault(a => a.Id == usuario.Id);
                    }

                    //verificação do endereço de entrega
                    if (usuario.EnderecosEntrega.SingleOrDefault(a => a.Id == enderecoEntrega.Id) == null)
                    {
                        usuario.EnderecosEntrega.Add(enderecoEntrega);
                    }
                    
                    //verificação do departamento
                    if (usuario.Departamentos.SingleOrDefault(a => a.Id == departamento.Id) == null)
                    {
                        usuario.Departamentos.Add(departamento);
                    }
                   
                   return usuario;
                }, new { Id = id });

            return usuarios.SingleOrDefault();
        }

        public void Insert(Usuario usuario)
        {
            _connection.Open();
            var transaction = _connection.BeginTransaction();

            try
            {
                string sql =
                    "INSERT INTO Usuarios (Nome, Email, Sexo, RG, CPF, NomeMae, SituacaoCadastro, DataCadastro) " +
                    "VALUES (@Nome, @Email, @Sexo, @RG, @CPF, @NomeMae, @SituacaoCadastro, @DataCadastro); " +
                    "SELECT CAST(SCOPE_IDENTITY() AS INT);";
                usuario.Id = _connection.Query<int>(sql, usuario, transaction).Single();

                if (usuario.Contato != null)
                {
                    usuario.Contato.UsuarioId = usuario.Id;
                    string sqlContato = "INSERT INTO Contatos (UsuarioID, Telefone, Celular) " +
                                        "VALUES (@UsuarioID, @Telefone, @Celular); " +
                                        "SELECT CAST(SCOPE_IDENTITY() AS INT);";
                    usuario.Contato.Id = _connection.Query<int>(sqlContato, usuario.Contato, transaction).Single();
                }
                
                if(usuario.EnderecosEntrega !=null && usuario.EnderecosEntrega.Count > 0)
                {
                    foreach (var enderecoEntrega in usuario.EnderecosEntrega)
                    {
                        enderecoEntrega.UsuarioId = usuario.Id;
                        string sqlEndereco = "INSERT INTO EnderecosEntrega " +
                                             "(UsuarioId, " +
                                             "NomeEndereco, " +
                                             "CEP, " +
                                             "Estado, " +
                                             "Cidade, " +
                                             "Bairro, " +
                                             "Endereco, " +
                                             "Numero, " +
                                             "Complemento) " +
                                             "VALUES " +
                                             "(@UsuarioId, " +
                                             "@NomeEndereco, " +
                                             "@CEP, " +
                                             "@Estado, " +
                                             "@Cidade, " +
                                             "@Bairro, " +
                                             "@Endereco, " +
                                             "@Numero, " +
                                             "@Complemento); " +
                                             "SELECT CAST(SCOPE_IDENTITY() AS INT)";
                        
                        enderecoEntrega.Id = _connection.Query<int>(sqlEndereco, enderecoEntrega, transaction).Single();
                    }
                }
                
                if(usuario.Departamentos !=null && usuario.Departamentos.Count > 0)
                {
                    foreach (var departamento in usuario.Departamentos)
                    {
                        string sqlUsuariosDepartamentos = "INSERT INTO UsuariosDepartamentos " +
                                             "(UsuarioId, " +
                                             "DepartamentoId) " +
                                             "VALUES " +
                                             "(@UsuarioId, " +
                                             "@DepartamentoId) ";
                                             
                        
                        _connection.Execute(sqlUsuariosDepartamentos, new 
                            { UsuarioId = usuario.Id, DepartamentoId =  departamento.Id}, transaction);
                    }
                }

                transaction.Commit();
            }
            catch (Exception)
            {
                try
                {
                    transaction.Rollback();
                }
                catch (Exception)
                {
                    
                }
            }
            finally
            {
                _connection.Close();
            }
        }

        public void Update(Usuario usuario)
        {
            _connection.Open();
            var transaction = _connection.BeginTransaction();

            try
            {
                string sql =
                    "UPDATE Usuarios SET Nome = @Nome, Email = @Email, Sexo = @Sexo, RG = @RG, CPF = @CPF, NomeMae = @NomeMae, SituacaoCadastro = @SituacaoCadastro, DataCadastro = @DataCadastro " +
                    "WHERE Id = @Id";
                _connection.Execute(sql, usuario, transaction);

                if (usuario.Contato != null)
                {
                    string sqlContato =
                        "UPDATE Contatos SET UsuarioId = @UsuarioId, Telefone = @Telefone, Celular = @Celular " +
                        "WHERE Id = @Id ";
                    _connection.Execute(sqlContato, usuario.Contato, transaction);
                }

                string sqlDeletarEnderecosEntrega = "DELETE FROM EnderecosEntrega WHERE UsuarioId = @Id";
                _connection.Execute(sqlDeletarEnderecosEntrega, usuario, transaction);

                if (usuario.EnderecosEntrega != null && usuario.EnderecosEntrega.Count > 0)
                {
                    foreach (var enderecoEntrega in usuario.EnderecosEntrega)
                    {
                        enderecoEntrega.UsuarioId = usuario.Id;
                        string sqlEndereco = "INSERT INTO EnderecosEntrega " +
                                             "(UsuarioId, " +
                                             "NomeEndereco, " +
                                             "CEP, " +
                                             "Estado, " +
                                             "Cidade, " +
                                             "Bairro, " +
                                             "Endereco, " +
                                             "Numero, " +
                                             "Complemento) " +
                                             "VALUES " +
                                             "(@UsuarioId, " +
                                             "@NomeEndereco, " +
                                             "@CEP, " +
                                             "@Estado, " +
                                             "@Cidade, " +
                                             "@Bairro, " +
                                             "@Endereco, " +
                                             "@Numero, " +
                                             "@Complemento); " +
                                             "SELECT CAST(SCOPE_IDENTITY() AS INT)";

                        enderecoEntrega.Id = _connection.Query<int>(sqlEndereco, enderecoEntrega, transaction).Single();
                    }
                }

                ;

                string sqlDeletarUsuariosDepartamentos = "DELETE FROM UsuariosDepartamentos WHERE UsuarioId = @Id";
                _connection.Execute(sqlDeletarUsuariosDepartamentos, usuario, transaction);

                if (usuario.Departamentos != null && usuario.Departamentos.Count > 0)
                {
                    foreach (var departamento in usuario.Departamentos)
                    {
                        string sqlUsuariosDepartamentos = "INSERT INTO UsuariosDepartamentos " +
                                                          "(UsuarioId, " +
                                                          "DepartamentoId) " +
                                                          "VALUES " +
                                                          "(@UsuarioId, " +
                                                          "@DepartamentoId) ";


                        _connection.Execute(sqlUsuariosDepartamentos, new
                            { UsuarioId = usuario.Id, DepartamentoId = departamento.Id }, transaction);
                    }
                }

                transaction.Commit();
            }
            catch (Exception)
            {
                try
                {
                    transaction.Rollback();
                }
                catch (Exception)
                {
                }
            }
            finally
            {
                _connection.Close();
            }
        }

        public void Delete(int id)
        {
            _connection.Execute("DELETE FROM Usuarios WHERE Id = @Id", new { Id = id });
        }
    }
}