using Coldairarrow.Business;
using Coldairarrow.Entity;
using Coldairarrow.Util;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Coldairarrow.Api.Controllers
{
    [Route("/Face/[controller]/[action]")]
    public class ExpressionController : BaseApiController
    {
        #region DI

        public ExpressionController(IExpressionBusiness expressionBus)
        {
            _expressionBus = expressionBus;
        }

        IExpressionBusiness _expressionBus { get; }

        #endregion

        #region 获取

        [HttpPost]
        public async Task<PageResult<Expression>> GetDataList(PageInput<ConditionDTO> input)
        {
            return await _expressionBus.GetDataListAsync(input);
        }

        [HttpPost]
        public async Task<Expression> GetTheData(IdInputDTO input)
        {
            return await _expressionBus.GetTheDataAsync(input.id);
        }

        /// <summary>
        /// 获取所有表情
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        [NoCheckJWT]
        public async Task<List<Expression>> GetAllListAsync(Expression input)
        {
            return await _expressionBus.GetAllListAsync(input);
        }

        #endregion

        #region 提交

        [HttpPost]
        public async Task SaveData(Expression data)
        {
            if (data.Id.IsNullOrEmpty())
            {
                InitEntity(data);

                await _expressionBus.AddDataAsync(data);
            }
            else
            {
                await _expressionBus.UpdateDataAsync(data);
            }
        }

        [HttpPost]
        public async Task DeleteData(List<string> ids)
        {
            await _expressionBus.DeleteDataAsync(ids);
        }

        #endregion
    }
}