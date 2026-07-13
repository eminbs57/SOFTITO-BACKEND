using System.Collections.Generic;
using System.Threading.Tasks;

namespace ObiletApp.Core.Interfaces
{

    public interface IDapperQueryService
    {
        Task<IEnumerable<T>> QueryAsync<T>(string sql, object param = null);
        Task<IEnumerable<TReturn>> QueryAsync<TFirst, TSecond, TReturn>(string sql, System.Func<TFirst, TSecond, TReturn> map, object param = null, string splitOn = "Id");
        Task<T> QueryFirstOrDefaultAsync<T>(string sql, object param = null);
        Task<int> ExecuteAsync(string sql, object param = null);
    }
}
