using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Dapper;
using GrafanaAlerts.MnemonicForm.DTO;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;

namespace GrafanaAlerts.MnemonicForm.Repositories
{
    public class MnemonicsRepository : IMnemonicsRepository
    {
        private readonly string _connectionString;
        
        public MnemonicsRepository(IConfiguration configuration)
        {
            _connectionString = configuration["ConnectionString"];
        }
        
        public List<AppDTO> LoadKe()
        {
            using var connection = new SqlConnection(_connectionString);
            var kes = connection.Query<AppDTO>("select * from dbo.V_Grafana_APP");
            
            return kes.ToList();
        }

        public List<RoleDTO> LoadRoles()
        {
            using var connection = new SqlConnection(_connectionString);
            var kes = connection.Query<RoleDTO>("select * from dbo.V_Grafana_OS");
            
            return kes.ToList();
        }

        public List<string> LoadPriorities()
        {
            return new List<string>()
            {
                "1",
                "2",
                "3",
                "4",
                "5",
                "6"
            };
        }

        public List<string> LoadInitiatorTypes()
        {
            return new List<string>()
            {
                "Кожанный мешок",
                "Организация"
            };
        }
    }
}