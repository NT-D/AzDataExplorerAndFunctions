using System.Collections.Generic;
using System.Threading.Tasks;

namespace CseSample.Services
{
    public interface ITestTableService
    {
        Task<IEnumerable<TestTable>> GetLatestNDaysData(int days);
    }
}