using Nimb3s.Data.Abstractions;
using System.Data.SqlClient;

namespace Nimb3s.Gaia.Core.Repositories.Sql
{
    public class GaiaDatabaseContext : DbContext
    {
        private ExampleRepository exampleRepository;

        public ExampleRepository HttpRequestRepository =>
                exampleRepository ?? (exampleRepository = new ExampleRepository(UnitOfWork));

        public GaiaDatabaseContext()
            :base(new UnitOfWorkFactory<SqlConnection>(@"Data Source=.\sqlexpress;Initial Catalog=Gaia;Integrated Security=true"))
        {

        }


        public GaiaDatabaseContext(IUnitOfWorkFactory unitOfWorkFactory)
            :base(unitOfWorkFactory)
        {

        }
    }
}
