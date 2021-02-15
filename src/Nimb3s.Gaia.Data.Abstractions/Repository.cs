using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nimb3s.Data.Abstractions
{
    public static class Extensions
    {
        public static Dictionary<string, object> ToKeyValuePair<TEntity, TKey>(this IEntity<TKey> obj)
        {
            Dictionary<string, object> paramCollection = new Dictionary<string, object>();

            var properties = typeof(TEntity).GetProperties();

            foreach (var property in properties)
            {
                if (property.GetSetMethod() == null)
                    continue;

                var name = property.Name;
                var value = typeof(TEntity).GetProperty(property.Name).GetValue(obj, null);

                paramCollection.Add(name, value);

            }

            return paramCollection;
        }

        public static Dictionary<string, object> ToPrimaryKey<TEntity, TKey>(this IEntity<TKey> obj, string[] primaryKeyNames)
        {
            Dictionary<string, object> paramCollection = new Dictionary<string, object>();

            var properties = typeof(TEntity).GetProperties();

            foreach (var primaryKeyName in primaryKeyNames)
            {
                if (properties.Any(i => i.Name.Equals(primaryKeyName, StringComparison.OrdinalIgnoreCase)))
                {
                    var value = typeof(TEntity).GetProperty(primaryKeyName).GetValue(obj, null);

                    paramCollection.Add(primaryKeyName, value);
                }
            }

            return null;
        }
    }


    public abstract class Repository<TEntity, TKey> : IRepository<TEntity, TKey> 
        where TEntity: IEntity<TKey>
    {
        private readonly string[] validEntityKeyNames = { "id" };
        protected readonly string entityName = typeof(TEntity).Name.Replace("Entity", string.Empty);

        protected readonly IDbConnection connection;
        protected readonly IDbTransaction transaction;

        public virtual string Schema => "dbo";

        public Repository(UnitOfWork unitOfWork)
        {
            connection = unitOfWork.Transaction.Connection;
            transaction = unitOfWork.Transaction;
        }

        public async Task<TEntity> GetAsync(TKey Id)
        {
            DynamicParameters dp = new DynamicParameters();

            dp.Add(nameof(Id), Id);

            return await connection
                .QuerySingleAsync<TEntity>(sql: $"{Schema}.p_Get{entityName}", param: dp, commandType: CommandType.StoredProcedure, transaction: transaction)
                ;
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            return (await connection
                .QueryAsync<TEntity>(sql: $"{Schema}.p_GetAll{entityName}", commandType: CommandType.StoredProcedure, transaction: transaction)
                ).AsList();
        }

        public async Task UpsertAsync(TEntity entity)
        {
            var @params = entity.ToKeyValuePair<TEntity, TKey>();

            DynamicParameters dp = new DynamicParameters();

            foreach (var param in @params)
            {
                dp.Add(param.Key, param.Value);
            }

            await connection.ExecuteAsync(sql: $"{Schema}.p_Upsert{entityName}", param: dp, commandType: CommandType.StoredProcedure, transaction: transaction);
        }

        public async Task DeleteAsync(TEntity entity)
        {
            var id = entity.ToPrimaryKey<TEntity, TKey>(validEntityKeyNames);

            if(id == null)
            {
                throw new ArgumentNullException("", $"{nameof(TEntity)} does not contain a valid primary key name. List of valid primary key names: {string.Join(",", validEntityKeyNames)}");
            }

            DynamicParameters dp = new DynamicParameters();

            foreach (var param in id)
            {
                dp.Add(id.Keys.First(), id.Values.First());
            }

            await connection.ExecuteAsync(sql: $"{Schema}.p_Delete{entityName}", param: dp, commandType: CommandType.StoredProcedure, transaction: transaction);
        }
    }
}
