using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using ObiletApp.Core.Interfaces;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace ObiletApp.Infrastructure.Services
{
    public class DapperQueryService : IDapperQueryService
    {
        private readonly string _connectionString;

        public DapperQueryService(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        private IDbConnection CreateConnection() => new SqlConnection(_connectionString);

        public async Task<IEnumerable<T>> QueryAsync<T>(string sql, object param = null)
        {
            using var connection = CreateConnection();
            return await connection.QueryAsync<T>(sql, param);
        }

        public async Task<IEnumerable<TReturn>> QueryAsync<TFirst, TSecond, TReturn>(string sql, System.Func<TFirst, TSecond, TReturn> map, object param = null, string splitOn = "Id")
        {
            using var connection = CreateConnection();
            return await connection.QueryAsync(sql, map, param, splitOn: splitOn);
        }

        public async Task<T> QueryFirstOrDefaultAsync<T>(string sql, object param = null)
        {
            using var connection = CreateConnection();
            return await connection.QueryFirstOrDefaultAsync<T>(sql, param);
        }

        public async Task<int> ExecuteAsync(string sql, object param = null)
        {
            using var connection = CreateConnection();
            return await connection.ExecuteAsync(sql, param);
        }
    }
}
