using Coldairarrow.Entity;
using Coldairarrow.Util;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Coldairarrow.Business
{
    public interface IServiceTelBusiness
    {
        Task<PageResult<ServiceTel>> GetDataListAsync(PageInput<ConditionDTO> input);
        Task<ServiceTel> GetTheDataAsync(string id);
        Task AddDataAsync(ServiceTel data);
        Task UpdateDataAsync(ServiceTel data);
        Task DeleteDataAsync(List<string> ids);
    }
}