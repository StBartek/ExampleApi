using LinqToDB.Configuration;
using System.Collections.Generic;
using System.Linq;

namespace Database
{
    public class ConnectionStringSettings : IConnectionStringSettings
    {
        public string ConnectionString { get; set; }
        public string Name { get; set; }
        public string ProviderName { get; set; }
        public bool IsGlobal => false;
    }
    public class DbConnectionSettings : ILinqToDBSettings
    {
        public IEnumerable<IDataProviderSettings> DataProviders => Enumerable.Empty<IDataProviderSettings>();

        public string DefaultConfiguration => "SqlServer";
        public string DefaultDataProvider => "SqlServer";

        public IEnumerable<IConnectionStringSettings> ConnectionStrings { get; }
        public DbConnectionSettings(string connectionString)
        {
            ConnectionStrings = new IConnectionStringSettings[] { 
                new ConnectionStringSettings { Name = DefaultConfiguration, ProviderName = DefaultDataProvider, ConnectionString = $@"{connectionString}" } };
        }

        public DbConnectionSettings((string confName, string connString)[] connectionStrings)
        {
            ConnectionStrings = connectionStrings
                .Select(c => new ConnectionStringSettings { Name = c.confName, ProviderName = DefaultDataProvider, ConnectionString = $@"{c.connString}" })
                .ToList();
        }        
    }
}
