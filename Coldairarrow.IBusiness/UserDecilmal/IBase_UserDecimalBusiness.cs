using Coldairarrow.Entity;
using Coldairarrow.Util;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Coldairarrow.Business
{
    public interface IBase_UserDecimalBusiness
    {
        Task<PageResult<Base_UserDecimal>> GetDataListAsync(PageInput<ConditionDTO> input);
        Task<Base_UserDecimal> GetTheDataAsync(string id);
        Task AddDataAsync(Base_UserDecimal data);
        Task UpdateDataAsync(Base_UserDecimal data);
        Task DeleteDataAsync(List<string> ids);
    }
}