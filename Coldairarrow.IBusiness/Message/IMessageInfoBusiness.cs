using Coldairarrow.Entity;
using Coldairarrow.Util;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Coldairarrow.Business
{
    public interface IMessageInfoBusiness
    {
        Task<PageResult<MessageInfo>> GetDataListAsync(PageInput<ConditionDTO> input);
        Task<MessageInfo> GetTheDataAsync(string id);
        Task AddDataAsync(MessageInfo data);
        Task UpdateDataAsync(MessageInfo data);
        Task DeleteDataAsync(List<string> ids);
    }
}