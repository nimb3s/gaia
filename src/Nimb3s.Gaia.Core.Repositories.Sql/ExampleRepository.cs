using Dapper;
using Nimb3s.Gaia.Core.Entities;
using Nimb3s.Data.Abstractions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace Nimb3s.Gaia.Core.Repositories.Sql
{
    public class ExampleRepository : Repository<ExampleEntity, Guid>
    {
        public override string Schema => "[Example]";

        public ExampleRepository(UnitOfWork unitOfWork)
            :base(unitOfWork)
        {

        }

        public async Task<IEnumerable<ExampleEntity>> GetAllByJobIdAndStatusAsync(Guid jobId, short workItemStatusTypeId)
        {
            DynamicParameters dp = new DynamicParameters();

            dp.Add(nameof(jobId), jobId);
            dp.Add(nameof(workItemStatusTypeId), workItemStatusTypeId);

            return (await connection
                .QueryAsync<ExampleEntity>(sql: $"{Schema}.p_GetAll{entityName}sByExampleIdAndStatus", param: dp, commandType: CommandType.StoredProcedure, transaction: transaction)
                )
                .AsList();
        }
    }
}
