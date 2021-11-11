using System.Collections.Generic;
using System.Linq;
using Dapper;
using GrafanaAlerts.MnemonicForm.DTO;
using Microsoft.AspNetCore.Server.Kestrel.Core;
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
        
        public List<string> LoadKe()
        {
            using var connection = new MySqlConnection(_connectionString);
            var kes = connection.Query<string>("select which returns all ke");
            //return kes.ToList();

            return new List<string>()
            {
                "Playstation 4",
                "xBox 360",
                "Playstation 5"
            };
        }

        public List<string> LoadRoles()
        {
            using var connection = new MySqlConnection(_connectionString);
            var kes = connection.Query<string>("select which returns all roles");
            //return kes.ToList();
            
            return new List<string>()
            {
                "Работничек",
                "Манагер",
                "Директор"
            };
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