using Coldairarrow.Entity;
using Coldairarrow.Util;
using EFCore.Sharding;
using LinqKit;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace Coldairarrow.Business
{
    public class ExpressionBusiness : BaseBusiness<Expression>, IExpressionBusiness, ITransientDependency
    {
        public ExpressionBusiness(IDbAccessor db)
            : base(db)
        {
        }

        #region 外部接口

        public async Task<PageResult<Expression>> GetDataListAsync(PageInput<ConditionDTO> input)
        {
            var q = GetIQueryable();
            var where = LinqHelper.True<Expression>();
            var search = input.Search;

            //筛选
            if (!search.Condition.IsNullOrEmpty() && !search.Keyword.IsNullOrEmpty())
            {
                var newWhere = DynamicExpressionParser.ParseLambda<Expression, bool>(
                    ParsingConfig.Default, false, $@"{search.Condition}.Contains(@0)", search.Keyword);
                where = where.And(newWhere);
            }

            return await q.Where(where).GetPageResultAsync(input);
        }

        public async Task<Expression> GetTheDataAsync(string id)
        {
            return await GetEntityAsync(id);
        }

        public async Task AddDataAsync(Expression data)
        {
            await InsertAsync(data);
        }

        public async Task UpdateDataAsync(Expression data)
        {
            await UpdateAsync(data);
        }

        public async Task DeleteDataAsync(List<string> ids)
        {
            await DeleteAsync(ids);
        }
        /// <summary>
        /// 获取所有表情
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>

        public async Task<List<Expression>> GetAllListAsync(Expression input)
        {
            var q = await GetIQueryable().OrderBy(p=>p.Sort).Where(p=>p.Deleted==false).ToListAsync();

            return q;


        }

        #endregion

        #region 私有成员

        #endregion
    }
}