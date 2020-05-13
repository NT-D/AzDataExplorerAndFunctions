using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Kusto.Data.Common;

namespace CseSample.Services
{
    public class TestTableService : ITestTableService
    {
        private string _database = Environment.GetEnvironmentVariable("TestDb");
        private string _table = Environment.GetEnvironmentVariable("TestTable");
        private ICslQueryProvider _kustoClient;
        public TestTableService(ICslQueryProvider kustoClient)
        {
            _kustoClient = kustoClient;
        }

        public async Task<IEnumerable<TestTable>> GetLatestNDaysData(int days)
        {
            if (days <= 0) throw new ArgumentOutOfRangeException("days should be positive value");

            DateTime today = DateTime.UtcNow;
            DateTime startPeriodDateTime = today.AddDays(-days);
            string query = $"{_table} | where TimeStamp between (datetime({startPeriodDateTime.ToString("s")}) .. datetime({today.ToString("s")}))";

            try
            {
                var result = new List<TestTable>();
                using (var reader = await _kustoClient.ExecuteQueryAsync(_database, query, null) as DataTableReader2)
                {
                    // Can be better performance with IAsyncEnumerable or other way
                    while (reader.Read())
                    {
                        result.Add(new TestTable()
                        {
                            // Need to investigate better way to convert table data to appropriate model with low operation/computing cost
                            TimeStamp = DateTime.Parse(reader[0].ToString()),
                            Name = reader[1].ToString(),
                            Metric = int.Parse(reader[2].ToString()),
                            Source = reader[3].ToString()
                        });
                    }
                }
                return result;
            }
            catch (Exception ex)
            {
                // Will be able to add specific catch statement throught debugging
                throw;
            }
        }
    }
}