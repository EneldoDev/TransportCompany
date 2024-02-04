using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using TransportCompany.Infrastructure.Common.Persistance;

namespace TransportCompany.Api.IntegrationTests.Common
{
    public class TestDatabase : IDisposable
    {
        public SqliteConnection Connection { get; }

        private TestDatabase(string connectionString)
        {
            Connection = new SqliteConnection(connectionString);
        }

        public static TestDatabase CreateAndInitialize()
        {
            var testDb = new TestDatabase("DataSource=:memory:");
            testDb.InitializeTestDb();
            return testDb;
        }

        public void InitializeTestDb()
        {
            Connection.Open();
            var options = new DbContextOptionsBuilder<CompanyDbContext>().UseSqlite(Connection).Options;
            using var context = new CompanyDbContext(options);
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();
        }

        public void ResetDb()
        {
            Connection.Close();
            InitializeTestDb();
        }

        public void Dispose()
        {
            Connection.Close();
        }
    }
}
