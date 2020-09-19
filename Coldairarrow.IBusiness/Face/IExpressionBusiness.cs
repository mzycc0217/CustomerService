using Coldairarrow.Entity;
using Coldairarrow.Util;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Coldairarrow.Business
{
    public interface IExpressionBusiness
    {
        Task<PageResult<Expression>> GetDataListAsync(PageInput<ConditionDTO> input);
        Task<Expression> GetTheDataAsync(string id);
        Task AddDataAsync(Expression data);
        Task UpdateDataAsync(Expression data);
        Task DeleteDataAsync(List<string> ids);

        /// <summary>
        /// 获取所有表情
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<List<Expression>> GetAllListAsync(Expression input);
    }
}