using Coldairarrow.Entity;
using Coldairarrow.Util;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Coldairarrow.Business
{
    public interface IMenu_ServiseBusiness
    {
        Task<PageResult<Menu_Servise>> GetDataListAsync(PageInput<ConditionDTO> input);
        Task<Menu_Servise> GetTheDataAsync(string id);
        Task AddDataAsync(Menu_Servise data);
        Task UpdateDataAsync(Menu_Servise data);
        Task DeleteDataAsync(List<string> ids);
    }
}