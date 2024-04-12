using System;
using Dapper;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;


using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
namespace Langua.DAL
{
    public class SqlDataAccess : ISqlDataAccess
    {
        private readonly string _connectionString ;
        public SqlDataAccess(string Connection)
        {
            _connectionString = Connection;
        }
        async Task<List<T>> ISqlDataAccess.LoadData<T, U>(string sp_cmd, U parameters)
        {
            using(var connection = new SqlConnection(_connectionString))
            {
                var result = await connection.QueryAsync<T>(sp_cmd, parameters);
                return result.ToList();
            }
        }

        Task<T> ISqlDataAccess.LoadDataById<T, U>(string storedProcedure, U parameters)
        {
            throw new NotImplementedException();
        }

        Task ISqlDataAccess.SaveData<T>(string sp_cmd, T parameters)
        {
            throw new NotImplementedException();
        }
    }
}
